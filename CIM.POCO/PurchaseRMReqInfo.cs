using MyHibernateUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using PropertyChanged;

namespace POCO
{
  [Serializable]
  public class PurchaseRMReqInfo : DataElement
  {
    public virtual int ID { get; set; }
    public override int getID() => ID;
    public virtual string OrderNo { get; set; }
    public virtual string OldOrderNo { get; set; }
    public virtual PurchaseOrder oPO { get; set; }
    public virtual ePurchaseStatus Status { get; set; }

    public virtual RawMaterialCategory RMCategory { get; set; }
    public virtual ePOTENCYMETHOD PotencyMethod { get; set; }
    public virtual string ActiveIngredientName { get; set; }
    public virtual double? MethodParameter { get; set; }
    public virtual UnitType PotencyUnit { get; set; }
    public virtual double ReqPotencyQty { get; set; }
    private double _ReqWeight = 0;
    public virtual double ReqWeight
    {
      get { return _ReqWeight; }
      set
      {
        _ReqWeight = value;
        if (PotencyMethod == ePOTENCYMETHOD.NONE)
          RefBuyingWeight = value;
      }
    }
    private RawMaterialSpec _RefRMSpec = null;
    public virtual RawMaterialSpec RefRMSpec
    {
      get { return _RefRMSpec; }
      set
      {
        if (BuyingRMSpec == null && value != null)
          BuyingRMSpec = value;
        _RefRMSpec = value;
      }
    }
    private RawMaterialSpecDetail _RefRMSpecDetail = null;
    public virtual RawMaterialSpecDetail RefRMSpecDetail
    {
      get { return _RefRMSpecDetail; }
      set
      {
        if (BuyingRMSpecDetail == null && value != null)
          BuyingRMSpecDetail = value;
        _RefRMSpecDetail = value;
      }
    }

    private double _RefBuyingWeight = 0;
    public virtual double RefBuyingWeight
    {
      get { return _RefBuyingWeight; }
      set
      {
        _RefBuyingWeight = value;
        OnPropertyChanged("RefBuyingWeight");
        OnPropertyChanged("dispRefBuyingRM");
      }
    }

    public virtual double ReceivedWeight { get; set; } = 0;
    public virtual string dispReceivedWeight => Math.Round(ReceivedWeight, Constant.OPIDIGIT, MidpointRounding.AwayFromZero) + " kg";
    public virtual double BuyingWeight { get; set; }
    public virtual double BuyingPotencyQty { get; set; }
    private RawMaterialSpec _BuyingRMSpec = null;
    public virtual RawMaterialSpec BuyingRMSpec
    {
      get { return _BuyingRMSpec; }
      set
      {
        _BuyingRMSpec = value;
        OnPropertyChanged("BuyingRMSpec");
        OnPropertyChanged("dispBuyingProdName");
      }
    }
    public virtual string dispBuyingProdName => BuyingRMSpec?.RawMaterialName ?? "";
    private RawMaterialSpecDetail _BuyingRMSpecDetail = null;
    public virtual RawMaterialSpecDetail BuyingRMSpecDetail
    {
      get { return _BuyingRMSpecDetail; }
      set
      {
        _BuyingRMSpecDetail = value;
        //if (RefRMSpecDetail != null)
        switch (PotencyMethod)
        {
          case ePOTENCYMETHOD.NONE:
            RefBuyingWeight = ReqWeight;
            break;
          case ePOTENCYMETHOD.PURITY:
            RefBuyingWeight = (value == null) ? 0 : ReqWeight * MethodParameter.Value / value.Potency.Value;//RefRMSpecDetail.Potency.Value / value.Potency.Value;
            break;
          case ePOTENCYMETHOD.POTENCY:
            RefBuyingWeight = (value == null) ? 0 : ReqWeight * MethodParameter.Value / value.Potency.Value;//RefRMSpecDetail.Potency.Value / value.Potency.Value;
            break;
          case ePOTENCYMETHOD.CONCENTRATION:
            RefBuyingWeight = (value == null) ? 0 : ReqWeight * MethodParameter.Value / value.Factor;//RefRMSpecDetail.Factor / value.Factor;
            break;
          case ePOTENCYMETHOD.STANDARDIZE:
            RefBuyingWeight = (value == null) ? 0 : ReqWeight * MethodParameter.Value / value.Potency.Value;//RefRMSpecDetail.Potency.Value / value.Potency.Value;
            break;
        }
      }
    }


    private DateTime _PurchaseDate = new DateTime(2050, 12, 31);
    public virtual DateTime PurchaseDate { get; set; } = new DateTime(2050, 12, 31);
    private double _BuyingQty = 0;
    public virtual double BuyingQty
    {
      get { return _BuyingQty; }
      set
      {
        _BuyingQty = value;
        OnPropertyChanged("BuyingQty");
        if (BuyingUnit != null)
        {
          if (BuyingUnit.uType == eUNITTYPE.PO_VOLUME && BuyingRMSpec != null)
            BuyingWeight = _BuyingQty * BuyingUnit.Multiply * ((BuyingRMSpec.Density != null && BuyingRMSpec.Density.HasValue) ? BuyingRMSpec.Density.Value : 1);
          else if (BuyingUnit.uType == eUNITTYPE.PO_WEIGHT)
            BuyingWeight = _BuyingQty * BuyingUnit.Multiply;
          BuyingWeight = Math.Round(BuyingWeight, 3, MidpointRounding.AwayFromZero);
          TotalPrice = Math.Round(_BuyingQty * UnitCost, 2, MidpointRounding.AwayFromZero);

          OnPropertyChanged("BuyingWeight");
          OnPropertyChanged("TotalPrice");
        }
        OnPropertyChanged("BuyingQty");
      }
    }
    public virtual string displayQty => BuyingQty.ToString() + ((BuyingUnit != null) ? BuyingUnit.AbbrName : "");
    private UnitType _BuyingUnit = null;
    public virtual UnitType BuyingUnit
    {
      get { return _BuyingUnit; }
      set
      {
        _BuyingUnit = value;
        if (_BuyingUnit != null)
        {
          if (_BuyingUnit.uType == eUNITTYPE.PO_VOLUME && BuyingRMSpec != null)
            BuyingWeight = BuyingQty * _BuyingUnit.Multiply * ((BuyingRMSpec.Density != null && BuyingRMSpec.Density.HasValue) ? BuyingRMSpec.Density.Value : 1);
          else if (_BuyingUnit.uType == eUNITTYPE.PO_WEIGHT)
            BuyingWeight = _BuyingQty * _BuyingUnit.Multiply;

          BuyingWeight = Math.Round(BuyingWeight, 2, MidpointRounding.AwayFromZero);
          TotalPrice = Math.Round(BuyingQty * UnitCost, 2, MidpointRounding.AwayFromZero);

          OnPropertyChanged("TotalPrice");
          OnPropertyChanged("BuyingWeight");
        }
        OnPropertyChanged("BuyingUnit");
      }
    }

    public virtual string dispBuyingUnit => BuyingUnit?.AbbrName ?? "";
    public virtual double TotalPrice { get; set; } = 0;
    public virtual string dispTotalPrice => TotalPrice.ToString("C2");
    public virtual string displayPurchaseDate => (PurchaseDate.Year == 2050) ? "" : PurchaseDate.ToShortDateString();
    public virtual bool bOverNormalReceiving => (oPO != null) ? DateTime.Now > oPO.DeliveryDate : DateTime.Now.Subtract(_PurchaseDate).Days >= 30 ? true : false;
    public virtual bool bOverWarningReceiving => DateTime.Now.Subtract(_PurchaseDate).Days >= 90 ? true : false;
    public virtual string dispReqRM => Math.Round(ReqWeight, Constant.OPIDIGIT, MidpointRounding.AwayFromZero).ToString() + " kg";
    public virtual string dispRefBuyingRM => Math.Round(RefBuyingWeight, Constant.OPIDIGIT, MidpointRounding.AwayFromZero).ToString() + " kg";
    public virtual string dispBuyingRM => Math.Round(BuyingWeight, Constant.OPIDIGIT, MidpointRounding.AwayFromZero) + " kg";
    public virtual double VirtualStockRM
    {
      get
      {
        double logicalBuyingWt = ReqWeight;
        if (BuyingWeight > 0)
        {
          if (PotencyMethod == ePOTENCYMETHOD.PURITY)
            logicalBuyingWt = BuyingWeight * BuyingRMSpecDetail.Potency.Value / MethodParameter.Value;
          else if (PotencyMethod == ePOTENCYMETHOD.POTENCY)
            logicalBuyingWt = BuyingWeight * BuyingRMSpecDetail.Potency.Value / MethodParameter.Value;

          else if (PotencyMethod == ePOTENCYMETHOD.CONCENTRATION)
            logicalBuyingWt = BuyingWeight * BuyingRMSpecDetail.Factor / MethodParameter.Value;
          else if (PotencyMethod == ePOTENCYMETHOD.STANDARDIZE)
            logicalBuyingWt = BuyingWeight * BuyingRMSpecDetail.Potency.Value / MethodParameter.Value;
          else
            logicalBuyingWt = BuyingWeight;
        }
        return logicalBuyingWt;
        /*
        if (Status == ePurchaseStatus.PROCESSING)
            return (ReqWeight > BuyingWeight) ? ReqWeight : BuyingWeight;
        else if (Status == ePurchaseStatus.PURCHASED)
            return BuyingWeight;
        else
            return 0;*/
      }
    }
    public virtual string displayVirtualStockRM => Math.Round(VirtualStockRM, Constant.OPIDIGIT, MidpointRounding.AwayFromZero).ToString() + " kg";
    public virtual string displaySpecDetailParameter
    {
      get
      {
        //if (Status == ePurchaseStatus.PROCESSING)
        {
          //if (BuyingRMSpecDetail != null)
          //    return BuyingRMSpecDetail.displaySpecDesc;
          if (PotencyMethod == ePOTENCYMETHOD.NONE) return "";
          if (BuyingRMSpec != null && BuyingRMSpec.RawMaterialSpecDetails != null && BuyingRMSpec.RawMaterialSpecDetails.Count > 0)
          {
            string sSpec = BuyingRMSpec.RawMaterialSpecDetails[0].displaySpecDesc;
            for (int i = 1; i < BuyingRMSpec.RawMaterialSpecDetails.Count; i++)
              sSpec += "," + BuyingRMSpec.RawMaterialSpecDetails[i].displaySpecDesc;
            return sSpec;
          }
          //if (RefRMSpecDetail != null)
          //    return RefRMSpecDetail.displaySpecDesc;
          if (RefRMSpec != null && RefRMSpec.RawMaterialSpecDetails != null && RefRMSpec.RawMaterialSpecDetails.Count > 0)
          {
            string sSpec = RefRMSpec.RawMaterialSpecDetails[0].displaySpecDesc;
            for (int i = 1; i < RefRMSpec.RawMaterialSpecDetails.Count; i++)
              sSpec += "," + RefRMSpec.RawMaterialSpecDetails[i].displaySpecDesc;
            return sSpec;
          }
          if (PotencyMethod == ePOTENCYMETHOD.CONCENTRATION)
            return MethodParameter.ToString() + ":1";
          else if (PotencyMethod == ePOTENCYMETHOD.POTENCY)
          {
            string s = MethodParameter.ToString();
            if (PotencyUnit != null)
              s = s + " " + PotencyUnit.AbbrName;
            return s;
          }
          else if (PotencyMethod == ePOTENCYMETHOD.PURITY)
          {
            return MethodParameter.ToString() + "%";
          }
          else if (PotencyMethod == ePOTENCYMETHOD.STANDARDIZE)
            return MethodParameter.ToString() + "% " + ActiveIngredientName;
          return "";
        }
        /*else if (Status == ePurchaseStatus.PURCHASED)
            return (BuyingRMSpecDetail != null) ? BuyingRMSpecDetail.displaySpecDesc : "";
        else
            return "";*/
      }
    }
    public virtual string displaySupplier => BuyingRMSpec?.Supplier.Name ?? RefRMSpec?.Supplier.Name ?? "";
    public virtual string ItemDescriptionForPO
    {
      get
      {
        if (BuyingRMSpec == null) return "";


        if (PotencyMethod == ePOTENCYMETHOD.POTENCY &&
            PotencyUnit.AbbrName.Equals("PPCB") == false &&
            PotencyUnit.AbbrName.Equals("PPPB") == false)
        {
          return BuyingRMSpec.RawMaterialName + " " + PotencyUnit.AbbrName;
        }
        if (PotencyMethod == ePOTENCYMETHOD.CONCENTRATION ||
            PotencyMethod == ePOTENCYMETHOD.STANDARDIZE)
        {
          string sDesc = BuyingRMSpec.RawMaterialName;
          foreach (RawMaterialSpecDetail rmsd in BuyingRMSpec.RawMaterialSpecDetails)
          {
            if (rmsd.PotencyMethod == ePOTENCYMETHOD.STANDARDIZE)
              sDesc += " " + rmsd.Potency.ToString() + "% " + rmsd.ActiveIngredient.Name;
          }
          foreach (RawMaterialSpecDetail rmsd in BuyingRMSpec.RawMaterialSpecDetails)
          {
            if (rmsd.PotencyMethod == ePOTENCYMETHOD.CONCENTRATION)
            {
              string sRatio = rmsd.Factor.ToString() + ":1";
              if (sDesc.Contains(sRatio) == false)
                sDesc += " (" + sRatio + ")";
            }
          }
          return sDesc;
        }
        return BuyingRMSpec.RawMaterialName;
      }
    }

    public virtual IList<FFI_PRReq> oFFI_PRReqs { get; set; } = new List<FFI_PRReq>();
    public virtual FFI_PRReq AddFFI(FabFormulationItem ffi, VirtualLotInfo vli)
    {
      FFI_PRReq fp = new FFI_PRReq(ffi, this, vli);
      oFFI_PRReqs.Add(fp);
      return fp;
    }

    public virtual UnitType CostCurrency { get; set; }
    private double _UnitCost = 0;
    public virtual double UnitCost
    {
      get { return _UnitCost; }
      set
      {
        _UnitCost = value;
        TotalPrice = BuyingQty * _UnitCost;
        OnPropertyChanged("UnitCost");
        OnPropertyChanged("TotalPrice");
        //OnPropertyChanged("InvoicePrice");
      }
    }
    public virtual string dispUnitCost => UnitCost.ToString("C2");
    public virtual int SamplingWt { get; set; } = 0;
    public virtual bool RadiationTest { get; set; } = false;
    public virtual eREPROCESS Reprocess { get; set; } = eREPROCESS.UNDEFINED;
    public virtual string sReprocess { get => Reprocess == eREPROCESS.UNDEFINED ? "" : Reprocess.ToString(); }
    public virtual string Comment4Production { get; set; }
    //memory
    public virtual bool bDelete { get; set; } = false;
  }
}
