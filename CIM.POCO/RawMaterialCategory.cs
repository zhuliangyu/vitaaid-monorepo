using MyHibernateUtil;
using System;


namespace POCO
{
  [Serializable]
  public class RawMaterialCategory : DataElement
  {
    public RawMaterialCategory() { }
    public virtual int ID { get; set; }
    public override int getID() => ID;
    public virtual string CategoryCode { get; set; }
    public virtual string CategoryName { get; set; }
    public virtual int CountIdx { get; set; }
    public virtual string HSCode { get; set; }
    // transient data member for purchase system
    private int _Flag = 0;
    public virtual int Flag { get { return _Flag; } set { _Flag = value; } }
    // transient data member for Product Management System
    public virtual double dUnitCost { get; set; }
    public virtual string sRefReceivingNo { get; set; }
    public virtual bool MustNotMill { get; set; } = false;
    // memory object
    private bool _bSelect = false;
    public virtual bool bSelect
    {
      get => _bSelect;
      set
      {
        _bSelect = value;
      }
    }
    public virtual string Selected { get => (bSelect) ? "V" : ""; set { } }
  }
}
