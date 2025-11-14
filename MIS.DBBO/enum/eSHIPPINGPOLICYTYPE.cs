using System.Collections.Generic;

namespace MIS.DBBO
{

  public static class eSHIPPINGPOLICYTYPE
  {
    public const string
        CANADA = "CA AREA", // CA
        USA = "USA AREA",
        FREE_SHIPPING_DAY_CA = "FREE SHIPPING DAY:CA AREA";
    private static List<string> SortedList = new List<string> { eSHIPPINGPOLICYTYPE.CANADA, eSHIPPINGPOLICYTYPE.USA, eSHIPPINGPOLICYTYPE.FREE_SHIPPING_DAY_CA };
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
