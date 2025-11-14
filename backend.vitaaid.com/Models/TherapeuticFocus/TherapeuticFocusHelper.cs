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
using WebDB.DBPO.Helper;
using static backend.vitaaid.com.ServicesHelper;

namespace backend.vitaaid.com.Model
{
  public static class TherapeuticFocusHelper
  {
    public static GridViewModel GetTherapeuticFocusModel()
    {
      return new GridViewModel();
    }
    private static List<TherapeuticFocus> ReloadTherapeuticFocuses()
    {
      var oSession = DBServer[eST.READONLY];
      try
      {
        oSession.Clear();
        var oTherapeuticFocuses = oSession.QueryDataElement<TherapeuticFocus>()
                                                   .OrderByDescending(x => x.ID).ToList();
        UnitTypeHelper.getUnitTypes(eUNITTYPE.THERAPEUTIC_FOCUS, oSession)
            .Select(x => new { x.Name, cate_id = x.AbbrName.ToInt() })
            .ToList()
            .Also(x =>
            {
              oTherapeuticFocuses.Action(tf =>
                          tf.sCategory = x.Where(c => c.cate_id == tf.Category)
                                          .FirstOrDefault()?.Name ?? string.Empty);
            });

        HttpContext.Current.Session["TherapeuticFocus"] = oTherapeuticFocuses;
        return oTherapeuticFocuses;
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
    public static List<TherapeuticFocus> GetTherapeuticFocuses()
    {
      if (HttpContext.Current.Session["TherapeuticFocus"] == null)
        ReloadTherapeuticFocuses();

      return HttpContext.Current.Session["TherapeuticFocus"] as List<TherapeuticFocus>;
    }
    public static void AddNewTherapeuticFocus(TherapeuticFocus tf, SessionProxy oSession)
    {
      (HttpContext.Current.Session["User"] as ApplicationUser)?.Let(x => DataElement.sDefaultUserID = x.UserName);

      ITransaction xact = null;
      try
      {
        xact = oSession.BeginTransaction();

        //tf.PreAdd();
        oSession.SaveObj(tf);
        xact.Commit();
        //HttpContext.Current.Session["TherapeuticFocus"] = null;
        ReloadTherapeuticFocuses();
      }
      catch (Exception ex)
      {
        xact?.Rollback();
      }
    }

    public static void UpdateTherapeuticFocus(TherapeuticFocus oDirtyTherapeuticFocus, SessionProxy oSession)
    {
      (HttpContext.Current.Session["User"] as ApplicationUser)?.Let(x => DataElement.sDefaultUserID = x.UserName);
      ITransaction xact = null;
      try
      {
        xact = oSession.BeginTransaction();

        oDirtyTherapeuticFocus.SaveObj(oSession);
        xact.Commit();
        ReloadTherapeuticFocuses();
      }
      catch (Exception ex)
      {
        xact?.Rollback();
      }
    }

    public static void DeleteTherapeuticFocuses(List<long> ids)
    {
      (HttpContext.Current.Session["User"] as ApplicationUser)?.Let(x => DataElement.sDefaultUserID = x.UserName);
      var oSession = DBServer[eST.SESSION0];
      ITransaction xact = null;
      try
      {
        oSession.Clear();

        var oTherapeuticFocuses = oSession.QueryDataElement<TherapeuticFocus>().Where(x => ids.Contains(x.ID)).ToList();

        xact = oSession.BeginTransaction();
        oTherapeuticFocuses.Action(x => x.DeleteObj(oSession));
        xact.Commit();
        ReloadTherapeuticFocuses();
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
    public static void UpdateImage(TherapeuticFocus oTF, string filePath, byte[] data, string propertyName)
    {
      try
      {
        string oldValue = oTF.GetPropertyValue(propertyName) as string ?? string.Empty;
        string oldFileServerPath = HttpContext.Current.Server.MapPath(EnvHelper.BLOG_DIR + oldValue);
        if ((data?.Length ?? 0) > 0)
        {
          if ((filePath?.Length ?? 0) > 0)
          {// update/replace
            bool bOverwrite = oldValue.Equals(filePath);
            if (FileHelper.BinaryWriteTo(data, HttpContext.Current.Server.MapPath(EnvHelper.BLOG_DIR + filePath), bOverwrite))
            {
              oTF.SetPropertyValue(propertyName, filePath);
              oTF.iState = MySystem.Base.eOPSTATE.DIRTY;
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
            oTF.SetPropertyValue(propertyName, string.Empty);
            oTF.iState = MySystem.Base.eOPSTATE.DIRTY;
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