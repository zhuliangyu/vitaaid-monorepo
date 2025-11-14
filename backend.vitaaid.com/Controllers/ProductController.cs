using backend.vitaaid.com.Model;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using MyHibernateUtil;
using MySystem.Base.Extensions;
using MySystem.Base.Helpers;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebDB.DBBO;
using WebDB.DBPO;
using WebDB.DBPO.Helper;
using static backend.vitaaid.com.ServicesHelper;
using ProductHelper = backend.vitaaid.com.Model.ProductHelper;

namespace backend.vitaaid.com.Controllers
{
  public class ProductController : BaseController
  {

    HtmlEditorSettings htmlEditorSettings = new HtmlEditorSettings();
    public ActionResult Index()
    {
      if (!AuthHelper.IsAuthenticated())
        return RedirectToAction("SignIn", "Account");

      HttpContext.Session["Products"] = null;
      return View(ProductHelper.GetProducts());
    }
    public ActionResult ProductPartial()
    {
      if (!AuthHelper.IsAuthenticated())
        return RedirectToAction("SignIn", "Account");

      var products = ProductHelper.GetProducts();
      ViewData["ProductID"] = products.FirstOrDefault()?.ID ?? 0;
      return PartialView("ProductPartial", products);
    }

    public ActionResult ProductDetailsPage(long id)
    {
      ViewBag.ShowBackButton = true;
      return View(ProductHelper.GetProducts().Where(x => x.ID == id).ToList().UniqueOrDefault());
    }
    [ValidateAntiForgeryToken]
    public ActionResult ProductCustomActionPartial(string customAction)
    {
      try
      {
        if (customAction == "delete")
          SafeExecute(() => PerformDelete());
        return ProductPartial();
      }
      catch (Exception)
      {
        throw;
      }
    }
    [ValidateAntiForgeryToken]
    public ActionResult ProductAddNewPartial(Product product)
    {
      var oSession = DBServer[eST.SESSION0];
      try
      {
        oSession.Clear();
        product.Category = CategoryHelper.namesToVitaaidCategory(EditorExtension.GetValue<string>("VitaaidCategoryComboBox"), oSession);
        product.Category1 = CategoryHelper.namesToAllergyCategory(EditorExtension.GetValue<string>("AllergyCategoryComboBox"), oSession);
        return UpdateModelWithDataValidation(product, oSession, ProductHelper.AddNewProduct);
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
    public ActionResult ProductUpdatePartial(Product product)
    {
      var oSession = DBServer[eST.SESSION0];
      try
      {
        oSession.Clear();
        product.Category = CategoryHelper.namesToVitaaidCategory(EditorExtension.GetValue<string>("VitaaidCategoryComboBox"), oSession);
        product.Category1 = CategoryHelper.namesToAllergyCategory(EditorExtension.GetValue<string>("AllergyCategoryComboBox"), oSession);
        return UpdateModelWithDataValidation(product, oSession, ProductHelper.UpdateProduct);
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

    private ActionResult UpdateModelWithDataValidation(Product product, SessionProxy oSession, Action<Product, SessionProxy> updateMethod)
    {
      if (ModelState.IsValid)
        SafeExecute(() => updateMethod(product, oSession));
      else
        ViewBag.GeneralError = "Please, correct all errors.";
      return ProductPartial();
    }
    private void PerformDelete()
    {
      Request.Params["SelectedRows"]?.Also(selectedRowIds =>
      {
        if (!string.IsNullOrWhiteSpace(selectedRowIds))
          ProductHelper.DeleteProducts(selectedRowIds.Split(',').ToList().ConvertAll(id => long.Parse(id)));
      });
    }


    public ActionResult ProductEditFormPartial(long id)
    {
      var item = ProductHelper.GetProducts().Where(x => x.ID == id).ToList().UniqueOrDefault();
      return PartialView("_ProductEditPartial", item);
    }
    public ActionResult ProductEditFormUpdatePartial(Product p, long id)
    {
      var oSession = DBServer[eST.SESSION0];
      try
      {
        oSession.Clear();

        var item = ProductHelper.GetProducts().Where(x => x.ID == id).ToList().UniqueOrDefault();
        item.ProductCode = p.ProductCode;
        //item.ProductName = p.ProductName;
        item.Category = CategoryHelper.namesToVitaaidCategory(EditorExtension.GetValue<string>("VitaaidCategoryComboBox"), oSession);
        item.Category1 = CategoryHelper.namesToAllergyCategory(EditorExtension.GetValue<string>("AllergyCategoryComboBox"), oSession);
        item.Size = p.Size;
        item.NPN = p.NPN;
        item.Tags = p.Tags;
        item.Published = p.Published;
        item.VisibleSite = p.VisibleSite;
        item.ProductSheet = p.ProductSheet;
        item.Featured = p.Featured;

        item.iState = MySystem.Base.eOPSTATE.DIRTY;
        SafeExecute(() => ProductHelper.UpdateProduct(item, oSession));
        return new ContentResult();//HtmlEditorExtension.GetCustomDataCallbackResult("");
      }
      catch (Exception)
      {
        throw;
      }
      finally
      {
        oSession.Close();
      }

      //var item = ProductHelper.GetProducts().Where(x => x.ID == id).ToList().UniqueOrDefault();
      //return PartialView("_ProductEditPartial", p);
    }

    public ActionResult DragAndDropFileUpload([ModelBinder(typeof(DragAndDropSupportBinder))] IEnumerable<UploadedFile> ucDragAndDrop)
    {
      var logger = NLog.LogManager.GetCurrentClassLogger();//Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
      logger.Info("DragAndDropFileUpload - " + ucDragAndDrop.FirstOrDefault()?.FileNameInStorage ?? "eMPTY ...");
      List<UploadedFile> uploadedFilesList = ucDragAndDrop.ToList();
      var fileName = EditorExtension.GetValue<string>("FileNameInServer_hiddenField");
      uploadedFilesList?.FirstOrDefault().Also(x =>
      {
        FileHelper.BinaryWriteTo(x.FileBytes, Server.MapPath(EnvHelper.PRODUCT_SHEET_DIR + fileName));
      });
      logger.Info("DragAndDropFileUpload - " + Server.MapPath(EnvHelper.PRODUCT_SHEET_DIR + fileName));
      return null;
    }

    // ----------- BEGIN Therapeutic images -----------
    public ActionResult BinaryImagePhotoUpdate()
    {

      var result = BinaryImageEditExtension.GetCallbackResult();
      return result;
    }


    /*
    public ActionResult HtmlEditorPart(Product product, string type)
    {
        return PartialView("Product" + type+ "HtmlEditor", product);
    }
    */
    public ActionResult HtmlEditorPart(long id, string type)
    {
      var item = ProductHelper.GetProducts().Where(x => x.ID == id).ToList().UniqueOrDefault();
      return PartialView("Product" + (type == "ProductName" ? "Name" : type) + "HtmlEditor", item);
    }
    public ActionResult HtmlEditorUpdatePart(string htmlData, long id, string type)
    {
      var oSession = DBServer[eST.SESSION0];
      try
      {
        var item = ProductHelper.GetProducts().Where(x => x.ID == id).ToList().UniqueOrDefault();
        if (type.Equals("Description")) item.Description = htmlData;
        else if (type.Equals("ProductName")) item.ProductName = htmlData;
        else if (type.Equals("Caution")) item.Caution = htmlData;
        else if (type.Equals("Suggest")) item.Suggest = htmlData;
        else if (type.Equals("Function"))
          item.Function = htmlData;

        oSession.Clear();
        item.iState = MySystem.Base.eOPSTATE.DIRTY;
        SafeExecute(() => ProductHelper.UpdateProduct(item, oSession));

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

    // ----------- BEGIN product images -----------
    //public ActionResult SetViewData(string target, string value)
    //{
    //    ViewData[target] = value;
    //    return new EmptyResult();
    //}
    public ActionResult ProductImageWithForm(int ProductID)
    {
      ViewData["ProductID"] = ProductID;
      var images = ProductHelper.GetImages(ProductID, (s) => Server.MapPath(s));
      return PartialView("_ProductImageWithForm", images);
    }
    public ActionResult ProductImageDetailPartial(int ProductID)
    {
      ViewData["ProductID"] = ProductID;
      var images = ProductHelper.GetImages(ProductID, (s) => Server.MapPath(s));
      return PartialView("_ProductImagePartial", images);
    }
    public ActionResult BinaryImageColumnPhotoUpdate(string Target)
    {

      var result = BinaryImageEditExtension.GetCallbackResult();
      return result;
    }

    //[ValidateAntiForgeryToken]
    //public ActionResult ProductImageAddNewPartial(int ProductID, ProductImage productImage)
    //{
    //    return UpdateModelWithDataValidation(ProductID, productImage, ProductHelper.UpdateProductImage);
    //}
    [ValidateAntiForgeryToken]
    public ActionResult ProductImageUpdatePartial(int ProductID, ProductImage productImage)
    {
      var oSession = DBServer[eST.SESSION0];
      try
      {
        oSession.Clear();
        EditorExtension.GetValue<string>("NormalImage_hiddenFieldID")?.Also(x =>
        {
          if (ProductHelper.SaveImage(productImage.NormalImage, x, (s) => Server.MapPath(s)))
            productImage.ImageName = x;

        });
        EditorExtension.GetValue<string>("LargeImage_hiddenFieldID")?.Also(x =>
        {
          if (string.IsNullOrWhiteSpace(x) == false &&
                    ProductHelper.SaveImage(productImage.LargeImage, "large/" + x, (s) => Server.MapPath(s)))
            productImage.LargeImageName = "large/" + x;
        });
        if ((productImage.NormalImage?.Length ?? 0) == 0)
        {
          ProductHelper.DeleteImage(productImage.ImageName, (s) => Server.MapPath(s));
          productImage.ImageName = string.Empty;
        }
        if ((productImage.LargeImage?.Length ?? 0) == 0)
        {
          ProductHelper.DeleteImage(productImage.LargeImageName, (s) => Server.MapPath(s));
          productImage.LargeImageName = string.Empty;
        }
        return UpdateModelWithDataValidation(ProductID, productImage, oSession, ProductHelper.UpdateProductImage);
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
    public ActionResult ProductIamgeDeletePartial(int ProductID, ProductImage productImage)
    {
      var oSession = DBServer[eST.SESSION0];
      try
      {
        oSession.Clear();
        SafeExecute(() => ProductHelper.DeleteProductImage(productImage, oSession));
        return ProductImageDetailPartial(ProductID);
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


    private ActionResult UpdateModelWithDataValidation(int ProductID, ProductImage productImage, SessionProxy oSession, Action<int, ProductImage, SessionProxy> updateMethod)
    {
      //if (ModelState.IsValid)
      SafeExecute(() => updateMethod(ProductID, productImage, oSession));
      //else
      //ViewBag.GeneralError = "Please, correct all errors.";
      return ProductImageDetailPartial(ProductID);
    }
    // ----------- END product images -----------


    // BEGIN INGREDIENT
    public ActionResult ProductIngredientPartial(int ProductID)
    {
      ViewData["ProductID"] = ProductID;
      var oIngredients = ProductHelper.GetProducts().Where(x => x.ID == ProductID).UniqueOrDefault()?.oIngredients;
      return PartialView("_ProductIngredientPartial", oIngredients);
    }
    public ActionResult ProductIngredientUpdatePartial(int ProductID, Ingredient ingredient)
    {
      var oSession = DBServer[eST.SESSION0];
      try
      {
        oSession.Clear();
        ViewData["ProductID"] = ProductID;
        return UpdateModelWithDataValidation(ProductID, ingredient, oSession, ProductHelper.UpdateProductIngredient);
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
    public ActionResult ProductIngreditntDeletePartial(int ProductID, Ingredient ingredient)
    {
      var oSession = DBServer[eST.SESSION0];
      try
      {
        oSession.Clear();
        SafeExecute(() => ProductHelper.DeleteProductIngredient(ingredient, oSession));
        return ProductIngredientPartial(ProductID);
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
    private ActionResult UpdateModelWithDataValidation(int ProductID, Ingredient ingredient, SessionProxy oSession, Action<int, Ingredient, SessionProxy> updateMethod)
    {
      try
      {
          //if(ModelState.IsValid)
          SafeExecute(() => updateMethod(ProductID, ingredient, oSession));
        //else
        //    ViewBag.GeneralError = "Please, correct all errors.";
        return ProductIngredientPartial(ProductID);
      }
      catch (Exception)
      {
        throw;
      }
    }

  }
}