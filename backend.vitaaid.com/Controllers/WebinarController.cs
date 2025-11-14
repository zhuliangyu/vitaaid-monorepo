using backend.vitaaid.com.Model;
using DevExpress.Web.Mvc;
using MySystem.Base.Extensions;
using MySystem.Base.Helpers;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebDB.DBBO;
using WebDB.DBPO;
using WebDB.DBPO.Helper;
using static backend.vitaaid.com.ServicesHelper;
using MyHibernateUtil;

namespace backend.vitaaid.com.Controllers
{
  public class WebinarController : BaseController
  {
    HtmlEditorSettings htmlEditorSettings = new HtmlEditorSettings();
    // GET: Webinar
    public ActionResult Index()
    {
      if (!AuthHelper.IsAuthenticated())
        return RedirectToAction("SignIn", "Account");

      HttpContext.Session["Webinar"] = null;
      return View(WebinarHelper.GetWebinares());
    }

    public ActionResult WebinarPartial()
    {
      if (!AuthHelper.IsAuthenticated())
        return RedirectToAction("SignIn", "Account");

      var webinars = WebinarHelper.GetWebinares();
      ViewData["WebinarID"] = webinars.FirstOrDefault()?.ID ?? 0;
      return PartialView("WebinarPartial", webinars);
    }

    [ValidateAntiForgeryToken]
    public ActionResult WebinarCustomActionPartial(string customAction)
    {
      if (customAction == "delete")
        SafeExecute(() => PerformDelete());
      return WebinarPartial();
    }
    [ValidateAntiForgeryToken]
    public ActionResult WebinarAddNewPartial(Webinar webinars)
    {
      var oSession = DBServer[eST.SESSION0];
      try
      {
        oSession.Clear();
        //if (string.IsNullOrWhiteSpace(webinars.sCategory) == false)
        //    webinars.Category = UnitTypeHelper.getUnitType(eUNITTYPE.THERAPEUTIC_FOCUS, webinars.sCategory).AbbrName.ToInt();
        return UpdateModelWithDataValidation(webinars, oSession, WebinarHelper.AddNewWebinar);
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
    public ActionResult WebinarUpdatePartial(Webinar webinars)
    {
      var oSession = DBServer[eST.SESSION0];
      try
      {
        oSession.Clear();
        return UpdateModelWithDataValidation(webinars, oSession, WebinarHelper.UpdateWebinar);

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

    private ActionResult UpdateModelWithDataValidation(Webinar webinars, SessionProxy oSession, Action<Webinar, SessionProxy> updateMethod)
    {
      if (ModelState.IsValid)
        SafeExecute(() => updateMethod(webinars, oSession));
      else
        ViewBag.GeneralError = "Please, correct all errors.";
      return WebinarPartial();
    }
    private void PerformDelete()
    {
      Request.Params["SelectedRows"]?.Also(selectedRowIds =>
      {
        if (!string.IsNullOrWhiteSpace(selectedRowIds))
          WebinarHelper.DeleteWebinares(selectedRowIds.Split(',').ToList().ConvertAll(id => long.Parse(id)));
      });
    }


    public ActionResult WebinarEditFormPartial(long id)
    {
      var item = WebinarHelper.GetWebinares().Where(x => x.ID == id).ToList().UniqueOrDefault();
      item?.Also(x =>
      {
        var BrokenFileName = Server.MapPath(EnvHelper.BASE_DIR + @"broken.png");
        x.ThumbnailImage = (string.IsNullOrWhiteSpace(x.Thumbnail)) ? null : FileHelper.ReadOutBinary(Server.MapPath(EnvHelper.WEBINAR_DIR + x.Thumbnail), BrokenFileName);
      });

      return PartialView("_WebinarEditPartial", item);
    }

    public ActionResult WebinarEditFormUpdatePartial(Webinar oWebinar, long id)
    {
      var oSession = DBServer[eST.SESSION0];
      try
      {
        oSession.Clear();

        var item = WebinarHelper.GetWebinares().Where(x => x.ID == id).ToList().UniqueOrDefault();
        //if (string.IsNullOrWhiteSpace(oWebinar.sCategory) == false)
        //    item.Category = UnitTypeHelper.getUnitType(eUNITTYPE.THERAPEUTIC_FOCUS, oWebinar.sCategory).AbbrName.ToInt();
        item.Issue = oWebinar.Issue;
        item.Reference = oWebinar.Reference;
        item.Author = oWebinar.Author;
        item.Topic = oWebinar.Topic;
        item.VideoLink = oWebinar.VideoLink;
        item.Tags = oWebinar.Tags;
        item.MaxRegistrants = oWebinar.MaxRegistrants;
        item.Published = oWebinar.Published;
        item.iState = MySystem.Base.eOPSTATE.DIRTY;
        EditorExtension.GetValue<string>("ThumbnailImage_hiddenFieldID")?.Also(x =>
        {
          SafeExecute(() => WebinarHelper.UpdateImage(item, x, oWebinar.ThumbnailImage, "Thumbnail"));
        });

        SafeExecute(() => WebinarHelper.UpdateWebinar(item, oSession));
        return new ContentResult();
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


    // *************** HTML EDITOR ********************
    public ActionResult HtmlEditorPart(long id, string type)
    {
      var item = WebinarHelper.GetWebinares().Where(x => x.ID == id).ToList().UniqueOrDefault();
      return PartialView("Webinar" + type + "HtmlEditor", item);
    }
    public ActionResult HtmlEditorUpdatePart(string htmlData, long id, string type)
    {
      var oSession = DBServer[eST.SESSION0];
      try
      {
        oSession.Clear();
        var item = WebinarHelper.GetWebinares().Where(x => x.ID == id).ToList().UniqueOrDefault();
        if (type.Equals("Content")) item.WebinarContent = htmlData;
        item.iState = MySystem.Base.eOPSTATE.DIRTY;
        SafeExecute(() => WebinarHelper.UpdateWebinar(item, oSession));

        return HtmlEditorExtension.GetCustomDataCallbackResult(string.Empty);
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


    //public ActionResult HtmlEditorPartialImageUpload(string EditorName)
    //{
    //    HtmlEditorExtension.SaveUploadedFile(EditorName, HtmlEditorFileSettings.ImageUploadValidationSettings, HtmlEditorFileSettings.ImageUploadDirectory);
    //    return null;
    //}

    //public ActionResult HtmlEditorPartialImageSelectorUpload(string EditorName)
    //{
    //    HtmlEditorExtension.SaveUploadedImage(EditorName, HtmlEditorFileSettings.ImageSelectorSettings);
    //    return null;
    //}

    // ----------- BEGIN Therapeutic images -----------
    public ActionResult BinaryImagePhotoUpdate()
    {

      var result = BinaryImageEditExtension.GetCallbackResult();
      return result;
    }

    // ----------- END product images -----------

  }
}