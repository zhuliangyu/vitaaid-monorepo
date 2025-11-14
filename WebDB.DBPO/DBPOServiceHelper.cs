using MyHibernateUtil;
using NHibernate;

namespace WebDB.DBPO
{
  public static class DBPOServiceHelper
  {
    private static ORMServer DBServer;
    public static ORMServer WebDBServer => WebDB.DBPO.DBPOServiceHelper.DBServer;
    //public static ISession WebDBSession => DBServer.oSession;
    public static void setup_WebDBPO_DBServer(ORMServer dbServer) => DBServer = dbServer;
    public static void setup_WebDBPO_DBServer(string sFileName, string FactoryName) => DBServer = new ORMServer().DoDBConnect(sFileName, FactoryName);
  }
}
