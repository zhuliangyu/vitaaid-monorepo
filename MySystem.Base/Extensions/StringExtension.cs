using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MySystem.Base.Extensions
{
  public static class StringExtension
  {
    public static string LastSubstring(this string self, int length) => self.Substring(self.Length - length, length);
    public static void spliteLine(this string source, out string[] lines, int iLineLen)
    {
      try
      {
        lines = null;
        if (source == null || iLineLen == 0) return;
        string[] worlds = source.Replace('\t', ' ').Replace("  ", " ").Split(' ');
        int iCurrLen = 0;
        string sLines = "";
        foreach (string w in worlds)
        {
          if (iCurrLen > 0 && w.Length + iCurrLen > iLineLen)
          {
            sLines += "\n" + w;
            iCurrLen = w.Length;
          }
          else
          {
            sLines += ((iCurrLen == 0) ? "" : " ") + w;
            iCurrLen += w.Length;
          }
        }
        string[] sLineAry = sLines.Split('\n');
        lines = new string[sLineAry.Length];
        for (int i = 0; i < lines.Count(); i++)
        {
          lines[i] = sLineAry[i];
        }
        return;
      }
      catch (Exception ex) { throw ex; }
    }

    public static string[] ToNotEmptyLines(this string value)
    {
      return value.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
    }

    public static string[] ToLines(this string value)
    {
      return value.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
    }

    public static string[] TrimStringArray(this IEnumerable<string> array)
    {
      return array.Select(item => item.Trim()).ToArray();
    }
    public static string MakeAbsFileName(this String fileName, string path)
    {
      path = path?.Trim() ?? "";
      if (path.Length > 0 && path[path.Length - 1] != '\\')
        path += "\\";
      return path + fileName.Trim();
    }
    public static T GetEnumFromDescription<T>(this string description, T defaultValue) where T : Enum
    {
      if (string.IsNullOrEmpty(description)) return defaultValue;

      var fields = typeof(T).GetFields();
      foreach (var field in fields)
      {
        DescriptionAttribute attribute
            = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
        if (attribute == null)
          continue;
        if (attribute.Description == description)
        {
          return (T)field.GetValue(null);
        }
      }
      return defaultValue;
    }
  }
}