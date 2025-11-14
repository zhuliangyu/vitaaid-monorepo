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
  [Route("api/protocols")]
  [ApiController]
  public class ProtocolController : ControllerBase
  {
    private readonly ILogger<ProtocolController> _logger;
    public ProtocolController(ILogger<ProtocolController> logger)
    {
      _logger = logger;
    }


    // GET: api/protocols
    [HttpGet]
    public IActionResult Get()
    {
      _logger.LogDebug("Get all protocols.");
      var oSession = DBServer[eST.READONLY];
      try
      {
        oSession.Clear();

        Expression<Func<TherapeuticProtocol, bool>> predicate = ExpressionExtension.True<TherapeuticProtocol>();
        predicate = predicate.And(x => x.Published);
        //if (category > 0)
        //    predicate = predicate.And(x => x.Category == category);
        var oProtocols = oSession.QueryDataElement<TherapeuticProtocol>()
                 .Where(predicate)
                 .OrderByDescending(x => x.ID)
                 .ToList()
                 .Also(x => x.Action(y =>
                 {
                   y.BlogContent = y.BlogContent.Substring(0, 250);
                 }));

        return Ok(oProtocols);
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

    // GET: api/protocols/latest?count={count}
    [HttpGet("latest")]
    public IActionResult Latest(int count)
    {
      _logger.LogDebug("Get lastest protocols, count={0}.", count);
      var oSession = DBServer[eST.READONLY];
      try
      {
        oSession.Clear();

        Expression<Func<TherapeuticProtocol, bool>> predicate = ExpressionExtension.True<TherapeuticProtocol>();
        predicate = predicate.And(x => x.Published);
        //if (category > 0)
        //    predicate = predicate.And(x => x.Category == category);
        var oProtocols = oSession.QueryDataElement<TherapeuticProtocol>()
                 .Where(predicate)
                 .OrderByDescending(x => x.ID)
                 .Take(count)
                 .ToList()
                 .Also(x => x.Action(y =>
                 {
                   y.BlogContent = y.BlogContent.Substring(0, 100);
                 }));

        return Ok(oProtocols);
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

    // GET: api/protocols/{id}?country={country}
    [HttpGet("{id}")]
    public IActionResult GetProtocol(int id, string Country)
    {
      _logger.LogInformation("GetProtocol, id={0}, country={1}", id, Country);
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

        var oProtocol = oSession.QueryDataElement<TherapeuticProtocol>()
                                .Where(x => x.ID == id && x.Published == true)
                                .ToList().UniqueOrDefault();
        if (oProtocol == null)
          return NotFound();

        oProtocol.RelatedProducts = ProductHelper.GetRelatedsProduct(oProtocol.Tags, site, oSession, 3); ;
        return Ok(oProtocol);
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
