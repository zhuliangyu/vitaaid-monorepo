using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MyHibernateUtil;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using vitaaid.com;
using vitaaid.com.Jwt;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using static WebDB.DBPO.DBPOServiceHelper;
using static MIS.DBPO.DBPOServiceHelper;
using vitaaid.com.Middleware;
using static CIM.DBPO.DBPOServiceHelper;
using System.IO;

namespace vitaaid.com
{
  public class Startup
  {
    public virtual sysconfig sc { get; set; }
    public virtual string EnvironmentName { get => sc.DefaultDBFactory; }
    public virtual bool bPROD { get => sc.bPROD; }
    public static string ms_ContentRoot = "";
    public static string ms_MessageLogFolder = "";
    public static string ms_MessageFileName = "";
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
      ms_ContentRoot = configuration.GetValue<string>(WebHostDefaults.ContentRootKey) + "\\";
      //ms_MessageLogFolder = ms_ContentRoot + "log\\";
      //if (!Directory.Exists(ms_MessageLogFolder))
      //  Directory.CreateDirectory(ms_MessageLogFolder);
      //lock (ms_MessageFileName)
      //{
      //  messageFileName = string.Format("{0}{1}.txt", MessageLogFolder, DateTime.Now.ToString("yyyyMMdd"));
      //  using (StreamWriter sw = (File.Exists(messageFileName)) ? new StreamWriter(messageFileName, true) : File.CreateText(messageFileName))
      //  {
      //    HttpRequestMessageProperty httpReq = (HttpRequestMessageProperty)request.Properties[HttpRequestMessageProperty.Name];

      //    OperationContext context = OperationContext.Current;
      //    MessageProperties properties = context.IncomingMessageProperties;
      //    RemoteEndpointMessageProperty endpoint = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;

      //    sw.WriteLine("{0} {1,-15} {2,-6} : {3}",
      //        DateTime.Now.ToString("HH:mm:ss"), "IN  " + endpoint.Address, auth,
      //        requestUri.AbsoluteUri.Substring(requestUri.AbsoluteUri.ToString().IndexOf("MESServiceImpl.svc") + "MESServiceImpl.svc".Length + 1));
      //    if (!request.IsEmpty)
      //    {
      //      sw.WriteLine("{0,33} : {1}", " ", this.MessageToString(ref request));
      //    }
      //  }
      //}
    }
    //public static void WriteLog(string log)
    //{
    //  lock (ms_MessageFileName)
    //  {
    //    ms_MessageFileName = string.Format("{0}{1}.txt", ms_MessageLogFolder, DateTime.Now.ToString("yyyyMMdd"));
    //    using (StreamWriter sw = (File.Exists(ms_MessageFileName)) ? new StreamWriter(ms_MessageFileName, true) : File.CreateText(ms_MessageFileName))
    //    {
    //      sw.WriteLine("{0} {1}", DateTime.Now.ToString("HH:mm:ss"), log);
    //    }
    //  }
    //}

    public IConfiguration Configuration { get; set; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {

      sc = sysconfig.load(ms_ContentRoot + "sysconfig.xml");
      ECSServerObj.RESTfullObject.setConfig(sc.bPROD, ms_ContentRoot + "va.mis.ecsopicfg.ini");
      
      ServicesHelper.DBServer.DoDBConnect(ms_ContentRoot + "webdb.hibernate.cfg.xml", EnvironmentName);
      setup_WebDBPO_DBServer(ServicesHelper.DBServer);
      
      ServicesHelper.VAMISDBServer.DoDBConnect(ms_ContentRoot + "VA.MIS.hibernate.cfg.xml", EnvironmentName);
      setup_MISDBPO_DBServer(ServicesHelper.VAMISDBServer);

      ServicesHelper.CIMDBServer.DoDBConnect(ms_ContentRoot + "cim.hibernate.cfg.xml", EnvironmentName, true);
      setup_CIMDBPO_DBServer(ServicesHelper.CIMDBServer);

      services.AddOptions();

      services.AddControllers()
              .AddNewtonsoftJson(options =>
              {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
              });

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
      });

      // In production, the React files will be served from this directory
      services.AddSpaStaticFiles(configuration =>
      {
        configuration.RootPath = "ClientApp/build";
      });

      // get JWT config from appsettings.json
      var jwtOptions = Configuration.GetSection(nameof(JwtOptions));
      // setup JwtIssuerOptions
      services.Configure<JwtOptions>(options =>
      {
        options.Issuer = jwtOptions[nameof(JwtOptions.Issuer)];
        options.Audience = jwtOptions[nameof(JwtOptions.Audience)];
        options.ValidFor = TimeSpan.FromMinutes(Int32.Parse(jwtOptions[nameof(JwtOptions.ValidFor)]));//TimeSpan.FromMinutes(ECSServerObj.RESTfullObject.ms_opiCfg?.SessionTimeout ?? 60);
              options.SecretKey = jwtOptions[nameof(JwtOptions.SecretKey)];
              // get SymmetricSecurityKey 
              options.SigningCredentials = new SigningCredentials(
                  new SymmetricSecurityKey(Encoding.ASCII.GetBytes(options.SecretKey)), SecurityAlgorithms.HmacSha256);
      });

      // STEP1: setup Authentication method to verify if HTTP Requests are valid
      services
          // check Authorization of HTTP Header if having valid JWT Bearer Token
          .AddAuthentication(options =>
          {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
          })
          // set check items of JWT Bearer Token
          .AddJwtBearer(options =>
          {
            options.TokenValidationParameters = new TokenValidationParameters
            {
              ValidateIssuer = true,
              ValidIssuer = jwtOptions[nameof(JwtOptions.Issuer)],

              ValidateAudience = true,
              ValidAudience = jwtOptions[nameof(JwtOptions.Audience)],

              ValidateIssuerSigningKey = true,
              IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions[nameof(JwtOptions.SecretKey)])),

              RequireExpirationTime = true,
              ValidateLifetime = true,

              ClockSkew = TimeSpan.Zero
            };
          });

      //Adds services required for using options.
      services.AddSingleton(typeof(ITokenManager), typeof(TokenManager));
      services.AddHealthChecks();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
          c.SwaggerEndpoint("v1/swagger.json", "My API V1");
        });
      }
      else
      {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"), appBuilder =>
      {
        appBuilder.UseMiddleware<ApiKeyMiddleware>()
                        .UseMiddleware<UserInfoMiddleware>();
      });

      app.UseAuthentication();

      app.UseHttpsRedirection();
      app.UseStaticFiles();
      app.UseSpaStaticFiles();

      app.UseRouting();
      app.UseAuthorization();

      app.UseHealthChecks("/health", new HealthCheckOptions { ResponseWriter = JsonResponseWriter });

      app.UseEndpoints(endpoints =>
      {
              //endpoints.MapControllerRoute(
              //    name: "default",
              //    pattern: "{controller}/{action=Index}/{id?}");
              endpoints.MapControllers();
      });

      app.UseSpa(spa =>
      {
        spa.Options.SourcePath = "ClientApp";

        if (env.IsDevelopment())
        {
          spa.UseReactDevelopmentServer(npmScript: "start");
        }
      });
    }
    private async Task JsonResponseWriter(HttpContext context, HealthReport report)
    {
      context.Response.ContentType = "application/json";
      await JsonSerializer.SerializeAsync(context.Response.Body, new { Status = report.Status.ToString() },
          new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
    }
  }
}
