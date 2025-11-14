using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;

namespace MySystem.Base.Helpers
{
  public static class FileHelper
  {
    public static IEnumerable<FileInfo> GetFilesByExtion(string path, params string[] exts)
    {
      try
      {
        var extensions = exts.Select(x => x.StartsWith(".") ? x : "." + x).ToArray();

        return (new DirectoryInfo(path)).EnumerateFiles().Where(fn => extensions.Contains(fn.Extension, StringComparer.InvariantCultureIgnoreCase));
      }
      catch (Exception) { return new List<FileInfo>(); }
    }

    public static void SplitFileName(string sFullName, out string sPath, out string sFileName)
    {
      try
      {
        sPath = ".\\";
        sFileName = "";
        string sTmpFileName = sFullName;
        if (sFullName == null || sFullName.Length == 0) return;
        int iPos = sFullName.LastIndexOf('\\');
        if (iPos == -1)
          iPos = sFullName.LastIndexOf('/');
        if (iPos != -1)
        {
          sPath = sFullName.Substring(0, iPos);
          sTmpFileName = sFullName.Substring(iPos + 1);
        }
        sFileName = sTmpFileName;
      }
      catch (Exception ex) { throw ex; }
    }

    public static void SplitFileName(string sFullName, out string sFileName)
    {
      try
      {
        string sPath = ".\\";
        SplitFileName(sFullName, out sPath, out sFileName);
      }
      catch (Exception ex) { throw ex; }
    }

    public static void SplitFileName(string sFullName, out string sPath, out string sFileNameWithoutExt, out string sExtenionName)
    {
      try
      {
        sFileNameWithoutExt = sExtenionName = "";

        string sTmpFileName = "";
        SplitFileName(sFullName, out sPath, out sTmpFileName);
        var iPos = sTmpFileName.LastIndexOf(".");
        if (iPos != -1)
        {
          sFileNameWithoutExt = sTmpFileName.Substring(0, iPos);
          sExtenionName = sTmpFileName.Substring(iPos + 1);
        }
      }
      catch (Exception ex) { throw ex; }
    }

    public static string getFileName(string sFullName)
    {
      try
      {
        string sPath = ".\\";
        string sFileName = "";
        SplitFileName(sFullName, out sPath, out sFileName);
        return sFileName;
      }
      catch (Exception ex) { throw ex; }
    }

    public static void getExtName(string sFileName, out string sExtName)
    {
      try
      {
        int iPos = sFileName.LastIndexOf(".");
        if (iPos != -1)
          sExtName = sFileName.Substring(iPos + 1);
        else
          sExtName = "";
      }
      catch (Exception ex) { throw ex; }
    }

    public static string getExtName(string sFileName)
    {
      try
      {
        string sExtName = "";
        getExtName(sFileName, out sExtName);
        return sExtName;
      }
      catch (Exception ex) { throw ex; }
    }
    public static byte[] ReadOutBinary(string FileName, string BrokenFile = null)
    {
      try
      {
        var loadedFile = File.Exists(FileName) ? FileName :
                       (string.IsNullOrWhiteSpace(BrokenFile) == false && File.Exists(BrokenFile)) ? BrokenFile : "";
        if (loadedFile == "") return null;
        using (FileStream stream = new FileStream(loadedFile, FileMode.Open, FileAccess.Read))
        {
          byte[] imageInByteArray = new byte[stream.Length];
          stream.Read(imageInByteArray, 0, (int)stream.Length);
          return imageInByteArray;
        }
      }
      catch (Exception)
      {
        return null;
      }
    }
    public static bool BinaryWriteTo(byte[] data, string fileName, bool bOverwrite = true)
    {
      try
      {
        if (string.IsNullOrWhiteSpace(fileName) || (data?.Length ?? 0) == 0) return false;
        if (File.Exists(fileName))
        {
          if (!bOverwrite)
            return true;
          File.Delete(fileName);
        }
        using (var ms = new MemoryStream(data))
        {
          using (var fs = new FileStream(fileName, FileMode.Create))
          {
            ms.WriteTo(fs);
          }
        }
        return true;

      }
      catch (Exception)
      {
        return false;
      }
    }
    private static Dictionary<string, (bool read, bool write)> DirAccessRight = new Dictionary<string, (bool read, bool write)>();
    public static void setAccessRight(string path,  (bool read, bool write) right)
    {
      if (DirAccessRight.ContainsKey(path))
        DirAccessRight[path] = right;
      else
        DirAccessRight.Add(path, right);
    }
    private static (bool read, bool write)  _getAccessRight(string path)
    {
      try
      {
        DirectoryInfo dirInfo = new DirectoryInfo(path);
        var accessControlList = dirInfo.GetAccessControl();
        if (accessControlList == null)
          return (false, false);

        var accessRules = accessControlList.GetAccessRules(true, true, typeof(System.Security.Principal.SecurityIdentifier));
        if (accessRules == null)
          return (false, false);

        var readAllow = true;
        var writeAllow = true;

        foreach (System.Security.AccessControl.FileSystemAccessRule rule in accessRules)
        {
          if (rule.AccessControlType == AccessControlType.Deny) {
            if ((FileSystemRights.Read & rule.FileSystemRights) == FileSystemRights.Read)
              readAllow = false;
            if ((FileSystemRights.Write & rule.FileSystemRights) == FileSystemRights.Write)
              writeAllow = false;
          }
        }
        DirAccessRight.Add(path, (readAllow, writeAllow));
        return (readAllow, writeAllow);
      }
      catch (Exception)
      {
        throw;
      }
    }
    public static (bool read, bool write) GetAccessRight(string path)
    {
      try
      {
        if (DirAccessRight.ContainsKey(path))
          return DirAccessRight[path];
        if (!Directory.Exists(path))
          return (false, false);
        return _getAccessRight(path);
      }
      catch (Exception)
      {
        throw;
      }
    }

    public static bool CanRead(string path)
    {
      try
      {

        if (DirAccessRight.ContainsKey(path))
          return DirAccessRight[path].read;
        if (!Directory.Exists(path))
          return false;
        return _getAccessRight(path).read;
      }
      catch (Exception)
      {
        throw;
      }
    }
    public static bool CanWrite(string path)
    {
      try
      {
        if (DirAccessRight.ContainsKey(path))
          return DirAccessRight[path].write;
        if (!Directory.Exists(path))
          return false;
        return _getAccessRight(path).write;
      }
      catch (Exception)
      {
        throw;
      }
    }
    public static bool CanReadWrite(string path)
    {
      try
      {
        (bool read, bool write) right;
        if (DirAccessRight.ContainsKey(path))
          right = DirAccessRight[path];
        if (!Directory.Exists(path))
          return false;

        right = _getAccessRight(path);
        return right.write && right.read;
      }
      catch (Exception)
      {
        throw;
      }
    }

  }
}