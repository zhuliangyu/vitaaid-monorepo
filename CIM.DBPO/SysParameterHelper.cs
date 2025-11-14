using MyHibernateUtil;
using POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySystem.Base;
using MySystem.Base.Extensions;

namespace CIM.DBPO
{
  public static class SysParameterHelper
  {
    public static string getBatchRecordDocPath(SessionProxy oSession)
    {
      try
      {
        string DocPath = ".\\";
        //DBSession1.Clear();
        oSession.QueryDataElement<SysParameter>()
          .Where(x => x.AppName == "MESModeling" && x.ConfigName == "BatchRecordDocPath")
          .ToList().FirstOrDefault()
          ?.Also(setting =>
          {
            if (string.IsNullOrWhiteSpace(setting.ConfigVal2))
              return;
            DocPath = setting.ConfigVal2;
            if (!DocPath.EndsWith("\\"))
              DocPath += "\\";
          });
        return DocPath;
      }
      catch (Exception ex) { throw ex; }
    }
  }
}
