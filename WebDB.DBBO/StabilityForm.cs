using MyHibernateUtil;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebDB.DBBO
{
  [Serializable]
  public class StabilityForm : DataElement
  {
    public virtual int ID { get; set; }
    public override int getID() => ID;
    public virtual int ProductID { get; set; }
    public virtual string Code { get; set; }
    public virtual string Name { get; set; }
    public virtual int LotID { get; set; }
    public virtual string LotNumber { get; set; }
    public virtual DateTime ProduceDate { get; set; } = DateTime.Now;
    public virtual string sProduceDate { get { return ProduceDate.ToString("MMM-yyyy"); } }
    public virtual DateTime ExpiryDate { get; set; } = DateTime.Now;
    public virtual string sExpiryDate { get { return ExpiryDate.ToString("MMM-yyyy"); } }
    public virtual string Servings { get; set; }
    public virtual string PackageForm { get; set; }
    public virtual string PackageClosure { get; set; }
    public virtual string PackageCap { get; set; } = "";
    public virtual string PackageBottle { get; set; } = "";
    public virtual string PackageDesiccant { get; set; }
    public virtual string PackageCotton { get; set; }
    public virtual string Comment { get; set; }
    public virtual string ReviewedBy { get; set; }
    public virtual DateTime ReviewedDate { get; set; } = SysVal.NilDate;
    public virtual string ReviewedResult { get; set; }
    public virtual string DispReviewedDate { get { return (ReviewedDate.Year == SysVal.NilDate.Year) ? "" : ReviewedDate.ToString("MMM/dd/yyyy"); } }
    public virtual string DispReviewedBy
    {
      get { return ((ReviewedBy == null) ? "" : ReviewedBy + ", Date:" + DispReviewedDate/* + "       Result:" + ReviewedResult*/); }
    }

    public virtual string sSuperscriptDesc { get; set; }
    public virtual double ShelfLife { get; set; }
    public virtual string DispShelfLife
    {
      get { return (ShelfLife > 0) ? ShelfLife.ToString() + " months" : "NA"; }
    }
    public virtual int StorageCondition { get; set; }
    public virtual string sStorageCondition { get => (StorageCondition == 0) ? "room temperature" : "refrigerate"; }
    public virtual string SOPRev { get; set; } = "03";
    public virtual bool Published { get; set; } = false;
    // Memory Object
    public virtual string LProductImg { get; set; } = "";
    public virtual IList<StabilityTestData> oTestData { get; set; }
    [JsonIgnore]
    public virtual object Tag { get; set; }
    public virtual bool SomeDataIsInvalid()
    {
      return (oTestData.Where(x => x.bValidResult == false || x.bValidSpec == false).Any());
    }
    public virtual int ServingSize { get; set; } = 1;
    public virtual string ServingUnit { get; set; } = "Capsule";
    public virtual string sServingSize { get => string.Format("{0} {1}(s)", ServingSize, ServingUnit); }
    public virtual int ServingsPerContainer { get; set; } = 1;
  }
}
