using POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CIM.DBPO.DBPOServiceHelper;
using MyHibernateUtil.Extensions;
using MyHibernateUtil;

namespace CIM.DBPO
{
  public class RecentValueHelper
  {
    public static RecentValue updateRecentVal(SessionProxy oCIMSession, IList<RecentValue> rvList, string tag, string sVal)
    {
      try
      {
        if (string.IsNullOrWhiteSpace(sVal))
          return null;
        var tmpRV = new RecentValue(tag, sVal);
        if (!rvList.Contains(tmpRV))
        {
          oCIMSession.SaveObj(tmpRV);
          rvList.Add(tmpRV);
        }
        return tmpRV;
      }
      catch (Exception ex) { throw ex; }
    }
  }
}