using MyHibernateUtil;
using MySystem.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace POCO
{
  [Serializable]
  public class TestDefinition : POCOBase, IMESSequence
  {
    public virtual int ID { get; set; }
    public override int getID() => ID;
    public virtual int Sequence { get; set; }
    public virtual Production oProduct { get; set; }
    private string _TestName;
    public virtual string TestName
    {
      get { return _TestName; }
      set
      {
        _TestName = value;
        OnPropertyChanged("TestName");
      }
    }
    private string _TestDesc;
    public virtual string TestDesc
    {
      get { return _TestDesc; }
      set
      {
        _TestDesc = value;
        OnPropertyChanged("TestDesc");
      }
    }
    private string _LabelClaim;
    public virtual string LabelClaim
    {
      get { return _LabelClaim; }
      set
      {
        _LabelClaim = value;
        OnPropertyChanged("LabelClaim");
      }
    }
    private double? _LowerBound;
    public virtual double? LowerBound
    {
      get { return _LowerBound; }
      set
      {
        _LowerBound = value;
        OnPropertyChanged("LowerBound");
        OnPropertyChanged("displayLimit");
      }
    }
    private double? _UpperBound;
    public virtual double? UpperBound
    {
      get { return _UpperBound; }
      set
      {
        _UpperBound = value;
        OnPropertyChanged("UpperBound");
        OnPropertyChanged("displayLimit");
      }
    }
    private string _Assay;
    public virtual string Assay
    {
      get { return _Assay; }
      set
      {
        _Assay = value;
        OnPropertyChanged("Assay");
      }
    }

    public virtual string displayLimit
    {
      get
      {
        if (LowerBound != null && UpperBound != null && LowerBound.HasValue && UpperBound.HasValue)
          return LowerBound.ToString() + " ~ " + UpperBound.ToString() + "%";
        else
          return "";
      }
    }

    // memory
    public static string QBI = "Quantification by Input";
    private bool _bDelete = false;
    public virtual bool bDelete
    {
      get { return _bDelete; }
      set
      {
        _bDelete = value;
        //OnPropertyChanged("TestName");
      }
    }
  }
}
