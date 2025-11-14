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
  [Route("api/StabilityForms")]
  [ApiController]
  public class StabilityFormController : ControllerBase
  {

    private readonly ILogger<StabilityFormController> _logger;
    public StabilityFormController(ILogger<StabilityFormController> logger)
    {
      _logger = logger;
    }

    // GET: api/StabilityForms/{lotNo}?country=
    [HttpGet("{lotNo}")]
    public IActionResult Get(string LotNo, string Country)
    {
      if (string.IsNullOrWhiteSpace(LotNo) || string.IsNullOrWhiteSpace(Country))
        return Ok(null);

      _logger.LogInformation("Get StabilityForm, lot={0}, country={1}", LotNo, Country);
      var oSession = DBServer[eST.READONLY];
      try
      {
        eWEBSITE site = eWEBSITE.CA;
        try
        {
          site = (eWEBSITE)Enum.Parse(typeof(eWEBSITE), Country.ToUpper());
        }
        catch (Exception)
        {
          return Ok(null);
        }
        oSession.Clear();

        IQueryable<StabilityForm> query = oSession.QueryDataElement<StabilityForm>()
                                    .Where(x => x.LotNumber.ToUpper() == LotNo.ToUpper() && x.Published == true);

        char lastChar = LotNo.Last();
        if ((lastChar < '0' || lastChar > '9') && query.ToList().Count == 0)
        {
            // We have to support both LotNo format (LotNo and LotNo2)
            // Only search for LotNo2 only if LotNo can't find any results
            String LotNo2 = LotNo.Substring(0, LotNo.LastIndexOf(x => x >= '0' && x <= '9') + 1);
            query = oSession.QueryDataElement<StabilityForm>()
            .Where(x => x.LotNumber.ToUpper() == LotNo2.ToUpper() && x.Published == true);
        }

        var oSF = query.ToList().UniqueOrDefault();
        oSF?.Also(x =>
        {
          Expression<Func<StabilityTestData, bool>> predicate = y => y.oStabilityForm == x;
          if (site != eWEBSITE.ALL)
          {
            predicate = predicate.And(ExpressionExtension.False<StabilityTestData>()
                                              .Or(x => x.VisibleSite == eWEBSITE.ALL)
                                              .Or(x => x.VisibleSite == site));
          }

          x.oTestData = SortTestData(oSession.QueryDataElement<StabilityTestData>()
                                       .Where(predicate)
                                       .ToList());


          x.oTestData.Action(oTD => oTD.TestLimitInfoFromSpec());
          var imgs = oSession.CreateSQLQuery("SELECT ImageName FROM ProductImage WHERE ProductCode = '" + x.Code + "' AND FrontImage = 1")
                   .List();
          if (imgs.Count > 0)
            x.LProductImg = (string)imgs[0];

          //(string)DBSession.CreateSQLQuery("SELECT size2 FROM Products WHERE ProductCode = '" + x.Code + "'")
          //                                   .UniqueResult();
          if (string.IsNullOrWhiteSpace(x.LProductImg))
            x.LProductImg = "large/EmptyProduct.png";
        });

        return Ok(oSF);
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

    // GET: api/StabilityForms/last?filter={0}&byLotNo={1}&byCode={2}&byName={3}&chkTop={4}&count={5}
    [HttpGet("last")]
    public IActionResult Last(string filter = "", bool byLotNo = true, bool byCode = true, bool byName = true, bool chkTop = true, int count = 0)
    {
      _logger.LogInformation("Last StabilityForm, filter={0}, byLotNo={1}, byCode={2}, byName={3}, chkTop={4}, count={5}", filter, byLotNo, byCode, byName, chkTop, count);
      var oSession = DBServer[eST.READONLY];
      try
      {
        oSession.Clear();

        Expression<Func<StabilityForm, bool>> predicate = ExpressionExtension.True<StabilityForm>();
        string sFilter = filter?.Trim()?.ToLower() ?? "";
        if (sFilter.Length > 0)
        {
          Expression<Func<StabilityForm, bool>> others = ExpressionExtension.False<StabilityForm>();
          if (byLotNo)
            others = others.Or(x => x.LotNumber.ToLower().Contains(sFilter));
          if (byCode)
            others = others.Or(x => x.Code.ToLower().Contains(sFilter));
          if (byName)
            others = others.Or(x => x.Name.ToLower().Contains(sFilter));
          predicate = predicate.And(others);
        }

        IQueryable<StabilityForm> query = oSession.QueryDataElement<StabilityForm>()
                                                   .Where(predicate)
                                                   .OrderByDescending(x => x.ID);
        if (count > 0 && chkTop == true)
          query = query.Take(count);
        var oSFs = query.ToList();
        oSFs.Action(x =>
        {
          x.oTestData = SortTestData(oSession.QueryDataElement<StabilityTestData>()
                                       .Where(y => y.oStabilityForm == x)
                                       .ToList());
        });

        return Ok(oSFs);
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

    // GET: api/StabilityForms/newest
    [HttpGet("newest")]
    public IActionResult Newest()
    {
      _logger.LogInformation("Newest StabilityForm");

      var oSession = DBServer[eST.READONLY];
      try
      {
        oSession.Clear();
        return Ok(oSession.QueryDataElement<StabilityForm>()
                        .Select(x => x.ID)
                        .OrderByDescending(x => x)
                        .Take(1)
                        .ToList()
                        .FirstOrDefault());
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

    // POST api/StabilityForms
    [Authorize]
    [HttpPost]
    public IActionResult Post([FromBody] IList<StabilityForm> oNewerSFs)
    {
      if (oNewerSFs.Count == 0)
        return Ok(0);

      _logger.LogInformation("New {0} StabilityForms", oNewerSFs.Count);
      var oSession = DBServer[eST.SESSION0];
      ITransaction xact = null;
      try
      {
        oSession.Clear();

        try
        {
          xact = oSession.BeginTransaction();

          oNewerSFs.Action(x =>
          {
            x.oTestData.Action(y => y.Also(t => t.oStabilityForm = x).SaveObj(oSession));
            x.PreAdd();
            oSession.Save(x);
          });
          xact.Commit();
          return Ok(oNewerSFs.Count());
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
      catch (Exception)
      {

        throw;
      }
    }
    // PUT api/StabilityForms/{id}
    [Authorize]
    [HttpPut("{id}")]
    public IActionResult Put(long id, [FromBody] StabilityForm oDirtySF)
    {
      _logger.LogInformation("Update StabilityForm, id={0}, lot={1}", id, oDirtySF.LotNumber);
      var oSession = DBServer[eST.SESSION0];
      try
      {
        oSession.Clear();
        ITransaction xact = null;
        try
        {
          xact = oSession.BeginTransaction();
          oDirtySF.oTestData.Action(x =>
          {
            if (x.iState == MySystem.Base.eOPSTATE.INIT)
              return;
            x.oStabilityForm = oDirtySF;
            if (x.iState == MySystem.Base.eOPSTATE.DELETE)
              x.DeleteObj(oSession);
            else
              x.SaveObj(oSession);
          });
          oDirtySF.SaveObj(oSession);
          xact.Commit();
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
      catch (Exception)
      {

        throw;
      }
    }

    public static IList<StabilityTestData> SortTestData(IList<StabilityTestData> TestDataList)
    {

      var PhysicalTDs = new List<StabilityTestData>();
      var PerformanceTDs = new List<StabilityTestData>();
      var ChemicalTDs = new List<StabilityTestData>();
      var MicrobiologicalTDs = new List<StabilityTestData>();
      var MiscTDs = new List<StabilityTestData>();
      var CriticalTDs = new List<StabilityTestData>();
      var otherTDs = new List<StabilityTestData>();
      var sortedList = new List<StabilityTestData>();
      TestDataList.Action(x =>
      {
        if (x.GroupName.StartsWith("Physical"))
          PhysicalTDs.Add(x);
        else if (x.GroupName.StartsWith("Performance"))
          PerformanceTDs.Add(x);
        else if (x.GroupName.StartsWith("Chemical"))
          ChemicalTDs.Add(x);
        else if (x.GroupName.StartsWith("Microbiological"))
          MicrobiologicalTDs.Add(x);
        else if (x.GroupName.StartsWith("Critical"))
          CriticalTDs.Add(x);
        else if (x.GroupName.StartsWith("Misc"))
          MiscTDs.Add(x);
        else
          otherTDs.Add(x);
      });
      sortedList.AddRange(PhysicalTDs.OrderBy(x => x.ID));
      sortedList.AddRange(PerformanceTDs.OrderBy(x => x.ID));
      sortedList.AddRange(ChemicalTDs.OrderBy(x => x.ID));
      sortedList.AddRange(MicrobiologicalTDs.OrderBy(x => x.ID));
      sortedList.AddRange(otherTDs.OrderBy(x => x.GroupName).ThenBy(x => x.ID));
      sortedList.AddRange(MiscTDs.OrderBy(x => x.ID));
      sortedList.AddRange(CriticalTDs);
      return sortedList;
    }
    //// GET api/<StabilityFormController>/5
    //[HttpGet("{id}")]
    //public string Get(int id)
    //{
    //    return "value";
    //}

    //// POST api/<StabilityFormController>
    //[HttpPost]
    //public void Post([FromBody] string value)
    //{
    //}

    //// PUT api/<StabilityFormController>/5
    //[HttpPut("{id}")]
    //public void Put(int id, [FromBody] string value)
    //{
    //}

    //// DELETE api/<StabilityFormController>/5
    //[HttpDelete("{id}")]
    //public void Delete(int id)
    //{
    //}
  }
}
