using System;
using System.IO;
using System.Xml.Serialization;

namespace MyHibernateUtil
{
  public class sysconfig
  {
    public string DefaultDBFactory { get; set; } = "Development";
    public string DBCollation { get; set; } = "Chinese_Taiwan_Stroke_CI_AS";
    public bool bPROD
    {
      get => DefaultDBFactory == "Production";
      set
      {
        DefaultDBFactory = (value) ? "Production" : "Development";
      }
    }
    public void write(string FileName = @"sysconfig.xml")
    {
      try
      {
        System.Xml.Serialization.XmlSerializer writer =
            new System.Xml.Serialization.XmlSerializer(typeof(sysconfig));

        System.IO.StreamWriter file = new System.IO.StreamWriter(FileName);
        writer.Serialize(file, this);
        file.Close();
      }
      catch (Exception)
      {
        throw;
      }
    }
    public static sysconfig load(string FileName = @"sysconfig.xml")
    {
      try
      {
        if (File.Exists(FileName))
        {
          XmlSerializer deserializer = new XmlSerializer(typeof(sysconfig));
          TextReader textReader = new StreamReader(@FileName);
          var sc = (sysconfig)(deserializer.Deserialize(textReader));
          textReader.Close();
          return sc;
        }
        else
        {
          var sc = new sysconfig();
          sc.write();
          return sc;
        }
      }
      catch (Exception)
      {
        throw;
      }
    }
  }
}
