using MyHibernateUtil;
using static MIS.DBPO.DBPOServiceHelper;

namespace MIS.DBPO
{
    public static class OrderHelper
    {
		//public static bool bDuplicatePONo(string PONo, int excludeOrderID = 0) => bDuplicatePONo(PONo, MISDB[eST.SESSION0], excludeOrderID);
		public static bool bDuplicatePONo(string PONo, SessionProxy oSession, int excludeOrderID = 0)
		{
			string sql = "SELECT ID FROM VAOrder WHERE ID <> " + excludeOrderID.ToString() + " AND PONo = '" + PONo + "'";
			return oSession.CreateSQLQuery(sql).List().Count != 0;

		}

	}
}
