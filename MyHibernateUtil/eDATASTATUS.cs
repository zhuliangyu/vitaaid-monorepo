using System.Collections.Generic;

namespace MyHibernateUtil
{
    public static class eDATASTATUS
    {
        public const string
            ACTIVE = "A",
            DELETE = "D";
        private static List<string> SortedList = new List<string> { eDATASTATUS.ACTIVE, eDATASTATUS.DELETE };
        public static IList<string> ToStringList()
        {
            return SortedList;
        }
        public static int Idx(string sEnumVal)
        {
            if (sEnumVal == null || sEnumVal.Length == 0 || SortedList.Contains(sEnumVal) == false)
                return -1;
            else
                return SortedList.IndexOf(sEnumVal) + 1;
        }
    }
}
