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
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace vitaaid.com.Controllers
{
  [Route("api/categories")]
  [ApiController]
  public class CategoryController : ControllerBase
  {
    private readonly ILogger<CategoryController> _logger;
    public CategoryController(ILogger<CategoryController> logger)
    {
      _logger = logger;
    }

    // GET: api/categories?type={0}
    // type:
    // 1: SynerPlex Category
    // 2: Vita Aid Category
    // 3: Type of Practice
    // 4: DETOX MEAL RECIPES
    [HttpGet]
    public IActionResult Get(eUNITTYPE type)
    {
      var oSession = DBServer[eST.READONLY];
      try
      {
        oSession.Clear();

        var express = ExpressionExtension.True<UnitType>();
        if (type != 0)
          express = express.And(x => x.uType == type);
        var oCategory = oSession.Query<UnitType>()
                                .Where(express)
                                .OrderBy(x => x.uType)
                                .ThenBy(x => x.ID)
                                .ToList();

        return Ok(oCategory);
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
