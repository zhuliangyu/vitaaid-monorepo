using System;

namespace MIS.DBBO
{
  [Serializable]
  public class POSInvoiceDetailBackOrder : POSItemDetailBase
  {
    public POSInvoiceDetailBackOrder() { }
    public POSInvoiceDetailBackOrder(POSInvoiceDetail oInvoiceDetail)
    {
      InvoiceNO = oInvoiceDetail.InvoiceNO;
      AccountNO = oInvoiceDetail.AccountNO;
      ProductCode = oInvoiceDetail.ProductCode;
      ProductName = oInvoiceDetail.ProductName;
      //MESProductCode = oInvoiceDetail.MESProductCode;
      CountOrder = oInvoiceDetail.CountOrder;
      CountShipped = oInvoiceDetail.CountShipped;
      CountUnit = oInvoiceDetail.CountUnit;
      UnitPrice = oInvoiceDetail.UnitPrice;
      Discount = oInvoiceDetail.Discount;
      Tax = oInvoiceDetail.Tax;
      GST = oInvoiceDetail.GST;
      PST = oInvoiceDetail.PST;
      HST = oInvoiceDetail.HST;
      PackingListNO = oInvoiceDetail.PackingListNO;
      CreatedTime = oInvoiceDetail.CreatedTime;
      UpdatedTime = oInvoiceDetail.UpdatedTime;
      Comment = oInvoiceDetail.Comment;
    }
    public virtual int InvoiceSequenceNumber { get; set; }
    public virtual double? CustomerDiscount { get; set; }
    public virtual string CustomerEmail1 { get; set; }
    public virtual string SalesRep { get; set; }
    public virtual string CustomerOwner { get; set; }
    public virtual bool bCheck { get; set; } // memory object, UI purpose
  }
}
