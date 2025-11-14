using MyHibernateUtil;
using NHibernate;

namespace backend.vitaaid.com
{
    public static class ServicesHelper
    {
        private static ORMServer oDBServer = new ORMServer();
        public static ORMServer DBServer { get => oDBServer; }
        //public static ISession DBSession { get { return DBServer.oSession; } }
        //public static ISession DBSession1 { get { return DBServer.oSession1; } } // for read only session

    // shopping cart(VA MIS) DB
    private static ORMServer oVAMISDBServer = new ORMServer();
    public static ORMServer VAMISDBServer { get => oVAMISDBServer; }
    public static ORMServer VAMISDBBO = ServicesHelper.VAMISDBServer;
    //public static ISession VAMISSession { get { return VAMISDBServer.oSession; } }
    //public static ISession VAMISSession1 { get { return VAMISDBServer.oSession1; } }
    //public static ISession VAMISROSession { get { return VAMISDBServer.oROSession; } } // for read only session


  }
}
