using MyHibernateUtil;
using System;

namespace POCO
{
  [Serializable]
  public class RunCard : DataElement
  {
    public RunCard() { }
    public virtual int ID { get; set; }
    public virtual Lot oLot { get; set; }
    public virtual String LotNo { get => oLot?.No ?? ""; set { } }
    public virtual PackageMaterial oCapsule { get; set; }
    public virtual String OPID { get; set; } = "";
    public virtual String Status { get; set; } = "";
    public virtual String Message { get; set; } = "";
    public virtual String Addendum { get; set; } = "";
    public virtual double CapsuleWt { get; set; }
    public virtual double StdRMWt { get; set; }
    public virtual double StandardWt { get { return CapsuleWt + StdRMWt; } set { } }
    public virtual double StdWt { get => StandardWt; }
    public virtual double TargetLSLWt { get; set; } = 0.0;
    public virtual double TargetUSLWt { get; set; } = 0.0;
    public virtual double TargetLCLWt { get; set; } = 0.0;
    public virtual double TargetUCLWt { get; set; } = 0.0;
    public virtual String CapsuleSpeed { get; set; } = "";
    public virtual String Param1 { get; set; } = "";
    public virtual String Param2 { get; set; } = "";
    public virtual int ExtraInfo { get; set; } = 0;
    public virtual string GroupCode { get; set; }
    public virtual int GroupSize { get => oLot?.GroupSize ?? 0; set { } }
    public virtual double TotalWeight { get => oLot?.TotalApplyWeight ?? 0.0; set { } }
    public virtual string EQName { get; set; }

    public override int getID()
    {
      return ID;
    }
  }
}
