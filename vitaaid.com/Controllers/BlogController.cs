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
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace vitaaid.com.Controllers
{
  [Route("api/blog")]
  [ApiController]
  public class BlogController : ControllerBase
  {
    private readonly ILogger<BlogController> _logger;
    public BlogController(ILogger<BlogController> logger)
    {
      _logger = logger;
    }

    // GET: api/blog?category={category}
    [HttpGet]
    public IActionResult Get(int category)
    {
      _logger.LogDebug("Blog Get");
      var oSession = DBServer[eST.READONLY];
      try
      {
        oSession.Clear();

        var blogCategories = oSession.QueryDataElement<UnitType>().Where(x => x.uType == eUNITTYPE.THERAPEUTIC_FOCUS).ToList();

        Expression<Func<TherapeuticFocus, bool>> predicate = ExpressionExtension.True<TherapeuticFocus>();
        predicate = predicate.And(x => x.Published);
        if (category > 0)
          predicate = predicate.And(x => x.Category == category);
        var oArticles = oSession.QueryDataElement<TherapeuticFocus>()
                               .Where(predicate)
                               .OrderByDescending(x => x.ID)
                               .ToList()
                               .Also(x => x.Action(y =>
                               {
                                 y.BlogContent = "";
                                 y.sCategory = blogCategories.Where(c => c.AbbrName == y.Category.ToString()).FirstOrDefault()?.Name ?? "";
                               }));

        return Ok(oArticles);
      }
      catch (Exception)
      {
        throw;
      }
      finally
      {
        oSession.Close();
      }
    }

    // GET: api/blog/latest?count={count}
    [HttpGet("latest")]
    public IActionResult Latest(int count)
    {
      _logger.LogDebug("Blog Latest");
      var oSession = DBServer[eST.READONLY];
      try
      {
        oSession.Clear();

        Expression<Func<TherapeuticFocus, bool>> predicate = ExpressionExtension.True<TherapeuticFocus>();
        predicate = predicate.And(x => x.Published);
        if (count <= 0)
          count = 2;
        var oArticles = oSession.QueryDataElement<TherapeuticFocus>()
                               .Where(predicate)
                               .OrderByDescending(x => x.ID)
                               .Take(count)
                               .ToList()
                               .Also(x => x.Action(y =>
                               {
                                 y.BlogContent = "";
                               //y.sCategory = blogCategories.Where(c => c.AbbrName == y.Category.ToString()).FirstOrDefault()?.Name ?? "";
                             }));

        return Ok(oArticles);
      }
      catch (Exception)
      {
        throw;
      }
      finally
      {
        oSession.Close();
      }
    }


    // GET: api/blog/{id}?country={country}
    [HttpGet("{id}")]
    public IActionResult GetArticle(int id, string Country)
    {
      _logger.LogDebug("Blog GetArticle");
      var oSession = DBServer[eST.READONLY];
      try
      {
        oSession.Clear();

        eWEBSITE site = eWEBSITE.CA;
        try
        {
          site = (string.IsNullOrWhiteSpace(Country)) ? eWEBSITE.CA : (eWEBSITE)Enum.Parse(typeof(eWEBSITE), Country.ToUpper());
        }
        catch (Exception) { }

        var oArticle = oSession.QueryDataElement<TherapeuticFocus>()
                                .Where(x => x.ID == id && x.Published == true)
                                .ToList().UniqueOrDefault();
        if (oArticle == null)
          return NotFound();

        oSession.QueryDataElement<UnitType>()
                .Where(x => x.uType == eUNITTYPE.THERAPEUTIC_FOCUS && x.AbbrName == oArticle.Category.ToString())
                .ToList().FirstOrDefault()
                ?.Let(x => oArticle.sCategory = x.Name);

        oArticle.RelatedProducts = ProductHelper.GetRelatedsProduct(oArticle.Tags, site, oSession, 5); ;
        return Ok(oArticle);
      }
      catch (Exception)
      {
        throw;
      }
      finally
      {
        oSession.Close();
      }
    }
  }
}
