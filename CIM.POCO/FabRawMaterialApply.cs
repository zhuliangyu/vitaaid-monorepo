using MyHibernateUtil;
using System;
using System.ComponentModel;


namespace POCO
{
  [Serializable]
  public class FabRawMaterialApply : DataElement
  {
    public virtual int ID { get; set; }
    public override int getID() => ID;
    public virtual FabRawMaterialReq RawMaterialReq { get; set; }
    public virtual FabFormulation Formulation { get; set; }
    public virtual FabFormulationItem FormulationItem { get; set; }
    public virtual RawMaterialDetail RMDetail { get; set; }
    public virtual string BarCode { get; set; }
    public virtual double? ReqWeight { get; set; }
    public virtual string displayReqWeight { get { return Math.Round((decimal)ReqWeight, Constant.OPIDIGIT, MidpointRounding.AwayFromZero) + " " + WeightUnit.AbbrName; } }
    //        public virtual double? StockWeightBefore { get; set; }
    //        public virtual string displayStockWeightBefore { get { return (StockWeightBefore.HasValue) ? Math.Round((decimal)StockWeightBefore.Value, 5) + " " + WeightUnit.AbbrName : ""; } }
    public virtual double? ApplyWeight { get; set; }
    public virtual string displayApplyWeight { get { return (ApplyWeight.HasValue) ? Math.Round((decimal)ApplyWeight.Value, Constant.OPIDIGIT, MidpointRounding.AwayFromZero) + " " + WeightUnit.AbbrName : ""; } }
    //        public virtual double? StockWeightAfter { get; set; }
    //        public virtual string displayStockWeightAfter { get { return (StockWeightAfter.HasValue) ? Math.Round((decimal)StockWeightAfter.Value, 5) + " " + WeightUnit.AbbrName : ""; } }
    public virtual UnitType WeightUnit { get; set; }
    public virtual DateTime? DispenseDate { get; set; }
    public virtual string DispenseUserID { get; set; }

    public virtual string Command { get; set; }
    public virtual string Note { get; set; }

    public virtual bool Calibrate { get; set; }

    public virtual int ConfirmType { get; set; }
    public virtual string ConfirmUserID { get; set; }

    private eRMREQSTATUS _Status = eRMREQSTATUS.INIT;
    public virtual eRMREQSTATUS Status
    {
      get { return _Status; }
      set { _Status = value; }
    }

    public virtual string ScannedBarCode { get; set; }

    public virtual int LotID { get; set; }
    public virtual string WHRawMaterialName { get; set; }
    public virtual string WHOldCode { get; set; }
    private bool _Addenda = false;
    public virtual bool Addenda { get { return _Addenda; } set { _Addenda = value; } }

    private int _BatchGroup = 1;
    public virtual int BatchGroup { get { return _BatchGroup; } set { _BatchGroup = value; } }
    public virtual string Comment { get; set; }
    public virtual string StockLocation { get; set; }

    //memory object
    public virtual object FabRMApplyResultObj { get; set; } = null; // for RESTFull
  }
}
