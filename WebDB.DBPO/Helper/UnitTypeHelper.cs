using MyHibernateUtil.Extensions;
using MySystem.Base.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDB.DBBO;
using static WebDB.DBPO.DBPOServiceHelper;
using WebDB.DBPO.Extensions;
using MySystem.Base;
using static MySystem.Base.EmailAttributes;
using MyHibernateUtil;

namespace WebDB.DBPO.Helper
{
  public static class UnitTypeHelper
  {
    public static IList<UnitType> getUnitTypes(eUNITTYPE type, SessionProxy oSession) => oSession.QueryDataElement<UnitType>().Where(x => x.uType == type).OrderBy(x => x.ID).ToList();
    public static IList<string> getUnitTypeNames(eUNITTYPE type, SessionProxy oSession) => oSession.CreateSQLQuery("SELECT Name FROM UnitType WHERE uType = " + (int)type + " ORDER BY ID").List<string>();
    public static UnitType getUnitType(eUNITTYPE type, string Name, SessionProxy oSession) => oSession.QueryDataElement<UnitType>().Where(x => x.uType == type && x.Name == Name).UniqueOrDefault();
    public static EmailAttributes buildEmailAttributes(SessionProxy oSession)
    {
      var attributes = new EmailAttributes();
      oSession.QueryDataElement<UnitType>()
                .Where(x => x.uType == eUNITTYPE.EMAIL_SETTING)
                .ToList()
                .Action(x =>
                {
                  if (x.Name.Equals(C_PORT))
                    attributes[C_PORT] = x.AbbrName.ToInt();
                  else if (x.Name.Equals(C_SSL))
                    attributes[C_SSL] = (string.IsNullOrWhiteSpace(x.AbbrName) == false &&
                                         (x.AbbrName == "1" || x.AbbrName.ToLower().Equals("true"))) ? true : false;
                  else if (attributes.Keys.Contains(x.Name))
                    attributes[x.Name] = x.AbbrName;
                });
      return attributes;
    }
  }
}
