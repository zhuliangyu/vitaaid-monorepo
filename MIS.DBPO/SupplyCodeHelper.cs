using MIS.DBBO;
using MyHibernateUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIS.DBPO
{
  public static class SupplyCodeHelper
  {
    public static string getNextSupplyCode(SessionProxy oMISSession, string sFabLotNo, string prefixCode)
    {
      try
      {
        int iStartIdx = StartIdxForSupplyCode(oMISSession, sFabLotNo);
        return getNextSupplyCode(oMISSession, prefixCode, ref iStartIdx);
      }
      catch (Exception)
      {
        throw;
      }
    }
    public static string getNextSupplyCode(SessionProxy oMISSession, string prefixCode)
    {
      if (string.IsNullOrEmpty(prefixCode))
        throw new Exception("prefix code can not be empty.");
      var iStartIdx = oMISSession.QueryDataElement<SupplyCode_v>().Where(x => x.SupplyCode.StartsWith(prefixCode)).ToList().Count();
      return getNextSupplyCode(oMISSession, prefixCode, ref iStartIdx);
    }
    public static int StartIdxForSupplyCode(SessionProxy oMISSession, string sFabLotNo)
    {
      try
      {
        if (sFabLotNo == null || sFabLotNo.Length == 0) return 100;
        IList<VitaAidFinishProduct> oFPs = oMISSession.QueryDataElement<VitaAidFinishProduct>().Where(x => x.FabLotNo == sFabLotNo).ToList();
        IList<VitaAidSemiProduct> oSPs = oMISSession.QueryDataElement<VitaAidSemiProduct>().Where(x => x.FabLotNo == sFabLotNo).ToList();
        return oFPs.Count() + oSPs.Count();
      }
      catch (Exception)
      {
        return 100;
      }
    }
    public static string getNextSupplyCode(SessionProxy oMISSession, string prefixCode, ref int iStartIdx)
    {
      try
      {
        string sSupplyCode = prefixCode + ((iStartIdx == 0) ? "" : "-" + iStartIdx.ToString());
        while (true)
        {
          if (oMISSession.QueryDataElement<SupplyCode_v>().Where(x => x.SupplyCode == sSupplyCode).ToList().Count() == 0) //IsValidSupplyCode(sSupplyCode))
            break;
          iStartIdx++;
          sSupplyCode = prefixCode + "-" + iStartIdx.ToString();
        }
        return sSupplyCode;
      }
      catch (Exception)
      {

        throw;
      }
    }
  }
}
