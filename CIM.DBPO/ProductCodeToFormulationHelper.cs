using MyHibernateUtil;
using MyHibernateUtil.Extensions;
using MySystem.Base.Extensions;
using NHibernate;
using NHibernate.Transform;
using POCO;
using System.Collections.Generic;
using System.Linq;
using static CIM.DBPO.DBPOServiceHelper;
namespace CIM.DBPO
{
  public class PCodeMap
  {
    public virtual string FCode { get; set; }
    public virtual string PCode { get; set; }
  }
  public static class ProductCodeToFormulationHelper
  {
    //public static IList<string> GetActiveProductCode() 
    //{
    //    return CIMDBSession.CreateQuery("SELECT DISTINCT x.ProductCode " +
    //                                    "FROM ProductCodeToFormulation x, Production p " +
    //                                    "WHERE " + DataElement.sHideDeletedDataWhere() + " AND " +
    //                                    "      x.IsOEM = 0 AND p.Code = x.FormulationCode AND p.isActive = 1 " +
    //                                    "ORDER BY x.ProductCode").List<string>();
    //}

    public static IList<ProductCodeToFormulation> GetActiveVAProductCodeDetail(SessionProxy oCIMSession)
    {
      var PCodeMaps = oCIMSession.CreateSQLQuery("SELECT ProductCodeToFormulationID as ID, ProductCode, FormulationCode, IsOEM, Servings, SampleServings, TemperatureSensitive, " +
                                                          "DataStatus, CreatedDate, CreatedID, UpdatedDate, UpdatedID, MESPackageSpecID, Name as ProductName, " +
                                                          "FormulationID, FormulationName, NPN, Version, ServingSize, ServingUnit, ServingsPerContainer " +
                                      "FROM ActiveVAProductCodeDetail_v " +
                                      "WHERE IsOEM = 0 AND " + DataElement.sHideDeletedDataWhere() +
                                      "ORDER BY FormulationCode, ProductCode")
          .AddScalar("ID", NHibernateUtil.Int32)
          .AddScalar("ProductCode", NHibernateUtil.String)
          .AddScalar("FormulationCode", NHibernateUtil.String)
          .AddScalar("IsOEM", NHibernateUtil.Boolean)
          .AddScalar("Servings", NHibernateUtil.Int32)
          .AddScalar("SampleServings", NHibernateUtil.Int32)
          .AddScalar("TemperatureSensitive", NHibernateUtil.Boolean)
          .AddScalar("DataStatus", NHibernateUtil.String)
          .AddScalar("CreatedDate", NHibernateUtil.DateTime)
          .AddScalar("CreatedID", NHibernateUtil.String)
          .AddScalar("UpdatedDate", NHibernateUtil.DateTime)
          .AddScalar("UpdatedID", NHibernateUtil.String)
          .AddScalar("MESPackageSpecID", NHibernateUtil.Int32)
          .AddScalar("ProductName", NHibernateUtil.String)
          .AddScalar("FormulationID", NHibernateUtil.Int32)
          .AddScalar("FormulationName", NHibernateUtil.String)
          .AddScalar("NPN", NHibernateUtil.String)
          .AddScalar("Version", NHibernateUtil.Int32)
          .AddScalar("ServingSize", NHibernateUtil.Int32)
          .AddScalar("ServingUnit", NHibernateUtil.String)
          .AddScalar("ServingsPerContainer", NHibernateUtil.Int32)
          .SetResultTransformer(Transformers.AliasToBean(typeof(ProductCodeToFormulation))).List<ProductCodeToFormulation>();
      return PCodeMaps;
    }

    public static IList<ProductCodeToFormulation> GetActiveAllProductCodeDetail(SessionProxy oCIMSession)
    {
      var PCodeMaps = oCIMSession.CreateSQLQuery("SELECT ProductCodeToFormulationID as ID, ProductCode, FormulationCode, IsOEM, Servings, SampleServings, TemperatureSensitive, " +
                                                          "DataStatus, CreatedDate, CreatedID, UpdatedDate, UpdatedID, MESPackageSpecID, Name as ProductName, " +
                                                          "FormulationID, FormulationName, NPN, Version, ServingSize, ServingUnit, ServingsPerContainer " +
                                      "FROM ActiveAllProductCodeDetail_v " +
                                      "WHERE " + DataElement.sHideDeletedDataWhere() +
                                      "ORDER BY FormulationCode, ProductCode")
          .AddScalar("ID", NHibernateUtil.Int32)
          .AddScalar("ProductCode", NHibernateUtil.String)
          .AddScalar("FormulationCode", NHibernateUtil.String)
          .AddScalar("IsOEM", NHibernateUtil.Boolean)
          .AddScalar("Servings", NHibernateUtil.Int32)
          .AddScalar("SampleServings", NHibernateUtil.Int32)
          .AddScalar("TemperatureSensitive", NHibernateUtil.Boolean)
          .AddScalar("DataStatus", NHibernateUtil.String)
          .AddScalar("CreatedDate", NHibernateUtil.DateTime)
          .AddScalar("CreatedID", NHibernateUtil.String)
          .AddScalar("UpdatedDate", NHibernateUtil.DateTime)
          .AddScalar("UpdatedID", NHibernateUtil.String)
          .AddScalar("MESPackageSpecID", NHibernateUtil.Int32)
          .AddScalar("ProductName", NHibernateUtil.String)
          .AddScalar("FormulationID", NHibernateUtil.Int32)
          .AddScalar("FormulationName", NHibernateUtil.String)
          .AddScalar("NPN", NHibernateUtil.String)
          .AddScalar("Version", NHibernateUtil.Int32)
          .AddScalar("ServingSize", NHibernateUtil.Int32)
          .AddScalar("ServingUnit", NHibernateUtil.String)
          .AddScalar("ServingsPerContainer", NHibernateUtil.Int32)
          .SetResultTransformer(Transformers.AliasToBean(typeof(ProductCodeToFormulation))).List<ProductCodeToFormulation>();
      return PCodeMaps;
    }

    public static IList<string> FCodeNotInWIPPlanning(SessionProxy oCIMSession)
    {
      return oCIMSession.CreateSQLQuery("SELECT FormulationCode FROM FCodeNotInWIPPlanning_v ORDER BY FormulationCode")
                         .List<string>();
    }

    public static string getUSAProductCode(SessionProxy oSession, string code)
    {
      var tokens = code.Split('-');
      if (tokens.Length < 2 || tokens[1].StartsWith("9"))
        return code;
      var oMap = oSession.QueryDataElement<ProductCodeToFormulation>().Where(x => x.ProductCode == code).UniqueOrDefault();
      if ((oMap?.CAProduct ?? false) == false)
        return code;
      
      var oUSMap = oSession.QueryDataElement<ProductCodeToFormulation>()
                    .Where(x => x.FormulationCode == oMap.FormulationCode && x.CAProduct == false)
                    .ToList()
                    .FirstOrDefault();
      return oUSMap?.ProductCode ?? code;
    }

  }
}
