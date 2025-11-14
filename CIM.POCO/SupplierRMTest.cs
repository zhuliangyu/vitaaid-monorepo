using MyHibernateUtil;
using System;
using System.ComponentModel;

namespace POCO
{
  [Serializable]
  public class SupplierRMTest : POCOBase
  {
    public virtual int ID { get; set; }
    public override int getID() => ID;
    public virtual Supplier SupplierObj { get; set; }
    public virtual string Supplier { get; set; }
    private string _ReceivingNo = "";
    public virtual string ReceivingNo
    {
      get { return _ReceivingNo; }
      set
      {
        _ReceivingNo = value;
        OnPropertyChanged("ReceivingNo");
      }
    }
    private string _RawMaterialName;
    public virtual string RawMaterialName
    {
      get { return _RawMaterialName; }
      set
      {
        _RawMaterialName = value;
        OnPropertyChanged("RawMaterialName");
      }
    }
    public virtual MESRawMaterial RawMaterialObj { get; set; }
    private string _SpecCode { get; set; }
    public virtual string SpecCode
    {
      get { return _SpecCode; }
      set
      {
        _SpecCode = value;
        OnPropertyChanged("SpecCode");
      }
    }
    public virtual RawMaterialSpec RMSpec { get; set; }
    public virtual string CreatedID { get; set; }
    public virtual DateTime CreatedDate { get; set; }
    public virtual string UpdatedID { get; set; }
    public virtual DateTime UpdatedDate { get; set; }

    public virtual string Comment { get; set; }
    public virtual string Status { get; set; }

    public virtual int TestYear { get; set; } = 0;
    private DateTime _TestDate = new DateTime(2050, 12, 31);
    public virtual DateTime TestDate
    {
      get { return _TestDate; }
      set
      {
        if (value.Year != 1)
          _TestDate = value;
        OnPropertyChanged("TestDate");
      }
    }
  }
}
