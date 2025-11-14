using System;
using PropertyChanged;
using MyHibernateUtil;

namespace MIS.DBBO
{
    [Serializable]
    public class SalesCommission : DataElement
    {
        public SalesCommission() { }

        public SalesCommission(POSInvoice invoice, CustomerAccount _oCA, string transferTo, double portion)
        {
            oCA = _oCA;
            InvoiceNo = invoice.InvoiceNo;
            InvoiceDate = invoice.InvoiceDate.Value;
            AccountNO = invoice.AccountNO;
            SalesRep = (transferTo == "") ? invoice.SalesRep : transferTo;
            Currency1 = invoice.Currency1;
            ExchangeRate = (invoice.ExchangeRate == null) ? 1 : (double)invoice.ExchangeRate;
            BillToName = (string.IsNullOrEmpty(invoice.BillToName) == false) ? invoice.BillToName : invoice.BillToPerson;
            NetSales = invoice.NetSales;
            Adjustment = (invoice.Adjustment == null) ? 0 : (decimal)invoice.Adjustment;
            OriginalSalesActivity = invoice.SalesActivity;
            IssuedMonth = InvoiceDate.Year * 100 + InvoiceDate.Month;
            InheritFrom = invoice.SalesRep; //(inheritFrom == "") ? invoice.SalesRep : inheritFrom;
            Portion = (portion <= 0) ? 1 : portion;
            SalesActivity = Decimal.Round(Decimal.Floor(invoice.SalesActivity * (decimal)(Portion * 100.0)) / (decimal)100.0, 2);
        }
        public SalesCommission(POSInvoice invoice) : this(invoice, null, "", 1)
        {
        }
        public virtual int ID { get; set; }
        public virtual string InvoiceNo { get; set; }
        public virtual DateTime InvoiceDate { get; set; }
        public virtual string sInvoiceDate { get => (InvoiceDate.Year == 1) ? "" : InvoiceDate.ToString("MM/dd/yyyy"); }
        public virtual string AccountNO { get; set; }
        public virtual int OrderIdxNo { get; set; }
        public virtual string SalesRep { get; set; }
        public virtual string InheritFrom { get; set; }
        public virtual string SalesRepNote { get => (SalesRep == InheritFrom) ? "" : InheritFrom; }
        public virtual double Portion { get; set; } = 1;
        public virtual string Currency1 { get; set; }
        public virtual double ExchangeRate { get; set; } = 1;
        public virtual string sExchangeRate { get => (ExchangeRate == 1) ? "" : ExchangeRate.ToString(); }
        public virtual string BillToName { get; set; }
        public virtual decimal NetSales { get; set; } = 0;
        public virtual string sNetSales { get => (NetSales < 0) ? "(" + decimal.Round((-1 * NetSales), 2, MidpointRounding.AwayFromZero).ToString() + ")" : NetSales.ToString(); }
        public virtual decimal SalesActivity { get; set; } = 0;
        public virtual decimal OriginalSalesActivity { get; set; } = 0;
        public virtual string sSalesActivity { get => (SalesActivity < 0) ? "(" + decimal.Round((-1 * SalesActivity), 2, MidpointRounding.AwayFromZero).ToString() + ")" : SalesActivity.ToString(); }
        public virtual string sSalesActivityWithCurrencySymbol { get => ((Currency1 == "CAD") ? "" : Currency1 + "*") + sSalesActivity; }
        public virtual decimal SalesActivityCAD { get => decimal.Round(SalesActivity * (decimal)ExchangeRate, 2, MidpointRounding.AwayFromZero); }
        public virtual decimal Adjustment { get; set; } = 0;
        public virtual string sAdjustment { get => ((decimal)Adjustment < 0) ? "(" + decimal.Round((-1 * (decimal)Adjustment), 2, MidpointRounding.AwayFromZero).ToString() + ")" : "0"; }
        public virtual double CommissionRate { get; set; }
        public virtual string sCommissionRate { get => CommissionRate + "%"; }
        public virtual decimal CommissionAmount { get; set; } = 0;
        public virtual int IssuedMonth { get; set; }
        [DoNotNotify]
        public virtual CustomerAccount oCA { get; set; } = null;
        // for report
        public virtual decimal NetSalesInCAD { get => Currency1 == "CAD" ? NetSales : 0; set => throw new NotImplementedException(); }
        public virtual decimal NetSalesInNONCAD { get => Currency1 != "CAD" ? NetSales : 0; set => throw new NotImplementedException(); }
        public virtual decimal AdjustmentInCAD { get => Currency1 == "CAD" ? Adjustment : 0; set => throw new NotImplementedException(); }
        public virtual decimal AdjustmentInNONCAD { get => Currency1 != "CAD" ? Adjustment : 0; set => throw new NotImplementedException(); }
        public virtual decimal SalesActivityInCAD { get => Currency1 == "CAD" ? SalesActivity : 0; set => throw new NotImplementedException(); }
        public virtual decimal SalesActivityInNONCAD { get => Currency1 != "CAD" ? SalesActivity : 0; set => throw new NotImplementedException(); }
        public virtual decimal CommissionInCAD { get => Currency1 == "CAD" ? CommissionAmount : 0; set => throw new NotImplementedException(); }
        public virtual decimal CommissionInNONCAD { get => Currency1 != "CAD" ? CommissionAmount : 0; set => throw new NotImplementedException(); }
        public virtual string sAdjustmentInCAD { get => ((decimal)AdjustmentInCAD < 0) ? "(" + decimal.Round((-1 * (decimal)AdjustmentInCAD), 2, MidpointRounding.AwayFromZero).ToString() + ")" : "0"; }
        public virtual string sAdjustmentInNONCAD { get => ((decimal)AdjustmentInNONCAD < 0) ? "(" + decimal.Round((-1 * (decimal)AdjustmentInNONCAD), 2, MidpointRounding.AwayFromZero).ToString() + ")" : "0"; }

        public override int getID()
        {
            return ID;
        }
    }

}
