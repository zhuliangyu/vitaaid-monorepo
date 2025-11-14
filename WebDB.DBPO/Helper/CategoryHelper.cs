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
using MyHibernateUtil;

namespace WebDB.DBPO.Helper
{
    public static class CategoryHelper
    {
        public static UnitType[] getVitaaidCategory(SessionProxy oSession) => oSession.QueryDataElement<UnitType>().Where(x => x.uType ==  eUNITTYPE.PRODUCT_CATEGORY).ToList().ToArray();
        public static UnitType[] getAllergyCategory(SessionProxy oSession) => oSession.QueryDataElement<UnitType>().Where(x => x.uType ==  eUNITTYPE.ALLERGY_CATEGORY).ToList().ToArray();
        public static string namesToVitaaidCategory(string names, SessionProxy oSession)
        {
            if (string.IsNullOrWhiteSpace(names)) return "";
            return namesToCategoryIDs(names, getVitaaidCategory(oSession));
        }
        public static string namesToAllergyCategory(string names, SessionProxy oSession)
        {
            if (string.IsNullOrWhiteSpace(names)) return "";
            return namesToCategoryIDs(names, getAllergyCategory(oSession));
        }
        private static string namesToCategoryIDs(string names, IList<UnitType> categorys)
        {
            return names.Split(';')
                        .Select(x => categorys.Where(c => c.Name == x).FirstOrDefault()?.AbbrName)
                        .Aggregate((a, b) => a + ";" + b);

        }
    }
}
