using MyHibernateUtil;
using System;

namespace POCO
{
  [Serializable]
  public class PMDetailApplyLog : POCOBase
  {
    public virtual int ID { get; set; }
    public override int getID() => ID;
    public virtual string PackageCode { get; set; }
    public virtual ePACKAGETYPE PackageType { get; set; }
    public virtual string PackageName { get; set; }
    public virtual string ReceivingNo { get; set; }
    public virtual int BoxNo { get; set; }
    public virtual string LotNo { get; set; }
    public virtual string ProductCode { get; set; }
    public virtual string ProductionName { get; set; }
    public virtual string LogType { get; set; }
    public virtual double ApplyWeight { get; set; }
    public virtual int ApplyCount { get; set; }
    public virtual double BeforeStockWeight { get; set; }
    public virtual double AfterStockWeight { get; set; }
    public virtual int BeforeStockCount { get; set; }
    public virtual int AfterStockCount { get; set; }
    public virtual DateTime CreatedDate { get; set; }
    public virtual string CreatedID { get; set; }
    public virtual string Comment { get; set; }

    public virtual string dispQuantity
    {
      get
      {
        if (PackageType == ePACKAGETYPE.CAPSULE)
          return (ApplyCount.ToString() + "/" + ApplyWeight + "kg");
        return ApplyCount.ToString();
      }
    }

    public virtual string dispOperDate
    {
      get
      {
        if (string.IsNullOrWhiteSpace(CreatedID))
          return "　　　　　　　　/" + CreatedDate.ToString("MM-dd-yyyy");
        else
          return CreatedID + "/" + CreatedDate.ToString("MM-dd-yyyy");
      }
    }

    public virtual string dispPkgType
    {
      get
      {
        switch (PackageType)
        {
          case ePACKAGETYPE.BOTTLE:
            return "BOTTLE";
          case ePACKAGETYPE.CAP:
            return "CAP";
          case ePACKAGETYPE.CAPSULE:
            return "CAPSULE";
          case ePACKAGETYPE.DESICCANT:
            return "DESICCANT";
          case ePACKAGETYPE.LABEL:
            return "LABEL";
          case ePACKAGETYPE.SCOOPS:
            return "SCOOPS";
          default:
            return "UNKNOWN";
        }
      }
    }
    public virtual string GroupCode { get; set; }

    public virtual string DisplayGroupCode
    {
      get => (string.IsNullOrWhiteSpace(GroupCode)) ? Lot.ToRootGroupCode(LotNo) : GroupCode;
    }
  }
}
