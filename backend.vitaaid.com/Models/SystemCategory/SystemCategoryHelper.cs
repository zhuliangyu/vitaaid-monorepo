using DevExpress.Web.Mvc;
using MyHibernateUtil;
using MyHibernateUtil.Extensions;
using MySystem.Base.Extensions;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI.WebControls;
using WebDB.DBBO;
using static backend.vitaaid.com.ServicesHelper;

namespace backend.vitaaid.com.Model
{
  public static class SystemCategoryHelper
  {
    public static GridViewModel GetSystemCategoryModel()
    {
      return new GridViewModel();
    }
    private static List<WebDB.DBBO.UnitType> ReloadSystemCategorys(eUNITTYPE? type = null)
    {
      var oSession = DBServer[eST.READONLY];
      try
      {
        oSession.Clear();
        Expression<Func<WebDB.DBBO.UnitType, bool>> predicate = ExpressionExtension.True<WebDB.DBBO.UnitType>();
        if (type != null && type.HasValue)
          predicate = predicate.And(x => x.uType == type.Value);
        IQueryable<WebDB.DBBO.UnitType> query = oSession.QueryDataElement<WebDB.DBBO.UnitType>()
                                                   .Where(predicate)
                                                   .OrderBy(x => x.ID);
        var oSystemCategorys = query.ToList();
        HttpContext.Current.Session["SystemCategorys"] = oSystemCategorys;
        return oSystemCategorys;
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
    public static List<WebDB.DBBO.UnitType> GetSystemCategorys(eUNITTYPE? type = null)
    {
      if (HttpContext.Current.Session["SystemCategorys"] == null)
        ReloadSystemCategorys(type);

      return HttpContext.Current.Session["SystemCategorys"] as List<WebDB.DBBO.UnitType>;
    }
    public static void AddNewSystemCategory(WebDB.DBBO.UnitType p, SessionProxy oSession)
    {
      (HttpContext.Current.Session["User"] as ApplicationUser)?.Let(x => DataElement.sDefaultUserID = x.UserName);
      ITransaction xact = null;
      try
      {
        xact = oSession.BeginTransaction();

        //p.PreAdd();
        oSession.SaveObj(p);
        xact.Commit();
        HttpContext.Current.Session["SystemCategorys"] = null;
      }
      catch (Exception)
      {
        xact?.Rollback();
      }
    }

    public static void UpdateSystemCategory(WebDB.DBBO.UnitType oDirtySystemCategory, SessionProxy oSession)
    {
      (HttpContext.Current.Session["User"] as ApplicationUser)?.Let(x => DataElement.sDefaultUserID = x.UserName);
      ITransaction xact = null;
      try
      {
        xact = oSession.BeginTransaction();
        oDirtySystemCategory.SaveObj(oSession);
        xact.Commit();
        HttpContext.Current.Session["SystemCategorys"] = null;
      }
      catch (Exception)
      {
        xact?.Rollback();
      }
    }

    public static void DeleteSystemCategorys(List<long> ids)
    {
      (HttpContext.Current.Session["User"] as ApplicationUser)?.Let(x => DataElement.sDefaultUserID = x.UserName);
      var oSession = DBServer[eST.SESSION0];
      ITransaction xact = null;
      try
      {
        oSession.Clear();
        var oSystemCategorys = oSession.QueryDataElement<WebDB.DBBO.UnitType>().Where(x => ids.Contains(x.ID)).ToList();

        xact = oSession.BeginTransaction();
        oSystemCategorys.Action(x => x.DeleteObj(oSession));
        xact.Commit();
        HttpContext.Current.Session["SystemCategorys"] = null;
      }
      catch (Exception)
      {
        xact?.Rollback();
      }
      finally
      {
        oSession.Close();
      }
    }
  }
}