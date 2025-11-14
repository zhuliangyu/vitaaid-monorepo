using MIS.DBBO;

namespace MIS.DBPO
{
  public static class VAPaymentExtension
  {
    public static void setInvoiceData(this VAPayment self, POSInvoice oInvoice)
    {
      self.oInvoice = oInvoice;
      self.InvoiceNo = oInvoice.InvoiceNo;
      self.InvoiceDate = oInvoice.InvoiceDate;
      self.AccountNO = oInvoice.AccountNO;
      self.InvoiceTotalAmount = oInvoice.Total;
    }
    public static void copyInvoiceData(this VAPayment self, VAPayment refPayment)
    {
      self.oInvoice = refPayment.oInvoice;
      self.InvoiceNo = refPayment.InvoiceNo;
      self.InvoiceDate = refPayment.InvoiceDate;
      self.AccountNO = refPayment.AccountNO;
      self.InvoiceTotalAmount = refPayment.InvoiceTotalAmount;
    }
  }
}
