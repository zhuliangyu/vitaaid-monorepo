using MyHibernateUtil;
using MySystem.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace POCO
{
  [Serializable]
  public class FabFormulationItem : POCOBase, IMESSequence
  {
    // FabFormulationItem Flag
    public static int FFI_DEL_ADJUST = 0x0001;
    public static int FFI_MOD_ADJUST = 0x0002;
    public static int FFI_ADD_ADJUST = 0x0004;

    public virtual int ID { get; set; }
    public override int getID() => ID;
    public virtual int Sequence { get; set; }
    public virtual FabFormulation Formulation { get; set; }

    private double _LabelClaim = 0.0;
    public virtual double LabelClaim { get { return _LabelClaim; } set { _LabelClaim = value; } }
    public virtual UnitType LabelClaimUnit { get; set; }
    private double _Excess = 0.0;
    public virtual double Excess { get { return _Excess; } set { _Excess = value; } }
    private double _TheoreticalFillingQty = 0;
    public virtual double TheoreticalFillingQty
    {
      get { return _TheoreticalFillingQty; }
      set
      {
        _TheoreticalFillingQty = value;
        OnPropertyChanged("TheoreticalFillingQty");
        OnPropertyChanged("DisplayTheoreticalFillingQty");
        OnPropertyChanged("CandidatedCountInWH");
      }
    }
    public virtual string DisplayTheoreticalFillingQty { get => (TheoreticalFillingQty == 0) ? "" : TheoreticalFillingQty.ToString(); }
    public virtual UnitType FillingQtyUnit { get; set; }
    public virtual RawMaterialCategory RawMaterial { get; set; }
    public virtual ePOTENCYMETHOD PotencyMethod { get; set; }
    public virtual string ActiveIngredientName { get; set; }
    public virtual double? MethodParameter { get; set; }
    public virtual string displayMethodParameter
    {
      get
      {
        if (MethodParameter == null || MethodParameter.HasValue == false ||
            PotencyMethod == ePOTENCYMETHOD.NONE || PotencyMethod == ePOTENCYMETHOD.PURITY || PotencyMethod == ePOTENCYMETHOD.POTENCY)
          return "";
        return MethodParameter.ToString();
      }
    }
    public virtual Ingredient ActiveIngredient { get; set; }
    private int _GroupNo = 1;
    public virtual int GroupNo { get { return _GroupNo; } set { _GroupNo = value; } }
    public virtual string Note { get; set; }

    private bool _bEnough = true;
    public virtual bool bEnough { get { return _bEnough; } set { _bEnough = value; } }
    private int _Flag = 0;
    public virtual int Flag { get { return _Flag; } set { _Flag = value; } }
    public virtual int OldFlag { get; set; }

    public virtual bool isADDAdjust { get { return ((Flag & FFI_ADD_ADJUST) == FFI_ADD_ADJUST); } }
    public virtual bool isDELAdjust { get { return ((Flag & FFI_DEL_ADJUST) == FFI_DEL_ADJUST); } }
    public virtual bool isMODAdjust { get { return ((Flag & FFI_MOD_ADJUST) == FFI_MOD_ADJUST); } }
    public virtual bool isCriticalRM
    {
      get
      {/*
            if (GroupNo == 5 || RMReqList.Count != 1)
                return false;
            if (RMReqList[0].oMRM == null)
            {
                IList<MESRawMaterial> mrmList = BEOBase<MESRawMaterial>.GetXObjs1("x.ReceivingNo='" + oRM.RMReq.ReceivingNo + "'");
                oRM.RMReq.oMRM = (mrmList != null && mrmList.Count() == 1) ? mrmList[0] : null;
            }
            */
        return (GroupNo != 5 && RMReqList.Count == 1 &&
                                         RMReqList[0].oMRM != null && RMReqList[0].oMRM.StockWeight <= 0.05);

      }
    }
    public virtual bool bCriticalRM { get; set; }

    private NSeqList<FabRawMaterialReq> _RMReqList = new NSeqList<FabRawMaterialReq>();
    public virtual NSeqList<FabRawMaterialReq> RMReqList
    {
      get { return _RMReqList; }
      set { _RMReqList = value; }
    }
    private Object _RMReqCandidateList = null;
    public virtual Object RMReqCandidateList
    {
      get { return _RMReqCandidateList; }
      set
      {
        if (value == null)
          _RMReqCandidateList = null;
        else
          _RMReqCandidateList = value;
      }
    }
    public virtual string strLabelClaim
    {
      get
      {
        return LabelClaim.ToString() + ((LabelClaimUnit != null) ? " " + LabelClaimUnit.AbbrName : "");
      }
    }
    public static string EnCode(string Category, string rmCode)
    {
      if (Category != null && rmCode != null &&
                  Category.Length > 0 && rmCode.Length > 0)
        return Category + " [" + rmCode + "]";
      else
        return "";
    }
    public static void DeCode(string str, out string Category, out string rmCode)
    {
      if (str == null || str.Length == 0 ||
          str.Contains("[") == false || str.Contains("]") == false)
      {
        rmCode = "";
        Category = "";
        return;
      }
      char[] delimiterChars = { '[', ']' };
      string[] token = str.Split(delimiterChars);
      Category = token[0].Trim();
      rmCode = token[1].Trim();
    }
    public virtual string strCodeCategory
    {
      get
      {
        return (RawMaterial == null) ? "" : EnCode(RawMaterial.CategoryName, RawMaterial.CategoryCode);
      }
    }
    public virtual string mainCategory
    {
      get
      {
        return (RawMaterial != null && RawMaterial.CategoryCode.Length >= 3) ? RawMaterial.CategoryCode.Substring(0, 3) : "";
      }
    }
    private bool _Exchangeable = true;
    public virtual bool Exchangeable
    {
      get { return _Exchangeable; }
      set
      {
        _Exchangeable = value;
        OnPropertyChanged("Exchangeable");
        OnPropertyChanged("dispExchangeable");
      }
    }
    public virtual string dispExchangeable
    {
      get
      {
        return (PotencyMethod == ePOTENCYMETHOD.CONCENTRATION || PotencyMethod == ePOTENCYMETHOD.STANDARDIZE) ?
            Exchangeable.ToString() :
            "";
      }
    }
    public virtual string Assay { get; set; }
    public virtual double? LowerBound { get; set; }
    public virtual double? UpperBound { get; set; }

    public virtual object RawMaterialDetailInWH { get; set; }
    public virtual string sTag1 { get; set; }
    public virtual string sTag2 { get; set; }
    public virtual object oTag { get; set; }
    public virtual object oTag1 { get; set; }
    public virtual double dTag { get; set; }
    public virtual string printDTag
    {
      get
      {
        return Math.Round((decimal)dTag, Constant.OPIDIGIT, MidpointRounding.AwayFromZero) + " kg";
      }
    }
    public virtual double? dAlterParameter { get; set; }
    public virtual IList<double> AlterParameters { get; set; }
    public virtual bool bEmptyAlterParameters
    {
      get { return (AlterParameters == null || AlterParameters.Count == 0) ? true : false; }
    }
  }
}
