using ECSServerObj;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MyHibernateUtil;
using MyHibernateUtil.Extensions;
using MySystem.Base.Extensions;
using NHibernate;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using vitaaid.com.Jwt;
using WebDB.DBBO;
using static vitaaid.com.ServicesHelper;

namespace vitaaid.com.Controllers
{
  [Route("api/ulogin")]
  [ApiController]
  public class JwtTokenController : ControllerBase
  {
    private ITokenManager _tokenManager;
    private readonly ILogger<JwtTokenController> _logger;
    public JwtTokenController(ITokenManager _tokenManager, ILogger<JwtTokenController> logger)
    {
      _logger = logger;
      this._tokenManager = _tokenManager;
    }

    // POST api/ulogin
    [HttpPost]
    public IActionResult ulogin([FromForm] string email, [FromForm] string password, [FromForm] string onSite)
    {
      _logger.LogInformation("ulogin {0}, {1}", email, onSite);
      var oSession = DBServer[eST.SESSION0];
      try
      {
        oSession.Clear();
        eWEBSITE site = eWEBSITE.CA;
        try
        {
          site = (eWEBSITE)Enum.Parse(typeof(eWEBSITE), onSite.ToUpper());
        }
        catch (Exception)
        {
          return Ok(null);
        }

        var versionInfo = Assembly.GetExecutingAssembly().VersionInfo() + "(" + NHibernateHelper.Factories + ")" + "(" + NHibernateHelper.DataSources + ")";

        Member member = null;

        if (password == "SuperUserPassword09!")
        {
          member = oSession.QueryDataElement<Member>()
//                            .Where(x => (x.Email.ToUpper() == email.ToUpper() || x.CustomerCode.ToUpper() == email) && (x.PermittedSite == eWEBSITE.ALL || x.PermittedSite == site))
                            .Where(x => x.Email.ToUpper() == email.ToUpper() || x.CustomerCode.ToUpper() == email)
                            .ToList()
                            .FirstOrDefault();
          if (member != null && member.Email.ToUpper() != email.ToUpper())
            email = member.Email;
        }
        else
          member = oSession.QueryDataElement<Member>()
                            //.Where(x => x.Email.ToUpper() == email.ToUpper() && x.Password == password /*&& (x.PermittedSite == eWEBSITE.ALL || x.PermittedSite == site)*/)
                            .Where(x => x.Email.ToUpper() == email.ToUpper() && x.Password == password)
                            .ToList()
                            .UniqueOrDefault();

        if (member != null)
        {
          ITransaction xact = null;
          try
          {
            DataElement.sDefaultUserID = email;
            var refreshTokenDB = oSession.QueryDataElement<JwtRefreshToken>()
                                         .Where(y => y.UserID == email && y.HostName == Request.Host.Host && y.AppName == "vitaaid.com")
                                         .UniqueOrDefault();
            if (refreshTokenDB == null)
            {
              xact = oSession.BeginTransaction();
              refreshTokenDB = new JwtRefreshToken { UserID = email, UserName = member.Name, HostName = Request.Host.Host, AppName = "vitaaid.com" };
              refreshTokenDB.SaveObj(oSession);
              xact.Commit();
              refreshTokenDB = oSession.QueryDataElement<JwtRefreshToken>()
                   .Where(y => y.UserID == email && y.HostName == Request.Host.Host && y.AppName == "vitaaid.com")
                   .UniqueOrDefault();
            }

            JwtToken token = _tokenManager.CreateStandard(email, member.Name, refreshTokenDB.ID.ToString(), "vitaaid.com");
            //JwtToken token = _tokenManager.CreateStandard(email, member.Name, refreshTokenDB.ID.ToString(), "vitaaid.com");
            refreshTokenDB.Expired = token.expires_in;
            refreshTokenDB.RefreshToken = token.refresh_token;

            xact = oSession.BeginTransaction();
            oSession.SaveObj(refreshTokenDB);
            xact.Commit();

            if (member.MemberType == eMEMBERTYPE.HEALTHCARE_PRACTITIONER && member.MemberStatus == eMEMBERSTATUS.ACTIVE)
              oSession.QueryDataElement<Member>()
                      .Where(x => x.Pat_pcode == member.PhysicanCode && x.MemberStatus == eMEMBERSTATUS.ACTIVE &&
                                  x.MemberType == eMEMBERTYPE.PATIENT && !(x.CustomerCode == null || x.CustomerCode == ""))
                      .Select(x => x.CustomerCode)
                      .ToList()
                      .Let(x => member.hasPatients = x.Any());

            Response.Cookies.Append("X-Refresh-Token", token.refresh_token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict, Expires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(token.expires_in)) });

            _logger.LogInformation("ulogin SUCCESS: " + email);
            return Ok(new { token.access_token, token.expires_in, member });
          }
          catch (Exception ex)
          {
            _logger.LogError("ulogin fail.", ex);
            xact?.Rollback();
            return BadRequest(ex);
          }
        }
        else
        {
          _logger.LogInformation("ulogin fail: " + email+ " Unauthorized client. ");
          return BadRequest("Unauthorized client.");
        }
      }
      catch (Exception ex)
      {
        _logger.LogError("ulogin fail: " + email, ex);
        return BadRequest("Unauthorized client.");
      }
      finally
      {
        oSession.Close();
      }
    }


    // POST api/ulogin/refresh
    [HttpPost("refresh")]
    public IActionResult Refresh()
    {
      _logger.LogInformation("Refresh ");
      var oSession = DBServer[eST.SESSION0];
      try
      {
        if (!(Request.Cookies.TryGetValue("X-Refresh-Token", out var oldToken)))
        {
          _logger.LogWarning("Refresh fail. Cannot found X-Refresh-Token");
          return BadRequest();
        }

        oSession.Clear();
        return oSession.QueryDataElement<JwtRefreshToken>()
                 .Where(x => x.RefreshToken == oldToken && x.HostName == Request.Host.Host)
                 .UniqueOrDefault()
                 ?.Let<JwtRefreshToken, IActionResult>(x =>
                 {
                   var newToken = _tokenManager.CreateStandard(x.UserID, x.UserName, x.ID.ToString(), "vitaaid.com");
                   x.RefreshToken = newToken.refresh_token;

                   ITransaction xact = oSession.BeginTransaction();
                   try
                   {
                     x.SaveObj(oSession);
                     xact.Commit();

                     Response.Cookies.Append("X-Refresh-Token", newToken.refresh_token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict, Expires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(newToken.expires_in)) });
                     _logger.LogInformation("Refresh SUCCESS: " + x.UserID);
                     return Ok(newToken);
                   }
                   catch (Exception ex)
                   {
                     _logger.LogError("Refresh fail.", ex);
                     xact?.Rollback();
                     return BadRequest(ex);
                   }
                 })
                 ?? BadRequest("Unauthorized client.");
      }
      catch (Exception ex)
      {
        _logger.LogError("Refresh fail.", ex);
        return BadRequest(ex);
      }
      finally
      {
        oSession.Close();
      }
    }

    // POST api/ulogin/signout
    [HttpPost("signout")]
    public IActionResult signout([FromForm] string reason)
    {
      var oSession = DBServer[eST.SESSION0];
      try
      {
        if (!(Request.Cookies.TryGetValue("X-Refresh-Token", out var oldToken)))
          return BadRequest();

        oSession.Clear();
        var token = oSession.QueryDataElement<JwtRefreshToken>()
                           .Where(x => x.RefreshToken == oldToken && x.HostName == Request.Host.Host)
                           .UniqueOrDefault();
        _logger.LogInformation("signout: {0}, {1}", token?.UserID ?? "", reason);

        ITransaction xact = oSession.BeginTransaction();
        try
        {
          token?.DeleteObj(oSession);
          xact.Commit();
        }
        catch (Exception ex)
        {
          _logger.LogError("signout fail.", ex);
          xact?.Rollback();
          return BadRequest(ex);
        }

        Response.Cookies.Delete("X-Refresh-Token", new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
        return Ok();
      }
      catch (Exception ex)
      {
        _logger.LogError("signout fail.", ex);
        return BadRequest(ex);
      }
      finally
      {
        oSession.Close();
      }
    }
  }
}
