using System;
using PropertyChanged;
using MyHibernateUtil;

namespace MIS.DBBO
{
    [Serializable]
    public class DistributorSalesCommission : DataElement
    {
        public DistributorSalesCommission() { }

        public DistributorSalesCommission(DistributorSalesDetail oDSD, string sSalesRep, double dPortion, double exchangeRate)
        {
            oCA = oDSD.oDistributor;
            InvoiceNo = oDSD.OrderNumber;
            InvoiceDate = oDSD.OrdersDate;
            AccountNO = oDSD.oDistributor.CustomerCode;
            SalesRep = sSalesRep;// oSalesRep.ShortName;
            Currency1 = ((oDSD.oDistributor.CustomerCountry1?.ToUpper() ?? "") == "CANADA") ? "CAD" : "USD";
            ExchangeRate = exchangeRate;
            BillToName = oDSD.DistributorName;
            NetSales = oDSD.NetSales;
            Portion = dPortion;
            OriginalSalesActivity = NetSales;//Math.Round(Decimal.Floor(NetSales * 100 * (decimal)dPortion) / (decimal)100.0, 2);
            IssuedMonth = ToIssueMonth(oDSD.CommissionMonth);
            OrderCategory = oDSD.Province;
            SalesActivity = Math.Round(Decimal.Floor(oDSD.SalesActivity * 100 * (decimal)dPortion) / (decimal)100.0, 2, MidpointRounding.AwayFromZero);
            // Decimal.Round(Decimal.Floor(invoice.SalesActivity * (decimal)(Portion * 100.0)) / (decimal)100.0, 2);
            CommissionRate = oDSD.oDistributor.OverridedCommissionRate ?? 5;
            CommissionAmount = decimal.Round(SalesActivity * (decimal)ExchangeRate * (decimal)CommissionRate * (decimal)0.01, 2, MidpointRounding.AwayFromZero);
        }
        public static int ToIssueMonth(DateTime oDT) => oDT.Year * 100 + oDT.Month;
        public virtual int ID { get; set; }
        public virtual string InvoiceNo { get; set; }
        public virtual DateTime InvoiceDate { get; set; }
        public virtual string sInvoiceDate { get => (InvoiceDate.Year == 1) ? "" : InvoiceDate.ToString("MM/dd/yyyy"); }
        public virtual string AccountNO { get; set; }
        public virtual string SalesRep { get; set; }
        public virtual string Currency1 { get; set; }
        public virtual double ExchangeRate { get; set; } = 1;
        public virtual string sExchangeRate { get => (ExchangeRate == 1) ? "" : ExchangeRate.ToString(); }
        public virtual string BillToName { get; set; }
        public virtual string OrderCategory { get; set; }
        public virtual decimal NetSales { get; set; }
        public virtual string sNetSales { get => (NetSales < 0) ? "(" + decimal.Round((-1 * NetSales), 2, MidpointRounding.AwayFromZero).ToString() + ")" : NetSales.ToString(); }
        public virtual decimal SalesActivity { get; set; } = 0;
        public virtual decimal OriginalSalesActivity { get; set; } = 0;
        public virtual string sSalesActivity { get => (SalesActivity < 0) ? "(" + decimal.Round((-1 * SalesActivity), 2, MidpointRounding.AwayFromZero).ToString() + ")" : SalesActivity.ToString(); }
        public virtual string sSalesActivityWithCurrencySymbol { get => ((Currency1 == "CAD") ? "" : Currency1 + "*") + sSalesActivity; }
        public virtual decimal SalesActivityCAD { get => decimal.Round(SalesActivity * (decimal)ExchangeRate, 2, MidpointRounding.AwayFromZero); }
        public virtual double Portion { get; set; } = 0;
        public virtual double CommissionRate { get; set; }
        public virtual string sCommissionRate { get => CommissionRate + "%"; }
        public virtual decimal CommissionAmount { get; set; } = 0;
        public virtual int IssuedMonth { get; set; }
        [DoNotNotify]
        public virtual CustomerAccount oCA { get; set; } = null;
        // for report
        public virtual decimal NetSalesInCAD { get => Currency1 == "CAD" ? NetSales : 0; set => throw new NotImplementedException(); }
        public virtual decimal NetSalesInNONCAD { get => Currency1 != "CAD" ? NetSales : 0; set => throw new NotImplementedException(); }
        public virtual decimal SalesActivityInCAD { get => Currency1 == "CAD" ? SalesActivity : 0; set => throw new NotImplementedException(); }
        public virtual decimal SalesActivityInNONCAD { get => Currency1 != "CAD" ? SalesActivity : 0; set => throw new NotImplementedException(); }
        public virtual decimal CommissionInCAD { get => Currency1 == "CAD" ? CommissionAmount : 0; set => throw new NotImplementedException(); }
        public virtual decimal CommissionInNONCAD { get => Currency1 != "CAD" ? CommissionAmount : 0; set => throw new NotImplementedException(); }

        public override int getID()
        {
            return ID;
        }
    }

}
