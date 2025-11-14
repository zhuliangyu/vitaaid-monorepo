using backend.vitaaid.com.Model;
using DevExpress.Web.Mvc;
using MyHibernateUtil;
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

namespace backend.vitaaid.com.Controllers
{
  public class TherapeuticFocusController : BaseController
  {
    HtmlEditorSettings htmlEditorSettings = new HtmlEditorSettings();
    // GET: TherapeuticFocus
    public ActionResult Index()
    {
      if (!AuthHelper.IsAuthenticated())
        return RedirectToAction("SignIn", "Account");

      HttpContext.Session["TherapeuticFocus"] = null;
      return View(TherapeuticFocusHelper.GetTherapeuticFocuses());
    }

    public ActionResult TherapeuticFocusPartial()
    {
      if (!AuthHelper.IsAuthenticated())
        return RedirectToAction("SignIn", "Account");

      var tfs = TherapeuticFocusHelper.GetTherapeuticFocuses();
      ViewData["TherapeuticFocusID"] = tfs.FirstOrDefault()?.ID ?? 0;
      return PartialView("TherapeuticFocusPartial", tfs);
    }

    [ValidateAntiForgeryToken]
    public ActionResult TherapeuticFocusCustomActionPartial(string customAction)
    {
      if (customAction == "delete")
        SafeExecute(() => PerformDelete());
      return TherapeuticFocusPartial();
    }
    [ValidateAntiForgeryToken]
    public ActionResult TherapeuticFocusAddNewPartial(TherapeuticFocus tfs)
    {
      var oSession = DBServer[eST.SESSION0];
      try
      {
        oSession.Clear();
        if (string.IsNullOrWhiteSpace(tfs.sCategory) == false)
          tfs.Category = UnitTypeHelper.getUnitType(eUNITTYPE.THERAPEUTIC_FOCUS, tfs.sCategory, oSession).AbbrName.ToInt();
        return UpdateModelWithDataValidation(tfs, oSession, TherapeuticFocusHelper.AddNewTherapeuticFocus);

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
    public ActionResult TherapeuticFocusUpdatePartial(TherapeuticFocus tfs)
    {
      var oSession = DBServer[eST.SESSION0];
      try
      {
        oSession.Clear();
        return UpdateModelWithDataValidation(tfs, oSession, TherapeuticFocusHelper.UpdateTherapeuticFocus);
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

    private ActionResult UpdateModelWithDataValidation(TherapeuticFocus tfs, SessionProxy oSession, Action<TherapeuticFocus, SessionProxy> updateMethod)
    {
      if (ModelState.IsValid)
        SafeExecute(() => updateMethod(tfs, oSession));
      else
        ViewBag.GeneralError = "Please, correct all errors.";
      return TherapeuticFocusPartial();
    }
    private void PerformDelete()
    {
      Request.Params["SelectedRows"]?.Also(selectedRowIds =>
      {
        if (!string.IsNullOrWhiteSpace(selectedRowIds))
          TherapeuticFocusHelper.DeleteTherapeuticFocuses(selectedRowIds.Split(',').ToList().ConvertAll(id => long.Parse(id)));
      });
    }


    public ActionResult TherapeuticFocusEditFormPartial(long id)
    {
      var item = TherapeuticFocusHelper.GetTherapeuticFocuses().Where(x => x.ID == id).ToList().UniqueOrDefault();
      item?.Also(x =>
      {
        var BrokenFileName = Server.MapPath(EnvHelper.BASE_DIR + @"broken.png");
        x.BannerImage = (string.IsNullOrWhiteSpace(x.Banner)) ? null : FileHelper.ReadOutBinary(Server.MapPath(EnvHelper.BLOG_DIR + x.Banner), BrokenFileName);
        x.ThumbImage = (string.IsNullOrWhiteSpace(x.Thumb)) ? null : FileHelper.ReadOutBinary(Server.MapPath(EnvHelper.BLOG_DIR + x.Thumb), BrokenFileName);
      });

      return PartialView("_TherapeuticFocusEditPartial", item);
    }

    public ActionResult TherapeuticFocusEditFormUpdatePartial(TherapeuticFocus tf, long id)
    {
      var oSession = DBServer[eST.SESSION0];
      try
      {
        oSession.Clear();

        var item = TherapeuticFocusHelper.GetTherapeuticFocuses().Where(x => x.ID == id).ToList().UniqueOrDefault();
        if (string.IsNullOrWhiteSpace(tf.sCategory) == false)
          item.Category = UnitTypeHelper.getUnitType(eUNITTYPE.THERAPEUTIC_FOCUS, tf.sCategory, oSession).AbbrName.ToInt();
        item.Author = tf.Author;
        item.Issue = tf.Issue;
        item.Volume = tf.Volume;
        item.No = tf.No;
        item.Published = tf.Published;
        item.VisibleSite = tf.VisibleSite;
        item.Tags = tf.Tags;
        item.iState = MySystem.Base.eOPSTATE.DIRTY;
        EditorExtension.GetValue<string>("ThumbImage_hiddenFieldID")?.Also(x =>
        {
          SafeExecute(() => TherapeuticFocusHelper.UpdateImage(item, x, tf.ThumbImage, "Thumb"));
        });
        EditorExtension.GetValue<string>("BannerImage_hiddenFieldID")?.Also(x =>
        {
          SafeExecute(() => TherapeuticFocusHelper.UpdateImage(item, x, tf.BannerImage, "Banner"));
        });

        SafeExecute(() => TherapeuticFocusHelper.UpdateTherapeuticFocus(item, oSession));
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
      var item = TherapeuticFocusHelper.GetTherapeuticFocuses().Where(x => x.ID == id).ToList().UniqueOrDefault();
      return PartialView("TherapeuticFocus" + type + "HtmlEditor", item);
    }
    public ActionResult HtmlEditorUpdatePart(string htmlData, long id, string type)
    {
      var oSession = DBServer[eST.SESSION0];
      try
      {
        oSession.Clear();

        var item = TherapeuticFocusHelper.GetTherapeuticFocuses().Where(x => x.ID == id).ToList().UniqueOrDefault();
        if (type.Equals("Topic")) item.Topic = htmlData;
        if (type.Equals("BlogContent")) item.BlogContent = htmlData;
        if (type.Equals("Reference")) item.Reference = htmlData;
        item.iState = MySystem.Base.eOPSTATE.DIRTY;
        SafeExecute(() => TherapeuticFocusHelper.UpdateTherapeuticFocus(item, oSession));

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
    //public ActionResult SetViewData(string target, string value)
    //{
    //    ViewData[target] = value;
    //    return new EmptyResult();
    //}
    //public ActionResult ProductImageWithForm(int ProductID)
    //{
    //    ViewData["ProductID"] = ProductID;
    //    var images = ProductHelper.GetImages(ProductID, (s) => Server.MapPath(s));
    //    return PartialView("_ProductImageWithForm", images);
    //}
    //public ActionResult ProductImageDetailPartial(int ProductID)
    //{
    //    ViewData["ProductID"] = ProductID;
    //    var images = ProductHelper.GetImages(ProductID, (s) => Server.MapPath(s));
    //    return PartialView("_ProductImagePartial", images);
    //}
    public ActionResult BinaryImagePhotoUpdate()
    {

      var result = BinaryImageEditExtension.GetCallbackResult();
      return result;
    }

    //[ValidateAntiForgeryToken]
    //public ActionResult ProductImageAddNewPartial(int ProductID, ProductImage productImage)
    //{
    //    return UpdateModelWithDataValidation(ProductID, productImage, ProductHelper.UpdateProductImage);
    //}
    //[ValidateAntiForgeryToken]
    //public ActionResult TherapeuticFocusImageUpdatePartial(TherapeuticFocus tfs, string Target)
    //{
    //    if (Target == "ThumbImage")
    //        EditorExtension.GetValue<string>("ThumbImage_hiddenFieldID")?.Let(x =>
    //        {
    //            SafeExecute(() => TherapeuticFocusHelper.UpdateImage(tfs.ID, x, tfs.ThumbImage, "Thumb"));
    //        });
    //    else if (Target == "BannerImage")
    //        EditorExtension.GetValue<string>("BannerImage_hiddenFieldID")?.Let(x =>
    //        {
    //            SafeExecute(() => TherapeuticFocusHelper.UpdateImage(tfs.ID, x, tfs.BannerImage, "Banner"));
    //        });
    //    return new ContentResult();
    //}
    // ----------- END product images -----------

  }
}
