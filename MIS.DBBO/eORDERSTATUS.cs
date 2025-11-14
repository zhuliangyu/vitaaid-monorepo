using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POCO
{
    public static class eORDERSTATUS
    {
        public const string
            INIT = "INIT",
            PROC = "PROC",
            SHIP = "SHIP",
            END = "END",
            BACKORDER = "BACKORDER",
            ABORT = "ABORT";
        private static List<string> SortedList = new List<string> { eORDERSTATUS.INIT, eORDERSTATUS.PROC, eORDERSTATUS.SHIP, eORDERSTATUS.END, eORDERSTATUS.BACKORDER, eORDERSTATUS.ABORT };
        public static IList<string> ToStringList()
        {
            return SortedList;
        }
        public static int Idx(string sEnumVal)
        {
            if (sEnumVal == null || sEnumVal.Length == 0 || SortedList.Contains(sEnumVal) == false)
                return -1;
            return SortedList.IndexOf(sEnumVal) + 1;
        }
    }
}
