using MyHibernateUtil;
using NHibernate;

namespace vitaaid.com
{
  public static class ServicesHelper
	{
    // WebDB
		private static ORMServer oDBServer = new ORMServer();
		public static ORMServer DBServer { get => oDBServer; }
    public static ORMServer WebDBServer { get => oDBServer; }

    // shopping cart DB: VA.MIS
    // VAMISDBServer 连接的数据库是由 VA.MIS.hibernate.cfg.xml 文件定义的数据库
    private static ORMServer oVAMISDBServer = new ORMServer();
		public static ORMServer VAMISDBServer { get => oVAMISDBServer; }
    public static ORMServer VAMISDBBO = ServicesHelper.VAMISDBServer;

    // CIM DB
    private static ORMServer oCIMDBServer = new ORMServer();
		public static ORMServer CIMDBServer { get => oCIMDBServer; }
    public static ORMServer CIMDBBO = ServicesHelper.CIMDBServer;
  }
}
