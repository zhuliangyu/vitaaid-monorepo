using System;
using System.Collections.Generic;
using MyHibernateUtil;

namespace MIS.DBBO
{
  [Serializable]
  public class POSInvoiceBase : DataElement
  {
    public virtual int ID { get; set; }
    public virtual string InvoiceNo { get; set; }
    public virtual System.DateTime? InvoiceDate { get; set; }
    public virtual string AccountNO { get; set; }
    public virtual string PurchaseNO { get; set; }
    public virtual string PaymentTerm { get; set; }
    public virtual System.DateTime? DueDate { get; set; }
    public virtual string SalesRep { get; set; }
    public virtual string CustomerRep { get; set; }
    public virtual string ShippedVia { get; set; }
    public virtual string FOB { get; set; }
    public virtual string Currency1 { get; set; }
    public virtual bool bUSD { get => !string.IsNullOrEmpty(Currency1) && Currency1.ToUpper() == "USD"; set { } }
    public virtual double? ExchangeRate { get; set; }
    public virtual decimal? ShipHandling { get; set; }
    public virtual string sShippingFee { get => (ShipHandling != null) ? (Decimal.Round((decimal)ShipHandling, 2, MidpointRounding.AwayFromZero)).ToString() : "0"; }
    public virtual decimal ExtendedAreaSubcharge { get; set; } = 0;
    public virtual string Tax { get; set; }
    public virtual double? GST { get; set; }
    public virtual double? PST { get; set; }
    public virtual double? HST { get; set; }
    public virtual string BillToName { get; set; }
    public virtual string BillToAddress { get; set; }
    public virtual string BillToCityPostCode { get; set; }
    public virtual string BillToCity { get; set; }
    public virtual string BillToProvince { get; set; }
    public virtual string BillToPostCode { get; set; }
    public virtual string BillToCountry { get; set; }
    public virtual string BillToTel { get; set; }
    public virtual string BillToFax { get; set; }
    public virtual string BillToPerson { get; set; }
    public virtual string FullBillAddress
    {
      get { return BillToAddress + " " + BillToCity + "," + BillToCountry; }
    }
    public virtual string ShipToName { get; set; }
    public virtual string ShipToAddress { get; set; }
    public virtual string ShipToCityPostCode { get; set; }
    public virtual string ShipToCity { get; set; }
    public virtual string ShipToProvince { get; set; }
    public virtual string ShipToPostCode { get; set; }
    public virtual string ShipToCountry { get; set; }
    public virtual string ShipToTel { get; set; }
    public virtual string ShipToFax { get; set; }
    public virtual string ShipToPerson { get; set; }
    public virtual string FullShipAddress
    {
      get { return ShipToAddress + " " + ShipToCity + "," + ShipToCountry; }
    }
    public virtual string CreatedTime { get; set; }
    public virtual string UpdatedTime { get; set; }
    public virtual string InvoiceMsg1 { get; set; }
    public virtual string InvoiceMsg2 { get; set; }
    public virtual string InvoiceMsg3 { get; set; }
    public virtual string InvoiceMsg4 { get; set; }
    public virtual string InvoiceMsg5 { get; set; }
    public virtual string InvoiceMsg6 { get; set; }
    public virtual string InvoiceMsg7 { get; set; }
    public virtual string InvoiceMsg8 { get; set; }
    public virtual string Comment { get; set; }
    public virtual string PackingMsg1 { get; set; }
    public virtual string PackingMsg2 { get; set; }
    public virtual string PackingMsg3 { get; set; }
    public virtual decimal? Adjustment { get; set; }
    public virtual string sAdjustment { get => (Adjustment != null && (decimal)Adjustment < 0) ? decimal.Round((-1 * (decimal)Adjustment), 2, MidpointRounding.AwayFromZero).ToString() : "0"; }
    public virtual string AdjustmentTax { get; set; }
    public virtual double? AdjustmentGST { get; set; }
    public virtual double? AdjustmentPST { get; set; }
    public virtual double? AdjustmentHST { get; set; }
    public virtual double? DiscountRate { get; set; }
    public virtual System.DateTime? InvoiceDate2 { get; set; }
    public virtual System.DateTime? DueDate2 { get; set; }
    public virtual string PaymentType { get; set; }
    public virtual string FOBLabel { get; set; }
    public virtual decimal NetSales { get; set; } = 0;
    public virtual string sNetSales { get => (NetSales < 0) ? "(" + decimal.Round((-1 * NetSales), 2, MidpointRounding.AwayFromZero).ToString() + ")" : NetSales.ToString(); }
    public virtual decimal SalesActivity { get; set; } = 0;
    public virtual decimal SalesActivityCAD { get => decimal.Round(SalesActivity * (decimal)(ExchangeRate ?? 1), 2, MidpointRounding.AwayFromZero); }
    public virtual decimal SubTotal { get; set; } = 0;
    public virtual string sSubTotal { get => (SubTotal < 0) ? "(" + decimal.Round((-1 * SubTotal), 2, MidpointRounding.AwayFromZero).ToString() + ")" : SubTotal.ToString(); }
    public virtual decimal dTaxAmount { get; set; }
    public virtual string sTaxAmount
    {
      get
      {
        return (dTaxAmount < 0) ? "(" + Decimal.Round(dTaxAmount * -1, 2, MidpointRounding.AwayFromZero).ToString() + ")" :
               (dTaxAmount > 0) ? Decimal.Round(dTaxAmount, 2, MidpointRounding.AwayFromZero).ToString() : "";
      }
    }
    public virtual decimal Total { get; set; } = 0;
    public virtual string sTotal { get => Decimal.Round(Total, 2, MidpointRounding.AwayFromZero).ToString(); }
    public virtual decimal? dTotalByEarlyPayment { get; set; } = null;
    public virtual decimal BalanceDue { get; set; }
    public virtual string sBalanceDue { get => Decimal.Round(BalanceDue, 2, MidpointRounding.AwayFromZero).ToString(); }
    public virtual double TaxRate { get; set; }
    public virtual string TaxTitle { get; set; }
    public virtual string ExtraAdjustmentTitle { get; set; }
    public virtual decimal ExtraAdjustmentAmount { get; set; } = 0;
    public virtual string CustomerPONo { get; set; } = "";
    public virtual IList<POSInvoiceDetail> oInvoiceDetails { get; set; } = new List<POSInvoiceDetail>();
    public virtual eORDERTYPE OrderType { get; set; } = eORDERTYPE.NORMAL;
    public virtual decimal Credits { get; set; }
    public virtual decimal DueByTotalMinusCredit { get => Total - Credits; }
    public virtual eINVOICESTATUS InvoiceStatus { get; set; } = eINVOICESTATUS.UNPAID;
    public virtual bool Voided { get; set; } = false;
    public virtual string InternalNotice { get; set; }
    // memory object
    public virtual bool bIssuePayment { get; set; } = false;
    public virtual bool bOverdue { get; set; }
    public virtual bool bOverdue30 { get; set; }
    public virtual bool bOverdue60 { get; set; }
    public virtual bool bOverdue6x { get; set; }
    public virtual decimal CurrentDue { get; set; } = 0;
    public virtual decimal Overdue1_30 { get; set; } = 0;
    public virtual decimal Overdue31_60 { get; set; } = 0;
    public virtual decimal Overdue_61 { get; set; } = 0;
    public override int getID()
    {
      return ID;
    }
  }
}
