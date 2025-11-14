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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace vitaaid.com.Controllers
{
  [Route("api/Bingo")]
  [ApiController]
  public class BingoController : ControllerBase
  {
    // GET: api/Bingo
    [HttpGet]
    public IActionResult Get()
    {
      var oSession = DBServer[eST.READONLY];
      try
      {
        oSession.Clear();
        var questions = oSession.QueryDataElement<Bingo>()
                           .ToList()
                           .Select(x => new { x.ID, x.Sponsor, x.Question, x.ChoiceAry, x.AnswerAry, x.SponsorType }).ToList();
        return Ok(questions);
      }
      catch (Exception ex)
      {
        return BadRequest(ex);
      }
      finally
      {
        oSession.Close();
      }
    }

    // Get: api/Bingo/draw?winners={0}
    [HttpGet("draw")]
    public IActionResult Get(int winners)
    {
      ITransaction xact = null;
      var oSession = DBServer[eST.SESSION0];
      try
      {
        oSession.Clear();
        var totalUsers = oSession.QueryDataElement<CampaignUser>()
                           .Where(x => x.ProgramName == "Clinical Success Summit 2022")
                           .ToList();

        totalUsers.Action(x => x.PrizeItem = "");
        var shuffled = totalUsers.OrderBy(a => Guid.NewGuid()).ToList();
        shuffled.Take(winners).ToList().ForEachWithIndex((x, idx) => x.PrizeItem = (idx + 1).ToString());
        xact = oSession.BeginTransaction();
        totalUsers.Action(x => oSession.SaveObj(x));
        xact.Commit();

        return Ok(totalUsers);
      }
      catch (Exception ex)
      {
        return BadRequest(ex);
      }
      finally
      {
        oSession.Close();
      }
    }

    // PUT: api/Bingo/compaignuser?firstname={0}&lastname={1}&email={2}&programname={3}
    [HttpPut("compaignuser")]
    public IActionResult compaignuser(string firstname, string lastname, string email, string programname)
    {
      ITransaction xact = null;
      var oSession = DBServer[eST.SESSION0];
      try
      {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(firstname) || string.IsNullOrWhiteSpace(lastname))
          return Ok();
        oSession.Clear();

        var oldUser = oSession.QueryDataElement<CampaignUser>()
            .Where(x => x.Email.ToLower().Equals(email.ToLower()))
            .ToList();

        xact = oSession.BeginTransaction();
        if (oldUser.Any())
        {
          oldUser.First().Also(x =>
          {
            x.Email = email;
            x.FirstName = firstname;
            x.LastName = lastname;
            x.ProgramName = programname;

          }).SaveObj(oSession);
        }
        else
        {
          (new CampaignUser()
          {
            Email = email,
            FirstName = firstname,
            LastName = lastname,
            ProgramName = programname
          }).SaveObj(oSession);
        }
        xact.Commit();
        return Ok();
      }
      catch (Exception)
      {
        xact?.Rollback();
        return Ok();
      }
      finally
      {
        oSession.Close();
      }
    }
  }
}
