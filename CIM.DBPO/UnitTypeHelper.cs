using POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DBPO
{
    public class UnitTypeHelper
    {
        public static double dblRnd(double d, UnitType ut = null)
        {
            try
            {
                if (ut != null && ut.AbbrName == "mg")
                    return Math.Round(d, 3, MidpointRounding.AwayFromZero);
                else
                    return Math.Round(d, Constant.OPIDIGIT, MidpointRounding.AwayFromZero);

            }
            catch (Exception ex) { throw ex; }
        }
        public static double dblRnd(double? d, UnitType ut)
        {
            try
            {
                if (d == null || d.HasValue == false) return 0.0;
                return dblRnd(d.Value, ut);
            }
            catch (Exception ex) { throw ex; }
        }

        public static string dblToStr(double d, UnitType ut)
        {
            try
            {
                return dblRnd(d, ut).ToString();
            }
            catch (Exception ex) { throw ex; }
        }
        public static string dblToStr(double? d, UnitType ut)
        {
            if (d == null || d.HasValue == false)
                return "";
            return dblToStr(d.Value, ut);
        }
    }
}
