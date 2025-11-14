using DevExpress.Web.Mvc;
using MIS.DBBO;
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
  public static class MemberHelper
  {
    public static GridViewModel GetMemberModel()
    {
      return new GridViewModel();
    }
    private static List<Member> ReloadMembers()
    {
      var oSession = DBServer[eST.READONLY];
      try
      {
        oSession.Clear();
        var oMembers = oSession.QueryDataElement<Member>()
                                 .OrderByDescending(x => x.JoinTime).ToList();
        HttpContext.Current.Session["Member"] = oMembers;
        return oMembers;
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
    public static List<Member> GetMembers()
    {
      if (HttpContext.Current.Session["Member"] == null)
        ReloadMembers();

      return HttpContext.Current.Session["Member"] as List<Member>;
    }
    public static bool CheckAccount(string AccountNo)
    {
      var oVMSession = VAMISDBServer[eST.READONLY];
      try
      {
        oVMSession.Clear();
        return oVMSession.QueryDataElement<CustomerAccount>()
                             .Where(x => x.CustomerCode == AccountNo)
                             .ToList()
                             .Any();
      }
      catch (Exception)
      {
        throw;
      }
      finally
      {
        oVMSession.Close();
      }
    }
    public static IList<ECSServerObj.WS_Employee> getSalesRep()
    {
      try
      {
        return ECSServerObj.RESTfullObject.listEmployeesByGroup("Sales Representative")?.oData ?? new List<ECSServerObj.WS_Employee>();
      }
      catch (Exception)
      {

        throw;
      }
    }
    public static void AddNewAccountToInvoiceSystem(Member oMember)
    {
      var oVMSession = VAMISDBServer[eST.SESSION0];
      ITransaction xact = null;
      try
      {
        oVMSession.Clear();
        if (string.IsNullOrWhiteSpace(oMember.CustomerCode) || oMember.CustomerCode.Length < 5 || oVMSession.QueryDataElement<CustomerAccount>().Where(x => x.CustomerCode == oMember.CustomerCode).ToList().Any())
          return;

        // create account in the invoice System
        CustomerAccount oCA = new CustomerAccount();

        oCA.CustomerCode = oMember.CustomerCode;
        oCA.CustomerName = oMember.Prefix.ToString() + " " + oMember.FirstName + " " + oMember.LastName + " " +
                          ((oMember.PractitionerType == "Naturopathic Doctor") ? " ND" : "");
        oCA.CustomerOwner = oCA.CustomerName;
        oCA.ContactPersonal = oCA.CustomerName;
        oCA.ContactPersonalTel = oMember.Telephone;
        oCA.ContactPersonalEmail = oMember.Email;
        oCA.CustomerPersonal1 = oCA.CustomerOwner;
        oCA.CustomerName1 = oMember.ClinicName;
        oCA.CustomerAddress1 = oMember.Address;
        oCA.CustomerCity1 = oMember.City;
        oCA.CustomerProvince1 = oMember.Province;
        oCA.CustomerPostalCode1 = oMember.ZipCode;
        oCA.CustomerCountry1 = oMember.Country;
        oCA.CustomerTel1 = oMember.Telephone;
        oCA.CustomerFax1 = oMember.Fax;
        oCA.CustomerPersonal2 = oCA.CustomerOwner;
        oCA.CustomerName2 = oMember.ClinicName;
        oCA.CustomerAddress2 = oMember.Address;
        oCA.CustomerCity2 = oMember.City;
        oCA.CustomerProvince2 = oMember.Province;
        oCA.CustomerPostalCode2 = oMember.ZipCode;
        oCA.CustomerCountry2 = oMember.Country;
        oCA.CustomerTel2 = oMember.Telephone;
        oCA.CustomerFax2 = oMember.Fax;
        oCA.oPaymentTerm = (oVMSession.QueryDataElement<PaymentTerm>().Where(x => x.DaysDue == 0).ToList().FirstOrDefault());
        oCA.PaymentType = "Credit Card";
        oCA.CustomerEmail1 = oMember.Email;
        oCA.CustomerEmail2 = oMember.Email;
        oCA.CustomerOwnerTitle = oMember.Prefix.ToString();
        oCA.InvoiceToCustomer = "Email ONLY";
        oCA.IsActive = true;
        oCA.PricePolicy = (oMember.Country == "Canada" || oMember.Country == "CA") ?
                     ((oMember.MemberType == eMEMBERTYPE.PATIENT) ? ePRICEPOLICY.MSRP : ePRICEPOLICY.STANDARD) :
                     ((oMember.MemberType == eMEMBERTYPE.PATIENT) ? ePRICEPOLICY.MSRP_USD : ePRICEPOLICY.STANDARD_USD);
        var oSalesRep = ECSServerObj.RESTfullObject.listEmployeesByGroup("Sales Representative")?.oData?.Where(x => x.ShortName == oMember.SalesRep)?.FirstOrDefault();
        oCA.SalesRep = oMember.SalesRep;
        oCA.SalesRepID = oSalesRep?.Account;

        var oAddress = new CustomerAddress();
        oAddress.AddressPerson = oCA.CustomerPersonal1;
        oAddress.AddressName = oCA.CustomerName1;
        oAddress.Address = oCA.CustomerAddress1;
        oAddress.City = oCA.CustomerCity1;
        oAddress.Province = oCA.CustomerProvince1;
        oAddress.PostalCode = oCA.CustomerPostalCode1;
        oAddress.Country = oCA.CustomerCountry1;
        oAddress.Tel = oCA.CustomerTel1;
        oAddress.DefaultBillingAddress = true;
        oAddress.DefaultShippingAddress = true;
        oAddress.Fax = oCA.CustomerFax1;
        oAddress.oCustomer = oCA;

        xact = oVMSession.BeginTransaction();
        oAddress.SaveObj(oVMSession);
        oCA.SaveObj(oVMSession);
        xact.Commit();
      }
      catch (Exception)
      {
        xact?.Rollback();
      }
      finally
      {
        oVMSession.Close();
      }
    }
    public static void AddNewMember(Member member, SessionProxy oSession)
    {
      (HttpContext.Current.Session["User"] as ApplicationUser)?.Let(x => DataElement.sDefaultUserID = x.UserName);

      ITransaction xact = null;
      try
      {
        xact = oSession.BeginTransaction();

        //member.PreAdd();
        oSession.SaveObj(member);
        xact.Commit();
        //HttpContext.Current.Session["Member"] = null;
        ReloadMembers();
      }
      catch (Exception ex)
      {
        xact?.Rollback();
      }
    }

    public static void UpdateMember(Member oDirtyMember, SessionProxy oSession)
    {
      (HttpContext.Current.Session["User"] as ApplicationUser)?.Let(x => DataElement.sDefaultUserID = x.UserName);
      ITransaction xact = null;
      try
      {
        xact = oSession.BeginTransaction();
        oDirtyMember.SaveObj(oSession);
        xact.Commit();
        ReloadMembers();
      }
      catch (Exception ex)
      {
        xact?.Rollback();
      }
    }

    public static void DeleteMembers(List<long> ids)
    {
      (HttpContext.Current.Session["User"] as ApplicationUser)?.Let(x => DataElement.sDefaultUserID = x.UserName);

      var oSession = DBServer[eST.SESSION0];
      ITransaction xact = null;
      try
      {
        oSession.Clear();
        var oMembers = oSession.QueryDataElement<Member>().Where(x => ids.Contains(x.ID)).ToList();

        xact = oSession.BeginTransaction();
        oMembers.Action(x => x.DeleteObj(oSession));
        xact.Commit();
        ReloadMembers();
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
    public static void UpdateImage(Member oMember, string filePath, byte[] data, string propertyName)
    {
      try
      {
        string oldValue = oMember.GetPropertyValue(propertyName) as string ?? string.Empty;
        string oldFileServerPath = HttpContext.Current.Server.MapPath(EnvHelper.MEMBER_DIR + oldValue); ;
        if ((data?.Length ?? 0) > 0)
        {
          if ((filePath?.Length ?? 0) > 0)
          {// update/replace
            bool bOverwrite = oldValue.Equals(filePath);
            if (FileHelper.BinaryWriteTo(data, HttpContext.Current.Server.MapPath(EnvHelper.MEMBER_DIR + filePath), bOverwrite))
            {
              oMember.SetPropertyValue(propertyName, filePath);
              oMember.iState = MySystem.Base.eOPSTATE.DIRTY;
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
            oMember.SetPropertyValue(propertyName, string.Empty);
            oMember.iState = MySystem.Base.eOPSTATE.DIRTY;
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