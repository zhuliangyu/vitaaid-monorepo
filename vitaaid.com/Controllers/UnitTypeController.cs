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
  [Route("api/UnitTypes")]
  [ApiController]
  public class UnitTypeController : ControllerBase
  {
    private readonly ILogger<UnitTypeController> _logger;
    public UnitTypeController(ILogger<UnitTypeController> logger)
    {
      _logger = logger;
    }

    // GET: api/UnitTypes/{type}
    [HttpGet("{type}")]
    public IActionResult Get(eUNITTYPE type)
    {
      var oSession = DBServer[eST.SESSION0];
      try
      {
        oSession.Clear();

        return Ok(oSession.QueryDataElement<UnitType>().Where(x => x.uType == type).OrderBy(x => x.ID).ToList());
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

    // GET: api/UnitTypes/{type}/{name}
    [HttpGet("{type}/{name}")]
    public IActionResult getUnitType(eUNITTYPE type, string name)
    {
      var oSession = DBServer[eST.SESSION0];
      try
      {
        oSession.Clear();

        return Ok(oSession.QueryDataElement<UnitType>().Where(x => x.uType == type && x.Name == name).UniqueOrDefault());
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
