using System;
using MyHibernateUtil;
using MySystem.Base.Extensions;

namespace MIS.DBBO
{
  public class VitaAidProductBase : DataElement
  {
    public virtual int ID { get; set; }
    public virtual string SupplyCode { get; set; }
    public virtual string ProductCode { get; set; }
    public virtual string ProductName { get; set; }
    public virtual string RetailLotNo { get; set; }
    public virtual string FabLotNo { get; set; }
    public virtual string MESProductCode { get; set; }
    public virtual string FormulationCode { get; set; }
    public virtual int Servings { get; set; }
    public virtual string DispServingTitle
    {
      get
      {
        return ((FabLotNo == null) ? "Servings:" :
            FabLotNo.Contains("P") ? "Servings:" :
            FabLotNo.Contains("L") ? "Servings:" : "Capsules:");
      }
    }
    public virtual int StockCount { get; set; }
    public virtual int SupplyCount { get; set; }
    public virtual int InitSupplyCount { get; set; }
    public virtual DateTime ExpiredDate { get; set; } = DateTime.Now;
    public virtual string StockLocation { get; set; }
    public virtual string LabelVersion { get; set; }
    public virtual string Status { get; set; }
    public virtual string Comment { get; set; }
    public virtual int StockLastYear { get; set; }
    public virtual DateTime ClosedDate { get; set; } = new DateTime(DateTime.Now.Year, 9, 30);
    public virtual string GroupCode { get; set; }
    public virtual double? StdWtPerUnit { get; set; }
    public virtual bool IsOEM { get; set; } = false;
    public virtual decimal UnitCost { get; set; } = (decimal)0.0;
    public virtual bool Salable { get; set; } = true;
    public virtual bool IsDisposal { get; set; } = false;
    public virtual DateTime DisposalDate { get; set; } = DateTimeExtension.NilDate;
    public virtual bool IsSample { get; set; }
    public override int getID()
    {
      return ID;
    }
    public virtual bool bSemiProduct { get { return !IsFinishProduct; } set { IsFinishProduct = !value; } }
    public virtual bool IsFinishProduct { get; set; } = true;
    public virtual bool bQuarantine { get { return (Status != null && Status.Equals("QUARANTINE")); } }
    public virtual DateTime LastQuarantineDate { get; set; }
    public virtual string DispLastQuarantineDate => (LastQuarantineDate.Year == 1) ? "" : LastQuarantineDate.ToString();
    public virtual void SetQuarantine(bool bValue = true)
    {
      if (bValue)
      {
        Status = "QUARANTINE";
        LastQuarantineDate = DateTime.Now;
      }
      else
        Status = "QUARANTINE";
    }
    public virtual int OverdueLevelQuarantineArea
    {
      get
      {
        if (LastQuarantineDate.Year == 1) return 0;
        var diff = DateTime.Now - LastQuarantineDate;
        if (diff.TotalDays < 7) return 0;
        else if (diff.TotalDays >= 7 && diff.TotalDays < 10) return 1;
        else return 2;
      }
    }
    public virtual string QuarantineDays
    {
      get
      {
        if (LastQuarantineDate.Year == 1) return "";
        return Math.Floor((DateTime.Now - LastQuarantineDate).TotalDays).ToString() + " days";
      }
    }
    public virtual int OverdueLevelFPArea { get => 0; }
    public virtual string ReleasedDays
    {
      get => "";
    }
    public virtual string Desc
    {
      get
      {
        return RetailLotNo + ":" + LabelVersion + ":" + ProductCode + ":" + ProductName + " - " + StockCount + " bottles";
      }
    }
  }
}
