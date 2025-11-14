using System;
using MyHibernateUtil;
using MySystem.Base.Extensions;

namespace MIS.DBBO
{
  public class SettlementDetail : DataElement
  {
    public virtual int ID { get; set; }
    public virtual string InvoiceNumber { get; set; }
    public virtual System.DateTime? XactTime { get; set; }
    public virtual decimal Amount { get; set; }
    public virtual string Currency { get; set; } = "CAD";
    public virtual string XactType { get; set; }
    public virtual string ApprovalCode { get; set; } // ex: credit card, id, gift card
    public virtual string AccountData { get; set; }
    public virtual string PaymentNote { get; set;  }
    public virtual DateTime? SettleDate { get; set; }
    public virtual string TransactionID { get; set; }
    public virtual string UserID { get; set; }
    public virtual int PaymentID { get; set; }
    // memory object
    public virtual string bCheck { get; set; } = "";
    public virtual decimal PayAmount { get => XactType.Contains("Return") ? -1 * Amount : Amount; }
    public virtual string bConfirmed { get => (PaymentID > 0) ? "V" : ""; }
    public override int getID()
    {
      return ID;
    }
  }
}
