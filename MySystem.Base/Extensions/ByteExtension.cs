using System;
using System.IO;

namespace MySystem.Base.Extensions
{
    public static class ByteExtension
    {
        public static void SaveAs(this Byte[] source, string sFileName)
        {
            try
            {
                sFileName = sFileName.Trim();

                if ((source?.Length ?? 0) == 0)
                    new FileInfo(sFileName).Create();
                else
                    File.WriteAllBytes(sFileName, source);
            }
            catch (Exception ex) { throw ex; }
        }

        public static bool Load(this Byte[] source, string absFileName, out int size, out string fileName)
        {
            try
            {
                absFileName = absFileName.Trim();
                int iPos = absFileName.LastIndexOf('\\');

                fileName = (iPos >= 0) ? absFileName.Substring(iPos + 1) : absFileName;
                size = 0;
                FileInfo fi = new FileInfo(fileName);
                if (fi.Exists == false || fi.Length == 0)
                    source = null;
                else
                {
                    source = File.ReadAllBytes(absFileName);
                    size = (int)fi.Length;
                }
                return fi.Exists;
            }
            catch (Exception ex) { throw ex; }
        }
    }
}