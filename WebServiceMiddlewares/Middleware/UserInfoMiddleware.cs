using Microsoft.AspNetCore.Http;
using MyHibernateUtil;
using MySystem.Base.Extensions;
using System.Linq;
using System.Threading.Tasks;
using vitaaid.com.Jwt;

namespace vitaaid.com.Middleware
{
  public class UserInfoMiddleware
    {
        public static string HEADER_VITAAID_USER = "Vitaaid-User";
        private readonly RequestDelegate _next;
        private ITokenManager _tokenManager;
        public UserInfoMiddleware(RequestDelegate next, ITokenManager _tokenManager)
        {
            _next = next;
            this._tokenManager = _tokenManager;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var userInfo = context.Request.Headers.Where(x => x.Key == HEADER_VITAAID_USER).FirstOrDefault().Value;
            if (string.IsNullOrWhiteSpace(userInfo))
                _tokenManager.GetToken(context)?.Let((token) => DataElement.sDefaultUserID = (string)token.Payload["name"]);
            else
                DataElement.sDefaultUserID = userInfo[0];
            await _next(context);
        }
    }
}