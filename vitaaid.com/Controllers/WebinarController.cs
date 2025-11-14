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
  [Route("api/webinars")]
  [ApiController]
  public class WebinarController : ControllerBase
  {
    private readonly ILogger<WebinarController> _logger;
    public WebinarController(ILogger<WebinarController> logger)
    {
      _logger = logger;
    }


    // GET: api/webinars
    [HttpGet]
    public IActionResult Get()
    {
      _logger.LogDebug("Get all webinars.");
      var oSession = DBServer[eST.READONLY];
      try
      {
        oSession.Clear();

        Expression<Func<Webinar, bool>> predicate = ExpressionExtension.True<Webinar>();
        predicate = predicate.And(x => x.Published);
        //if (category > 0)
        //    predicate = predicate.And(x => x.Category == category);
        var oWebinars = oSession.QueryDataElement<Webinar>()
                 .Where(predicate)
                 .OrderByDescending(x => x.ID)
                 .ToList()
                 .Also(x => x.Action(y =>
                 {
                   y.WebinarContent = "";
                 }));

        return Ok(oWebinars);
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

    // GET: api/webinars/latest?count={count}
    [HttpGet("latest")]
    public IActionResult Latest(int count)
    {
      _logger.LogDebug("Get lastest webinars, count={0}.", count);
      var oSession = DBServer[eST.READONLY];
      try
      {
        oSession.Clear();

        Expression<Func<Webinar, bool>> predicate = ExpressionExtension.True<Webinar>();
        predicate = predicate.And(x => x.Published);
        //if (category > 0)
        //    predicate = predicate.And(x => x.Category == category);
        var oWebinars = oSession.QueryDataElement<Webinar>()
                 .Where(predicate)
                 .OrderByDescending(x => x.ID)
                 .Take(count)
                 .ToList()
                 .Also(x => x.Action(y =>
                 {
                   y.WebinarContent = "";
                 }));

        return Ok(oWebinars);
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

    // GET: api/webinars/{id}?country={country}
    [HttpGet("{id}")]
    public IActionResult GetWebinar(int id, string Country)
    {
      _logger.LogInformation("GetWebinar, id={0}, country={1}", id, Country);
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

        var oWebinar = oSession.QueryDataElement<Webinar>()
                                .Where(x => x.ID == id && x.Published == true)
                                .ToList().UniqueOrDefault();
        if (oWebinar == null)
          return NotFound();

        oWebinar.RelatedProducts = ProductHelper.GetRelatedsProduct(oWebinar.Tags, site, oSession, 3); ;
        return Ok(oWebinar);
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
  }
}
