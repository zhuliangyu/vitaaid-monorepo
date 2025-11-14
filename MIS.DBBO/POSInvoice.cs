using System;
using MyHibernateUtil;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MIS.DBBO
{
  [Serializable]
  public class POSInvoice : POSInvoiceBase
  {
    public virtual VAOrder oOrder { get; set; }
    // memory object
    public virtual ObservableCollection<VAPayment> oPayments { get; set; } = null;
    public virtual bool bSimplestCreditCardPaymentCase(string Currency) {
      return (oPayments?.Where(p => p.PaymentMethod == eVAPAYMENT.CREDIT_CARD &&
                                    p.PaymentStatus != "DECLINED" && p.PaymentStatus != "VOID" &&
                                    p.Currency == Currency)?.Count() ?? 0)== 1;

    }
    public virtual IList<VAPayment> UnconfirmedCreditCardPayment
    {
      get =>
        oPayments?.Where(x => x.PaymentMethod == eVAPAYMENT.CREDIT_CARD &&
                              x.PaymentStatus != "DECLINED" && x.PaymentStatus != "VOID" &&
                              string.IsNullOrWhiteSpace(x.TransactionID))?.ToList() ?? new List<VAPayment>();
    }
    // for UI Checkbox
    public virtual bool bCheck { get; set; }
    public virtual bool bAR { get; set; }
    //[DoNotNotify]
    private static string MakeFilePath(string sType, string sInvoiceNo, string sInvoicePathBase, out string sInvoicePath, out bool bInvoiceFileExist, bool bDefaultByMonth = true)
    {
      sInvoicePath = "";
      bInvoiceFileExist = false;
      string sInvoiceFile = "";
      //D17082096
      string sCategory = sInvoiceNo.Substring(0, 1);
      string sYear = "20" + sInvoiceNo.Substring(1, 2);
      string sMonth = sInvoiceNo.Substring(3, 2);
      if (sCategory == "M")
        //\\server16\Invoice\2018\Manufacture(M)2018\Invoice\06\M18060008.pdf
        sInvoicePath = sInvoicePathBase + sYear + "\\Manufacture(M)" + sYear + "\\" + sType + "\\";
      else if (sCategory == "W")
        //\\server16\Invoice\2018\WholeSales(W)2018\Invoice\08\W18060008.pdf
        sInvoicePath = sInvoicePathBase + sYear + "\\WholeSales(W)" + sYear + "\\" + sType + "\\";
      else if (sCategory == "E")
      {
        sYear = "20" + sInvoiceNo.Substring(3, 2);
        sMonth = sInvoiceNo.Substring(5, 2);
        sInvoicePath = sInvoicePathBase + sYear + "\\Employee(E)" + sYear + "\\" + sType + "\\";
        //sMonth = (sInvoiceNo.Length >= 7) ? sInvoiceNo.Substring(5, 2) : DateTime.Now.Month.ToString("D2");
      }
      else
        //\\server16\Invoice\2018\Doctor(D)2018\Invoice\08\D18060008.pdf
        sInvoicePath = sInvoicePathBase + sYear + "\\Doctor(D)" + sYear + "\\" + sType + "\\";

      if (File.Exists(sInvoicePath + sMonth + "\\" + sInvoiceNo + ".pdf"))
      {
        sInvoiceFile = sInvoicePath + sMonth + "\\" + sInvoiceNo + ".pdf";
        sInvoicePath += sMonth + "\\";
        bInvoiceFileExist = true;
      }
      else if (File.Exists(sInvoicePath + sInvoiceNo + ".pdf"))
      {
        sInvoiceFile = sInvoicePath + sInvoiceNo + ".pdf";
        bInvoiceFileExist = true;
      }
      else
      {
        if (bDefaultByMonth)
          sInvoicePath += sMonth + "\\";
        sInvoiceFile = sInvoicePath + sInvoiceNo + ".pdf";
      }
      return sInvoiceFile;
    }
    private static string MakeFileWithLogoPath(string sType, string sInvoiceNo, string sInvoicePathBase, out string sInvoicePath, out bool bInvoiceFileExist, bool bDefaultByMonth = true)
    {
      sInvoicePath = "";
      bInvoiceFileExist = false;
      string sInvoiceFile = "";
      //D17082096
      string sCategory = sInvoiceNo.Substring(0, 1);
      string sYear = "20" + sInvoiceNo.Substring(1, 2);
      string sMonth = sInvoiceNo.Substring(3, 2);
      if (sCategory == "M")
        //\\server16\Invoice\2018\Manufacture(M)2018\Invoice\06\M18060008.pdf
        sInvoicePath = sInvoicePathBase + sYear + "\\Manufacture(M)" + sYear + "\\" + sType + "\\";
      else if (sCategory == "W")
        //\\server16\Invoice\2018\WholeSales(W)2018\Invoice\08\W18060008.pdf
        sInvoicePath = sInvoicePathBase + sYear + "\\WholeSales(W)" + sYear + "\\" + sType + "\\";
      else if (sCategory == "E")
        sInvoicePath = sInvoicePathBase + sYear + "\\Employee(E)" + sYear + "\\" + sType + "\\";
      else
        //\\server16\Invoice\2018\Doctor(D)2018\Invoice\08\D18060008.pdf
        sInvoicePath = sInvoicePathBase + sYear + "\\Doctor(D)" + sYear + "\\" + sType + "\\";

      if (File.Exists(sInvoicePath + sMonth + "\\" + sInvoiceNo + ".pdf"))
      {
        sInvoiceFile = sInvoicePath + sMonth + "\\" + sInvoiceNo + ".pdf";
        sInvoicePath += sMonth + "\\";
        bInvoiceFileExist = true;
      }
      else if (File.Exists(sInvoicePath + sInvoiceNo + ".pdf"))
      {
        sInvoiceFile = sInvoicePath + sInvoiceNo + ".pdf";
        bInvoiceFileExist = true;
      }
      else
      {
        if (bDefaultByMonth)
          sInvoicePath += sMonth + "\\";
        sInvoiceFile = sInvoicePath + sInvoiceNo + ".pdf";
      }
      return sInvoiceFile;
    }
    public static string InvoiceFile(string sInvoiceNo, string sInvoicePathBase, out string sInvoicePath, out bool bInvoiceFileExist)
    {
      return MakeFilePath("Invoice", sInvoiceNo, sInvoicePathBase, out sInvoicePath, out bInvoiceFileExist);
    }
    public static string InvoiceFile(string sInvoiceNo, string[] sInvoicePathBases, out string sInvoicePath, out bool bInvoiceFileExist)
    {
      sInvoicePath = "";
      bInvoiceFileExist = false;
      string sInvoiceFile = "";
      foreach (var sInvoicePathBase in sInvoicePathBases)
      {
        sInvoiceFile = MakeFilePath("Invoice", sInvoiceNo, sInvoicePathBase, out sInvoicePath, out bInvoiceFileExist);
        if (bInvoiceFileExist)
          return sInvoiceFile;
      }
      return "";
    }
    public static string POFile(string sInvoiceNo, string sInvoicePathBase, out string sInvoicePath, out bool bInvoiceFileExist)
    {
      return MakeFilePath("Purchase Order", sInvoiceNo, sInvoicePathBase, out sInvoicePath, out bInvoiceFileExist, false);
    }
    public static string PackingListFile(string sInvoiceNo, string sInvoicePathBase, out string sInvoicePath, out bool bInvoiceFileExist)
    {
      return MakeFilePath("Packing List", sInvoiceNo, sInvoicePathBase, out sInvoicePath, out bInvoiceFileExist, false);
    }
    private bool UseCAD { get => string.IsNullOrEmpty(Currency1) || Currency1 == "CAD"; }
    // for report
    public virtual string sInvoiceDate { get => (InvoiceDate == null || ((DateTime)(InvoiceDate)).Year == 1) ? "" : InvoiceDate.Value.ToString("MM/dd/yyyy"); }
    public virtual string sSalesActivity { get => (SalesActivity < 0) ? "(" + decimal.Round((-1 * SalesActivity), 2, MidpointRounding.AwayFromZero).ToString() + ")" : SalesActivity.ToString(); }
    public virtual string sSalesActivityWithCurrencySymbol { get => ((UseCAD) ? "" : Currency1 + "*") + sSalesActivity; }
    public virtual string CustomerName { get => (string.IsNullOrEmpty(BillToName) == false) ? BillToName : BillToPerson; }
    public virtual decimal NetSalesInCAD { get => UseCAD ? NetSales : 0; set => throw new NotImplementedException(); }
    public virtual decimal NetSalesInNONCAD { get => !UseCAD ? NetSales : 0; set => throw new NotImplementedException(); }
    public virtual decimal AdjustmentInCAD { get => UseCAD ? Adjustment ?? 0 : 0; set => throw new NotImplementedException(); }
    public virtual decimal AdjustmentInNONCAD { get => !UseCAD ? Adjustment ?? 0 : 0; set => throw new NotImplementedException(); }
    public virtual decimal SalesActivityInCAD { get => UseCAD ? SalesActivity : 0; set => throw new NotImplementedException(); }
    public virtual decimal SalesActivityInNONCAD { get => !UseCAD ? SalesActivity : 0; set => throw new NotImplementedException(); }
    public virtual string sAdjustmentInCAD { get => ((decimal)AdjustmentInCAD < 0) ? "(" + decimal.Round((-1 * (decimal)AdjustmentInCAD), 2, MidpointRounding.AwayFromZero).ToString() + ")" : "0"; }
    public virtual string sAdjustmentInNONCAD { get => ((decimal)AdjustmentInNONCAD < 0) ? "(" + decimal.Round((-1 * (decimal)AdjustmentInNONCAD), 2, MidpointRounding.AwayFromZero).ToString() + ")" : "0"; }
    public virtual string Tag { get; set; } = "";
  }
}
