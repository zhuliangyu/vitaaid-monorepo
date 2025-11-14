using MyHibernateUtil;
using MySystem.Base.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDB.DBBO;

namespace WebDB.DBPO.Helper
{
  public class ProductHelper
  {
    public static IList<Product> GetRelatedsProduct(string Tags, eWEBSITE site, SessionProxy oSession, int takeCount = 0)
    {
      try
      {
        if (string.IsNullOrWhiteSpace(Tags)) return new List<Product>();
        var allRelatedProducts = 
          Tags.Split(';').SelectMany(x => oSession.QueryDataElement<Product>()
                                                  .Where(p => p.Tags.Contains(x) && p.Published &&
                                                             (p.VisibleSite == eWEBSITE.ALL || site == eWEBSITE.ALL || p.VisibleSite == site))
                                                  .ToList())
                         .ToList()
                         .GroupBy(x => x.ID)
                         .OrderByDescending(x => x.Count())
                         .Select(x => x.First())
                         .ToList();

        return ((takeCount > 0) ? allRelatedProducts.Take(takeCount).ToList() : allRelatedProducts)
                  .Also(x =>
                  {
                    x.Action(p =>
                    {
                      p.RepresentativeImage = oSession.QueryDataElement<ProductImage>()
                                                      .Where(i => i.oProduct == p && i.FrontImage)
                                                      .ToList()
                                                      .FirstOrDefault()?.ImageName ?? "";

                    });
                  }); ;
      }
      catch (Exception)
      {
        throw;
      }
    }
  }
}
