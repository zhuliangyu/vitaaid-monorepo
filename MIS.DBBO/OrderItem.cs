using System;
using System.Collections.Generic;
using MyHibernateUtil;
using PropertyChanged;

namespace MIS.DBBO
{
    [Serializable]
    public abstract class OrderItem : DataElement
    {
        public virtual int ID { get; set; }
        public virtual VAOrder oVAOrder { get; set; }
        public virtual string Name { get; set; }
        public virtual string Code { get; set; }
        public virtual string PONo { get => oVAOrder?.PONo ?? ""; set { } }
        public virtual string CodeOnInvoice { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public virtual string NameOnInvoice { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public virtual decimal UnitPrice { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public virtual string Unit { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public virtual double OrderQty { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public virtual string ProductCode { get => throw new NotImplementedException(); }
        public virtual bool IsSampleProduct { get => ((ProductCode?.Substring(0, 1) ?? "_").ToUpper() == "X"); }
        public virtual string ProductName { get => throw new NotImplementedException(); }
        //public virtual string MESProductCode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public virtual double? ShipQty { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        //public virtual double BackorderQty { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public virtual DateTime? ExpiredDate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public virtual double Weight { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public virtual string StockLocation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public virtual string StockLocationByLot { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public virtual string RetailLotNo { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public virtual int StockCountSnapShot { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public virtual double DiscountPercentage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        private decimal _RawAmount = 0;
        public virtual decimal RawAmount { get { return (_RawAmount = decimal.Round(UnitPrice * (decimal)ShipQty, 2, MidpointRounding.AwayFromZero)); } set { _RawAmount = value; } }
        public virtual decimal Amount { get; set; } = 0;
        public virtual string Remark { get; set; }
        public virtual string FromBONo { get => oFromBackOrder?.PONo ?? ""; set { } }
        public virtual VAOrder oFromBackOrder { get; set; }
        // memory data
        public virtual string Notes { get; set; }
        public virtual string RemarkAndNotes { get => (Remark ?? "") + (Notes ?? ""); }
        public virtual string LIN01 { get; set; }
        public virtual IList<VAGiftItem> oGifts { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public virtual IList<VAOrderItemByLot> oItemsByLot { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public virtual VAOrderItemByLot getItemByLot(string sRetailLotNo)
        {
            foreach (VAOrderItemByLot oItemByLot in oItemsByLot)
                if (oItemByLot.RetailLotNo.Equals(sRetailLotNo))
                    return oItemByLot;
            return null;
        }
        public virtual VAGiftItem NewGiftItemBy(AppliedCustomerDiscount _oRefDiscount, double _Qty) { return null; }
        public abstract decimal CaculateAmount();
        public virtual Dictionary<string, decimal> CaculateTax()
        {
            try
            {
                return new Dictionary<string, decimal>();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public virtual bool bByProductOrStaticDiscount { get; } = false; // discount because meet special-product-discount or extra custom-discount or Gift
        public virtual string sDesc { get { return ProductCode + " " + ProductName; } }
        public virtual string sQty { get { return OrderQty.ToString(); } }
        public virtual string sShipQty { get { return ShipQty.ToString(); } }
        public virtual int iBackorderQty
        {
            get
            {
                return (int)(OrderQty - ShipQty);
            }
        }
        public virtual string sBackorderQty
        {
            get
            {
                int BackorderQty = (int)(OrderQty - ShipQty);
                return (BackorderQty == 0) ? "" : BackorderQty.ToString();
            }
        }
        private string _sDiscountPercentage;
        public virtual string sDiscountPercentage
        {
            get
            {
                try
                {
                    return (DiscountPercentage == 0) ? "" : DiscountPercentage.ToString() + "%";
                }
                catch (Exception)
                {
                    return "";
                }
            }
            set { _sDiscountPercentage = value; }
        }
        public virtual string sUnitPrice { get { return "$" + UnitPrice.ToString("F2"); } }
        public virtual string sAmount { get { return (Amount < 0) ? "(" + (Amount * -1).ToString("F2") + ")" : Amount.ToString("F2"); } }
        public virtual string ItemName { get { return Name; } }
        public virtual string ItemCode { get { return Code; } }
        public virtual double dStaticDiscount
        {
            get
            {
                DiscountDecorator oDD = null;
                oDD = this as DiscountDecorator;
                while (oDD != null)
                {
                    if (oDD.bStaticDiscount)
                        return oDD.DiscountPercentage;
                    oDD = (oDD.oDecoratedItem as DiscountDecorator);
                }
                return 0;
            }
        }
        public virtual DiscountDecorator oStaticDiscountDecorator
        {
            get
            {
                DiscountDecorator oDD = null;
                for (oDD = this as DiscountDecorator; oDD != null; oDD = (oDD.oDecoratedItem as DiscountDecorator))
                {
                    if (oDD.bStaticDiscount)
                        return oDD;
                }
                return null;
            }
        }
        public virtual bool TemperatureSensitive { get; set; } = false;
        
        // 对温度敏感的商品是否需要冷藏车运输
        public virtual bool ShippingByRefriderator { get => TemperatureSensitive && ShipQty > 0; }


        // memory object
        [DoNotNotify]
        public virtual object Tag { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        [DoNotNotify]
        public virtual IList<VAOrderItemByLot> oDeletedItemsByLot { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public virtual void SetDefaultUnitPrice()
        {
            throw new NotImplementedException();
        }
        public virtual decimal DefaultUnitPrice { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public virtual string sAboutLotNoInfo
        {
            get
            {
                if (oItemsByLot.Count == 0)
                    return "";
                else if (oItemsByLot.Count == 1 && string.IsNullOrWhiteSpace(oItemsByLot[0].RetailLotNo) == false)
                    return "Lot no.:" + oItemsByLot[0].RetailLotNo;
                else
                {
                    string sInfo = "";
                    foreach (VAOrderItemByLot oItemByLot in oItemsByLot)
                        if (oItemByLot.ShipQty > 0)
                            sInfo += ((sInfo.Length > 0) ? ", " : "") + oItemByLot.sLotInfoForReport;
                    return (sInfo.Length > 0) ? "Lot no.(Qty):" + sInfo : "";
                }
            }
        }
        public override int getID()
        {
            return ID;
        }
    }
}
