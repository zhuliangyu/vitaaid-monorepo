using MyHibernateUtil;
using System;
using System.IO;


namespace POCO
{
  [Serializable]
  public class AttachedFile : AttachedFileBase
  {
    public AttachedFile(string absFileName) { Load(absFileName); }
    public AttachedFile() { }
    public virtual Guid FSGUID { get; set; }
    private string _Name = "";
    public virtual string Name
    {
      get { return _Name; }
      set { _Name = (value == null) ? "" : value.Trim(); }
    }
    public virtual string Memo { get; set; }
    public virtual int Size { get; set; }

    public virtual bool SaveAs(string sFileName)
    {
      try
      {
        if (sFileName == null || sFileName.Length == 0) return false;

        sFileName = sFileName.Trim();

        if (FileBody == null || FileBody.Length == 0)
          new FileInfo(sFileName).Create();
        else
          File.WriteAllBytes(sFileName, FileBody);
        return true;
      }
      catch (Exception ex) { throw ex; }
    }

    public virtual bool SaveAs(string sPath, string sFileName)
    {
      try
      {
        sPath = (sPath != null) ? sPath.Trim() : "";
        if (sPath.Length > 0 && sPath[sPath.Length - 1] != '\\') sPath += "\\";

        sFileName = (sFileName != null) ? sFileName.Trim() : Name;
        if (sFileName == null || sFileName.Length == 0) return false;

        string absFileName = sPath + sFileName;

        if (FileBody == null || FileBody.Length == 0)
          new FileInfo(absFileName).Create();
        else
          File.WriteAllBytes(absFileName, FileBody);
        return true;
      }
      catch (Exception ex) { throw ex; }
    }

    public virtual bool Load(string absFileName)
    {
      try
      {
        if (absFileName == null || absFileName.Length == 0) return false;
        absFileName = absFileName.Trim();
        int iPos = absFileName.LastIndexOf('\\');
        string sPath = "";
        string sFileName = absFileName.Trim();
        if (iPos != -1)
        {
          sPath = absFileName.Substring(0, iPos).Trim();
          sFileName = absFileName.Substring(iPos + 1).Trim();
        }

        if (sFileName == null || sFileName.Length == 0) return false;

        if (sPath.Length > 0 && sPath[sPath.Length - 1] != '\\') sPath += "\\";

        Name = sFileName;

        FileInfo fi = new FileInfo(absFileName);
        if (fi.Exists == false || fi.Length == 0)
        {
          FileBody = null;
          return (fi.Exists == true && fi.Length == 0) ? true : false;
        }
        else
        {
          FileBody = File.ReadAllBytes(absFileName);
          Size = (int)fi.Length;
          return true;
        }
      }
      catch (Exception ex) { throw ex; }
    }


    public virtual bool Load(string sPath, string sFileName)
    {
      try
      {
        sPath = (sPath != null) ? sPath.Trim() : "";
        if (sPath.Length > 0 && sPath[sPath.Length - 1] != '\\') sPath += "\\";

        if (sFileName != null && sFileName.Length > 0)
          Name = sFileName.Trim();

        if (Name == null || Name.Length == 0) return false;

        string absFileName = sPath + Name;
        FileInfo fi = new FileInfo(absFileName);
        if (fi.Exists == false || fi.Length == 0)
        {
          FileBody = null;
          return (fi.Exists == true && fi.Length == 0) ? true : false;
        }
        else
        {
          FileBody = File.ReadAllBytes(absFileName);
          Size = (int)fi.Length;
          return true;
        }
      }
      catch (Exception ex) { throw ex; }
    }
  }
}
