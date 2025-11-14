using System;
using System.Collections.ObjectModel;
using EnumsNET;
using MyHibernateUtil;

namespace MIS.DBBO
{
  public class VAPayment : DataElement
  {
    public virtual int ID { get; set; }
    public virtual POSInvoice oInvoice { get; set; }
    public virtual string InvoiceNo { get; set; }
    public virtual System.DateTime? InvoiceDate { get; set; }
    public virtual string AccountNO { get; set; }
    public virtual decimal InvoiceTotalAmount { get; set; }
    public virtual decimal BalanceDueBeforePaying { get; set; } = 0;
    public virtual decimal PayAmount { get; set; } = 0;
    public virtual string Currency { get; set; } = "CAD";
    public virtual double ExchangeRate { get; set; } = 1;

    public virtual eVAPAYMENT PaymentMethod { get; set; } = eVAPAYMENT.CUSTOMER_CREDIT;
    public virtual string PaymentType { get => (PaymentMethod).AsString(EnumFormat.Description); set { } }
    public virtual string PaymentNote { get; set; } // ex: credit card, id, gift card
    public virtual decimal BalanceDue { get; set; } = 0;
    public virtual bool bPaidByCreditOrGiftCard { get => (PaymentMethod == eVAPAYMENT.EMPLOYEE_CREDIT || PaymentMethod == eVAPAYMENT.CUSTOMER_CREDIT || PaymentMethod == eVAPAYMENT.GIFT_CARD); }
    public virtual string PaymentStatus { get; set; }
    public virtual string TransactionID { get; set; }
    public virtual string Comment { get; set; }
    public virtual DateTime TransactionDate { get; set; } = DateTime.Now;

    // memory object
    public virtual ObservableCollection<POSInvoiceDetail> oInvoiceDetails { get; set; }
    public virtual string Tag { get; set; } = "";

    public override int getID()
    {
      return ID;
    }
  }
}
