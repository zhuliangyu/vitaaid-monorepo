using System;
using MyHibernateUtil;
using PropertyChanged;

namespace MIS.DBBO
{
  [Serializable]
  public class VAOrderItemByLot : POCOBase
  {
    public virtual int ID { get; set; }
    public virtual VAOrder oVAOrder { get; set; }
    public virtual VAOrderItem oItemOwner { get; set; }
    public virtual string SupplyCode { get; set; }
    public virtual string Name { get; set; }
    public virtual string Code { get; set; }
    //public virtual string MESProductCode { get; set; }
    public virtual string PONo { get; set; }
    public virtual double OrderQty { get; set; }
    public virtual double? ShipQty { get; set; }
    public virtual string StockLocation { get; set; } = "";
    public virtual string RetailLotNo { get; set; }
    public virtual int StockCountSnapShot { get; set; }
    public virtual int ReservedCountBottle { get; set; } = 0;
    public virtual DateTime? ExpiredDate { get; set; }
    [DoNotNotify]
    public virtual object Tag { get; set; }
    public virtual string sLotInfoForReport
    {
      get
      {
        return (string.IsNullOrWhiteSpace(RetailLotNo) || ShipQty <= 0) ? "" : RetailLotNo + "(" + ShipQty.ToString() + ")";
      }
    }

    public override int getID()
    {
      return ID;
    }
  }
}
