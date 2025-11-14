using backend.vitaaid.com.Code;
using backend.vitaaid.com.Model;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using MIS.DBBO;
using MyHibernateUtil;
using MyHibernateUtil.Extensions;
using MySystem.Base;
using MySystem.Base.Extensions;
using MySystem.Base.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebDB.DBBO;
using WebDB.DBPO;
using WebDB.DBPO.Helper;
using static backend.vitaaid.com.ServicesHelper;
using static MySystem.Base.EmailAttributes;

namespace backend.vitaaid.com.Controllers
{
  public class DragAndDropSupportBinder : DevExpressEditorsBinder
  {
    public DragAndDropSupportBinder()
    {
      UploadControlBinderSettings.ValidationSettings.Assign(UploadControlHelper.UploadValidationSettings);
      UploadControlBinderSettings.FileUploadCompleteHandler = UploadControlHelper.ucDragAndDrop_FileUploadComplete;
    }
  }

  public class MemberController : BaseController
  {
    // GET: Member
    public ActionResult Index()
    {
      if (!AuthHelper.IsAuthenticated())
        return RedirectToAction("SignIn", "Account");

      HttpContext.Session["Member"] = null;
      return View(MemberHelper.GetMembers());
    }

    public ActionResult MemberPartial()
    {
      if (!AuthHelper.IsAuthenticated())
        return RedirectToAction("SignIn", "Account");

      var members = MemberHelper.GetMembers();
      ViewData["MemberID"] = members.FirstOrDefault()?.ID ?? 0;
      return PartialView("MemberPartial", members);
    }
    public ActionResult CheckAccount(string AccountNo)
    {
      try
      {
        return new ContentResult { Content = string.IsNullOrWhiteSpace(AccountNo) ? "" : MemberHelper.CheckAccount(AccountNo).ToString() };
      }
      catch (Exception)
      {

        throw;
      }
    }

    [ValidateAntiForgeryToken]
    public ActionResult MemberCustomActionPartial(string customAction)
    {
      if (customAction == "delete")
        SafeExecute(() => PerformDelete());
      return MemberPartial();
    }
    [ValidateAntiForgeryToken]
    public ActionResult MemberAddNewPartial(Member members)
    {
      var oSession = DBServer[eST.SESSION0];
      try
      {
        oSession.Clear();
        //if (string.IsNullOrWhiteSpace(members.sCategory) == false)
        //    members.Category = UnitTypeHelper.getUnitType(eUNITTYPE.THERAPEUTIC_FOCUS, members.sCategory).AbbrName.ToInt();
        return UpdateModelWithDataValidation(members, oSession, MemberHelper.AddNewMember);
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
    public ActionResult MemberUpdatePartial(Member members)
    {
      var oSession = DBServer[eST.SESSION0];
      try
      {
        oSession.Clear();
        return UpdateModelWithDataValidation(members, oSession, MemberHelper.UpdateMember);
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

    private ActionResult UpdateModelWithDataValidation(Member members, SessionProxy oSession, Action<Member, SessionProxy> updateMethod)
    {
      if (ModelState.IsValid)
        SafeExecute(() => updateMethod(members, oSession));
      else
        ViewBag.GeneralError = "Please, correct all errors.";
      return MemberPartial();
    }
    private void PerformDelete()
    {
      Request.Params["SelectedRows"]?.Also(selectedRowIds =>
      {
        if (!string.IsNullOrWhiteSpace(selectedRowIds))
          MemberHelper.DeleteMembers(selectedRowIds.Split(',').ToList().ConvertAll(id => long.Parse(id)));
      });
    }


    public ActionResult MemberEditFormPartial(long id)
    {
      var item = MemberHelper.GetMembers().Where(x => x.ID == id).ToList().UniqueOrDefault();
      return PartialView("_MemberEditPartial", item);
    }

    public ActionResult MemberEditFormUpdatePartial(Member oMember, long id)
    {
      var oSession = DBServer[eST.SESSION0];
      try
      {
        oSession.Clear();
        var item = MemberHelper.GetMembers().Where(x => x.ID == id).ToList().UniqueOrDefault();
        if (string.IsNullOrWhiteSpace(oMember.CustomerCode) == false && string.IsNullOrWhiteSpace(item.CustomerCode))
        {
          MemberHelper.AddNewAccountToInvoiceSystem(oMember);
        }

        item.MemberStatus = oMember.MemberStatus;
        item.CustomerCode = oMember.CustomerCode;
        item.Prefix = oMember.Prefix;
        item.Name = oMember.Name;
        item.FirstName = oMember.FirstName;
        item.LastName = oMember.LastName;
        item.PhysicanCode = oMember.PhysicanCode;
        item.Pat_pcode = oMember.Pat_pcode;
        item.MemberType = oMember.MemberType;
        item.PractitionerType = oMember.PractitionerType;
        item.OtherPractitionerType = oMember.OtherPractitionerType;
        item.Email = oMember.Email;
        item.Password = oMember.Password;
        item.ClinicName = oMember.ClinicName;
        item.bReferral = oMember.bReferral;
        item.IsSubscribe = oMember.IsSubscribe;
        item.PermittedSite = oMember.PermittedSite;
        item.Address = oMember.Address;
        item.City = oMember.City;
        item.Province = oMember.Province;
        item.Country = oMember.Country;
        item.ZipCode = oMember.ZipCode;
        item.Telephone = oMember.Telephone;
        item.Fax = oMember.Fax;
        item.SalesRep = oMember.SalesRep;
        item.LicenceNo = oMember.LicenceNo;
        item.LicencePhoto = oMember.LicencePhoto;

        item.iState = MySystem.Base.eOPSTATE.DIRTY;

        SafeExecute(() => MemberHelper.UpdateMember(item, oSession));
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
    public ActionResult SendEmailToSalesRep(int ID)
    {
      var oSession = DBServer[eST.READONLY];
      try
      {
        oSession.Clear();
        var setting = UnitTypeHelper.buildEmailAttributes(oSession);
        var mailbody = oSession.QueryDataElement<WebDB.DBBO.UnitType>()
                                .Where(x => x.uType == eUNITTYPE.EMAIL_SETTING && x.Name == "FOLLOWUP_MAIL_BODY")
                                .ToList()
                                .FirstOrDefault();
        string result = "FAIL";
        MemberHelper.GetMembers().Where(x => x.ID == ID).ToList().UniqueOrDefault()
            ?.Also(x =>
            {
              var salesReps = (ECSServerObj.RESTfullObject.listEmployeesByGroup("CC_FOLLOWUP_EMAIL")?.oData ?? new List<ECSServerObj.WS_Employee>())
                              .Where(y => !string.IsNullOrEmpty(y.Email))
                              .Select(y => y.Email)
                              .ToList();
              MemberHelper.getSalesRep().Where(m => m.ShortName == x.SalesRep).ToList().Action(m =>
              {
                if (!string.IsNullOrEmpty(m.Email) && !salesReps.Contains(m.Email))
                  salesReps.Add(m.Email);
              });

              if (salesReps.IsNullOrEmpty())
                return;
              setting[C_FROM] = "info@vitaaid.com";
              setting[C_SUBJECT] = "New member registration";
              //setting[C_RECIPIENT] = "imskchen@gmail.com";
              setting[C_RECIPIENT] = salesReps.Aggregate((a, b) => a + ";" + b);

              setting[C_BODY] = mailbody?.Comment ?? "";
              setting.processVarables(C_BODY,
                          new Dictionary<string, string> { { "{SalesRep}", x.SalesRep },
                                                             { "{Prefix}", x.Prefix.ToString() },
                                                             { "{Name}", x.Name },
                                                             { "{Address}", x.Address },
                                                             { "{City}", x.City },
                                                             { "{Province}", x.Province },
                                                             { "{ZipCode}", x.ZipCode },
                                                             { "{Telephone}", x.Telephone },
                                                             { "{Email}", x.Email } });
              result = MailHelper.SendEmail(setting);
            });
        return new ContentResult { Content = result };
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
    public ActionResult SendActivationEmail(int ID)
    {
      var oSession = DBServer[eST.READONLY];
      try
      {
        oSession.Clear();

        var setting = UnitTypeHelper.buildEmailAttributes(oSession);
        var mailbody = oSession.QueryDataElement<WebDB.DBBO.UnitType>()
                                .Where(x => x.uType == eUNITTYPE.EMAIL_SETTING && x.Name == "ACTIVATION_MAIL_BODY")
                                .ToList()
                                .FirstOrDefault();
        string result = "FAIL";
        MemberHelper.GetMembers().Where(x => x.ID == ID).ToList().UniqueOrDefault()
            ?.Also(x =>
            {
              setting[C_FROM] = "info@vitaaid.com";
              setting[C_SUBJECT] = "Vita Aid Member Activation";
                      //setting[C_RECIPIENT] = "imskchen@gmail.com";
                      setting[C_RECIPIENT] = x.Email;
              setting[C_BODY] = mailbody?.Comment ?? "";
              setting.processVarables(C_BODY,
                          new Dictionary<string, string> { { "{Name}", x.Name }, { "{Email}", x.Email }, { "{Password}", x.Password } });
              result = MailHelper.SendEmail(setting);
            });
        return new ContentResult { Content = result };
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
        FileHelper.BinaryWriteTo(x.FileBytes, Server.MapPath(EnvHelper.MEMBER_DIR + fileName));
      });
      return null;
    }

    // ----------- BEGIN Therapeutic images -----------
    public ActionResult BinaryImagePhotoUpdate()
    {

      var result = BinaryImageEditExtension.GetCallbackResult();
      return result;
    }

    // ----------- END product images -----------

  }
}
