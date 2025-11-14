using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySystem.Base.Extensions
{
  public static class DoubleExtension
  {
    public static string dblToStr(this Double? d, int digits)
    {
      try
      {
        if (d == null || d.HasValue == false) return "";
        return Math.Round(d.Value, digits, MidpointRounding.AwayFromZero).ToString();
      }
      catch (Exception ex) { return ""; }
    }
    public static string dblToStr(this double d, int pecision)
    {
      try
      {
        return Math.Round(d, pecision, MidpointRounding.AwayFromZero).ToString();
      }
      catch (Exception ex) { throw ex; }
    }
  }
}
