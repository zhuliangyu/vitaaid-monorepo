using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MIS.DBBO;
using MyHibernateUtil;
using MyHibernateUtil.Extensions;
using MySystem.Base.Extensions;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using vitaaid.com.Jwt;
using static vitaaid.com.ServicesHelper;
namespace vitaaid.com.Controllers
{
  [Route("api/oauth")]
  [ApiController]
  public class oauthController : ControllerBase // for shopping cart oauth
  {
    private readonly ILogger<oauthController> _logger;
    private ITokenManager _tokenManager;
    public oauthController(ITokenManager _tokenManager, ILogger<oauthController> logger)
    {
      _logger = logger;
      this._tokenManager = _tokenManager;
    }

    // POST api/oauth/token
    [HttpPost("token")]
    public IActionResult token([FromForm] string account, [FromForm] string password, [FromForm] string customerCode, [FromForm] string storeName, [FromForm] string appName = "vitaaid.com")
    {
      _logger.LogInformation("login to shopping: Customer Code: {0}", customerCode);
      var oVMSession = VAMISDBServer[eST.SESSION0];
      ITransaction xact = null;
      try
      {
        // STEP0: check username and password
        var versionInfo = Assembly.GetExecutingAssembly().VersionInfo() + "(" + NHibernateHelper.Factories + ")" + "(" + NHibernateHelper.DataSources + ")";
        var oAuth = ECSServerObj.RESTfullObject.authApp(account, password, "H00", versionInfo);

        if (!oAuth.bResult)
        {
          _logger.LogInformation("Unauthorized client: {0}", account);
          return BadRequest("Unauthorized client.");
        }

        DataElement.sDefaultUserID = account;
        oVMSession.Clear();
        var refreshToken = oVMSession.QueryDataElement<ShoppingCartToken>()
                                       .Where(x => x.Account == account && x.CustomerCode == customerCode && x.StoreName == storeName)
                                       .UniqueOrDefault();
        if (refreshToken == null)
        {
          xact = oVMSession.BeginTransaction();
          refreshToken = new ShoppingCartToken { AppName = appName, Account = account, CustomerCode = customerCode, StoreName = storeName, HostName = Request.Host.Host };
          refreshToken.SaveObj(oVMSession);
          xact.Commit();
        }

        var token = _tokenManager.CreateStandard(account, customerCode, refreshToken.ID.ToString(), appName);
        ////JwtToken token = _tokenManager.CreateStandard(email, member.Name, refreshTokenDB.ID.ToString(), "vitaaid.com");
        refreshToken.Expired = token.expires_in;
        refreshToken.RefreshToken = token.refresh_token;

        xact = oVMSession.BeginTransaction();
        oVMSession.SaveObj(refreshToken);
        xact.Commit();

        Response.Cookies.Append("X-Refresh-ShoppingCartToken", token.refresh_token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict, Expires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(token.expires_in)) });

        return Ok(new { token.access_token, token.expires_in });
      }
      catch (Exception ex)
      {
        xact?.Rollback();
        _logger.LogError(ex, "An error occurred.");
        return BadRequest(ex);
      }
      finally
      {
        oVMSession.Close();
      }
    }

    // POST api/oauth/refresh
    [HttpPost("refresh")]
    public IActionResult Refresh([FromForm] string appName = "vitaaid.com")
    {
      var oVMSession = VAMISDBServer[eST.SESSION0];
      try
      {
        if (!Request.Cookies.TryGetValue("X-Refresh-ShoppingCartToken", out var oldToken))
          return BadRequest();


        oVMSession.Clear();
        return oVMSession.QueryDataElement<ShoppingCartToken>()
                 .Where(x => x.RefreshToken == oldToken && x.HostName == Request.Host.Host)
                 .UniqueOrDefault()
                 ?.Let<ShoppingCartToken, IActionResult>(x =>
                {
                  var newToken = _tokenManager.CreateStandard(x.Account, x.CustomerCode, x.ID.ToString(), x.AppName);
                  x.RefreshToken = newToken.refresh_token;

                  ITransaction xact = oVMSession.BeginTransaction();
                  try
                  {
                    x.SaveObj(oVMSession);
                    xact.Commit();

                    Response.Cookies.Append("X-Refresh-ShoppingCartToken", newToken.refresh_token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict, Expires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(newToken.expires_in)) });
                    return Ok(newToken);
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
        _logger.LogError(ex, "An error occurred.");
        return BadRequest(ex);
      }
      finally
      {
        oVMSession.Close();
      }
    }
  }
}
