using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MySystem.Base.Extensions
{
    public static class AssemblyExtension
    {
        public static string VersionInfo(this Assembly self)
        {
            var version = new System.Version(self.FullName.Split(',')[1].Split('=')[1]);
            return string.Format("{0} {1}.{2}.{3} ({4})"
                , new string(System.AppDomain.CurrentDomain.FriendlyName.TakeWhile(x => x != '.').ToArray())
                , version.Major
                , version.Minor
                , version.Build
                , self.RetrieveLinkerTimestamp());
        }

        public static DateTime RetrieveLinkerTimestamp(this Assembly self) => File.GetLastWriteTime(self.Location);
        /*
            string filePath = oAssembly.Location;
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;
            byte[] b = new byte[2048];
            System.IO.Stream s = null;

            try
            {
                s = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                s.Read(b, 0, 2048);
            }
            finally
            {
                if (s != null)
                {
                    s.Close();
                }
            }

            int i = System.BitConverter.ToInt32(b, c_PeHeaderOffset);
            int secondsSince1970 = System.BitConverter.ToInt32(b, i + c_LinkerTimestampOffset);
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);
            dt = dt.AddSeconds(secondsSince1970);
            dt = dt.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours);
            return dt;
        */
    }
}
