using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebDB.DBBO;
using MyHibernateUtil.Extensions;
using static vitaaid.com.ServicesHelper;
using NHibernate;
using MySystem.Base.Extensions;
using MyHibernateUtil;
using static MySystem.Base.Web.Constant;
using WebDB.DBPO.Extensions;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebDB.DBPO.Helper;
using WebDB.DBPO;
using MyToolkit.Base.County;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using MySystem.Base;
using static MySystem.Base.EmailAttributes;
using MySystem.Base.Helpers;
using Microsoft.Extensions.Logging;
using MIS.DBBO;
using vitaaid.com.Models.ShoppingCart;
using MIS.DBPO;
// using VA.MIS.APServer.Models.Account;
using vitaaid.com.Jwt;
using System.Net.Mail;
using static MySystem.Base.Helpers.MailHelper;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using vitaaid.com.Models.Account;

namespace vitaaid.com.Controllers
{

  public class JoinUsFormData
  {
    public virtual string Profession { get; set; }
    public virtual string FirstName { get; set; }
    public virtual string LastName { get; set; }
    public virtual string Email { get; set; }
  }
  public class ContactUsFormData
  {
    public virtual string Prefix { get; set; }
    public virtual string FirstName { get; set; }
    public virtual string LastName { get; set; }
    public virtual string Phone { get; set; }
    public virtual string Email { get; set; }
    public virtual string Content { get; set; }
  }
  public class OrderHistory
  {
    public virtual int Index { get; set; }
    public virtual int OrderID { get; set; }
    public virtual string Status { get; set; }
    public virtual string ShippingMethod { get; set; }
    public virtual string OrderNo { get; set; }
    public virtual string Name { get; set; }
    public virtual string OrderDate { get; set; }
    public virtual string PaymentMethod { get; set; }
    public virtual decimal Amount { get; set; }
  }
  public class ResetPasswordForm
  {
    public virtual string Email { get; set; }
    public virtual string NewPassword { get; set; }
    public virtual string ConfirmPassword { get; set; }
    public virtual string Token { get; set; } // old password when using in changing password
  }

  [Route("api/Members")]
  [ApiController]
  public class MemberController : ControllerBase
  {
    private IWebHostEnvironment _hostEnvironment;
    private ITokenManager _tokenManager;
    private readonly ILogger<MemberController> _logger;
    public MemberController(ITokenManager _tokenManager, IWebHostEnvironment environment, ILogger<MemberController> logger)
    {
      _hostEnvironment = environment;
      _logger = logger;
      this._tokenManager = _tokenManager;
    }

    // GET: api/Members/{email}
    [Authorize]
    [HttpGet("{email}")]
    public IActionResult Get(string email)
    {
      if (string.IsNullOrWhiteSpace(email))
        return Ok(null);

      _logger.LogInformation("get member: {0}", email);
      var oSession = DBServer[eST.READONLY];
      try
      {
        oSession.Clear();
//#if DEBUG
        return Ok(oSession.QueryDataElement<Member>()
                           .Where(x => x.Email.ToUpper() == email.ToUpper() || x.CustomerCode == email.ToUpper())
                           .ToList()
                           .FirstOrDefault());
//#else
        //return Ok(oSession.QueryDataElement<Member>()
        //                   .Where(x => x.Email.ToUpper() == email.ToUpper())
        //                   .ToList()
        //                   .UniqueOrDefault());
//#endif
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An error occurred, email=" + email);
        return BadRequest();
      }
      finally
      {
        oSession.Close();
      }
    }

    // GET: api/Members/validate
    [HttpGet("validate/{email}")]
    public IActionResult Validate(string email, int except = 0)
    {
        if (string.IsNullOrWhiteSpace(email))
          return Ok(false);

      _logger.LogInformation("Validate email: {0}, exceptID:{1}", email, except);
      var oSession = DBServer[eST.READONLY];
      try
      {
        oSession.Clear();

        return Ok(oSession.QueryDataElement<Member>()
                           .Where(x => x.Email.ToUpper() == email.ToUpper() && x.ID != except)
                           .ToList()
                           .IsNullOrEmpty() ? true : false
                           );
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An error occurred, email=" + email + ", except=" + except.ToString());
        return BadRequest();
      }
      finally
      {
        oSession.Close();
      }
    }

    // GET: api/Members/validate
    [HttpGet("validatepcode/{physicianCode}")]
    public IActionResult ValidatePCode(string physicianCode)
    {
        if (string.IsNullOrWhiteSpace(physicianCode))
          return Ok(false);

      _logger.LogInformation("Validate PCode: {0}", physicianCode);
      var oSession = DBServer[eST.READONLY];
      try
      {
        oSession.Clear();

        return Ok(oSession.QueryDataElement<Member>()
                           .Where(x => x.PhysicanCode.ToUpper() == physicianCode.ToUpper())
                           .ToList()
                           .Any());

      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An error occurred, physicianCode=" + physicianCode);
        return BadRequest();
      }
      finally
      {
        oSession.Close();
      }
    }

    // GET: api/Members/requestresetpassword
    [HttpGet("requestresetpassword/{email}")]
    public string RequestResetPassword(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
          return "empty email";

      _logger.LogInformation("Request Reset Password: {0}", email);
      var oSession = DBServer[eST.READONLY];
      try
      {
        oSession.Clear();
        var member = oSession.QueryDataElement<Member>()
                           .Where(x => x.Email.ToUpper() == email.ToUpper())
                           .UniqueOrDefault();
                           
        if (member == null)
          return "No account found for this email.";
        var token = _tokenManager.CreateStandard(email, member.Name, "", "vitaaid.com");
        SendRequestPasswordEmail(email, token.access_token, oSession);

        return "Ok";

      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An error occurred, email=" + email);
        return "Error";
      }
      finally
      {
        oSession.Close();
      }
    }

    private string getRequestPasswordMailBody()
    {
      string content = "";
      _logger.LogInformation("read file: " + Startup.ms_ContentRoot + "ResetPasswordMailBody.html");
      using (TextReader StrIn = new System.IO.StreamReader(Startup.ms_ContentRoot + "ResetPasswordMailBody.html"))
      {
        try
        {
          content = StrIn.ReadToEnd();
        }
        catch (Exception) { }
      }
      return content;
    }

    private void SendRequestPasswordEmail(string email, string token, SessionProxy oSession)
    {
      try
      {
        _logger.LogDebug("In SendRequestPasswordEmail() ...");
        _logger.LogDebug("getRequestPasswordMailBody() ...");
        string mailbody = getRequestPasswordMailBody();
        if (mailbody.Length == 0)
        {
          _logger.LogError("getRequestPasswordMailBody() ... FAIL");
          return;
        }
        else
          _logger.LogInformation("getRequestPasswordMailBody() ... SUCCESS");
        
        var varDictionary =
          new Dictionary<string, string> {
            { "{ResetPasswordURL}", Request.Scheme+ "://" + Request.Host + "/ResetPassword?token=" + token}
          };

        string result = "FAIL";

        var setting = UnitTypeHelper.buildEmailAttributes(oSession);
        setting[C_SUBJECT] = "Request to Reset Account Password for " + email;
        setting[C_BODY] = mailbody;
        setting.processVarables(C_BODY, varDictionary);

        setting[C_FROM] = "info@vitaaid.com";
        setting[C_RECIPIENT] = email;
        result = MailHelper.SendEmail(setting, (Attachment)null, null, true);
        if (result == SEND_SUCCESS)
          _logger.LogInformation("SendEmail to " + email + ":" + result);
        else
          _logger.LogError("SendEmail to " + email + ":" + result);

        _logger.LogDebug("Out SendRequestPasswordEmail().");
      }
      catch (Exception ex)
      {
        _logger.LogError("SendRequestPasswordEmail", ex);
        throw;
      }
    }

    // GET: api/Members/checktoken?token=
    [HttpGet("checktoken")]
    public IActionResult CheckToken(string token)
    {
      try
      {
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.ReadJwtToken(token);
        if (DateTime.UtcNow > securityToken.ValidTo)
          return Ok(false);
        return Ok(true);
      }
      catch (Exception)
      {

        throw;
      }
    }

    // PUT: api/Members/resetpassword
    [HttpPut("resetpassword/{email}")]
    public string ResetPassword([FromForm] ResetPasswordForm data, string email)
    {
        if (string.IsNullOrWhiteSpace(email))
          return "empty email";

      _logger.LogInformation("Reset Password: {0}", email);
      var oSession = DBServer[eST.SESSION0];
      ITransaction xact = null;
      try
      {
        oSession.Clear();

        var member = oSession.QueryDataElement<Member>()
                             .Where(x => x.Email.ToUpper() == email.ToUpper())
                             .UniqueOrDefault();
        if (member == null)
          return "No matching email address found";

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.ReadJwtToken(data.Token);
        if (DateTime.UtcNow > securityToken.ValidTo)
          return "Your password reset link has expired";

        if (securityToken.Payload.Claims.Where(x => x.Type == JwtRegisteredClaimNames.NameId).UniqueOrDefault()?.Value != email)
          return "Your password reset link and email do not match";

        xact = oSession.BeginTransaction();
        member.Password = data.NewPassword;
        member.SaveObj(oSession);
        xact.Commit();

        return "Ok";

      }
      catch (Exception ex)
      {
        xact?.Rollback();
        _logger.LogError(ex, "An error occurred, email=" + email);
        return "Error";
      }
      finally
      {
        oSession.Close();
      }
    }

    // PUT: api/Members/changepassword
    [HttpPut("changepassword/{email}")]
    public string ChangePassword([FromForm] ResetPasswordForm data, string email)
    {
      if (string.IsNullOrWhiteSpace(email))
        return "empty email";

      _logger.LogDebug("Change Password");
      var oSession = DBServer[eST.SESSION0];
      ITransaction xact = null;
      try
      {
        oSession.Clear();

        var member = oSession.QueryDataElement<Member>()
                           .Where(x => x.Email.ToUpper() == email.ToUpper())
                           .UniqueOrDefault();
        if (member == null)
          return "No matching email address found";

        if (member.Password != data.Token)
          return "Current password does not match";


        xact = oSession.BeginTransaction();
        member.Password = data.NewPassword;
        member.SaveObj(oSession);
        xact.Commit();

        return "Ok";
      }
      catch (Exception ex)
      {
        xact?.Rollback();
        _logger.LogError(ex, "An error occurred, email=" + email);
        return "Error";
      }
      finally
      {
        oSession.Close();
      }
    }

    // POST api/Members
    [HttpPost]
    public IActionResult Post([FromForm] Member oNewerMember, string type)
    {
      _logger.LogInformation("New member: {0} {1}", oNewerMember.Email, type);
      var oSession = DBServer[eST.SESSION0];
      ITransaction xact = null;
      try
      {
        oSession.Clear();

        var countryCode = oNewerMember.Country;
        var country = CountryHelper.LoadFromJSON("country.json");
        country.Where(x => x.isoCode == countryCode)
               .FirstOrDefault()
               ?.Also(x =>
               {
                 oNewerMember.Country = x.name;
               });

        oNewerMember.Name = oNewerMember.FirstName.Trim() + " " + oNewerMember.LastName.Trim();
        oNewerMember.Address = oNewerMember.Address1 + ((string.IsNullOrWhiteSpace(oNewerMember.Address2)) ? "" : " " + oNewerMember.Address2);
        if (type == "Practitioner")
        {
          oNewerMember.MemberType = eMEMBERTYPE.HEALTHCARE_PRACTITIONER;
          oNewerMember.Pat_pcode = "";
          if (!oNewerMember.PractitionerType.Equals("Other"))
            oNewerMember.OtherPractitionerType = "";
        }
        else if (type == "Patient")
        {
          oNewerMember.MemberType = eMEMBERTYPE.PATIENT;
          oNewerMember.PhysicanCode = "";
          oNewerMember.PractitionerType = "";
          oNewerMember.OtherPractitionerType = "";
        }
        else
        {
          oNewerMember.PractitionerType = "Medical Students";
          if (oNewerMember.MemberType != eMEMBERTYPE.STUDENT_Others)
            oNewerMember.OtherPractitionerType = "";
          oNewerMember.PhysicanCode = "";
          oNewerMember.Pat_pcode = "";
        }

        oNewerMember.bReferral = Request.Form["referral"].FirstOrDefault()?.Equals("on") ?? false;
        oNewerMember.IsSubscribe = Request.Form["newsletters"].FirstOrDefault()?.Equals("on") ?? false;

        xact = oSession.BeginTransaction();
        oSession.SaveObj(oNewerMember);
        xact.Commit();

        if (type == "Practitioner" || (Request.Form?.Files?.Any() ?? false))
        {
          xact = oSession.BeginTransaction();
          if (type == "Practitioner")
            oNewerMember.PhysicanCode = ((countryCode == "CA" || countryCode == "US") ? oNewerMember.Province : countryCode) + oNewerMember.FirstName.Substring(0, 1) + oNewerMember.LastName.Substring(0, 1) + oNewerMember.Telephone[^4..] + "-" + oNewerMember.ID;
          Request.Form?.Files?.FirstOrDefault()?.Also(x =>
          {
            try
            {
              oNewerMember.LicencePhoto = string.Format("S_{0}_{1}", oNewerMember.ID, x.FileName);
              string uniqueFileName = string.Format("ProductImages/member/{0}", oNewerMember.LicencePhoto);
              var path = Path.Combine(_hostEnvironment.WebRootPath, uniqueFileName);
              using (Stream fileStream = new FileStream(path, FileMode.Create))
                x.CopyTo(fileStream);
            }
            catch
            {
            }
          });
          oSession.SaveObj(oNewerMember);
          xact.Commit();

          SendNotificationEmailToVitaaid(oNewerMember, oSession);
        }
        return Ok(oNewerMember);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "MemberController.Post: An error occurred. Name=" + oNewerMember.Name + ", email=" + oNewerMember.Email);
        xact?.Rollback();
        return BadRequest(ex);
      }
      finally
      {
        oSession.Close();
      }
    }

    private void SendNotificationEmailToVitaaid(Member oNewerMember, SessionProxy oSession)
    {
      try
      {
        _logger.LogDebug("SendNotificationEmailToVitaaid() ...");
        var setting = UnitTypeHelper.buildEmailAttributes(oSession);
        setting[C_SUBJECT] = "New member registration";
        setting[C_BODY] = "A new member has applied for an account.\nPlease evaluate the personal information and confirm the registration.\n" +
                          "============================================================\n" +
                          "Member Details:\n" +
                          "Name : " + oNewerMember.Name + "\n" +
                          "Telephone : " + oNewerMember.Telephone + "\n" +
                          "Address : " + oNewerMember.Address + " " + oNewerMember.City + " " + oNewerMember.ZipCode + " " + oNewerMember.Province + " " + oNewerMember.Country + "\n" +
                          "Email : " + oNewerMember.Email + "\n";
        setting[C_FROM] = "info@vitaaid.com";
        setting[C_RECIPIENT] = "cs@vitaaid.com";
        var result = MailHelper.SendEmail(setting);
        if (result == SEND_SUCCESS)
          _logger.LogInformation("SendEmail to cs@vitaaid.com:" + result);
        else
          _logger.LogError("SendEmail to cs@vitaaid.com:" + result);
      }
      catch (Exception ex)
      {
        _logger.LogError("SendEmail to cs@vitaaid.com:" + ex.Message);
      }
    }
    // PUT api/Members
    [HttpPut("{ID}")]
    public IActionResult Put([FromForm] Member oUpdatingMember, int ID, string newCountryValue)
    {
      _logger.LogInformation("Update member: {0}, id={1}", oUpdatingMember.Email, ID);
      var oSession = DBServer[eST.SESSION0];
      ITransaction xact = null;
      try
      {
        oSession.Clear();
        var oMember = oSession.QueryDataElement<Member>().Where(x => x.ID == ID).UniqueOrDefault();
        if (oMember == null)
          return BadRequest();

        xact = oSession.BeginTransaction();
        oMember.Prefix = oUpdatingMember.Prefix;
        oMember.Pat_pcode = oUpdatingMember.Pat_pcode;
        oMember.Name = oUpdatingMember.Name;
        oMember.Email = oUpdatingMember.Email;
        oMember.Telephone = oUpdatingMember.Telephone;
        oMember.Fax = oUpdatingMember.Fax;
        oMember.ClinicName = oUpdatingMember.ClinicName;
        oMember.Address = oUpdatingMember.Address;
        oMember.City = oUpdatingMember.City;
        oMember.ZipCode = oUpdatingMember.ZipCode;
        oMember.Country = newCountryValue;
        oMember.Province = oUpdatingMember.Province;
        oMember.SaveObj(oSession);
        xact.Commit();
        return Ok(oMember);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An error occurred, ID=" + oUpdatingMember.ID.ToString());
        xact?.Rollback();
        return BadRequest(ex);
      }
      finally
      {
        oSession.Close();
      }
    }


    [HttpPost("joinusouremailinglist")]
    public IActionResult JoinUs([FromForm] JoinUsFormData data)
    {
      if (string.IsNullOrEmpty(data.Profession) || string.IsNullOrEmpty(data.FirstName)
        || string.IsNullOrEmpty(data.LastName) || string.IsNullOrEmpty(data.Email))
        return BadRequest();

      _logger.LogInformation("Join Us: {0}", data.Email);
      var oSession = DBServer[eST.READONLY];
      try
      {
        var setting = UnitTypeHelper.buildEmailAttributes(oSession);
        string result = "FAIL";
        setting[C_FROM] = "info@vitaaid.com";//data.Email;
        setting[C_SUBJECT] = "Join Us Our Emailing List from " + data.Email;
        setting[C_BODY] = "Join Us Our Emailing List\n" +
                          "======================\n" +
                          "Profession: " + data.Profession + "\n" +
                          "Fist Name: " + data.FirstName + "\n" +
                          "Last Name: " + data.LastName + "\n" +
                          "Email: " + data.Email + "\n" +
                          "============================================================\n";
#if DEBUG
        setting[C_RECIPIENT] = "alex@naturoaid.com";
#else
        setting[C_RECIPIENT] = "info@vitaaid.com";
#endif
        result = MailHelper.SendEmail(setting);
        return Ok(true);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An error occurred, email=" + data.Email);
        return BadRequest(ex);
      }
      finally
      {
        oSession.Close();
      }
    }

    [HttpPost("contactus")]
    public IActionResult ContactUs([FromForm] ContactUsFormData data)
    {
      if (string.IsNullOrEmpty(data.FirstName) && string.IsNullOrEmpty(data.LastName))
        return BadRequest();

      _logger.LogInformation("Contact Us: {0}", data.Email);
      var oSession = DBServer[eST.READONLY];
      try
      {
        var setting = UnitTypeHelper.buildEmailAttributes(oSession);
        string result = "FAIL";
        setting[C_FROM] = "info@vitaaid.com";//data.Email;
        setting[C_SUBJECT] = "Contact Us from " + data.Email;
        setting[C_BODY] = "Contact Us\n" +
                          "======================\n" +
                          "Prefix: " + data.Prefix + "\n" +
                          "Fist Name: " + data.FirstName + "\n" +
                          "Last Name: " + data.LastName + "\n" +
                          "Phone Number: " + data.Phone + "\n" +
                          "Email: " + data.Email + "\n" +
                          "Question: " + data.Content + "\n" +
                          "============================================================\n";
#if DEBUG
        setting[C_RECIPIENT] = "alex@naturoaid.com";
#else
        setting[C_RECIPIENT] = "info@vitaaid.com";
#endif
        result = MailHelper.SendEmail(setting);
        return Ok(true);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An error occurred, email=" + data.Email);
        return BadRequest(ex);
      }
      finally
      {
        oSession.Close();
      }
    }

    // GET: api/Members/{CustomerCode}/orderhistory
    [Authorize]
    [HttpGet("{CustomerCode}/orderhistory")]
    public IActionResult GetOrderHistory(string CustomerCode)
    {
        if (string.IsNullOrWhiteSpace(CustomerCode))
          return Ok(null);

      _logger.LogInformation("GetOrderHistory: {0}", CustomerCode);
      var oVMSession = VAMISDBServer[eST.READONLY];
      try
      {
        oVMSession.Clear();
        var orderHistories = oVMSession.QueryDataElement<VAOrder>()
                                    .Where(x => x.AccountNo == CustomerCode)
                                    .OrderByDescending(x => x.OrderDate)
                                    .ToList()
                                    .Select(x => new OrderHistory
                                    {
                                      OrderID = x.ID,
                                      OrderDate = x.OrderDate.ToShortDateString(),
                                      OrderNo = x.PONo,
                                      Name = x.TitleBill,
                                      Status = x.Status,
                                      PaymentMethod = x.PaymentType,
                                      ShippingMethod = x.ShippingMethod,
                                      Amount = x.BalanceDue
                                    }).ToList();
        orderHistories.ForEachWithIndex((x, idx) => x.Index = idx + 1);
        return Ok(orderHistories);
        //        return Ok((orderHistories.IsNullOrEmpty())? new List<OrderHistory>(): orderHistories);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An error occurred.");
        return BadRequest();
      }
      finally
      {
        oVMSession.Close();
      }
    }

    // GET: api/Members/{CustomerCode}/patientorderhistory
    [Authorize]
    [HttpGet("{CustomerCode}/patientorderhistory")]
    public IActionResult GetPatientOrderHistory(string CustomerCode)
    {
      if (string.IsNullOrWhiteSpace(CustomerCode))
        return Ok(null);

      _logger.LogInformation("GetPatientOrderHistory: {0}", CustomerCode);
      var oVMSession = VAMISDBServer[eST.READONLY];
      var oMISSession = DBServer[eST.READONLY];

      try
      {
        oVMSession.Clear();
        oMISSession.Clear();

        var oMember = oMISSession.QueryDataElement<Member>()
                                 .Where(x => x.CustomerCode == CustomerCode && x.MemberStatus == eMEMBERSTATUS.ACTIVE &&
                                             x.MemberType == eMEMBERTYPE.HEALTHCARE_PRACTITIONER)
                                 .ToList()
                                 .FirstOrDefault();
        if (oMember == null || string.IsNullOrEmpty(oMember.PhysicanCode))
          return BadRequest();
        var oPatients = oMISSession.QueryDataElement<Member>()
                                 .Where(x => x.Pat_pcode == oMember.PhysicanCode && x.MemberStatus == eMEMBERSTATUS.ACTIVE &&
                                             x.MemberType == eMEMBERTYPE.PATIENT && !(x.CustomerCode == null || x.CustomerCode == ""))
                                 .Select(x => x.CustomerCode)
                                 .ToList();
        var orderHistories = oVMSession.QueryDataElement<VAOrder>()
                                    .Where(x => oPatients.Contains(x.AccountNo))
                                    .OrderByDescending(x => x.OrderDate)
                                    .ToList()
                                    .Select(x => new OrderHistory
                                    {
                                      OrderID = x.ID,
                                      OrderDate = x.OrderDate.ToShortDateString(),
                                      OrderNo = x.PONo,
                                      Status = x.Status,
                                      Name = x.TitleBill,
                                      PaymentMethod = x.PaymentType,
                                      ShippingMethod = x.ShippingMethod,
                                      Amount = x.BalanceDue
                                    }).ToList();
        orderHistories.Action(x =>
        {
          oVMSession.QueryDataElement<POSInvoice>()
                    .Where(i => i.InvoiceNo == x.OrderNo)
                    .OrderByDescending(i => i.ID)
                    .ToList()
                    .FirstOrDefault()
                    ?.Let(i => x.Name = i.BillToName);
        });
        orderHistories.ForEachWithIndex((x, idx) => x.Index = idx + 1);
        return Ok(orderHistories);
        //        return Ok((orderHistories.IsNullOrEmpty())? new List<OrderHistory>(): orderHistories);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An error occurred.");
        return BadRequest();
      }
      finally
      {
        oMISSession.Close();
        oVMSession.Close();
      }
    }

    // GET: api/Members/{CustomerCode}/orderdetail?orderNo={orderNo}
    [Authorize]
    [HttpGet("{CustomerCode}/orderdetail")]
    public IActionResult GetOrderDetail(string CustomerCode, string orderNo)
    {
      if (string.IsNullOrWhiteSpace(CustomerCode) || string.IsNullOrWhiteSpace(orderNo))
        return Ok(null);
      _logger.LogInformation("GetOrderDetail: {0}, {1}", CustomerCode, orderNo);
      return _GetOrderDetail(CustomerCode, orderNo, false);
    }

    // GET: api/Members/{CustomerCode}/patientorderdetail?orderNo={orderNo}
    [Authorize]
    [HttpGet("{CustomerCode}/patientorderdetail")]
    public IActionResult GetPatientOrderDetail(string CustomerCode, string orderNo)
    {
      if (string.IsNullOrWhiteSpace(CustomerCode) || string.IsNullOrWhiteSpace(orderNo))
        return Ok(null);
      _logger.LogInformation("GetPatientOrderDetail: {0}, {1}", CustomerCode, orderNo);
      return _GetOrderDetail(CustomerCode, orderNo, true);
    }

    private IActionResult _GetOrderDetail(string CustomerCode, string orderNo, bool bPatientOrder)
    {
      var oSession = DBServer[eST.READONLY];
      var oVMSession = VAMISDBServer[eST.READONLY];
      try
      {
        oSession.Clear();
        oVMSession.Clear();
        var order = (bPatientOrder) ?
          oVMSession.QueryDataElement<VAOrder>()
                            .Where(x => x.PONo == orderNo)
                            .UniqueOrDefault()
                            :
          oVMSession.QueryDataElement<VAOrder>()
                            .Where(x => x.AccountNo == CustomerCode && x.PONo == orderNo)
                            .UniqueOrDefault();
        order?.LoadOrderItem(oVMSession);
        if (order.Status == eORDERSTATUS.INIT)
        {
          var oOrderData = oSession.QueryDataElement<OrderData>()
                                    .Where(x => x.VAOrderID == order.ID)
                                    .UniqueOrDefault();
          if (oOrderData != null)
          {
            oOrderData.OrderItems = oSession.QueryDataElement<OrderItemData>()
                                             .Where(x => x.PONo == oOrderData.PONo && x.oOrderData.ID == oOrderData.ID)
                                             .ToList()
                                             .ToArray();
            oOrderData.BillingAddress = new AddressData
            {
              DefaultBillingAddress = true,
              DefaultShippingAddress = false,
              AddressName = oOrderData.TitleBill,
              AddressPerson = oOrderData.CompanyNameBill,
              Address = oOrderData.AddrBill,
              City = oOrderData.CityBill,
              Province = oOrderData.ProvinceBill,
              PostalCode = oOrderData.ZipCodeBill,
              Country = oOrderData.CountryBill,
              Tel = oOrderData.TelBill
            };
            oOrderData.ShippingAddress = new AddressData()
            {
              DefaultBillingAddress = false,
              DefaultShippingAddress = true,
              AddressName = oOrderData.TitleShip,
              AddressPerson = oOrderData.CompanyNameShip,
              Address = oOrderData.AddrShip,
              City = oOrderData.CityShip,
              Province = oOrderData.ProvinceShip,
              PostalCode = oOrderData.ZipCodeShip,
              Country = oOrderData.CountryShip,
              Tel = oOrderData.TelShip

            };
            return Ok(oOrderData);
          }
        }
        var oInvoice = (bPatientOrder) ?
          oVMSession.QueryDataElement<POSInvoice>()
                                    .Where(x => x.InvoiceNo == orderNo)
                                    .UniqueOrDefault()
                                    :
                                    oVMSession.QueryDataElement<POSInvoice>()
                                    .Where(x => x.AccountNO == CustomerCode && x.InvoiceNo == orderNo)
                                    .UniqueOrDefault();

        var ShoppingCartOrder = (new OrderData()).buildFromPOSInvoice(oInvoice, order);
        return Ok(ShoppingCartOrder);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An error occurred.");
        return BadRequest();
      }
      finally
      {
        oSession.Close();
        oVMSession.Close();
      }
    }

  }
}
