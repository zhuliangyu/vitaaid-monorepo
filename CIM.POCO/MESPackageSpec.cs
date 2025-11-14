using MyHibernateUtil;
using System;
using System.ComponentModel;


namespace POCO
{
  [Serializable]
  public class MESPackageSpec : DataElement
  {
    public virtual int ID { get; set; }
    public override int getID() => ID;
    private string _PackageCode = "";
    public virtual string PackageCode
    {
      get { return _PackageCode; }
      set
      {
        _PackageCode = value;
        OnPropertyChanged("PackageCode");
        OnPropertyChanged("SerialNo");
      }
    }
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

    private string _Name = "";
    public virtual string Name { get { return _Name; } set { _Name = value; OnPropertyChanged("Name"); } }
    private string _Memo = "";
    public virtual string Memo { get { return _Memo; } set { _Memo = value; OnPropertyChanged("Memo"); } }
    private string _Version = "1";
    public virtual string Version { get { return _Version; } set { _Version = value; OnPropertyChanged("Version"); } }
    private bool _Disposal = false;
    public virtual bool Disposal { get { return _Disposal; } set { _Disposal = value; OnPropertyChanged("Disposal"); } }
    public virtual AttachedFile LabelFile { get; set; }
    private double? _Weight = 0;
    public virtual double? Weight { get { return _Weight; } set { _Weight = (value == null) ? 0 : value; OnPropertyChanged("Weight"); } }
    public virtual Supplier Supplier { get; set; }
    public virtual UnitType Unit { get; set; }
    public virtual string VendorItemCode { get; set; }
    private int _SafetyCount = 0;
    public virtual int SafetyCount { get { return _SafetyCount; } set { _SafetyCount = value; OnPropertyChanged("SafetyCount"); } }
    public virtual int SerialNo
    {
      get
      {
        if (PackageCode == null || PackageCode.Length < 8) return 0;
        return Int32.Parse(PackageCode.Substring(6, 2));
      }
      set
      {
        if (PackageCode == null || PackageCode.Length < 6) return;
        PackageCode = PackageCode.Substring(0, 6) + String.Format("{0:D02}", value);
        OnPropertyChanged("PackageCode");
        OnPropertyChanged("SerialNo");
      }
    }
    public virtual string CategoryCode
    {
      get
      {
        return PackageCode.Substring(0, 6);
      }
    }

    //        public virtual double? Weight { get; set; }
    public const int ATTR_COTTON = 1;
    public const int ATTR_INCCAP = 2;
    public virtual int OtherAttributes { get; set; }
    //private bool _bCotton = false;
    public virtual bool bCotton
    {
      get { return ((OtherAttributes & ATTR_COTTON) == ATTR_COTTON); }
      set
      {
        OtherAttributes |= ATTR_COTTON;
        OnPropertyChanged("bCotton");
      }
    }
    public virtual bool bIncCap
    {
      get { return ((OtherAttributes & ATTR_INCCAP) == ATTR_INCCAP); }
      set
      {
        OtherAttributes |= ATTR_INCCAP;
        OnPropertyChanged("bIncCap");
      }
    }
    // memory data
    private string _StockQty = "";
    public virtual string StockQty
    {
      get => _StockQty;
      set
      {
        _StockQty = value;
        OnPropertyChanged("StockQty");
      }
    }
    public virtual MESPackageSpec ShallowCopy()
    {
      return (MESPackageSpec)this.MemberwiseClone();
    }

    public virtual event PropertyChangedEventHandler PropertyChanged;
    public virtual void OnPropertyChanged(string name)
    {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null)
      {
        handler(this, new PropertyChangedEventArgs(name));
      }
    }

  }
}
