using MyHibernateUtil;
using System;
using System.ComponentModel;


namespace POCO
{
  [Serializable]
  public class RawMaterialSpecDetail : DataElement
  {
    public virtual int ID { get; set; }
    public override int getID() => ID;
    public virtual RawMaterialSpec RawMaterialSpec { get; set; }
    private string _ActiveIngredientName = "";
    public virtual string ActiveIngredientName
    {
      get { return _ActiveIngredientName; }
      set
      {
        _ActiveIngredientName = value;
        OnPropertyChanged("ActiveIngredientName");
      }
    }
    private double? _Potency;
    public virtual double? Potency
    {
      get { return _Potency; }
      set
      {
        _Potency = value;
        OnPropertyChanged("Potency");
      }
    }
    private UnitType _Unit;
    public virtual UnitType Unit
    {
      get { return _Unit; }
      set
      {
        _Unit = value;
        OnPropertyChanged("Unit");
      }
    }
    private UnitType _BaseUnit;
    public virtual UnitType BaseUnit
    {
      get { return _BaseUnit; }
      set
      {
        _BaseUnit = value;
        OnPropertyChanged("BaseUnit");
      }
    }
    private int _Factor;
    public virtual int Factor
    {
      get { return _Factor; }
      set
      {
        _Factor = value;
        OnPropertyChanged("Factor");
      }
    }
    private ePOTENCYMETHOD _PotencyMethod;
    public virtual ePOTENCYMETHOD PotencyMethod
    {
      get { return _PotencyMethod; }
      set
      {
        _PotencyMethod = value;
        OnPropertyChanged("PotencyMethod");
      }
    }
    private Ingredient _Actingredient;
    public virtual Ingredient ActiveIngredient
    {
      get { return _Actingredient; }
      set
      {
        _Actingredient = value;
        OnPropertyChanged("ActiveIngredient");
      }
    }
    public virtual bool Invalid { get; set; } = false;

    public virtual string displaySpecDetailParameter
    {
      get
      {
        if (PotencyMethod == ePOTENCYMETHOD.NONE) return "";
        if (PotencyMethod == ePOTENCYMETHOD.CONCENTRATION)
          return Factor.ToString();
        else if (PotencyMethod == ePOTENCYMETHOD.POTENCY)
        {
          string s = Potency.ToString();
          if (Unit != null)
            s = s + " " + Unit.AbbrName;
          if (BaseUnit != null)
            s = s + "/" + BaseUnit.AbbrName;
          return s;
        }
        else if (PotencyMethod == ePOTENCYMETHOD.PURITY)
          return (Potency).ToString();
        else if (PotencyMethod == ePOTENCYMETHOD.STANDARDIZE)
          return Potency.ToString();
        return "";
      }
    }
    public virtual string displaySpecDesc
    {
      get
      {
        if (PotencyMethod == ePOTENCYMETHOD.NONE) return "";
        if (PotencyMethod == ePOTENCYMETHOD.CONCENTRATION)
          return Factor.ToString() + ":1";
        else if (PotencyMethod == ePOTENCYMETHOD.POTENCY)
        {
          string s = Potency.ToString();
          if (Unit != null)
            s = s + " " + Unit.AbbrName;
          if (BaseUnit != null)
            s = s + "/" + BaseUnit.AbbrName;
          return s;
        }
        else if (PotencyMethod == ePOTENCYMETHOD.PURITY)
        {
          return Potency.Value.ToString() + "%";
        }
        else if (PotencyMethod == ePOTENCYMETHOD.STANDARDIZE)
          return Potency + "% " + ActiveIngredientName;
        return "";

      }
    }
    public virtual string displaySpecDescForPOReport
    {
      get
      {
        if (PotencyMethod == ePOTENCYMETHOD.NONE || PotencyMethod == ePOTENCYMETHOD.PURITY) return "";
        if (PotencyMethod == ePOTENCYMETHOD.CONCENTRATION)
          return Factor.ToString() + ":1";
        else if (PotencyMethod == ePOTENCYMETHOD.POTENCY)
        {
          string s = Potency.ToString();
          if (Unit != null)
            s = s + " " + Unit.AbbrName;
          if (BaseUnit != null)
            s = s + "/" + BaseUnit.AbbrName;
          return s;
        }
        else if (PotencyMethod == ePOTENCYMETHOD.STANDARDIZE)
          return Potency + "% " + ActiveIngredientName;
        return "";

      }
    }

  }
}
