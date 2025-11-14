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
  public static class TherapeuticProtocolHelper
  {
    public static GridViewModel GetTherapeuticProtocolModel()
    {
      return new GridViewModel();
    }
    private static List<TherapeuticProtocol> ReloadTherapeuticProtocols()
    {
      var oSession = DBServer[eST.READONLY];
      try
      {
        oSession.Clear();
        var oTherapeuticProtocols = oSession.QueryDataElement<TherapeuticProtocol>()
                                                   .OrderByDescending(x => x.ID).ToList();
        HttpContext.Current.Session["TherapeuticProtocol"] = oTherapeuticProtocols;
        return oTherapeuticProtocols;
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
    public static List<TherapeuticProtocol> GetTherapeuticProtocols()
    {
      if (HttpContext.Current.Session["TherapeuticProtocol"] == null)
        ReloadTherapeuticProtocols();

      return HttpContext.Current.Session["TherapeuticProtocol"] as List<TherapeuticProtocol>;
    }
    public static void AddNewTherapeuticProtocol(TherapeuticProtocol TherapeuticProtocol, SessionProxy oSession)
    {
      (HttpContext.Current.Session["User"] as ApplicationUser)?.Let(x => DataElement.sDefaultUserID = x.UserName);

      ITransaction xact = null;
      try
      {
        xact = oSession.BeginTransaction();

        //TherapeuticProtocol.PreAdd();
        oSession.SaveObj(TherapeuticProtocol);
        xact.Commit();
        //HttpContext.Current.Session["TherapeuticProtocol"] = null;
        ReloadTherapeuticProtocols();
      }
      catch (Exception ex)
      {
        xact?.Rollback();
      }
    }

    public static void UpdateTherapeuticProtocol(TherapeuticProtocol oDirtyTherapeuticProtocol, SessionProxy oSession)
    {
      (HttpContext.Current.Session["User"] as ApplicationUser)?.Let(x => DataElement.sDefaultUserID = x.UserName);
      ITransaction xact = null;
      try
      {
        xact = oSession.BeginTransaction();
        oDirtyTherapeuticProtocol.SaveObj(oSession);
        xact.Commit();
        ReloadTherapeuticProtocols();
      }
      catch (Exception ex)
      {
        xact?.Rollback();
      }
    }

    public static void DeleteTherapeuticProtocols(List<long> ids)
    {
      (HttpContext.Current.Session["User"] as ApplicationUser)?.Let(x => DataElement.sDefaultUserID = x.UserName);

      var oSession = DBServer[eST.SESSION0];
      ITransaction xact = null;
      try
      {
        oSession.Clear();
        var oTherapeuticProtocols = oSession.QueryDataElement<TherapeuticProtocol>().Where(x => ids.Contains(x.ID)).ToList();

        xact = oSession.BeginTransaction();
        oTherapeuticProtocols.Action(x => x.DeleteObj(oSession));
        xact.Commit();
        ReloadTherapeuticProtocols();
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
    public static void UpdateImage(TherapeuticProtocol oTherapeuticProtocol, string filePath, byte[] data, string propertyName)
    {
      try
      {
        string oldValue = oTherapeuticProtocol.GetPropertyValue(propertyName) as string ?? string.Empty;
        string oldFileServerPath = HttpContext.Current.Server.MapPath(EnvHelper.PROTOCOL_DIR + oldValue);
        if ((data?.Length ?? 0) > 0)
        {
          if ((filePath?.Length ?? 0) > 0)
          {// update/replace
            bool bOverwrite = oldValue.Equals(filePath);
            if (FileHelper.BinaryWriteTo(data, HttpContext.Current.Server.MapPath(EnvHelper.PROTOCOL_DIR + filePath), bOverwrite))
            {
              oTherapeuticProtocol.SetPropertyValue(propertyName, filePath);
              oTherapeuticProtocol.iState = MySystem.Base.eOPSTATE.DIRTY;
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
            oTherapeuticProtocol.SetPropertyValue(propertyName, string.Empty);
            oTherapeuticProtocol.iState = MySystem.Base.eOPSTATE.DIRTY;
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