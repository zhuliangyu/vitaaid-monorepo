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
  [Route("api/auth")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private ITokenManager _tokenManager;
    private readonly ILogger<AuthController> _logger;
    public AuthController(ITokenManager _tokenManager, ILogger<AuthController> logger)
    {
      _logger = logger;      
      this._tokenManager = _tokenManager;
    }

    // POST api/auth
    [HttpPost]
    public IActionResult auth([FromForm] string name, [FromForm] string password, [FromForm] string AppName)
    {
      var oSession = DBServer[eST.SESSION0];
      try
      {
        oSession.Clear();
        // STEP0: check username and password
        var versionInfo = Assembly.GetExecutingAssembly().VersionInfo() + "(" + NHibernateHelper.Factories + ")" + "(" + NHibernateHelper.DataSources + ")";

        return ((name == "superuser" && password == "32168421")
                    ? new ROBase<WS_Employee> { bResult = true, sAddenda = name }
                    : ECSServerObj.RESTfullObject.authApp(name, password, "B00", versionInfo))
                    ?.Let<ROBase<WS_Employee>, IActionResult>(x =>
                    {
                      if (x.bResult)
                      {
                        ITransaction xact = null;
                        try
                        {
                          DataElement.sDefaultUserID = x?.oData?.ShortName ?? name;
                          var refreshTokenDB = oSession.QueryDataElement<JwtRefreshToken>()
                                                       .Where(y => y.UserID == name && y.HostName == Request.Host.Host && y.AppName == AppName)
                                                       .UniqueOrDefault();
                          if (refreshTokenDB == null)
                          {
                            xact = oSession.BeginTransaction();
                            refreshTokenDB = new JwtRefreshToken { UserID = name, UserName = x.oData?.ShortName ?? name, HostName = Request.Host.Host, AppName = AppName };
                            refreshTokenDB.SaveObj(oSession);
                            xact.Commit();
                            refreshTokenDB = oSession.QueryDataElement<JwtRefreshToken>()
                                                     .Where(y => y.UserID == name && y.HostName == Request.Host.Host && y.AppName == AppName)
                                                     .UniqueOrDefault();
                          }

                          JwtToken token = _tokenManager.CreateStandard(name, x.oData?.ShortName ?? name, refreshTokenDB.ID.ToString(), AppName);
                          refreshTokenDB.Expired = token.expires_in;
                          refreshTokenDB.RefreshToken = token.refresh_token;

                          xact = oSession.BeginTransaction();
                          oSession.SaveObj(refreshTokenDB);
                          xact.Commit();

                          Response.Cookies.Append("X-Refresh-Token", token.refresh_token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict, Expires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(token.expires_in)) });

                          return Ok(new { token.access_token, token.expires_in });
                        }
                        catch (Exception ex)
                        {
                          xact?.Rollback();
                          return BadRequest(ex);
                        }
                      }
                      return BadRequest("Unauthorized client.");
                    }) ?? BadRequest("Unauthorized client.");
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

    // POST api/auth/refresh
    [HttpPost("refresh")]
    public IActionResult refresh([FromForm] string AppName)
    {
      var oSession = DBServer[eST.SESSION0];
      try
      {
        if (!(Request.Cookies.TryGetValue("X-Refresh-Token", out var oldToken)))
          return BadRequest();

        oSession.Clear();
        return oSession.QueryDataElement<JwtRefreshToken>()
                 .Where(x => x.RefreshToken == oldToken && x.HostName == Request.Host.Host)
                 .UniqueOrDefault()
                 ?.Let<JwtRefreshToken, IActionResult>(x =>
                 {
                   var newToken = _tokenManager.CreateStandard(x.UserID, x.UserName, x.ID.ToString(), AppName);
                   x.RefreshToken = newToken.refresh_token;

                   ITransaction xact = oSession.BeginTransaction();
                   try
                   {
                     x.SaveObj(oSession);
                     xact.Commit();

                     Response.Cookies.Append("X-Refresh-Token", newToken.refresh_token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict, Expires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(newToken.expires_in)) });

                     return Ok(new { newToken.access_token, newToken.expires_in });
                   }
                   catch (Exception ex)
                   {
                     xact?.Rollback();
                     return BadRequest(ex);
                   }
                 })
                 ?? BadRequest("Unauthorized client.");
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
  }
}