using backend.vitaaid.com.Model;
using MySystem.Base.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebDB.DBPO.Helper;
using static backend.vitaaid.com.ServicesHelper;
using MyHibernateUtil;

namespace backend.vitaaid.com.Controllers
{
  public class SystemCategoryController : BaseController
  {
    // GET: SystemCategory
    public ActionResult Index()
    {
      return View(SystemCategoryHelper.GetSystemCategorys());
    }
    public ActionResult SystemCategoryPartial()
    {
      return PartialView("SystemCategoryPartial", SystemCategoryHelper.GetSystemCategorys());
    }
    [ValidateAntiForgeryToken]
    public ActionResult SystemCategoryCustomActionPartial(string customAction)
    {
      if (customAction == "delete")
        SafeExecute(() => PerformDelete());
      return SystemCategoryPartial();
    }
    [ValidateAntiForgeryToken]
    public ActionResult SystemCategoryAddNewPartial(WebDB.DBBO.UnitType oType)
    {
      var oSession = DBServer[eST.SESSION0];
      try
      {
        oSession.Clear();
        return UpdateModelWithDataValidation(oType, oSession, SystemCategoryHelper.AddNewSystemCategory);
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
    [ValidateAntiForgeryToken]
    public ActionResult SystemCategoryUpdatePartial(WebDB.DBBO.UnitType oType)
    {
      var oSession = DBServer[eST.SESSION0];
      try
      {
        oSession.Clear();
        return UpdateModelWithDataValidation(oType, oSession, SystemCategoryHelper.UpdateSystemCategory);
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

    private ActionResult UpdateModelWithDataValidation(WebDB.DBBO.UnitType oType, SessionProxy oSession,  Action<WebDB.DBBO.UnitType, SessionProxy> updateMethod)
    {
      if (ModelState.IsValid)
        SafeExecute(() => updateMethod(oType, oSession));
      else
        ViewBag.GeneralError = "Please, correct all errors.";
      return SystemCategoryPartial();
    }
    private void PerformDelete()
    {
      if (!string.IsNullOrEmpty(Request.Params["SelectedRows"]))
      {
        List<long> selectedIds = Request.Params["SelectedRows"].Split(',').ToList().ConvertAll(id => long.Parse(id));
        SystemCategoryHelper.DeleteSystemCategorys(selectedIds);
      }
    }
  }
}