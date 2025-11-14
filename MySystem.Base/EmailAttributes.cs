using MySystem.Base.Extensions;
using System.Collections.Generic;

namespace MySystem.Base
{
  public class EmailAttributes : Dictionary<string, object>
  {
    public static string C_RECIPIENT { get; set; } = "C_RECIPIENT";
    public static string C_CCRECIPIENT { get; set; } = "C_CCRECIPIENT";
    public static string C_BCCRECIPIENT { get; set; } = "C_BCCRECIPIENT";
    public static string C_FROM { get; set; } = "C_FROM";
    public static string C_DISPLAYNAME { get; set; } = "C_DISPLAYNAME";
    public static string C_SUBJECT { get; set; } = "C_SUBJECT";
    public static string C_BODY { get; set; } = "C_BODY";
    public static string C_USERNAME { get; set; } = "C_USERNAME";
    public static string C_PASSWORD { get; set; } = "C_PASSWORD";
    public static string C_SERVER { get; set; } = "C_SERVER";
    public static string C_PORT { get; set; } = "C_PORT";
    public static string C_SSL { get; set; } = "C_SSL";
    public EmailAttributes()
    {
      this.Add(C_RECIPIENT, "");
      this.Add(C_CCRECIPIENT, "");
      this.Add(C_BCCRECIPIENT, "");
      this.Add(C_FROM, "");
      this.Add(C_DISPLAYNAME, "");
      this.Add(C_SUBJECT, "");
      this.Add(C_BODY, "");
      this.Add(C_USERNAME, "");
      this.Add(C_PASSWORD, "");
      this.Add(C_SERVER, "");
      this.Add(C_PORT, 25);
      this.Add(C_SSL, true);
    }

    public void addRecipient(string sRecipients)
    {
      if (string.IsNullOrWhiteSpace(sRecipients))
        return;
      if (string.IsNullOrWhiteSpace((string)this[C_RECIPIENT]))
        this[C_RECIPIENT] = sRecipients.Trim();
      else
        this[C_RECIPIENT] = sRecipients.Trim() + ";" + this[C_RECIPIENT];
    }
    public void processVarables(string attributeName, Dictionary<string, string> varValues)
    {
      if (string.IsNullOrWhiteSpace((string)this[attributeName]))
        return;
      foreach (string key in varValues.Keys)
        this[attributeName] = ((string)this[attributeName]).Replace(key, varValues[key]);
    }

    //public void processVarablesOfBody(Dictionary<string, string> varValues)
    //{
    //    if (string.IsNullOrWhiteSpace((string)this[C_BODY]))
    //        return;
    //    foreach (string key in varValues.Keys)
    //        this[C_BODY] = ((string)this[C_BODY]).Replace(key, varValues[key]);
    //}
    //public void processVarablesOfSubject(Dictionary<string, string> varValues)
    //{
    //    if (string.IsNullOrWhiteSpace((string)this[C_SUBJECT]))
    //        return;
    //    foreach (string key in varValues.Keys)
    //        this[C_SUBJECT] = ((string)this[C_SUBJECT]).Replace(key, varValues[key]);
    //}


    public EmailAttributes ShallowCopy()
    {
      var copy = new EmailAttributes();
      Keys.ForEach((key) => copy[key] = this[key]);
      return copy;
    }

  }
}
