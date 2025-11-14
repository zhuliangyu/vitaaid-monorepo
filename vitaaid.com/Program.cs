using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using NLog;
using System;
using System.IO;

namespace vitaaid.com
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var currDir = Directory.GetCurrentDirectory();
      NLog.GlobalDiagnosticsContext.Set("AppDirectory", currDir + "\\log");
      var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
      //var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
      try
      {
        logger.Error("init main function");
        CreateHostBuilder(args).Build().Run();
      }
      catch (Exception ex)
      {
        logger.Error(ex, "Error in init");
        throw;
      }
      finally
      {
        NLog.LogManager.Shutdown();
      }
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {


      return Host.CreateDefaultBuilder(args)
                 .ConfigureWebHostDefaults(webBuilder =>
                 {
                   webBuilder.UseWebRoot("ClientApp/build")
                             .UseStartup<Startup>()
                             .ConfigureLogging(logging =>
                                               {
                                                 logging.ClearProviders();
                                                 logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                                               })
                             .UseNLog();
                 });
    }
  }
}
