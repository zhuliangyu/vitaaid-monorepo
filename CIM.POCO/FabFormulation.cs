using MyHibernateUtil;
using MySystem.Base;
using System;


namespace POCO
{
  [Serializable]
  public class FabFormulation : POCOBase
  {
    public FabFormulation() { }
    public virtual int ID { get; set; }
    public override int getID() => ID;
    //public virtual FabProduction Production { get; set; }

    private double _TotalQty = 0.0;
    public virtual double TotalQty { get { return _TotalQty; } set { _TotalQty = value; } }
    public virtual UnitType TotalQtyUnit { get; set; }
    public virtual string dispTotalWeight { get { return TotalQty.ToString() + " " + TotalQtyUnit.AbbrName; } }
    public virtual string FormulationCode { get; set; }

    private NSeqList<FabFormulationItem> _Items;
    public virtual NSeqList<FabFormulationItem> Items
    {
      get
      {
        if (_Items == null)
          _Items = new NSeqList<FabFormulationItem>();
        return _Items;
      }
      set { _Items = value; }
    }

    private NSeqList<FabRawMaterialReq> _RawMaterialReqItems;
    public virtual NSeqList<FabRawMaterialReq> RawMaterialReqItems
    {
      get
      {
        if (_RawMaterialReqItems == null)
          _RawMaterialReqItems = new NSeqList<FabRawMaterialReq>();
        return _RawMaterialReqItems;
      }
      set { _RawMaterialReqItems = value; }
    }
    public virtual int TotalReqItems { get { return RawMaterialReqItems.Count; } }
  }
}
