using DevExpress.Web.Mvc;
using MyHibernateUtil;
using MyHibernateUtil.Extensions;
using MySystem.Base.Extensions;
using MySystem.Base.Helpers;
using NHibernate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI.WebControls;
using WebDB.DBBO;
using WebDB.DBPO;
using WebDB.DBPO.Extensions;
using WebDB.DBPO.Helper;
using static backend.vitaaid.com.ServicesHelper;

namespace backend.vitaaid.com.Model
{
  public static class ProductHelper
  {
    public static GridViewModel GetProductModel()
    {
      return new GridViewModel();
    }
    private static List<Product> ReloadProducts(eWEBSITE siteFilter = eWEBSITE.ALL)
    {
      var oSession = DBServer[eST.READONLY];
      try
      {
        oSession.Clear();

        // type:
        // 2: Vita Aid Category
        var vitaaidCategory = CategoryHelper.getVitaaidCategory(oSession); //DBSession.QueryDataElement<Category>().Where(x => x.Category_Type == 2).ToList();
                                                                           // 5: Allergy Category
        var allergyCategory = CategoryHelper.getAllergyCategory(oSession); //DBSession.QueryDataElement<Category>().Where(x => x.Category_Type == 5).ToList();
        Expression<Func<Product, bool>> predicate = ExpressionExtension.True<Product>();
        if (siteFilter != eWEBSITE.ALL)
          predicate = predicate.And(x => x.VisibleSite == eWEBSITE.ALL || x.VisibleSite == siteFilter);
        IQueryable<Product> query = oSession.QueryDataElement<Product>()
                                                   .Where(predicate)
                                                   .OrderBy(x => x.ProductName);
        var oProducts = query.ToList();
        oProducts.Action(x =>
        {
          x.PostProcess(vitaaidCategory, allergyCategory, oSession);
          if (string.IsNullOrWhiteSpace(x.RepresentativeImage))
            x.RepresentativeImage = "EmptyProduct.png";
          if (string.IsNullOrWhiteSpace(x.RepresentativeLargeImage))
            x.RepresentativeLargeImage = "EmptyProduct@2x.png";

        });
        HttpContext.Current.Session["Products"] = oProducts;
        return oProducts;
      }
      catch (Exception)
      {
        throw;
      }
      finally
      {
        oSession.Close();
      }
    }
    public static List<Product> GetProducts(eWEBSITE siteFilter = eWEBSITE.ALL)
    {
      if (HttpContext.Current.Session["Products"] == null)
        ReloadProducts(siteFilter).FirstOrDefault()?.Let(x => GetImages(x.ID, (s) => System.Web.HttpContext.Current.Server.MapPath(s)));

      return HttpContext.Current.Session["Products"] as List<Product>;
    }
    public static void AddNewProduct(Product p, SessionProxy oSession)
    {
      (HttpContext.Current.Session["User"] as ApplicationUser)?.Let(x => DataElement.sDefaultUserID = x.UserName);
      ITransaction xact = null;
      try
      {
        xact = oSession.BeginTransaction();

        p.oIngredients?.Action(y => y.Also(t => t.oProduct = p).SaveObj(oSession));
        p.oProductImages?.Action(y => y.Also(t => t.oProduct = p).SaveObj(oSession));
        //p.PreAdd();
        p.TimeStamp = DateTime.Now;
        p.SaveObj(oSession);
        xact.Commit();
        HttpContext.Current.Session["Products"] = null;
      }
      catch (Exception ex)
      {
        xact?.Rollback();
      }
    }

    public static void UpdateProduct(Product oDirtyProduct, SessionProxy oSession)
    {
      (HttpContext.Current.Session["User"] as ApplicationUser)?.Let(x => DataElement.sDefaultUserID = x.UserName);
      ITransaction xact = null;
      bool bRebuildSequence = false;
      try
      {
        xact = oSession.BeginTransaction();
        oDirtyProduct.oIngredients?.Action(x =>
        {
          if (x.iState == MySystem.Base.eOPSTATE.INIT)
            return;
          x.oProduct = oDirtyProduct;
          if (x.iState == MySystem.Base.eOPSTATE.DELETE)
          {
            x.DeleteObj(oSession);
            bRebuildSequence = true;
          }
          else
            x.SaveObj(oSession);
        });
        oDirtyProduct.oProductImages?.Action(x =>
        {
          if (x.iState == MySystem.Base.eOPSTATE.INIT)
            return;
          x.oProduct = oDirtyProduct;
          if (x.iState == MySystem.Base.eOPSTATE.DELETE)
            x.DeleteObj(oSession);
          else
            x.SaveObj(oSession);
        });
        oDirtyProduct.SaveObj(oSession);
        xact.Commit();
        if (bRebuildSequence)
        {
          xact = oSession.BeginTransaction();
          oSession.QueryDataElement<Ingredient>()
              .Where(x => x.oProduct.ID == oDirtyProduct.ID && x.GroupNo != 5)
              .OrderBy(x => x.Sequence)
              .ToList()
              .ForEachWithIndex((x, idx) =>
              {
                x.Sequence = idx + 1;
                x.SaveObj(oSession);
              });
          xact.Commit();
        }
        HttpContext.Current.Session["Products"] = null;
      }
      catch (Exception ex)
      {
        xact?.Rollback();
      }
    }

    public static void DeleteProducts(List<long> ids)
    {
      (HttpContext.Current.Session["User"] as ApplicationUser)?.Let(x => DataElement.sDefaultUserID = x.UserName);
      var oSession = DBServer[eST.SESSION0];
      ITransaction xact = null;
      try
      {
        oSession.Clear();
        var oProducts = oSession.QueryDataElement<Product>().Where(x => ids.Contains(x.ID)).ToList();
        xact = oSession.BeginTransaction();
        oProducts.Action(x => x.DeleteObj(oSession));
        xact.Commit();
        HttpContext.Current.Session["Products"] = null;
      }
      catch (Exception ex)
      {
        xact?.Rollback();
        throw ex;
      }
      finally
      {
        oSession.Close();
      }
    }
    public static IList<ProductImage> GetImages(int productID, Func<string, string> MapPath)
    {
      try
      {
        var BrokenFileName = MapPath(EnvHelper.BASE_DIR + @"broken.png");
        var images = ProductHelper.GetProducts().Where(x => x.ID == productID).ToList().UniqueOrDefault().oProductImages;

        images?.Action(image =>
        {
          image.NormalImage = (string.IsNullOrWhiteSpace(image.ImageName)) ? null : FileHelper.ReadOutBinary(MapPath(EnvHelper.PRODUCT_DIR + image.ImageName), BrokenFileName);
          image.LargeImage = (string.IsNullOrWhiteSpace(image.LargeImageName)) ? null : FileHelper.ReadOutBinary(MapPath(EnvHelper.PRODUCT_DIR + image.LargeImageName), BrokenFileName);
        });
        return images;
      }
      catch (Exception)
      {

        throw;
      }
    }
    public static bool SaveImage(byte[] data, string fileName, Func<string, string> MapPath)
    {
      try
      {
        if (string.IsNullOrEmpty(fileName)) return false;
        return FileHelper.BinaryWriteTo(data, MapPath(EnvHelper.PRODUCT_DIR + fileName));
      }
      catch (Exception)
      {
        return false;
      }
    }
    public static bool DeleteImage(string fileName, Func<string, string> MapPath)
    {
      try
      {
        if (string.IsNullOrWhiteSpace(fileName)) return false;
        var filePath = MapPath(EnvHelper.PRODUCT_DIR + fileName);
        if (File.Exists(filePath))
          File.Delete(filePath);
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public static void UpdateProductImage(int productID, ProductImage image, SessionProxy oSession)
    {
      (HttpContext.Current.Session["User"] as ApplicationUser)?.Let(x => DataElement.sDefaultUserID = x.UserName);
      ITransaction xact = null;
      try
      {
        GetProducts().Where(x => x.ID == productID)
                     .UniqueOrDefault()
                    ?.Also(p =>
                     {
                       image.oProduct = p;
                       xact = oSession.BeginTransaction();
                       image.SaveObj(oSession);
                       xact.Commit();
                     });
        ReloadProducts();
      }
      catch (Exception)
      {
        xact?.Rollback();
      }
    }
    public static void DeleteProductImage(ProductImage image, SessionProxy oSession)
    {
      (HttpContext.Current.Session["User"] as ApplicationUser)?.Let(x => DataElement.sDefaultUserID = x.UserName);
      ITransaction xact = null;
      try
      {
        oSession.QueryDataElement<ProductImage>().Where(x => x.ID == image.ID)
                                                  .FirstOrDefault()
                                                  ?.Also(x =>
                                                  {
                                                    xact = oSession.BeginTransaction();
                                                    x.DeleteObj(oSession);
                                                    xact.Commit();
                                                  });
        ReloadProducts();
      }
      catch (Exception)
      {
        xact?.Rollback();
        throw;
      }
    }
    public static void UpdateProductIngredient(int productID, Ingredient ingredient, SessionProxy oSession)
    {
      (HttpContext.Current.Session["User"] as ApplicationUser)?.Let(x => DataElement.sDefaultUserID = x.UserName);
      ITransaction xact = null;
      try
      {
        oSession.QueryDataElement<Ingredient>()
                 .Where(x => x.ID == ingredient.ID)
                 .UniqueOrDefault()
                 ?.Also(x =>
                 {
                   xact = oSession.BeginTransaction();
                   x.Sequence = ingredient.Sequence;
                   x.Name = ingredient.Name;
                   x.LabelClaim = ingredient.LabelClaim;
                   x.AdditionalInfo = ingredient.AdditionalInfo;
                   x.GroupNo = ingredient.GroupNo;
                   x.SaveObj(oSession);
                   xact.Commit();
                 });
        ReloadProducts();
      }
      catch (Exception)
      {
        xact?.Rollback();
      }
    }
    public static void DeleteProductIngredient(Ingredient ingredient, SessionProxy oSession)
    {
      (HttpContext.Current.Session["User"] as ApplicationUser)?.Let(x => DataElement.sDefaultUserID = x.UserName);
      ITransaction xact = null;
      try
      {
        oSession.QueryDataElement<Ingredient>().Where(x => x.ID == ingredient.ID)
                                                  .FirstOrDefault()
                                                  ?.Also(x =>
                                                  {
                                                    xact = oSession.BeginTransaction();
                                                    x.DeleteObj(oSession);
                                                    xact.Commit();
                                                  });
        ReloadProducts();
      }
      catch (Exception)
      {
        xact?.Rollback();
        throw;
      }
    }
  }
}