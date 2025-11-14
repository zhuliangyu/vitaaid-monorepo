using MyHibernateUtil;
using POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DBPO
{
  public static class RawMaterialSpecExtension
  {
    public static bool isUsed(this RawMaterialSpec self, SessionProxy oSession)
    {
      if (self == null || (self.Category?.ID ?? 0) == 0) return false;
      var hasRM = oSession.QueryDataElement<MESRawMaterial>()
                          .Where(x => x.RMCategory.ID == self.Category.ID)
                          .Select(x => x.ID)
                          .ToList()
                          .Any();
      if (!hasRM)
        hasRM = oSession.QueryDataElement<XMESRawMaterial>()
                          .Where(x => x.RMCategory.ID == self.Category.ID)
                          .Select(x => x.ID)
                          .ToList()
                          .Any();
      return hasRM;
    }
  }
}
