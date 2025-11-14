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
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace vitaaid.com.Controllers
{
  [Route("api/ProductImages")]
  [ApiController]
  public class ProductImagesController : ControllerBase
  {
    private readonly ILogger<ProductImagesController> _logger;
    public ProductImagesController(ILogger<ProductImagesController> logger)
    {
      _logger = logger;
    }

    private void PostProcess(Product oProduct, IList<UnitType> vitaaidCategory)
    {
      oProduct.Category?.Split(';')?.Also(cList =>
      {
        if (!cList.Any()) return;
        oProduct.VitaaidCategory = vitaaidCategory.Where(s => cList.Contains(s.AbbrName)).Select(s => s.AbbrName.ToInt()).ToList();
      });
    }

    // GET: api/ProductImages?productcode={0}
    [HttpGet]
    public IActionResult Get(string productcode)
    {
      if (string.IsNullOrWhiteSpace(productcode))
        return Ok(null);
      var oSession = DBServer[eST.READONLY];
      try
      {
        oSession.Clear();
        var oProductImages = oSession.QueryDataElement<ProductImage>()
                                      .Where(x => x.ProductCode == productcode)
                                      .OrderBy(x => x.ID)
                                      .ToList();

        return Ok(oProductImages);
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

    // POST api/ProductImages
    [Authorize]
    [HttpPost]
    public IActionResult Post([FromBody] IList<ProductImage> oNewProductImages)
    {
      if (oNewProductImages.Count == 0)
        return Ok(0);

      var oSession = DBServer[eST.SESSION0];
      ITransaction xact = null;
      try
      {
        oSession.Clear();
        oNewProductImages.Action(x => x.oProduct = oSession.QueryDataElement<Product>().Where(p => p.ProductCode == x.ProductCode).ToList().FirstOrDefault());

        xact = oSession.BeginTransaction();
        oNewProductImages.Action(x => x.SaveObj(oSession));
        xact.Commit();
        return Ok(oNewProductImages.Count());
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

    //PUT api/ProductImages/{id}/update
    [Authorize]
    [HttpPut("{id}/update")]
    public IActionResult Update([FromRoute] long id, [FromBody] object values)
    {
      var oSession = DBServer[eST.SESSION0];
      ITransaction xact = null;
      try
      {
        oSession.Clear();

        oSession.QueryDataElement<ProductImage>().Where(x => x.ID == id)
            .UniqueOrDefault()
            ?.Also(oDirtyProductImage =>
            {
              xact = oSession.BeginTransaction();
              JsonConvert.PopulateObject(values.ToString(), oDirtyProductImage);
              oDirtyProductImage.SaveObj(oSession);
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
      var oSession = DBServer[eST.SESSION0];
      ITransaction xact = null;
      try
      {
        oSession.Clear();
        var oProduct = oSession.QueryDataElement<ProductImage>().Where(x => x.ID == id).UniqueOrDefault();
        oProduct?.Also(x =>
        {
          xact = oSession.BeginTransaction();
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
  }
}
