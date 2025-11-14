using System;

namespace MySystem.Base.Extensions
{
  public static class DateTimeExtension
  {
    public static DateTime NilDate = new DateTime(2050, 12, 31);
    public static bool IsNil(this DateTime? self)
    {
      return (self == null || (self.HasValue && self.Value.IsNil()));
    }
    public static bool IsNil(this DateTime self)
    {
      return (self.Year == NilDate.Year && self.Month == NilDate.Month && self.Day == NilDate.Day);
    }

    public static string ToStr(this DateTime? self, string format, string NilStr = "")
    {
      return (self.IsNil()) ? NilStr : self.Value.ToString(format);
    }
    public static string ToStr(this DateTime self, string format, string NilStr = "")
    {
      return (self.IsNil()) ? NilStr : self.ToString(format);
    }
    public static DateTime EndOfDay(this DateTime? self)
    {
      return (self.IsNil()) ? DateTime.Now.EndOfDay() : self.Value.EndOfDay();
    }
    public static DateTime EndOfDay(this DateTime dtEnd)
    {
      try
      {
        return new DateTime(dtEnd.Year, dtEnd.Month, dtEnd.Day, 23, 59, 59);
      }
      catch (Exception ex) { throw ex; }
    }

    public static DateTime StartOfDay(this DateTime? self)
    {
      return (self.IsNil()) ? DateTime.Now.StartOfDay() : self.Value.StartOfDay();
    }
    public static DateTime StartOfDay(this DateTime dtEnd)
    {
      try
      {
        return new DateTime(dtEnd.Year, dtEnd.Month, dtEnd.Day, 0, 0, 0);
      }
      catch (Exception ex) { throw ex; }
    }
    public static DateTime EndOfMonth(this DateTime dtEnd)
    {
      try
      {
        return new DateTime(dtEnd.Year, dtEnd.Month, 1, 23, 59, 59).AddMonths(1).AddDays(-1);
      }
      catch (Exception ex) { throw ex; }
    }
    public static DateTime StartOfMonth(this DateTime dtEnd)
    {
      try
      {
        return new DateTime(dtEnd.Year, dtEnd.Month, 1, 0, 0, 0);
      }
      catch (Exception ex) { throw ex; }
    }
    public static DateTime NthOf(this DateTime CurDate, int Occurrence, DayOfWeek Day)
    {
      var fday = new DateTime(CurDate.Year, CurDate.Month, 1);
      var offsetOfFirstOccurrence = Day - fday.DayOfWeek;
      if (offsetOfFirstOccurrence > 0)
        return fday.AddDays(offsetOfFirstOccurrence + 7 * (Occurrence - 1));
      else if (offsetOfFirstOccurrence < 0)
        return fday.AddDays(offsetOfFirstOccurrence + 7 * (Occurrence));
      else if (Occurrence > 1) // offsetOfFirstOccurrence == 0
        return fday.AddDays(7 * (Occurrence - 1));
      else return fday;
    }
    public static bool SameDay(this DateTime self, DateTime target)
    {
      return self.ToStr("d") == target.ToStr("d");
    }
  }
}