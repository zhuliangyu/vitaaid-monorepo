using MyHibernateUtil;
using MyHibernateUtil.Extensions;
using MySystem.Base.Extensions;
using POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CIM.DBPO.DBPOServiceHelper;

namespace CIM.DBPO
{
  public static class ProductionHelper
  {
    public static string MakeMESProductCode(string sFormulationCode, string sLabelCode)

    {
      try
      {
        if (sFormulationCode == null || sFormulationCode.Length == 0 || sLabelCode == null || sLabelCode.Length == 0)
          return "";
        if (sFormulationCode.StartsWith("VA-") || sFormulationCode.StartsWith("SP-"))
        {
          string sProductCategory = "VA";
          string sProductSerial = "001";
          if (sFormulationCode.StartsWith("SP-"))
            sProductCategory = "SP";
          sProductSerial = sLabelCode.Substring(3, 3);
          return sProductCategory + "-" + sProductSerial;
        }
        else
          return sFormulationCode;
      }
      catch (Exception)
      {
        return "NA";
      }
    }
    public static List<FormulationItem> FlatFormulationItems(string formulationCode, SessionProxy oCIMSession)
    {
      try
      {
        var oItems = DoFlatFormulationItems(formulationCode, oCIMSession);
        oItems.Where(x => x.GroupNo != 5)
              .ForEachWithIndex((x, idx) => x.Sequence = idx + 1);
        oItems.Where(x => x.GroupNo == 5)
              .Action(x => x.Sequence = 999);
        return oItems.OrderBy(x => x.Sequence).ToList();
      }
      catch (Exception)
      {
        throw;
      }
    }
    private static List<FormulationItem> DoFlatFormulationItems(string formulationCode, SessionProxy oCIMSession)
    {
      try
      {
        var oItems = oCIMSession.QueryDataElement<POCO.Production>()
                                        .Where(x => x.isActive && x.Code == formulationCode)
                                        .ToList()
                                        .UniqueOrDefault()
                                        ?.FormulationObj?.Items ?? null;
        if (oItems.IsNullOrEmpty())
          return new List<FormulationItem>();

        var flatItems = new List<FormulationItem>();
        oItems.OrderBy(x => x.Sequence).Action(x =>
        {
          if (x.RawMaterial.CategoryCode.StartsWith("RBL"))
          {
            var childItems = FlatFormulationItems(x.RawMaterial.CategoryCode, oCIMSession);
            if (childItems.IsNullOrEmpty())
              flatItems.Add(x);
            else
              childItems.Action(ci => flatItems.Add(ci));
          }
          else
            flatItems.Add(x);
        });
        return flatItems;
      }
      catch (Exception)
      {
        throw;
      }
    }

  }
}
