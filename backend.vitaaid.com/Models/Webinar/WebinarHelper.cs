using DevExpress.Web.Mvc;
using MyHibernateUtil;
using MyHibernateUtil.Extensions;
using MySystem.Base.Extensions;
using MySystem.Base.Helpers;
using MySystem.Base.Reflection;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using WebDB.DBBO;
using WebDB.DBPO;
using static backend.vitaaid.com.ServicesHelper;

namespace backend.vitaaid.com.Model
{
  public static class WebinarHelper
  {
    public static GridViewModel GetWebinarModel()
    {
      return new GridViewModel();
    }
    private static List<Webinar> ReloadWebinares()
    {
      var oSession = DBServer[eST.READONLY];
      try
      {
        oSession.Clear();
        var oWebinares = oSession.QueryDataElement<Webinar>()
                                                   .OrderByDescending(x => x.ID).ToList();
        //UnitTypeHelper.getUnitTypes(eUNITTYPE.THERAPEUTIC_FOCUS)
        //    .Select(x => new { x.Name, cate_id = x.AbbrName.ToInt()})
        //    .ToList()
        //    .Let(x =>
        //    {
        //        oWebinares.Action(webinar =>
        //            webinar.sCategory = x.Where(c => c.cate_id == webinar.Category)
        //                            .FirstOrDefault()?.Name ?? "");
        //    });

        HttpContext.Current.Session["Webinar"] = oWebinares;
        return oWebinares;
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
    public static List<Webinar> GetWebinares()
    {
      if (HttpContext.Current.Session["Webinar"] == null)
        ReloadWebinares();

      return HttpContext.Current.Session["Webinar"] as List<Webinar>;
    }
    public static void AddNewWebinar(Webinar webinar, SessionProxy oSession)
    {
      (HttpContext.Current.Session["User"] as ApplicationUser)?.Let(x => DataElement.sDefaultUserID = x.UserName);

      ITransaction xact = null;
      try
      {
        xact = oSession.BeginTransaction();

        //webinar.PreAdd();
        oSession.SaveObj(webinar);
        xact.Commit();
        //HttpContext.Current.Session["Webinar"] = null;
        ReloadWebinares();
      }
      catch (Exception ex)
      {
        xact?.Rollback();
      }
    }

    public static void UpdateWebinar(Webinar oDirtyWebinar, SessionProxy oSession)
    {
      (HttpContext.Current.Session["User"] as ApplicationUser)?.Let(x => DataElement.sDefaultUserID = x.UserName);
      ITransaction xact = null;
      try
      {
        xact = oSession.BeginTransaction();
        oDirtyWebinar.SaveObj(oSession);
        xact.Commit();
        ReloadWebinares();
      }
      catch (Exception ex)
      {
        xact?.Rollback();
      }
    }

    public static void DeleteWebinares(List<long> ids)
    {
      (HttpContext.Current.Session["User"] as ApplicationUser)?.Let(x => DataElement.sDefaultUserID = x.UserName);
      ITransaction xact = null;
      var oSession = DBServer[eST.SESSION0];
      try
      {
        oSession.Clear();
        var oWebinares = oSession.QueryDataElement<Webinar>().Where(x => ids.Contains(x.ID)).ToList();

        xact = oSession.BeginTransaction();
        oWebinares.Action(x => x.DeleteObj(oSession));
        xact.Commit();
        ReloadWebinares();
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
    public static void UpdateImage(Webinar oWebinar, string filePath, byte[] data, string propertyName)
    {
      try
      {
        string oldValue = oWebinar.GetPropertyValue(propertyName) as string ?? string.Empty;
        string oldFileServerPath = HttpContext.Current.Server.MapPath(EnvHelper.WEBINAR_DIR + oldValue);
        if ((data?.Length ?? 0) > 0)
        {
          if ((filePath?.Length ?? 0) > 0)
          {// update/replace
            bool bOverwrite = oldValue.Equals(filePath);
            if (FileHelper.BinaryWriteTo(data, HttpContext.Current.Server.MapPath(EnvHelper.WEBINAR_DIR + filePath), bOverwrite))
            {
              oWebinar.SetPropertyValue(propertyName, filePath);
              oWebinar.iState = MySystem.Base.eOPSTATE.DIRTY;
              if ((bOverwrite == false) && System.IO.File.Exists(oldFileServerPath))
                System.IO.File.Delete(oldFileServerPath);
            }
          }
        }
        else
        {
          if ((oldValue?.Length ?? 0) > 0)
          { // delete
            if (System.IO.File.Exists(oldFileServerPath))
              System.IO.File.Delete(oldFileServerPath);
            oWebinar.SetPropertyValue(propertyName, string.Empty);
            oWebinar.iState = MySystem.Base.eOPSTATE.DIRTY;
          }
        }
      }
      catch (Exception)
      {

        throw;
      }
    }
  }
}