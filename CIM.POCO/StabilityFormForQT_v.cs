using MyHibernateUtil;
using MySystem.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;

namespace POCO
{
    [Serializable]
    public class StabilityFormForQT_v : DataElement
    {
    public virtual int ID { get; set; }
    public override int getID() => ID;
    public virtual Production oProduct { get; set; }
    public virtual Lot oLot { get; set; }
    public virtual string Location { get; set; }
    public virtual string LotNumber { get; set; }
    public virtual DateTime ProduceDate { get; set; } = DateTime.Now;
    public virtual DateTime ExpiryDate { get; set; } = DateTime.Now;
    public virtual string Servings { get; set; }
    public virtual string PackageForm { get; set; }
    public virtual string PackageClosure { get; set; }
    public virtual string PackageCap { get; set; } = "";
    public virtual string PackageBottle { get; set; } = "";
    public virtual string PackageDesiccant { get; set; }
    public virtual string PackageCotton { get; set; }
    public virtual string Comment { get; set; }
    public virtual NSeqList<StabilityTestData> oTestData { get; set; } = new NSeqList<StabilityTestData>();
    public virtual bool Published { get; set; } = false;
    public virtual string ReviewedBy { get; set; }
    //private DateTime _ReviewedDate = SysVal.NilDate;
    public virtual DateTime ReviewedDate { get; set; } = SysVal.NilDate; // { get { return _ReviewedDate; } set { _ReviewedDate = value; } }
    public virtual string ReviewedResult { get; set; }
    public virtual eStabilityTestStatus Status { get; set; }
    private DateTime _TestDate0 = SysVal.NilDate;
    public virtual DateTime TestDate0 { get { return _TestDate0; } set { _TestDate0 = value; } }
    private DateTime _TestDate12 = SysVal.NilDate;
    public virtual DateTime TestDate12 { get { return _TestDate12; } set { _TestDate12 = value; } }
    private DateTime _TestDate24 = SysVal.NilDate;
    public virtual DateTime TestDate24 { get { return _TestDate24; } set { _TestDate24 = value; } }
    private DateTime _TestDate36 = SysVal.NilDate;
    public virtual DateTime TestDate36 { get { return _TestDate36; } set { _TestDate36 = value; } }
    private DateTime _TestDate48 = SysVal.NilDate;
    public virtual DateTime TestDate48 { get { return _TestDate48; } set { _TestDate48 = value; } }
    public virtual double ShelfLife { get; set; }
    public virtual int Version { get; set; } = 1;
    public virtual StabilityForm oNextVersion { get; set; }
    public virtual StabilityForm oPrevVersion { get; set; }
    public virtual string SOPRev { get; set; } = "03";
    // memory object
    public virtual bool bCheck { get; set; } = false;
  }
}
