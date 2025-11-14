using MyHibernateUtil;
using MyHibernateUtil.Extensions;
using MySystem.Base.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDB.DBBO;
using static WebDB.DBPO.DBPOServiceHelper;

namespace WebDB.DBPO.Extensions
{
  public static class ProductExtension
  {
    public static void PostProcess(this Product self, UnitType[] vitaaidCategory, UnitType[] allergyCategory, SessionProxy oSession)
    {
      self.oIngredients = oSession.QueryDataElement<Ingredient>()
             .Where(i => i.oProduct == self)
             .OrderBy(i => i.Sequence)
             .ToList();
      if ((self.VisibleSite == eWEBSITE.US || self.VisibleSite == eWEBSITE.ALL) && self.ServingSize >= 1)
      {
        self.oIngredients.Where(x => string.IsNullOrWhiteSpace(x.LabelClaimUS))
                         .Action(x => x.LabelClaimUS = x.GenerateLabelClaimByServiceSize(self.ServingSize));
      }
      self.oProductImages = oSession.QueryDataElement<ProductImage>()
                              .Where(i => i.oProduct == self)
                              .OrderBy(i => i.ID)
                              .ToList();
      self.oProductImages.Where(i => i.FrontImage)
                         .FirstOrDefault()?.Also(x =>
                         {
                           self.RepresentativeImage = x.ImageName;
                           self.RepresentativeLargeImage = x.LargeImageName;
                         });


      self.Category?.Split(';')?.Also(cList =>
      {
        if (!cList.Any()) return;
        self.VitaaidCategory = vitaaidCategory.Where(s => cList.Contains(s.AbbrName)).Select(s => s.AbbrName.ToInt()).ToList();
      });
      self.Category1?.Split(';')?.Also(cList =>
      {
        if (!cList.Any()) return;
        self.AllergyCategory = allergyCategory.Where(s => cList.Contains(s.AbbrName)).Select(s => s.AbbrName.ToInt()).OrderBy(x => x).ToList();
      });
    }

    public static void Delete(this Product self, SessionProxy oSession)
    {
      oSession.QueryDataElement<Ingredient>()
                        .Where(y => y.oProduct == self)
                        .ToList()
                        .Action(ing => ing.DeleteObj(oSession));
      oSession.QueryDataElement<ProductImage>()
                        .Where(y => y.oProduct == self)
                        .ToList()
                        .Action(img => img.DeleteObj(oSession));
      self.DeleteObj(oSession);
    }
    public static bool VitaaidCategoryContains(this Product self, int iVal) => self.VitaaidCategory?.Contains(iVal) ?? false;
    public static bool AllergyCategoryContains(this Product self, int iVal) => self.AllergyCategory?.Contains(iVal) ?? false;
  }
}
