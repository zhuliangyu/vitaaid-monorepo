using MyHibernateUtil;
using MySystem.Base.Extensions;
using POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static CIM.DBPO.DBPOServiceHelper;

namespace CIM.DBPO
{
  public static class OrderNoControlHelper
  {
    public static string RetrievePONoFromSystem<T>(SessionProxy oCIMSession, Expression<Func<T, bool>> predicate, string oldPONo, string sVC, int iYear)
    {
      try
      {
        if (string.IsNullOrWhiteSpace(oldPONo) == false && oldPONo.StartsWith(sVC + "0") && oCIMSession.Query<T>().Where(predicate).ToList().Count() == 0)
        {
          //if (string.IsNullOrWhiteSpace(oldPONo) == false && CIMDBSession.Query<T>().Where(predicate).ToList().Count() == 0)
          if (oldPONo.Contains("-R"))
          {
            var tokens = oldPONo.Split(new String[] {"-R"}, StringSplitOptions.None);
            return tokens[0] + "-R" + (Int32.Parse(tokens[1]) + 1);
          }
          return oldPONo + "-R1";
        }

        // find next PONo
        var noControl = oCIMSession.Query<OrderNoControl>()
                                    .Where(x => x.VendorCode == sVC && x.Year == iYear)
                                    .ToList()
                                    .UniqueOrDefault();
        if (noControl == null)
          noControl = new OrderNoControl(sVC, iYear, 1);
        else
          noControl.SerierNo++;
        noControl.SaveObj(oCIMSession);
        return noControl.ToPONo();
      }
      catch (Exception)
      {

        throw;
      }
    }

  }
}
