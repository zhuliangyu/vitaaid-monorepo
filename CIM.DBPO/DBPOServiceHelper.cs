using MyHibernateUtil;
using NHibernate;

namespace CIM.DBPO
{
  public static class DBPOServiceHelper
  {
    private static ORMServer DBServer;
    //public static ISession CIMDBSession => DBServer.oSession;
    //public static ISession CIMDBROSession => DBServer.oROSession;
    public static void setup_CIMDBPO_DBServer(ORMServer dbServer) => DBServer = dbServer;
  }
}
