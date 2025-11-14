using MyHibernateUtil;
using NHibernate;

namespace MIS.DBPO
{
    public static class DBPOServiceHelper
    {
        public static ORMServer DBServer;
        public static ORMServer MISDB => DBPOServiceHelper.DBServer;
        //public static ISession MISDBSession => DBServer.oSession;
        //public static ISession MISDBSession1 => DBServer.oSession1;
        public static void setup_MISDBPO_DBServer(ORMServer dbServer) => DBServer = dbServer;
    }
}
