using MyHibernateUtil;
using MySystem.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace POCO
{
  [Serializable]
  public class FormulationItem : POCOBase, IMESSequence
  {
    public virtual int ID { get; set; }
    public override int getID() => ID;
    public virtual int Sequence { get; set; }
    public virtual Formulation Formulation { get; set; }
    //public virtual int Sequence { get; set; }
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

    public virtual string strLabelClaim
    {
      get
      {
        return LabelClaim.ToString() + ((LabelClaimUnit != null) ? " " + LabelClaimUnit.AbbrName : "");
      }
    }
    public virtual string strLimits
    {
      get
      {
        if ((LowerBound == null || LowerBound.HasValue == false) &&
            (UpperBound == null || UpperBound.HasValue == false)) return "";
        if ((LowerBound == null || LowerBound.HasValue == false))
          return "(<" + Math.Round(UpperBound.Value, 1, MidpointRounding.AwayFromZero).ToString() + "%)";
        else if ((UpperBound == null || UpperBound.HasValue == false))
          return "(>" + Math.Round(LowerBound.Value, 1, MidpointRounding.AwayFromZero).ToString() + "%)";
        else
          return "(" + Math.Round(LowerBound.Value, 1, MidpointRounding.AwayFromZero).ToString() + "-" + Math.Round(UpperBound.Value, 1, MidpointRounding.AwayFromZero).ToString() + "%)";
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

    public virtual object RawMaterialDetailInWH { get; set; }

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
  }
}
