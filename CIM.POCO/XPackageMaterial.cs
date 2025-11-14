using MyHibernateUtil;
using System;
using System.ComponentModel;


namespace POCO
{
  [Serializable]
  public class XPackageMaterial : POCOBase
  {
    public virtual int ID { get; set; }
    public override int getID() => ID;
    public virtual string ReceivingNo { get; set; }
    public virtual int BoxNo { get; set; }
    private string _PackingCode = "";
    public virtual string PackingCode
    {
      get { return _PackingCode; }
      set
      {
        _PackingCode = value;
        OnPropertyChanged("PackingCode");
      }
    }
    public virtual string Barcode
    {
      get
      {
        return PackingCode + "*" + ReceivingNo + "*" + BoxNo.ToString("D02");
      }
    }
    public virtual string Name { get; set; }
    private string _LotNumber = "NA";
    public virtual string LotNumber { get { return _LotNumber; } set { _LotNumber = value; } }
    private double? _WeightPerUnit = 0;
    public virtual double? WeightPerUnit { get { return _WeightPerUnit; } set { _WeightPerUnit = (value == null) ? 0 : value; } }
    private DateTime _ReleaseDate = DateTime.Now;
    public virtual DateTime ReleaseDate { get { return _ReleaseDate; } set { _ReleaseDate = value; } }
    private int _StockCount = 0;
    public virtual int StockCount
    {
      get { return _StockCount; }
      set
      {
        _StockCount = value;
        OnPropertyChanged("StockCount");
      }
    }
    public virtual int SupplyCount { get; set; }
    public virtual double? StockWeight { get; set; } = 0.0;
    public virtual double? SupplyWeight { get; set; } = 0.0;
    public virtual double? SafeStock { get; set; } = 0.0;
    private DateTime _ExpireDate = DateTime.Now;
    public virtual DateTime ExpireDate
    {
      get { return _ExpireDate; }
      set
      {
        _ExpireDate = (value.Year == 1) ? MySystem.Base.Extensions.DateTimeExtension.NilDate : value;
      }
    }
    public virtual string DispExpireDate
    {
      get
      {
        return (ExpireDate.Year == 2050) ? "" : ExpireDate.ToString("MM/dd/yyyy");
      }
    }
    private DateTime _RetestDate = DateTime.Now;
    public virtual DateTime RetestDate
    {
      get { return _RetestDate; }
      set
      {
        _RetestDate = (value.Year == 1) ? MySystem.Base.Extensions.DateTimeExtension.NilDate : value;
      }
    }
    public virtual string DispRetestDate
    {
      get
      {
        return (RetestDate == MySystem.Base.Extensions.DateTimeExtension.NilDate) ? "" : RetestDate.ToString("MM/dd/yyyy");
      }
    }
    private string _StockLocation = "";
    public virtual string StockLocation
    {
      get { return _StockLocation; }
      set
      {
        _StockLocation = value;
        OnPropertyChanged("StockLocation");
      }
    }
    public virtual string CreatedID { get; set; } = "";
    private DateTime _CreatedDate = DateTime.Now;
    public virtual DateTime CreatedDate { get { return _CreatedDate; } set { _CreatedDate = value; } }
    public virtual string UpdatedID { get; set; } = "";
    private DateTime _UpdatedDate = DateTime.Now;
    public virtual DateTime UpdatedDate { get { return _UpdatedDate; } set { _UpdatedDate = value; } }
    public virtual string Comment { get; set; }
    private string _Currency1 = "CAD";
    public virtual string Currency1
    {
      get { return _Currency1; }
      set
      {
        _Currency1 = value;
        OnPropertyChanged("Currency1");
      }
    }
    private double _ExchangeRate = 1;
    public virtual double? ExchangeRate
    {
      get { return _ExchangeRate; }
      set
      {
        _ExchangeRate = (value == null) ? 1 : value.Value;
        OnPropertyChanged("ExchangeRate");
      }
    }

    public virtual DateTime ExchangeRateDate { get; set; }
    public virtual Decimal UnitCostForeignCurrency { get; set; }
    public virtual Decimal UnitCostTax { get; set; }
    public virtual Decimal UnitCost { get; set; }
    public virtual Supplier Supplier { get; set; }
    private ePACKAGETYPE _PackageType;
    public virtual ePACKAGETYPE PackageType
    {
      get
      {
        return _PackageType;
      }
      set
      {
        _PackageType = value;
        OnPropertyChanged("PackageType");
      }
    }
    private bool _Disposal = false;
    public virtual bool Disposal { get { return _Disposal; } set { _Disposal = value; OnPropertyChanged("Disposal"); } }
    private bool _PriceChecked = false;
    public virtual bool PriceChecked
    {
      get { return _PriceChecked; }
      set
      {
        _PriceChecked = value;
        OnPropertyChanged("PriceChecked");
      }
    }

    private bool _InWH = true;
    public virtual bool InWH
    {
      get { return _InWH; }
      set
      {
        _InWH = value;
        OnPropertyChanged("InWH");
      }
    }

    private MESPackageSpec _PackageSpec { get; set; }
    public virtual MESPackageSpec PackageSpec
    {
      get { return _PackageSpec; }
      set
      {
        _PackageSpec = value;
        Supplier = (_PackageSpec != null) ? _PackageSpec.Supplier : null;
        if (_PackageSpec != null)
          PackingCode = _PackageSpec.PackageCode;
        OnPropertyChanged("PackageCode");
      }
    }

    public virtual string Division { get; set; }

    private double? _BoxWeight = 0;
    public virtual double? BoxWeight
    {
      get { return _BoxWeight; }
      set
      {
        _BoxWeight = (value == null) ? 0 : value;
        OnPropertyChanged("BoxWeight");
      }
    }

    private double? _GrossWeight = 0;
    public virtual double? GrossWeight
    {
      get { return _GrossWeight; }
      set
      {
        _GrossWeight = (value == null) ? 0 : value;
        OnPropertyChanged("GrossWeight");
      }
    }

    public virtual string ReceivedBy { get; set; }
    private DateTime _ReceivedDate = DateTime.Now;//new DateTime(2059,12,31);
    public virtual DateTime ReceivedDate
    {
      get { return _ReceivedDate; }
      set
      {
        if (value.Year > 1900)
          _ReceivedDate = value;
        OnPropertyChanged("ReceivedDate");
        OnPropertyChanged("ReceivedBy");
        OnPropertyChanged("isReceived");
      }
    }
    public virtual bool isReceived
    {
      get
      {
        return !(ReceivedDate.Year == 2059);
      }
    }

    public virtual string ApprovedBy { get; set; }
    private DateTime _ApprovedDate = new DateTime(2059, 12, 31);
    public virtual DateTime ApprovedDate
    {
      get { return _ApprovedDate; }
      set
      {
        if (value.Year > 1900)
          _ApprovedDate = value;
        OnPropertyChanged("ApprovedDate");
        OnPropertyChanged("ApprovedBy");
        OnPropertyChanged("isApproved");
      }
    }
    public virtual bool isApproved
    {
      get
      {
        return !(ApprovedDate.Year == 2059);
      }
    }

    private bool _bSpcialStorage = false;
    public virtual bool bSpcialStorage
    {
      get { return _bSpcialStorage; }
      set { _bSpcialStorage = value; }
    }
    public virtual string sSpecialStorge { get { return (bSpcialStorage) ? "Yes" : "No"; } }

    private bool _bDamage = true;
    public virtual bool bDamage
    {
      get { return _bDamage; }
      set { _bDamage = value; }
    }
    public virtual string sDamage { get { return (bDamage) ? "Yes" : "No"; } }

    private bool _bSealsIntact = true;
    public virtual bool bSealsIntact
    {
      get { return _bSealsIntact; }
      set { _bSealsIntact = value; }
    }
    public virtual string sSealsIntact { get { return (bSealsIntact) ? "Yes" : "No"; } }

    private bool _bLabelsIntact = true;
    public virtual bool bLabelsIntact
    {
      get { return _bLabelsIntact; }
      set { _bLabelsIntact = value; }
    }
    public virtual string sLabelsIntact { get { return (bLabelsIntact) ? "Yes" : "No"; } }

    private bool _bDeliveredCorrect = true;
    public virtual bool bDeliveredCorrect
    {
      get { return _bDeliveredCorrect; }
      set { _bDeliveredCorrect = value; }
    }
    public virtual string sDeliveredCorrect { get { return (bDeliveredCorrect) ? "Yes" : "No"; } }

    public virtual double QuantityReceived { get; set; }
    public virtual string sQRUnit { get { return (PackageType == ePACKAGETYPE.CAPSULE) ? "KG" : "PCS"; } }

    public virtual string SupplierName { get { return (Supplier == null) ? "" : Supplier.Name; } }
    public virtual string SupplierCode { get { return (Supplier == null) ? "" : Supplier.Code; } }
    public virtual string sPackageType { get { return PackageType.ToString(); } }
    public virtual double? StockLastYear { get; set; }
    private DateTime _ClosedDate = new DateTime(2050, 12, 31);
    public virtual DateTime ClosedDate
    {
      get { return _ClosedDate; }
      set
      {
        if (value.Year != 1)
          _ClosedDate = value;
        OnPropertyChanged("ClosedDate");
      }
    }
    private DateTime _DisposalDate = new DateTime(2050, 12, 31);
    public virtual DateTime DisposalDate
    {
      get { return _DisposalDate; }
      set
      {
        if (value.Year != 1)
          _DisposalDate = value;
        OnPropertyChanged("DisposalDate");
      }
    }
    public virtual string DispDisposalDate
    {
      get
      {
        return (DisposalDate.Year == 2050) ? "" : DisposalDate.ToString("MM/dd/yyyy");
      }
    }
    public virtual PackagePurchaseReq oPurchasePMReq { get; set; }
    public virtual string PONo { get => oPurchasePMReq?.OrderNo ?? ""; set { } }

  }
}
