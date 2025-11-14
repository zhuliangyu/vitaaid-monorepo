using MIS.DBBO;
using System.Collections.Generic;
using System.Linq;
using static MIS.DBPO.DBPOServiceHelper;
using MyHibernateUtil.Extensions;
using System;
using MyHibernateUtil;

namespace MIS.DBPO
{
  public static class VAGiftCardExtension
  {
    public static IList<VAPayment> getPayments(this VAGiftCard self) => self.getPayments(MISDB[eST.SESSION0]);
    public static IList<VAPayment> getPayments(this VAGiftCard self, SessionProxy oSession)
    {
      return oSession.QueryDataElement<VAPayment>()
          .Where(x => x.PaymentNote == self.Code)
          .ToList<VAPayment>();
    }
    //public static IList<VAPayment> getPayments(this VAGiftCard self, string sRefInvoiceNo)
    //{
    //    return MISDBSession.QueryDataElement<VAPayment>()
    //        .Where(x => x.PaymentNote == self.Code && x.oInvoice.InvoiceNo == sRefInvoiceNo)
    //        .ToList<VAPayment>();
    //}
    public static void UpdateLastUsedNote(this VAGiftCard self, decimal dUseAmount, string InvoiceNo)
    {
      self.LastUsedDate = DateTime.Now;
      if (dUseAmount >= 0)
        self.LastUsedNote = "Charge " + dUseAmount + ", Invoice: " + InvoiceNo;
      else
        self.LastUsedNote = "Refund " + dUseAmount + ", Invoice: " + InvoiceNo;
      self.LastUsedOn = InvoiceNo;
    }
    public static VAPayment charge(this VAGiftCard self, decimal dUseAmount, string InvoiceNo, SessionProxy oSession)
    {
      try
      {
        if (self.Balance <= 0)
          return null;
        if (self.UsageLimit == 1 && self.UsedCount >= 1 && self.LastUsedOn != InvoiceNo)
          return null;

        if (dUseAmount > 0)
        {
          VAPayment oPayment = new VAPayment
          {
            PayAmount = Math.Min(self.Balance, dUseAmount),
            PaymentType = self.Name,
            PaymentNote = self.Code,
            Currency = self.Currency,
            PaymentMethod = self.CreditType switch
            {
              eCREDITTYPE.EMPLOYEE_CREDIT => eVAPAYMENT.EMPLOYEE_CREDIT,
              eCREDITTYPE.CUSTOMER_CREDIT => eVAPAYMENT.CUSTOMER_CREDIT,
              eCREDITTYPE.GIFTCARD => eVAPAYMENT.GIFT_CARD,
            },
            PaymentStatus = "SUCCESS"
          };
          oPayment.BalanceDueBeforePaying = self.Balance;
          self.Balance = self.Balance - oPayment.PayAmount;
          oPayment.BalanceDue = self.Balance;

          self.UsedCount = self.getPayments(oSession).Where(x => x.InvoiceNo != InvoiceNo)
                                             .GroupBy(x => x.InvoiceNo)
                                             .Where(x => x.Sum(y => y.PayAmount) > 0).Count() + 1;
          self.LastUsedOn = InvoiceNo;
          return oPayment;
        }
        return null;
      }
      catch (Exception)
      {

        throw;
      }
    }
    public static VAPayment refund(this VAGiftCard self, decimal dRefundAmount, string InvoiceNo)
    {
      try
      {
        VAPayment oRefundPayment = new VAPayment
        {
          PayAmount = dRefundAmount * -1,
          PaymentType = self.Name,
          PaymentNote = self.Code,
          Currency = self.Currency,
          PaymentMethod = self.CreditType switch
          {
            eCREDITTYPE.EMPLOYEE_CREDIT => eVAPAYMENT.EMPLOYEE_CREDIT,
            eCREDITTYPE.CUSTOMER_CREDIT => eVAPAYMENT.CUSTOMER_CREDIT,
            eCREDITTYPE.GIFTCARD => eVAPAYMENT.GIFT_CARD,
          },
          PaymentStatus = "SUCCESS"
        };

        oRefundPayment.BalanceDueBeforePaying = self.Balance;
        self.Balance += dRefundAmount;
        oRefundPayment.BalanceDue = self.Balance;

        if (self.UsageLimit == 1 && self.Amount == self.Balance)
          self.UsedCount = 0;

        self.LastUsedOn = InvoiceNo;
        return oRefundPayment;
      }
      catch (Exception)
      {

        throw;
      }
    }
    public static VAPayment removeCredit(this VAGiftCard self, decimal credits, string InvoiceNo)
    {
      VAPayment oPayment = new VAPayment
      {
        PayAmount = credits,
        PaymentType = self.Name,
        PaymentNote = self.Code,
        Currency = self.Currency,
        PaymentMethod = eVAPAYMENT.CUSTOMER_CREDIT,
        PaymentStatus = "SUCCESS"
      };
      oPayment.BalanceDueBeforePaying = self.Balance;
      self.Balance = self.Balance - oPayment.PayAmount;
      self.Amount = self.Amount - oPayment.PayAmount;
      oPayment.BalanceDue = self.Balance;
      self.LastUsedOn = InvoiceNo;
      return oPayment;
    }

  }
}

