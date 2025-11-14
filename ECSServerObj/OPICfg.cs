using Newtonsoft.Json;
using System;
using System.IO;

namespace ECSServerObj
{
  public class OPICfg
  {
    public virtual string APServer { get; set; }
    public virtual string APServerDEV { get; set; }
    public virtual string APServerPROD { get; set; }
    private string _DataFormat = "json";
    public virtual string DataFormat { get { return _DataFormat; } set { _DataFormat = value; } }
    public virtual int CompanyID { get; set; } = 1;
    public virtual int SessionTimeout { get; set; } = 0;
    public virtual int BackupSessionTimeout { get; set; } = 0;
    public virtual bool bPROD
    {
      get => (APServer == APServerPROD);
      set
      {
        APServer = (value) ? APServerPROD : APServerDEV;
        SessionTimeout = (value) ? BackupSessionTimeout : 0;
      }
    }
    public static OPICfg load(System.IO.TextReader StrIn, bool bPROD = false)
    {
      try
      {
        string content = "";
        try
        {
          content = StrIn.ReadToEnd();
        }
        catch (Exception) { }

        if (content.Length > 0)
        {
          var cfg = JsonConvert.DeserializeObject<OPICfg>(content);
          cfg.APServer = (bPROD) ? cfg.APServerPROD : cfg.APServerDEV;
          cfg.BackupSessionTimeout = cfg.SessionTimeout;
          return cfg;
        }
        else
          return null;

      }
      catch (Exception ex) { throw ex; }
    }

    public static OPICfg load(System.IO.Stream StrIn, bool bPROD = false)
    {
      try
      {
        using (System.IO.StreamReader StrRead = new System.IO.StreamReader(StrIn))
        {
          try
          {
            var cfg = load(StrRead, bPROD);
            StrRead.Close();
            return cfg;
          }
          catch (Exception) { }
        }
        return null;
      }
      catch (Exception ex) { throw ex; }
    }
    public static OPICfg load(bool bPROD = false, string FileName = @"ecsopicfg.ini")
    {
      try
      {
        using (TextReader StrIn = new System.IO.StreamReader(FileName))
        {
          try
          {
            var cfg = load(StrIn, bPROD);
            StrIn.Close();
            return cfg;
          }
          catch (Exception) { }
        }
        return null;
      }
      catch (Exception)
      {
        throw;
      }
    }
    public void save(string FileName = @"ecsopicfg.ini")
    {
      try
      {
        var content = JsonConvert.SerializeObject(this);
        File.WriteAllText(FileName, content);
      }
      catch (Exception ex) { throw ex; }
    }
  }
}
