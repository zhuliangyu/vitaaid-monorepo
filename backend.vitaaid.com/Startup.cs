using Microsoft.Owin;
using MyHibernateUtil;
using Owin;
using System;
using System.IO;
using System.Web.Hosting;
using WebDB.DBPO;
using static MIS.DBPO.DBPOServiceHelper;

[assembly: OwinStartup(typeof(backend.vitaaid.com.Startup))]

// Files related to ASP.NET Identity duplicate the Microsoft ASP.NET Identity file structure and contain initial Microsoft comments.

namespace backend.vitaaid.com
{
  public partial class Startup
  {
    public virtual sysconfig sc { get; set; }
    public virtual string EnvironmentName { get => sc.DefaultDBFactory; }
    public virtual bool bPROD { get => sc.bPROD; }
    public static string ms_ContentRoot = "";
    public void Configuration(IAppBuilder app)
    {
      ms_ContentRoot = AppDomain.CurrentDomain.BaseDirectory;
      NLog.GlobalDiagnosticsContext.Set("AppDirectory", ms_ContentRoot + "\\log");
      var logger = NLog.LogManager.GetCurrentClassLogger();//Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
      logger.Info("Startup - Configuration");

      sc = sysconfig.load(ms_ContentRoot + "sysconfig.xml");
      ECSServerObj.RESTfullObject.setConfig(sc.bPROD, ms_ContentRoot + "ecsopicfg.ini");
      ServicesHelper.DBServer.DoDBConnect(ms_ContentRoot + "webdb.hibernate.cfg.xml", EnvironmentName);
      DBPOServiceHelper.setup_WebDBPO_DBServer(ServicesHelper.DBServer);

      ServicesHelper.VAMISDBServer.DoDBConnect(ms_ContentRoot + "VA.MIS.hibernate.cfg.xml", EnvironmentName);
      setup_MISDBPO_DBServer(ServicesHelper.VAMISDBServer);


      ConfigureAuth(app);
    }
  }
}