using MyHibernateUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace POCO
{
  [Serializable]
  public class RawMaterialSpec : DataElement
  {
    public virtual int ID { get; set; }
    public override int getID() => ID;
    private string _OldRawMaterialCode = "";
    public virtual string OldRawMaterialCode
    {
      get { return _OldRawMaterialCode; }
      set { _OldRawMaterialCode = value; }
    }
    public virtual string dispItemCode
    {
      get
      {
        if (OldRawMaterialCode != null && OldRawMaterialCode.Length > 0)
          return OldRawMaterialCode + "[ " + SpecCode + " ]";
        else
          return "[ " + SpecCode + " ]";
      }
    }
    public virtual string CategoryCode { get => Category?.CategoryCode ?? ""; set { } }

    private RawMaterialCategory _Category = null;
    public virtual RawMaterialCategory Category
    {
      get { return _Category; }
      set
      {
        _Category = value;
        OnPropertyChanged("Category");
      }
    }

    private Supplier _Suppler = null;
    public virtual Supplier Supplier
    {
      get { return _Suppler; }
      set
      {
        _Suppler = value;
        OnPropertyChanged("Supplier");
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

    public virtual string CategoryName { get => Category?.CategoryName ?? ""; set { } }
    public virtual string ab { get; set; }

    private string _SpecCode;
    public virtual string SpecCode
    {
      get { return _SpecCode; }
      set
      {
        _SpecCode = value;
        OnPropertyChanged("SpecCode");
      }
    }

    private IList<RawMaterialSpecDetail> _RawMaterialSpecDetails = new List<RawMaterialSpecDetail>();
    public virtual IList<RawMaterialSpecDetail> RawMaterialSpecDetails
    {
      get { return _RawMaterialSpecDetails; }
      set { _RawMaterialSpecDetails = value; }
    }
    public virtual IList<RawMaterialSpecDetail> ValidRawMaterialSpecDetails
    {
      get { return _RawMaterialSpecDetails?.Where(x => x.Invalid == false).ToList() ?? new List<RawMaterialSpecDetail>(); }
    }

    public virtual string DispSpecInfo
    {
      get
      {
        if (RawMaterialSpecDetails == null || RawMaterialSpecDetails.Count == 0)
          return "";
        else if (RawMaterialSpecDetails.Count == 1)
          return RawMaterialSpecDetails[0].displaySpecDesc;
        else
        {
          string rtnVal = RawMaterialSpecDetails[0].displaySpecDesc;
          for (int i = 1; i < RawMaterialSpecDetails.Count; i++)
            rtnVal += "," + RawMaterialSpecDetails[i].displaySpecDesc;
          return rtnVal;
        }
      }
    }

    public virtual string WCode
    {
      get { return WCodeEncode(CategoryCode, Supplier.Code); }
    }

    public static string WCodeEncode(string CategoryCode, string supplierCode)
    {
      try
      {
        if (supplierCode != null && supplierCode.Length > 0)
          return CategoryCode + "-" + supplierCode;
        else
          return CategoryCode;
      }
      catch (Exception) { throw; }
    }

    public virtual double? Density { get; set; }
    private string _Memo = "";
    public virtual string Memo { get { return _Memo; } set { _Memo = value; OnPropertyChanged("Memo"); } }
    private string _PurchaseMemo = "";
    public virtual string PurchaseMemo { get { return _PurchaseMemo; } set { _PurchaseMemo = value; OnPropertyChanged("PurchaseMemo"); } }

    // memory data
    public virtual bool bProduction { get; set; }
    private int _PurchasingState = 0;
    public virtual int PurchasingState
    {
      get { return _PurchasingState; }
      set
      {
        _PurchasingState = value;
      }
    }
  }
}
