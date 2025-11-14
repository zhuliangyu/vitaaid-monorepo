using backend.vitaaid.com.Model;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using MySystem.Base.Extensions;
using MySystem.Base.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebDB.DBBO;
using WebDB.DBPO;
using MyHibernateUtil;
using WebDB.DBPO.Helper;
using static backend.vitaaid.com.ServicesHelper;

namespace backend.vitaaid.com.Controllers
{
  public class TherapeuticProtocolController : BaseController
  {
    HtmlEditorSettings htmlEditorSettings = new HtmlEditorSettings();
    // GET: TherapeuticProtocol
    public ActionResult Index()
    {
      if (!AuthHelper.IsAuthenticated())
        return RedirectToAction("SignIn", "Account");

      HttpContext.Session["TherapeuticProtocol"] = null;
      return View(TherapeuticProtocolHelper.GetTherapeuticProtocols());
    }

    public ActionResult TherapeuticProtocolPartial()
    {
      if (!AuthHelper.IsAuthenticated())
        return RedirectToAction("SignIn", "Account");

      var TherapeuticProtocols = TherapeuticProtocolHelper.GetTherapeuticProtocols();
      ViewData["TherapeuticProtocolID"] = TherapeuticProtocols.FirstOrDefault()?.ID ?? 0;
      return PartialView("TherapeuticProtocolPartial", TherapeuticProtocols);
    }

    [ValidateAntiForgeryToken]
    public ActionResult TherapeuticProtocolCustomActionPartial(string customAction)
    {
      if (customAction == "delete")
        SafeExecute(() => PerformDelete());
      return TherapeuticProtocolPartial();
    }
    [ValidateAntiForgeryToken]
    public ActionResult TherapeuticProtocolAddNewPartial(TherapeuticProtocol TherapeuticProtocols)
    {
      //if (string.IsNullOrWhiteSpace(TherapeuticProtocols.sCategory) == false)
      //    TherapeuticProtocols.Category = UnitTypeHelper.getUnitType(eUNITTYPE.THERAPEUTIC_FOCUS, TherapeuticProtocols.sCategory).AbbrName.ToInt();
      var oSession = DBServer[eST.SESSION0];
      try
      {
        oSession.Clear();
        return UpdateModelWithDataValidation(TherapeuticProtocols, oSession, TherapeuticProtocolHelper.AddNewTherapeuticProtocol);
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
    public ActionResult TherapeuticProtocolUpdatePartial(TherapeuticProtocol TherapeuticProtocols)
    {
      var oSession = DBServer[eST.SESSION0];
      try
      {
        oSession.Clear();
        return UpdateModelWithDataValidation(TherapeuticProtocols, oSession, TherapeuticProtocolHelper.UpdateTherapeuticProtocol);
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

    private ActionResult UpdateModelWithDataValidation(TherapeuticProtocol TherapeuticProtocols, SessionProxy oSession, Action<TherapeuticProtocol, SessionProxy> updateMethod)
    {
      if (ModelState.IsValid)
        SafeExecute(() => updateMethod(TherapeuticProtocols, oSession));
      else
        ViewBag.GeneralError = "Please, correct all errors.";
      return TherapeuticProtocolPartial();
    }
    private void PerformDelete()
    {
      Request.Params["SelectedRows"]?.Also(selectedRowIds =>
      {
        if (!string.IsNullOrWhiteSpace(selectedRowIds))
          TherapeuticProtocolHelper.DeleteTherapeuticProtocols(selectedRowIds.Split(',').ToList().ConvertAll(id => long.Parse(id)));
      });
    }


    public ActionResult TherapeuticProtocolEditFormPartial(long id)
    {
      var item = TherapeuticProtocolHelper.GetTherapeuticProtocols().Where(x => x.ID == id).ToList().UniqueOrDefault();
      item?.Also(x =>
      {
        var BrokenFileName = Server.MapPath(EnvHelper.BASE_DIR + @"broken.png");
        x.BannerImage = (string.IsNullOrWhiteSpace(x.Banner)) ? null : FileHelper.ReadOutBinary(Server.MapPath(EnvHelper.PROTOCOL_DIR + x.Banner), BrokenFileName);
      });

      return PartialView("_TherapeuticProtocolEditPartial", item);
    }

    public ActionResult TherapeuticProtocolEditFormUpdatePartial(TherapeuticProtocol oTherapeuticProtocol, long id)
    {
      var oSession = DBServer[eST.SESSION0];
      try
      {
        oSession.Clear();

        var item = TherapeuticProtocolHelper.GetTherapeuticProtocols().Where(x => x.ID == id).ToList().UniqueOrDefault();
        //if (string.IsNullOrWhiteSpace(oTherapeuticProtocol.sCategory) == false)
        //    item.Category = UnitTypeHelper.getUnitType(eUNITTYPE.THERAPEUTIC_FOCUS, oTherapeuticProtocol.sCategory).AbbrName.ToInt();
        item.Author = oTherapeuticProtocol.Author;
        item.Issue = oTherapeuticProtocol.Issue;
        item.Topic = oTherapeuticProtocol.Topic;
        item.Tags = oTherapeuticProtocol.Tags;
        item.PDFFile = oTherapeuticProtocol.PDFFile;
        item.Published = oTherapeuticProtocol.Published;
        item.VisibleSite = oTherapeuticProtocol.VisibleSite;
        item.iState = MySystem.Base.eOPSTATE.DIRTY;
        EditorExtension.GetValue<string>("BannerImage_hiddenFieldID")?.Also(x =>
        {
          SafeExecute(() => TherapeuticProtocolHelper.UpdateImage(item, x, oTherapeuticProtocol.BannerImage, "Banner"));
        });

        SafeExecute(() => TherapeuticProtocolHelper.UpdateTherapeuticProtocol(item, oSession));
        return new ContentResult();
      }
      catch (Exception)
      {
        throw;
      }
      finally {
        oSession.Close();
      }
    }


    // *************** HTML EDITOR ********************
    public ActionResult HtmlEditorPart(long id, string type)
    {
      var item = TherapeuticProtocolHelper.GetTherapeuticProtocols().Where(x => x.ID == id).ToList().UniqueOrDefault();
      return PartialView("TherapeuticProtocol" + type + "HtmlEditor", item);
    }
    public ActionResult HtmlEditorUpdatePart(string htmlData, long id, string type)
    {
      var oSession = DBServer[eST.SESSION0];
      try
      {
        oSession.Clear();
        var item = TherapeuticProtocolHelper.GetTherapeuticProtocols().Where(x => x.ID == id).ToList().UniqueOrDefault();
        if (type.Equals("Content")) item.BlogContent = htmlData;
        item.iState = MySystem.Base.eOPSTATE.DIRTY;
        SafeExecute(() => TherapeuticProtocolHelper.UpdateTherapeuticProtocol(item, oSession));

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

    public ActionResult DragAndDropFileUpload([ModelBinder(typeof(DragAndDropSupportBinder))] IEnumerable<UploadedFile> ucDragAndDrop)
    {
      List<UploadedFile> uploadedFilesList = ucDragAndDrop.ToList();
      var fileName = EditorExtension.GetValue<string>("FileNameInServer_hiddenField");
      uploadedFilesList?.FirstOrDefault().Also(x =>
      {
        FileHelper.BinaryWriteTo(x.FileBytes, Server.MapPath(EnvHelper.PROTOCOL_DIR + fileName));
      });
      return null;
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
