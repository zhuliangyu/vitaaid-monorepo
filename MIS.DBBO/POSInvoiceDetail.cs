using System;

namespace MIS.DBBO
{
  [Serializable]
  public class POSInvoiceDetail : POSItemDetailBase
  {
    public virtual int InvoiceSequenceNumber { get; set; }
    public virtual decimal? UnitPriceFromCode { get; set; }
    public virtual string UPC { get; set; }
    public virtual string LotNumber { get; set; }
    public virtual decimal SalesActivity { get; set; }
    public virtual decimal CostOfGoodsSold { get; set; }
    public virtual string ItemType { get; set; }
    public virtual string DiscountName { get; set; }
  }
}
