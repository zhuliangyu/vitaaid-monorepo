using MyHibernateUtil;
using MyHibernateUtil.Extensions;
using MySystem.Base.Extensions;
using NHibernate;
using POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CIM.DBPO.DBPOServiceHelper;

namespace CIM.DBPO
{
  public static class VirtualLotInfoExtension
  {
    public static string GetNextVirtualLotNo(this VirtualLotInfo self, SessionProxy oCIMSession)
    {
      try
      {
        var FormulationCode = self.FormulationCode;
        var offset = FormulationCode.Length - 6;
        var codePart = (offset == 0) ? FormulationCode : FormulationCode.Substring((offset < 0) ? 0 : offset);
        var ps = oCIMSession.QueryDataElement<Lot>()
                          .Where(x => x.No.StartsWith("V" + codePart))
                          .Select(x => x.No.Substring(7).ToInt())
                          .ToList();
        var nextSearialNo = (ps.ToNullIfEmpty()?.Max() ?? 0) + 1;
        return "V" + codePart + nextSearialNo.ToString();
      }
      catch (Exception ex) { throw ex; }
    }
  }
}
