using MyHibernateUtil;
using System;
using System.Collections.Generic;


namespace POCO
{
  [Serializable]
  public class FabRawMaterialReq : DataElement
  {
    public virtual int ID { get; set; }
    public override int getID() => ID;
    public virtual FabFormulation Formulation { get; set; }
    public virtual FabFormulationItem FormulationItem { get; set; }
    public virtual int dispItemNum { get { return FormulationItem.Sequence; } }
    public virtual string SpecCode { get; set; }
    public virtual string ReceivingNo { get; set; }

    private Supplier _Supplier = null;
    public virtual Supplier Supplier
    {
      get { return _Supplier; }
      set
      {
        _Supplier = value;
        OnPropertyChanged("SupplierName");
      }
    }
    public virtual string SupplierName { get { return (Supplier != null) ? Supplier.Name : ""; } }

    public virtual RawMaterialSpecDetail SpecDetailOfRawMaterial { get; set; }
    public virtual double? ReqWeight { get; set; }
    public virtual double ReqWeightByG
    {
      get
      {
        if (ReqWeight == null) return 0;
        if (WeightUnit.AbbrName == "kg")
          return Math.Round(ReqWeight.Value * 1000, 1, MidpointRounding.AwayFromZero);
        else
          return Math.Round(ReqWeight.Value, 1, MidpointRounding.AwayFromZero);
      }
    }
    public virtual double? BackupReqWeight { get; set; } // for temporate using in the rollback case
    public virtual double? BackupReqWeight1 { get; set; } // for temporate using in listFabRMApplys for MESAPServer

    public virtual bool bTinyWT
    {
      get
      {
        if (ReqWeight == null) return true;
        if (WeightUnit.AbbrName.ToLower().Equals("kg"))
          return (ReqWeight <= 0.001);
        else
          return (ReqWeight <= 1);
      }

    }

    public virtual double? ApplyWeight { get; set; }
    public virtual string displayApplyWeight { get { return (ApplyWeight == null || ApplyWeight.HasValue == false) ? "" : Math.Round((decimal)ApplyWeight.Value, Constant.OPIDIGIT, MidpointRounding.AwayFromZero) + " " + WeightUnit.AbbrName; } }
    public virtual string ActSpec { get => oMRM?.ActSpec ?? null; }
    public virtual UnitType WeightUnit { get; set; }
    public virtual string displayReqWeight { get { return ((Addenda || ReqWeight == null) ? "" : Math.Round((decimal)ReqWeight, Constant.OPIDIGIT, MidpointRounding.AwayFromZero) + " " + WeightUnit.AbbrName); } }
    public virtual string displayReqWeightByG
    {
      get
      {
        if (ReqWeight == null) return "";
        if (WeightUnit.AbbrName == "kg")
          return Math.Round((decimal)ReqWeight * 1000, 1, MidpointRounding.AwayFromZero) + " g";
        else
          return Math.Round((decimal)ReqWeight, 1, MidpointRounding.AwayFromZero) + " g";
      }
    }
    public virtual string RMName { get; set; }
    public virtual string Comment { get; set; }

    private eRMREQSTATUS _Status = eRMREQSTATUS.INIT;
    public virtual eRMREQSTATUS Status { get { return _Status; } set { _Status = value; OnPropertyChanged("Status"); } }
    private bool _ValidInWH = true;
    public virtual bool ValidInWH { get { return _ValidInWH; } set { _ValidInWH = value; } }

    public virtual int LotID { get => oLot?.ID ?? 0; set { } }
    public virtual Lot oLot { get; set; }
    public virtual string displaySpecDetailParameter
    {
      get
      {
        if (SpecDetailOfRawMaterial == null)
          return "";
        return SpecDetailOfRawMaterial.displaySpecDetailParameter;
      }
    }


    public virtual IList<FabRawMaterialApply> RMApplyList { get; set; }
    public virtual bool bAllApplyEnd
    {
      get
      {
        try
        {
          if (RMApplyList != null && RMApplyList.Count > 0)
          {
            foreach (var rmapply in RMApplyList)
            {
              if (rmapply.Addenda == false && (rmapply.ReqWeight != null && rmapply.ReqWeight.HasValue && rmapply.ReqWeight > 0) && (rmapply.Status == eRMREQSTATUS.RESERVE || rmapply.Status == eRMREQSTATUS.DISPENSE))
                return false;
            }
          }
          return true;
        }
        catch (Exception)
        {
          return false;
        }
      }
    }

    public virtual Object WHInfo { get; set; }
    private bool _Addenda = false;
    public virtual bool Addenda { get { return _Addenda; } set { _Addenda = value; } }

    // memory object for showing retest date image
    public virtual MESRawMaterial oMRM { get; set; }
    public virtual XMESRawMaterial oXMRM { get; set; }
    public virtual bool bFinish { get; set; }
    public virtual RawMaterialSpec oRMS { get; set; }
    public virtual RawMaterialCategory oRMCategory { get => oRMS?.Category ?? FormulationItem?.RawMaterial ?? null; }
    public virtual double Density
    {
      get => (oMRM?.Density.HasValue ?? false) ? oMRM.Density.Value :
             (oMRM?.Density.HasValue ?? false) ? oMRM.Density.Value : 0.5;
    }
    public virtual string displayDensity
    {
      get
      {
        string sPart1 = (oMRM == null || oMRM.Density == null) ? "--" : oMRM.Density.ToString();
        string sPart2 = (oRMS == null || oRMS.Density == null) ? "--" : oRMS.Density.ToString();
        if (sPart1.Equals(sPart2) == true)
          return sPart1;
        return sPart1 + "(" + sPart2 + ")";
      }
    }

    public virtual double SpecRatio { get; set; }

    private bool _bDensityError = false;
    public virtual bool bDensityError
    {
      get { return _bDensityError; }
      set { _bDensityError = value; }
    }

    private bool _bHasDensity = false;
    public virtual bool bHasDensity
    {
      get { return _bHasDensity; }
      set { _bHasDensity = value; }
    }
    public virtual bool bMustReprocess { get => !string.IsNullOrEmpty(ReceivingNo) && (oMRM?.Reprocess ?? eREPROCESS.UNDEFINED) == eREPROCESS.MUST_REPROCESS; }
    public virtual bool bMayNeedToReprocess { get => !string.IsNullOrEmpty(ReceivingNo) && (oMRM?.Reprocess ?? eREPROCESS.UNDEFINED) == eREPROCESS.MAY_NEED_TO_REPROCESS; }
    public virtual bool bNoNeedToReprocess { get => !string.IsNullOrEmpty(ReceivingNo) && (oMRM?.Reprocess ?? eREPROCESS.UNDEFINED) == eREPROCESS.NO_NEED_TO_REPROCESS; }
    public virtual bool bMustNotMill { get => !string.IsNullOrEmpty(ReceivingNo) && oRMCategory.MustNotMill; }
    public virtual string Notice { get; set; } = "";

    #region add IMESSequence Feature
    public virtual int Sequence { get; set; }
    public virtual void AddTo(List<FabRawMaterialReq> TargetCol)
    {
      try
      {
        Sequence = TargetCol.Count + 1;
        TargetCol.Add(this);
      }
      catch (Exception ex) { throw ex; }
    }
    public static void RearrangeSeq(List<FabRawMaterialReq> ObjCol)
    {
      try
      {
        if (ObjCol == null) return;
        for (int i = 1; i <= ObjCol.Count; i++)
          ObjCol[i].Sequence = i;
      }
      catch (Exception ex) { throw ex; }
    }
    #endregion
  }
}
