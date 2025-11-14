using MyHibernateUtil;
using System;
using System.ComponentModel;


namespace POCO
{
  [Serializable]
  public class FabRawMaterialBatch : POCOBase
  {
    public virtual int ID { get; set; }
    public override int getID() => ID;
    public virtual FabFormulation Formulation { get; set; }
    public virtual FabFormulationItem FormulationItem { get; set; }
    public virtual int Sequence { get; set; }
    public virtual double ReqWeight { get; set; }
    public virtual string displayReqWeight { get { return Math.Round((decimal)ReqWeight, 5, MidpointRounding.AwayFromZero /*Constant.OPIDIGIT*/) + " " + WeightUnit.AbbrName; } }
    public virtual string displayReqWeightByG
    {
      get
      {
        if (WeightUnit.AbbrName.ToLower().Equals("kg"))
          return Math.Round((decimal)ReqWeight * 1000, 2, MidpointRounding.AwayFromZero) + " g";
        else
          return Math.Round((decimal)ReqWeight, 2, MidpointRounding.AwayFromZero) + " " + WeightUnit.AbbrName;
      }
    }
    public virtual double ApplyWeight { get; set; }
    public virtual string displayApplyWeight { get { return Math.Round((decimal)ApplyWeight, 5, MidpointRounding.AwayFromZero /*Constant.OPIDIGIT*/) + " " + WeightUnit.AbbrName; } }
    public virtual UnitType WeightUnit { get; set; }
    private DateTime _CreatedDate = DateTime.Now;
    public virtual DateTime CreatedDate { get { return _CreatedDate; } set { _CreatedDate = value; } }
    public virtual string CreatedID { get; set; }

    public virtual DateTime? DispenseDate { get; set; }
    public virtual string DispenseUserID { get; set; }

    public virtual string Command { get; set; }
    public virtual string Note { get; set; }

    public virtual int ConfirmType { get; set; }
    public virtual string ConfirmUserID { get; set; }

    public virtual string LotNo { get; set; }
    public virtual int BatchNo { get; set; }
    public virtual Lot oLot { get; set; }
    public virtual string BatchCode { get; set; }
    private int _BatchGroup = 1;
    public virtual int BatchGroup { get { return _BatchGroup; } set { _BatchGroup = value; } }
    public virtual bool bMultipleBatchGroup { get; set; } = false;
    public virtual string Comment { get; set; }
    public virtual bool bBatchEnd { get; set; }

    // MEMORY OBJECT
    public virtual string SubBag { get; set; }
    public virtual string SubBagWeight { get; set; }
  }
}
