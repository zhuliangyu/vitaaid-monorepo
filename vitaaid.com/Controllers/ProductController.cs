using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebDB.DBBO;
using MyHibernateUtil.Extensions;
using static vitaaid.com.ServicesHelper;
using NHibernate;
using MySystem.Base.Extensions;
using MyHibernateUtil;
using static MySystem.Base.Web.Constant;
using WebDB.DBPO.Extensions;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebDB.DBPO.Helper;
using WebDB.DBPO;
using MIS.DBBO;
using Microsoft.Extensions.Logging;
using static CIM.DBPO.DBPOServiceHelper;
using CIM.DBPO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace vitaaid.com.Controllers
{
  public class WishData
  {
    public virtual string CustomerCode { get; set; }
    public virtual int Qty { get; set; }
  }
  public class ElementalNutritionData
  {
    public virtual string ProductCode { get; set; }
    public virtual decimal Price { get; set; }
    public virtual int Calories { get; set; }

  }
  [Route("api/Products")]
  [ApiController]
  public class ProductController : ControllerBase
  {
    private readonly ILogger<ProductController> _logger;
    public ProductController(ILogger<ProductController> logger)
    {
      _logger = logger;
    }

    // GET: api/Products/{code}?country=
    [HttpGet("{code}")]
    public IActionResult Get(string code, string Country)
    {
      if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(Country))
        return BadRequest();//Ok(null);

      _logger.LogInformation("Get product:{0}, {1}", code, Country);
      var oSession = DBServer[eST.READONLY];
      var oCIMSession = CIMDBServer[eST.READONLY];
      try
      {
        eWEBSITE site = eWEBSITE.CA;
        try
        {
          site = (eWEBSITE)Enum.Parse(typeof(eWEBSITE), Country.ToUpper());
        }
        catch (Exception)
        {
          return BadRequest();
        }

        oSession.Clear();
        oCIMSession.Clear();

        // 2: Vita Aid Category
        var vitaaidCategory = CategoryHelper.getVitaaidCategory(oSession);
        // 5: Allergy Category
        var allergyCategory = CategoryHelper.getAllergyCategory(oSession);

        if (site == eWEBSITE.US)
        {
          code = ProductCodeToFormulationHelper.getUSAProductCode(oCIMSession, code);
          if (string.IsNullOrWhiteSpace(code))
            return BadRequest();
          _logger.LogInformation("Code for USA: {0}", code);
        }

        var oProduct = oSession.QueryDataElement<Product>()
                                .Where(x => x.ProductCode.ToUpper() == code.ToUpper() &&
                                           (x.VisibleSite == eWEBSITE.ALL || site == eWEBSITE.ALL || x.VisibleSite == site) &&
                                           x.Published == true)
                                .ToList().UniqueOrDefault();
        if (oProduct == null)
          return BadRequest();
        oProduct.Also(x => x.PostProcess(vitaaidCategory, allergyCategory, oSession));
        oProduct.oProductImages?.Action(x => x.loadImageWidthHeight(@"ClientApp/build/" + EnvHelper.PRODUCT_DIR, @"ClientApp/build/" + EnvHelper.PRODUCT_DIR));
        return Ok(oProduct);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An error occurred.");
        throw;
      }
      finally
      {
        oSession.Close();
        oCIMSession.Close();
      }
    }

    // GET: api/Products/alphabetlist?country={country}
    [HttpGet("alphabetlist")]
    public IActionResult AlphabetList(string country)
    {
      _logger.LogDebug("AlphabetList, {0}", country);
      var oSession = DBServer[eST.READONLY];
      try
      {
        oSession.Clear();
        eWEBSITE site = eWEBSITE.CA;
        try
        {
          site = (eWEBSITE)Enum.Parse(typeof(eWEBSITE), country.ToUpper());
        }
        catch (Exception)
        {
          return BadRequest();
        }

        var where = " WHERE VisibleSite = 255 OR VisibleSite = " + ((int)site).ToString();
        return Ok(oSession.CreateSQLQuery("SELECT DISTINCT SUBSTRING(ProductName, 1, 1) AS alphabet FROM products " + where + " ORDER BY alphabet").List());
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An error occurred.");
        throw;
      }
      finally
      {
        oSession.Close();
      }
    }

    // GET: api/Products/{category}/{country}
    [HttpGet("{category}/{country}")]
    public IActionResult GetByCategory(int category, string country)
    {
      _logger.LogDebug("get products by category[{0}], {1}", category, country);
      var oSession = DBServer[eST.READONLY];
      try
      {
        oSession.Clear();

        if (category == 0)
          return Ok(new List<Product>());

        eWEBSITE site = eWEBSITE.CA;
        try
        {
          site = (eWEBSITE)Enum.Parse(typeof(eWEBSITE), country.ToUpper());
        }
        catch (Exception)
        {
          return BadRequest();
        }

        var oProducts = oSession.QueryDataElement<Product>()
                               .Where(x => x.Published && (x.VisibleSite == eWEBSITE.ALL || site == eWEBSITE.ALL || x.VisibleSite == site) &&
                                           x.Category.Contains(category.ToString()))
                               .OrderBy(x => x.ProductName)
                               .ToList();

        oProducts.Action(x =>
        {
          x.oIngredients = oSession.QueryDataElement<Ingredient>()
                                   .Where(i => i.oProduct == x)
                                   .OrderBy(i => i.Sequence)
                                   .ToList();
          x.RepresentativeImage = oSession.QueryDataElement<ProductImage>()
                                          .Where(i => i.oProduct == x && i.FrontImage)
                                          .ToList()
                                          .FirstOrDefault()?.ImageName ?? "";
        });

        return Ok(oProducts);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An error occurred.");
        throw;
      }
      finally
      {
        oSession.Close();
      }
    }

    // GET: api/Products/alphabet?ch={alphabet}&country=
    [HttpGet("alphabet")]
    public IActionResult GetByAlphabet(string ch, string country)
    {
      if (string.IsNullOrWhiteSpace(ch))
        return Ok(new List<Product>());

      _logger.LogDebug("get products by Alphabet[{0}], {1}", ch, country);
      var oSession = DBServer[eST.READONLY];
      try
      {
        oSession.Clear();

        eWEBSITE site = eWEBSITE.CA;
        try
        {
          site = (eWEBSITE)Enum.Parse(typeof(eWEBSITE), country.ToUpper());
        }
        catch (Exception)
        {
          return BadRequest();
        }
        var oProducts = oSession.QueryDataElement<Product>()
                               .Where(x => x.Published && (x.VisibleSite == eWEBSITE.ALL || site == eWEBSITE.ALL || x.VisibleSite == site) &&
                                           x.ProductName.StartsWith(ch))
                               .OrderBy(x => x.ProductName)
                               .ToList();

        oProducts.Action(x =>
        {
          x.oIngredients = oSession.QueryDataElement<Ingredient>()
                                   .Where(i => i.oProduct == x)
                                   .OrderBy(i => i.Sequence)
                                   .ToList();
          x.RepresentativeImage = oSession.QueryDataElement<ProductImage>()
                                          .Where(i => i.oProduct == x && i.FrontImage)
                                          .ToList()
                                          .FirstOrDefault()?.ImageName ?? "";
        });

        return Ok(oProducts);

      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An error occurred.");
        throw;
      }
      finally
      {
        oSession.Close();
      }
    }

    // GET: api/Products/featured?country=
    [HttpGet("featured")]
    public IActionResult GetFeatureProducts(string country)
    {
      _logger.LogDebug("get feature products, {0}", country);
      var oSession = DBServer[eST.READONLY];
      try
      {
        oSession.Clear();
        eWEBSITE site = eWEBSITE.CA;
        try
        {
          site = (eWEBSITE)Enum.Parse(typeof(eWEBSITE), country.ToUpper());
        }
        catch (Exception)
        {
          return BadRequest();
        }
        var oProducts = oSession.QueryDataElement<Product>()
                 .Where(x => x.Published && (x.VisibleSite == eWEBSITE.ALL || site == eWEBSITE.ALL || x.VisibleSite == site) &&
                             x.Featured)
                 .ToList()
                 .Shuffle<Product>();

        oProducts.Action(x =>
        {
          x.RepresentativeImage = oSession.QueryDataElement<ProductImage>()
                                        .Where(i => i.oProduct == x && i.FrontImage)
                                        .ToList()
                                        .FirstOrDefault()?.ImageName ?? "";
        });

        return Ok(oProducts);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An error occurred.");
        throw;
      }
      finally
      {
        oSession.Close();
      }
    }

    [HttpGet]
    public IActionResult Get(string filter = "", bool byNPN = true, bool byCode = true, bool byName = true, eWEBSITE siteFilter = eWEBSITE.ALL, bool chkTop = true, int count = 0)
    {
      _logger.LogInformation("get products, filter={0}, byNPN={1}, byCode={2}, byNames={3], site={4}, chkTop={5}, count={6}", 
        filter, byNPN, byCode, byName, siteFilter, chkTop, count);
      var oSession = DBServer[eST.READONLY];
      try
      {
        oSession.Clear();

        // type:
        // 2: Vita Aid Category
        var vitaaidCategory = CategoryHelper.getVitaaidCategory(oSession);
        // 5: Allergy Category
        var allergyCategory = CategoryHelper.getAllergyCategory(oSession);
        Expression<Func<Product, bool>> predicate = ExpressionExtension.True<Product>();
        if (siteFilter != eWEBSITE.ALL)
          predicate = predicate.And(x => x.VisibleSite == eWEBSITE.ALL || x.VisibleSite == siteFilter);
        string sFilter = filter?.Trim()?.ToLower() ?? "";
        if (sFilter.Length > 0)
        {
          Expression<Func<Product, bool>> others = ExpressionExtension.False<Product>();
          if (byNPN)
            others = others.Or(x => x.NPN.ToLower().Contains(sFilter));
          if (byCode)
            others = others.Or(x => x.ProductCode.ToLower().Contains(sFilter));
          if (byName)
            others = others.Or(x => x.ProductName.ToLower().Contains(sFilter));
          predicate = predicate.And(others);
        }

        IQueryable<Product> query = oSession.QueryDataElement<Product>()
                                                   .Where(predicate)
                                                   .OrderBy(x => x.ProductName);
        if (count > 0 && chkTop == true)
          query = query.Take(count);
        var oProducts = query.ToList();
        oProducts.Action(x => x.PostProcess(vitaaidCategory, allergyCategory, oSession));

        return Ok(oProducts);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An error occurred.");
        throw;
      }
      finally
      {
        oSession.Close();
      }
    }

    // GET: api/Products/related/{code}?country=
    [HttpGet("related/{code}")]
    public IActionResult GetByTag(string code, string Country)
    {
      _logger.LogDebug("get related products by tag {0}, {1}", code, Country);
      if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(Country))
        return BadRequest();
      var oSession = DBServer[eST.READONLY];
      var oCIMSession = CIMDBServer[eST.READONLY];
      try
      {
        eWEBSITE site = eWEBSITE.CA;
        try
        {
          site = (eWEBSITE)Enum.Parse(typeof(eWEBSITE), Country.ToUpper());
        }
        catch (Exception)
        {
          return BadRequest();
        }
        oSession.Clear();
        oCIMSession.Clear();

        if (site == eWEBSITE.US)
        {
          code = ProductCodeToFormulationHelper.getUSAProductCode(oCIMSession, code);
          if (string.IsNullOrWhiteSpace(code))
            return BadRequest();
        }

        var oProduct = oSession.QueryDataElement<Product>()
                                .Where(x => x.ProductCode.ToUpper() == code.ToUpper() &&
                                           (x.VisibleSite == eWEBSITE.ALL || site == eWEBSITE.ALL || x.VisibleSite == site) &&
                                           x.Published == true)
                                .ToList().UniqueOrDefault();
        var allRelatedProducts = oProduct?.Tags?.Split(';')?.Where(x => string.IsNullOrWhiteSpace(x) == false)
                                            ?.SelectMany(tag =>
                                oSession.QueryDataElement<Product>()
                                         .Where(x => x.ID != oProduct.ID && x.Tags.Contains(tag.Trim() + ";") &&
                                                    (x.VisibleSite == eWEBSITE.ALL || site == eWEBSITE.ALL || x.VisibleSite == site) &&
                                                    x.Published == true)
                                         .ToList()) ?? new List<Product>();
        var oProducts = allRelatedProducts
                                         .GroupBy(o => o.ID)
                                         .Select(g => g.First())
                                         .Where(x => x.ID != oProduct.ID)
                                         .OrderBy(x => x.ID)
                                         .ToList();

        oProducts.Action(x =>
        {
          x.RepresentativeImage = oSession.QueryDataElement<ProductImage>()
                                        .Where(i => i.oProduct == x && i.FrontImage)
                                        .ToList()
                                        .FirstOrDefault()?.ImageName ?? "";
        });

        return Ok(oProducts);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An error occurred.");
        throw;
      }
      finally
      {
        oSession.Close();
        oCIMSession.Close();
      }
    }

    // GET: api/Products/nutrition?country=
    [HttpGet("nutrition")]
    public IActionResult GetNutrition(string Country)
    {
      _logger.LogDebug("GetNutrition, {0}", Country);
      var oVMSession = VAMISDBServer[eST.READONLY];
      var oCIMSession = CIMDBServer[eST.READONLY];
      try
      {
        if (string.IsNullOrWhiteSpace(Country))
          return BadRequest();

        oVMSession.Clear();
        oCIMSession.Clear();

        eWEBSITE site = eWEBSITE.CA;
        site = (eWEBSITE)Enum.Parse(typeof(eWEBSITE), Country.ToUpper());

        //Elemental Nutrition (Chocolate) => VA-149
        //KT-Elemental Nutrition (Chocolate) => VA-156
        string[] codes = { "VA-149", "VA-156" };
        string[] usCodes = { "VA-149", "VA-156" };
        IList<ElementalNutritionData> rtnData = new List<ElementalNutritionData>();
        if (site == eWEBSITE.US)
        {
          codes.ForEachWithIndex((x, idx) =>
          {
            usCodes[idx] = ProductCodeToFormulationHelper.getUSAProductCode(oCIMSession, x);
            oVMSession.QueryDataElement<FinishProductPrice>()
                                   .Where(y => y.ProductCode == usCodes[idx])
                                   .ToList()
                                   .Action(p => rtnData.Add(
                                                new ElementalNutritionData
                                                     {
                                                       ProductCode = x,
                                                       Price = p.USDPrice * 2
                                                     }));
          });
        }
        else
        {
          codes.ForEachWithIndex((x, idx) =>
          {
            oVMSession.QueryDataElement<FinishProductPrice>()
                                   .Where(y => y.ProductCode == x)
                                   .ToList()
                                   .Action(p => rtnData.Add(
                                                new ElementalNutritionData
                                                {
                                                  ProductCode = x,
                                                  Price = p.MSRPrice
                                                }));

          });
        }
        rtnData.Where(x => x.ProductCode == "VA-149").Action(x => x.Calories = 1800);
        rtnData.Where(x => x.ProductCode == "VA-156").Action(x => x.Calories = 1836);
        return Ok(rtnData);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An error occurred.");
        return BadRequest();
      }
      finally
      {
        oVMSession.Close();
        oCIMSession.Close();
      }
    }


    // POST api/Products
    //[Authorize]
    [HttpPost]
    public IActionResult Post([FromBody] IList<Product> oNewerProducts)
    {
      if (oNewerProducts.Count == 0)
        return Ok(0);
      _logger.LogInformation("New products:", oNewerProducts.Select(x=> x.ProductCode).Aggregate((x, y) => x + "," + y));
      var oSession = DBServer[eST.SESSION0];
      ITransaction xact = null;
      try
      {
        oSession.Clear();
        
        xact = oSession.BeginTransaction();

        oNewerProducts.Action(x =>
        {
          x.oIngredients?.Action(y => y.Also(t => t.oProduct = x).SaveObj(oSession));
          x.oProductImages?.Action(y => y.Also(t => t.oProduct = x).SaveObj(oSession));
          x.Category = x.VitaaidCategory?.Select(x => x.ToString())?.Aggregate((a, b) => a + ";" + b) ?? "";
          x.Category1 = x.AllergyCategory?.Select(x => x.ToString())?.Aggregate((a, b) => a + ";" + b) ?? "";
          //x.PreAdd();
          x.TimeStamp = DateTime.Now;
          oSession.SaveObj(x);
        });
        xact.Commit();
        return Ok(oNewerProducts.Count());
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An error occurred.");
        xact?.Rollback();
        return BadRequest(ex);
      }
      finally
      {
        oSession.Close();
      }
    }


    // PUT api/Products/{id}
    [Authorize]
    [HttpPut("{id}")]
    public IActionResult Put(long id, [FromBody] Product oDirtyProduct)
    {
      _logger.LogInformation("Update product, code={0}, name={1}", oDirtyProduct.ProductCode, oDirtyProduct.ProductName);

      var oSession = DBServer[eST.SESSION0];
      ITransaction xact = null;
      bool bRebuildSequence = false;
      try
      {
        oSession.Clear();
        xact = oSession.BeginTransaction();
        oDirtyProduct.oIngredients?.Action(x =>
        {
          if (x.iState == MySystem.Base.eOPSTATE.INIT)
            return;
          x.oProduct = oDirtyProduct;
          if (x.iState == MySystem.Base.eOPSTATE.DELETE)
          {
            x.DeleteObj(oSession);
            bRebuildSequence = true;
          }
          else
            x.SaveObj(oSession);
        });
        oDirtyProduct.oProductImages?.Action(x =>
        {
          if (x.iState == MySystem.Base.eOPSTATE.INIT)
            return;
          x.oProduct = oDirtyProduct;
          if (x.iState == MySystem.Base.eOPSTATE.DELETE)
            x.DeleteObj(oSession);
          else
            x.SaveObj(oSession);
        });
        oDirtyProduct.Category = oDirtyProduct.VitaaidCategory.Any() ? oDirtyProduct.VitaaidCategory?.Select(x => x.ToString())?.Aggregate((a, b) => a + ";" + b) ?? "" : "";
        oDirtyProduct.Category1 = oDirtyProduct.AllergyCategory.Any() ? oDirtyProduct.AllergyCategory?.Select(x => x.ToString())?.Aggregate((a, b) => a + ";" + b) ?? "" : "";
        oDirtyProduct.SaveObj(oSession);
        xact.Commit();
        if (bRebuildSequence)
        {
          xact = oSession.BeginTransaction();
          oSession.QueryDataElement<Ingredient>()
              .Where(x => x.oProduct.ID == id && x.GroupNo != 5)
              .OrderBy(x => x.Sequence)
              .ToList()
              .ForEachWithIndex((x, idx) =>
              {
                x.Sequence = idx + 1;
                x.SaveObj(oSession);
              });
          xact.Commit();
        }
        return Ok(0);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An error occurred.");
        xact?.Rollback();
        return BadRequest(ex);
      }
      finally
      {
        oSession.Close();
      }
    }

    //PUT api/Products/{id}/update
    [Authorize]
    [HttpPut("{id}/update")]
    public IActionResult Update([FromRoute] long id, [FromBody] object values)
    {
      _logger.LogInformation("Update product, id={0}", id);
      var oSession = DBServer[eST.SESSION0];
      ITransaction xact = null;
      try
      {
        oSession.Clear();
        oSession.QueryDataElement<Product>().Where(x => x.ID == id)
            .UniqueOrDefault()
            ?.Also(oDirtyProduct =>
            {
              xact = oSession.BeginTransaction();
              JsonConvert.PopulateObject(values.ToString(), oDirtyProduct);
              if ((values as Newtonsoft.Json.Linq.JObject)?.ContainsKey("vitaaidCategory") ?? false)
                oDirtyProduct.Category = (oDirtyProduct.VitaaidCategory.Any()) ? oDirtyProduct.VitaaidCategory.Select(x => x.ToString()).Aggregate((a, b) => a + ";" + b)
                                                                                       : "";
              if ((values as Newtonsoft.Json.Linq.JObject)?.ContainsKey("allergyCategory") ?? false)
                oDirtyProduct.Category1 = (oDirtyProduct.AllergyCategory.Any()) ? oDirtyProduct.AllergyCategory.Select(x => x.ToString()).Aggregate((a, b) => a + ";" + b)
                                                                                       : "";
              oDirtyProduct.SaveObj(oSession);
              xact.Commit();
            });
        return Ok(0);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An error occurred.");
        xact?.Rollback();
        return BadRequest(ex);
      }
      finally
      {
        oSession.Close();
      }
    }

    [Authorize]
    [HttpDelete]
    public ActionResult Delete(long id)
    {
      _logger.LogInformation("Delete product, id={0}", id);
      var oSession = DBServer[eST.SESSION0];
      ITransaction xact = null;
      try
      {
        oSession.Clear();
        var oProduct = oSession.QueryDataElement<Product>().Where(x => x.ID == id).UniqueOrDefault();
        oProduct?.Also(x =>
        {
          _logger.LogInformation("Delete product, code={0}, name={1}", oProduct.ProductCode, oProduct.ProductName);
          xact = oSession.BeginTransaction();

          oSession.QueryDataElement<Ingredient>()
                     .Where(y => y.oProduct == x)
                     .ToList()
                     .Action(ing => ing.DeleteObj(oSession));

          x.DeleteObj(oSession);
          xact.Commit();

        });
        return Ok(0);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An error occurred.");
        xact?.Rollback();
        return BadRequest(ex);
      }
      finally
      {
        oSession.Close();
      }
    }


    // VA.MIS Product
    // GET: api/Products/{code}/stocknprice?customercode=&country=
    //[Authorize]
    [HttpGet("{code}/stocknprice")]
    public IActionResult StockNPrice(string code, string customerCode, string country)
    {
      if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(customerCode) || string.IsNullOrWhiteSpace(country))
        return BadRequest();//Ok(null);
      _logger.LogInformation("Customer {0} get stock info and price of product {1}, country {2}", customerCode, code, country);      var oVMSession = VAMISDBServer[eST.READONLY];
      try
      {
        oVMSession.Clear();

        CustomerAccount customer = oVMSession.QueryDataElement<CustomerAccount>()
                                              .Where(c => c.CustomerCode == customerCode)
                                              .UniqueOrDefault();
        if (customer == null)
          return BadRequest();
        var productPrice = oVMSession.QueryDataElement<FinishProductPrice>()
                                       .Where(y => y.ProductCode == code)
                                       .UniqueOrDefault();
        if (productPrice == null)
          return BadRequest();

        var isCA = (country.ToUpper() == "CA");
        var StockCount = oVMSession.QueryDataElement<VitaAidFinishProduct>()
                                      .Where(y => y.ProductCode == code && y.IsSample == false && y.Salable && y.IsDisposal == false && y.IsOEM == false && y.StockCount > 0)
                                      .ToList()
                                      .Where(y => y.bQuarantine == false && y.ExpiredDate >= DateTime.Now)
                                      .Sum(y => y.StockCount);
        var UnitPrice = customer.PricePolicy switch
        {
          ePRICEPOLICY.STANDARD => (isCA) ? productPrice.StandardPrice : productPrice.USDPrice,
          ePRICEPOLICY.STANDARD_USD => (isCA) ? productPrice.StandardPrice : productPrice.USDPrice,
          ePRICEPOLICY.EMPLOYEE => productPrice.EmployeePrice,
          ePRICEPOLICY.MSRP_USD => (isCA) ? productPrice.MSRPrice : decimal.Round(productPrice.USDPrice * 2, 2, MidpointRounding.AwayFromZero),
          _ => (isCA) ? productPrice.MSRPrice : decimal.Round(productPrice.USDPrice * 2, 2, MidpointRounding.AwayFromZero),
        };
        var dropShipPrice = UnitPrice;
        if (isCA)
          dropShipPrice = customer.PricePolicy switch
          {
            ePRICEPOLICY.STANDARD => productPrice.USDPrice,
            ePRICEPOLICY.STANDARD_USD => productPrice.USDPrice,
            ePRICEPOLICY.EMPLOYEE => productPrice.EmployeePrice,
            ePRICEPOLICY.MSRP_USD => decimal.Round(productPrice.USDPrice * 2, 2, MidpointRounding.AwayFromZero),
            _ => decimal.Round(productPrice.USDPrice * 2, 2, MidpointRounding.AwayFromZero),
          };
        return Ok(new { stock = StockCount, price = UnitPrice, dropShipPrice = dropShipPrice });

      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An error occurred.");
        throw;
      }
      finally
      {
        oVMSession.Close();
      }
    }

    // PUT: api/Products/{code}/wishproduct
    [HttpPut("{code}/wishproduct")]
    public IActionResult WishProduct(string code, [FromBody] WishData wishData)
    {
      if (string.IsNullOrWhiteSpace(code) || wishData == null || string.IsNullOrEmpty(wishData.CustomerCode))
        return BadRequest();//Ok(null);
      if (wishData.Qty <= 0)
        return Ok();
      _logger.LogInformation("Customer {0}, wish {1} x products {2}", wishData.CustomerCode, wishData.Qty, code);
      var oVMSession = VAMISDBServer[eST.SESSION0];
      var oWebSession = WebDBServer[eST.READONLY];
      ITransaction xact = null;
      try
      {
        oVMSession.Clear();
        oWebSession.Clear();

        CustomerAccount customer = oVMSession.QueryDataElement<CustomerAccount>()
                                        .Where(c => c.CustomerCode == wishData.CustomerCode)
                                        .UniqueOrDefault();
        if (customer == null)
          return BadRequest();

        var productPrice = oVMSession.QueryDataElement<FinishProductPrice>()
                                 .Where(y => y.ProductCode == code)
                                 .UniqueOrDefault();
        if (productPrice == null)
          return BadRequest();

        var newObj = new WishProduct
        {
          oCustomer = customer,
          ProductCode = productPrice.ProductCode,
          ProductName = productPrice.ProductName,
          CustomerName = customer.CustomerName,
          CustomerEmail = customer.CustomerEmail1,
          Qty = wishData.Qty
        };

        oWebSession.QueryDataElement<Member>().Where(m => m.CustomerCode == wishData.CustomerCode).FirstOrDefault()
          ?.Also(oMember =>
          {
            newObj.CustomerEmail = oMember.Email;
            newObj.CustomerName = 
              oMember.Prefix switch {
                 ePREFIX.DR => "Dr. ",
                 ePREFIX.MR => "Mr. ",
                 ePREFIX.MS => "Ms. ",
                 _ => ""
              }
              + oMember.Name;
          });

        xact = oVMSession.BeginTransaction();
        newObj.SaveObj(oVMSession);
        xact.Commit();
        return Ok();
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An error occurred.");
        xact?.Rollback();
        return BadRequest(ex.ToString());
      }
      finally
      {
        oVMSession.Close();
        oWebSession.Close();
      }
    }
  }
}
