using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace vitaaid.com.Middleware
{
  public class ApiKeyMiddleware
  {
    private readonly RequestDelegate _next;
    private const string APIKEYNAME = "ApiKey";
    public ApiKeyMiddleware(RequestDelegate next)
    {
      _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
      if (context.Request.Method != "OPTIONS")
      {
        context.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey);

        var appSettings = context.RequestServices.GetRequiredService<Microsoft.Extensions.Configuration.IConfiguration>();
        var apiKey = appSettings[APIKEYNAME];
        if (string.IsNullOrWhiteSpace(apiKey) == false && apiKey.Equals(extractedApiKey) == false)
        {
          context.Response.StatusCode = 401;
          await context.Response.WriteAsync("Unauthorized client.");
          return;
        }
      }
      await _next(context);
    }
  }
}