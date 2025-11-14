using MyHibernateUtil;
using System;
using System.ComponentModel;

namespace POCO
{
  [Serializable]
  public class PrintRawMaterialLabelLog : POCOBase
  {
    public PrintRawMaterialLabelLog() { }
    public PrintRawMaterialLabelLog(RawMaterialDetail oRMD)
    {
      RawMaterialDetail = oRMD;
      RawMaterialSpec = oRMD.RawMaterialSpec;
      ReceivingNo = oRMD.ReceivingNo;
      BoxNumber = oRMD.BoxNumber;
      RetestDate = oRMD.RetestDate;
      Barcode = oRMD.Barcode;
      InWH = oRMD.InWH;
      RawMaterialLotNumber = oRMD.RawMaterialLotNumber;
      SupplyWeightBox = oRMD.SupplyWeightBox;
      BagWeight = oRMD.BagWeight;
      GrossWeight = oRMD.GrossWeight;
      StockWeight = oRMD.StockWeight;
      ReserveWeight = oRMD.ReserveWeight;
      StockLocation = oRMD.StockLocation;
      Disposal = oRMD.Disposal;
      SafeStock = oRMD.SafeStock;
      Comment = oRMD.Comment;
      UpdatedID = oRMD.UpdatedID;
      UpdatedDate = oRMD.UpdatedDate;
      RawMaterialName = oRMD.RawMaterialName;
    }

    public virtual int ID { get; set; }
    public override int getID() => ID;
    public virtual RawMaterialDetail RawMaterialDetail { get; set; }
    public virtual RawMaterialSpec RawMaterialSpec { get; set; }
    public virtual string ReceivingNo { get; set; }
    public virtual int BoxNumber { get; set; }
    public virtual DateTime RetestDate { get; set; }
    public virtual string Barcode { get; set; }
    public virtual bool InWH { get; set; }
    public virtual string RawMaterialLotNumber { get; set; }
    public virtual string CreatedID { get; set; }
    public virtual DateTime CreatedDate { get; set; }
    public virtual double? SupplyWeightBox { get; set; }
    public virtual double? BagWeight { get; set; }
    public virtual double? GrossWeight { get; set; }
    public virtual double? StockWeight { get; set; }
    public virtual double? ReserveWeight { get; set; }
    public virtual string StockLocation { get; set; }
    public virtual bool Disposal { get; set; }
    public virtual double? SafeStock { get; set; }
    public virtual string Comment { get; set; }
    public virtual string UpdatedID { get; set; }
    public virtual DateTime UpdatedDate { get; set; }
    public virtual string RawMaterialName { get; set; }
    public virtual string ShortRetestDate
    {
      get
      {
        return RetestDate.ToString("yyMM");
      }
      set
      {

        int year, month;
        if (Int32.TryParse(value.Substring(0, 2), out year) && Int32.TryParse(value.Substring(2, 2), out month))
        {
          year = 2000 + Int32.Parse(value.Substring(0, 2));
          month = Int32.Parse(value.Substring(2, 2));
          RetestDate = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
        }
      }
    }
    public virtual int Version { get; set; }
  }
}
