using MyHibernateUtil;
using POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CIM.DBPO.DBPOServiceHelper;
using MyHibernateUtil.Extensions;
using NHibernate;

namespace CIM.DBPO
{
  public static class LotExtension
  {
    public static IList<FabPackageReq> createFabPackageReq(this Lot source, SessionProxy oCIMSession)
    {
      return oCIMSession.QueryDataElement<ProductPackageReq>()
                  .Where(x => x.oProduct.ID == source.ModelingProduction.ID)
                  .OrderBy(x => x.PackageType)
                  .ToList()
                  .Select(x => new FabPackageReq
                  {
                    GroupNo = x.GroupNo,
                    oLot = source,
                    oPackageSpec = x.oPackageSpec,
                    SpecificSupplier = x.SpecificSupplier,
                    UpdatedDate = DateTime.Now,
                    UpdatedID = DataElement.sDefaultUserID,
                    CreatedDate = DateTime.Now,
                    CreatedID = DataElement.sDefaultUserID
                  }).ToList();
    }
    public static void loadFabPackageReq(this Lot source, SessionProxy oCIMSession)
    {
      source.oPackageReqs = oCIMSession.QueryDataElement<FabPackageReq>()
                                       .Where(x => x.oLot == source)
                                       .OrderBy(x => x.PackageType)
                                       .ToList();
    }
    public static string supplyCodeBase(this Lot self) => (self?.FabProduction?.Code ?? "") + "-" + (self?.sVersion ?? "");
    public static string MakeProductCode(this Lot self, string sLabelCode)
    {
      try
      {
        if (self == null || sLabelCode == null || sLabelCode.Length == 0)
          return "";

        // check if it is sample product
        if (sLabelCode.Substring(1, 1).Equals("S") || sLabelCode.Substring(1, 1).Equals("F"))
        {
          if (self.FabProduction.Code.StartsWith("VA-") || self.FabProduction.Code.StartsWith("SP-"))
          {
            string sProductCategory = "VA";
            string sProductSerial = "001";
            if (self.FabProduction.Code.StartsWith("SP-"))
              sProductCategory = "SP";
            sProductSerial = sLabelCode.Substring(3, 3);
            return "X" + sProductCategory + sProductSerial + "S";
          }
          else
            return "X" + self.FabProduction.Code + "S";
        }
        else
          return MakeMESProductCode(self, sLabelCode);
      }
      catch (Exception)
      {
        return "XS";
      }
    }
    public static string MakeMESProductCode(this Lot self, string sLabelCode)
    {
      try
      {
        if (self == null)
          return "";
        return ProductionHelper.MakeMESProductCode(self.FabProduction.Code, sLabelCode);
      }
      catch (Exception)
      {
        return "NA";
      }

    }
    public static int Servings(this Lot self, SessionProxy oCIMSession)
    {
      return oCIMSession.QueryDataElement<ProductCodeToFormulation>()
                        .Where(x => x.FormulationCode == self.FabProduction.Code)
                        .OrderByDescending(x => x.CAProduct)
                        .Take(1)
                        .ToList()
                       ?.FirstOrDefault()?.Servings ?? 1;
    }
  }
}
