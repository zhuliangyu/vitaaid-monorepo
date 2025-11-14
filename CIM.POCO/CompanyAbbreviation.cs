using MyHibernateUtil;
using System;


namespace POCO
{
  [Serializable]
  public class CompanyAbbreviation : POCOBase
  {
    public CompanyAbbreviation() { }
    public virtual int ID { get; set; }
    public override int getID() => ID;
    public virtual string Abbreviation { get; set; }
    public virtual string CompanyName { get; set; }
    public virtual string CompanyWeb { get; set; }
    public virtual string CompanyCharacter { get; set; }

    public virtual string ContactPersonalTitle { get; set; }
    public virtual string ContactPersonal { get; set; }
    public virtual string ContactPersonalEmail { get; set; }

    public virtual string ContactManagerTitle { get; set; }
    public virtual string ContactManager { get; set; }
    public virtual string ContactManagerEmail { get; set; }

    public virtual string CompanyAddress { get; set; }
    public virtual string CompanyCity { get; set; }
    public virtual string CompanyProvince { get; set; }
    public virtual string CompanyPostalCode { get; set; }
    public virtual string CompanyCountry { get; set; }

    public virtual string CompanyTel { get; set; }
    public virtual string CompanyFax { get; set; }

    public virtual string CompanyAddress1 { get; set; }
    public virtual string CompanyCity1 { get; set; }
    public virtual string CompanyProvince1 { get; set; }
    public virtual string CompanyPostalCode1 { get; set; }
    public virtual string CompanyCountry1 { get; set; }
    public virtual string CompanyTel1 { get; set; }
    public virtual string CompanyFax1 { get; set; }
    public virtual string Status { get; set; }
    public virtual string Comment { get; set; }

    private DateTime _CreatedDate = DateTime.Now;
    public virtual DateTime CreatedDate { get { return _CreatedDate; } set { _CreatedDate = value; } }
    private DateTime _UpdatedDate = DateTime.Now;
    public virtual DateTime UpdatedDate { get { return _UpdatedDate; } set { _UpdatedDate = value; } }
    public virtual string CreatedID { get; set; }
    public virtual string UpdatedID { get; set; }
    public virtual bool Active { get; set; } = true;
    public virtual string dispActive { get { return (Active) ? "V" : "X"; } }

    public virtual string FullAddress
    {
      get
      {
        return CompanyAddress + " " + CompanyCity + ", " + CompanyProvince + " " + CompanyPostalCode + " " + CompanyCountry;
      }
    }
    public virtual CompanyAbbreviation ShallowCopy()
    {
      return (CompanyAbbreviation)this.MemberwiseClone();
    }
  }
}
