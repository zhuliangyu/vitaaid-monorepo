using Microsoft.AspNetCore.Http;
using MyHibernateUtil;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebDB.DBBO
{
  [Serializable]
  public class Member : DataElement
  {
    public virtual int ID { get; set; }
    public override int getID() => ID;

    public virtual string Name { get; set; }
    public virtual string PractitionerType { get; set; }
    public virtual string OtherPractitionerType { get; set; }
    public virtual string ClinicName { get; set; }
    public virtual string Address { get; set; }
    public virtual string Province { get; set; }
    public virtual string Country { get; set; }
    public virtual string City { get; set; }
    public virtual string ZipCode { get; set; }
    public virtual string Telephone { get; set; }
    public virtual string Fax { get; set; }
    public virtual string Email { get; set; }
    public virtual DateTime JoinTime { get; set; } = DateTime.Now;
    public virtual string Password { get; set; }
    public virtual string LicenceNo { get; set; }
    public virtual bool bReferral { get; set; }
    public virtual eMEMBERTYPE MemberType { get; set; } = eMEMBERTYPE.HEALTHCARE_PRACTITIONER;
    public virtual string Reason1 { get; set; }
    public virtual string PhysicanCode { get; set; }
    public virtual string Pat_pcode { get; set; }
    public virtual string LicencePhoto { get; set; }
    public virtual string SalesRep { get; set; }
    public virtual string CustomerCode { get; set; }
    public virtual string FirstName { get; set; }
    public virtual string LastName { get; set; }
    public virtual eMEMBERSTATUS MemberStatus { get; set; } = eMEMBERSTATUS.INACTIVE;
    public virtual bool IsSubscribe { get; set; } = false;
    public virtual eWEBSITE PermittedSite { get; set; } = eWEBSITE.CA;
    public virtual ePREFIX Prefix { get; set; }
    public virtual string LicenceVerifyMethod { get; set; }
    // ui data
    public virtual string Address1 { get; set; }
    public virtual string Address2 { get; set; }
    public virtual string DisplayIDNName { get => "[" + ID + "]" + Name; }

    // memory data
    public virtual bool hasPatients { get; set; } = false;
  }
}
