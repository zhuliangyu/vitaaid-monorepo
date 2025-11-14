using System;
using System.Collections.Generic;
using System.Diagnostics;
using PropertyChanged;

namespace MIS.DBBO
{

    public class DiscountDecorator : OrderItem
    {
        public virtual OrderItem oDecoratedItem { get; set; }
        public virtual AppliedCustomerDiscount oDiscount { get; set; }
        public virtual eDISCOUNTTYPE DiscountType { get; set; } = eDISCOUNTTYPE.FIXEDPRODUCT_PER;
        public override string PONo
        {
            set
            {
                base.PONo = value;
                if (oDecoratedItem is VAOrderItem)
                    ((VAOrderItem)oDecoratedItem).PONo = value;
                if (oDecoratedItem is DiscountDecorator)
                    ((DiscountDecorator)oDecoratedItem).PONo = value;
            }
        }
        public virtual VAOrderItem DecoratedVAItem
        {
            get
            {
                if (oDecoratedItem is VAOrderItem)
                    return (VAOrderItem)oDecoratedItem;
                if (oDecoratedItem is DiscountDecorator)
                    return ((DiscountDecorator)oDecoratedItem).DecoratedVAItem;
                return null;
            }
        }
        private double _DiscountPercentage;
        public override double DiscountPercentage
        {
            get
            {
                if (oDiscount != null)
                {
                    //Debug.Assert(oDiscount.oDiscountProgram.DiscountType != eDISCOUNTTYPE.FIXEDCART_SUB && oDiscount.oDiscountProgram.DiscountType != eDISCOUNTTYPE.FIXEDCART_PER);
                    if (oDiscount.oDiscountProgram.DiscountType == eDISCOUNTTYPE.FIXEDPRODUCT_FREE || oDiscount.oDiscountProgram.DiscountType == eDISCOUNTTYPE.FIXEDCART_SUB || oDiscount.bApplied == false)
                        _DiscountPercentage = 0;
                    else
                        _DiscountPercentage = oDiscount.oDiscountProgram.DiscountPercentage();
                }
                else if (bStaticDiscount)
                    return _DiscountPercentage;
                else if (this is OptionDiscountDecorator)
                    _DiscountPercentage = 0;
                return _DiscountPercentage;
            }
            set
            {
                _DiscountPercentage = value;
            }
        }
        public override decimal RawAmount
        {
            get
            {
                return DecoratedVAItem.RawAmount;
            }
        }
        //public virtual eDISCOUNTPOLICY Policy { get; set; } = eDISCOUNTPOLICY.INNER; 
        public override IList<VAOrderItemByLot> oItemsByLot { get => DecoratedVAItem.oItemsByLot; set => DecoratedVAItem.oItemsByLot = value; }

        public override string sQty { get { return oDecoratedItem.sQty; } }
        public override string sShipQty { get { return oDecoratedItem.sShipQty; } }
        public override string CodeOnInvoice
        {
            get { return DecoratedVAItem.CodeOnInvoice; }
            set { DecoratedVAItem.CodeOnInvoice = value; }
        }
        public override string NameOnInvoice
        {
            get { return DecoratedVAItem.NameOnInvoice; }
            set { DecoratedVAItem.NameOnInvoice = value; }
        }
        public override double OrderQty
        {
            get { return DecoratedVAItem.OrderQty; }
            set { DecoratedVAItem.OrderQty = value; }
        }
        public override double? ShipQty
        {
            get { return DecoratedVAItem.ShipQty; }
            set { DecoratedVAItem.ShipQty = value; }
        }
        //public override string MESProductCode
        //{
        //    get { return DecoratedVAItem.MESProductCode; }
        //    set { DecoratedVAItem.MESProductCode = value; }
        //}
        public override string LIN01
        {
            get { return DecoratedVAItem?.LIN01 ?? ""; }
            set { DecoratedVAItem.LIN01 = value; }
        }

        //public override double BackorderQty
        //{
        //	get { return DecoratedVAItem.BackorderQty; }
        //	set { DecoratedVAItem.BackorderQty = value; }
        //}
        public override decimal UnitPrice { get => DecoratedVAItem.UnitPrice; set => DecoratedVAItem.UnitPrice = value; }
        [DoNotNotify]
        public override string sDiscountPercentage
        {
            get
            {
                if (!bByProductOrStaticDiscount)
                    return "";
                DiscountDecorator oDD = this;
                int iPercentageDisountCount = 0;
                double dPerDiscount = 0;
                while (oDD != null)
                {
                    if (oDD.oDiscount != null && oDD.oDiscount.bApplied == false)
                    {
                        oDD = (oDD.oDecoratedItem as DiscountDecorator);
                        continue;
                    }
                    //Debug.Assert(oDD.DiscountType != eDISCOUNTTYPE.FIXEDCART_PER && oDD.DiscountType != eDISCOUNTTYPE.FIXEDCART_SUB);
                    if (oDD.DiscountType == eDISCOUNTTYPE.FIXEDPRODUCT_PER && oDD.DiscountPercentage > 0)
                    {
                        iPercentageDisountCount++;
                        dPerDiscount = oDD.DiscountPercentage;
                    }
                    oDD = (oDD.oDecoratedItem as DiscountDecorator);
                }
                if (iPercentageDisountCount > 1)
                {
                    decimal dOriginalAmount = DecoratedVAItem.CaculateAmount();
                    decimal dFinalAmount = CaculateAmount();
                    dPerDiscount = (double)(decimal.Round((dOriginalAmount - dFinalAmount) * 100 / dOriginalAmount, 0, MidpointRounding.AwayFromZero));
                    if (dPerDiscount * 100 % 100 == 99)
                        dPerDiscount += 0.01;
                }
                return (dPerDiscount > 0) ? dPerDiscount.ToString() + "%" : "";
            }
        }

        public override string Unit { get { return DecoratedVAItem.Unit; } set { DecoratedVAItem.Unit = value; } }
        public virtual bool bDecorated { get; set; } = false;
        public override IList<VAGiftItem> oGifts { get; set; } = new List<VAGiftItem>();
        public override string ProductCode { get { return DecoratedVAItem.ProductCode; } }
        public override string ProductName { get { return DecoratedVAItem.ProductName; } }
        public override DateTime? ExpiredDate { get { return DecoratedVAItem.ExpiredDate; } set { DecoratedVAItem.ExpiredDate = value; } }
        public override double Weight { get { return DecoratedVAItem.Weight; } set { DecoratedVAItem.Weight = value; } }
        public override string StockLocation { get { return DecoratedVAItem.StockLocation; } set { DecoratedVAItem.StockLocation = value; } }
        public override string StockLocationByLot { get { return DecoratedVAItem.StockLocationByLot; } }
        public override string RetailLotNo { get { return DecoratedVAItem.RetailLotNo; } set { DecoratedVAItem.RetailLotNo = value; } }
        public override int StockCountSnapShot { get { return DecoratedVAItem.StockCountSnapShot; } set { DecoratedVAItem.StockCountSnapShot = value; } }
        public override bool TemperatureSensitive { get => DecoratedVAItem.TemperatureSensitive; set => DecoratedVAItem.TemperatureSensitive = value; }
        public override VAGiftItem NewGiftItemBy(AppliedCustomerDiscount _oRefDiscount, double _Qty) { return oDecoratedItem.NewGiftItemBy(_oRefDiscount, _Qty); }
        public override string FromBONo { get => DecoratedVAItem?.FromBONo ?? ""; set { } }
        public override VAOrder oFromBackOrder { get => DecoratedVAItem?.oFromBackOrder; set => DecoratedVAItem.oFromBackOrder = value; }
        public override decimal CaculateAmount()
        {
            Remark = "";
            if (oDiscount == null)//bExtraDiscount)
            {
                return (Amount = decimal.Round(oDecoratedItem.CaculateAmount() * ((decimal)(1 - 0.01 * _DiscountPercentage)), 2, MidpointRounding.AwayFromZero));
            }
            if (oDiscount.oDiscountProgram.bMeetItemCondition((double)OrderQty, UnitPrice) == false)
            {
                oGifts.Clear();
                return (Amount = oDecoratedItem.CaculateAmount());
            }
            if (oDiscount.oDiscountProgram.DiscountType == eDISCOUNTTYPE.FIXEDPRODUCT_FREE)
            {
                double dFreeCount = (oDiscount.oDiscountProgram.MinimumQuantity > 0) ?
                                (double)oDiscount.oDiscountProgram.DiscountAmount * (int)(OrderQty / (double)oDiscount.oDiscountProgram.MinimumQuantity) :
                                (double)oDiscount.oDiscountProgram.DiscountAmount;
                if (oGifts.Count > 0)
                {
                    oGifts[0].OrderQty = dFreeCount;
                    oGifts[0].ShipQty = dFreeCount;
                }
                else
                {
                    VAGiftItem oGift = oDecoratedItem.NewGiftItemBy(oDiscount, dFreeCount);
                    oGifts.Add(oGift);
                }
                Remark = "Get " + oGifts[0].OrderQty.ToString() + " FREE bottle(s) by " + oDiscount.oDiscountProgram.Name + " program!";
                return (Amount = oDecoratedItem.CaculateAmount());
            }
            else if (oDiscount.oDiscountProgram.DiscountType == eDISCOUNTTYPE.FIXEDPRODUCT_PER)
                return (Amount = decimal.Round(oDecoratedItem.CaculateAmount() * ((decimal)(1 - 0.01 * oDiscount.oDiscountProgram.DiscountAmount)), 2, MidpointRounding.AwayFromZero));
            else
                return (Amount = oDecoratedItem.CaculateAmount());
        }

        public override string ItemName { get { return ProductName; } }
        public override string ItemCode { get { return ProductCode; } }
        public virtual bool bStaticDiscount
        {
            get
            {
                return this is DiscountDecorator && (oDiscount == null);// || oDiscount.bApplied == false));
            }
        }

        public override bool bByProductOrStaticDiscount
        {
            get
            {
                if (bStaticDiscount) return true;
                if (oDiscount != null && 
                    (oDiscount.oDiscountProgram.DiscountType == eDISCOUNTTYPE.FIXEDPRODUCT_FREE || oDiscount.oDiscountProgram.DiscountType == eDISCOUNTTYPE.FIXEDPRODUCT_PER))
                    return true;
                return false;
            }

        }  // discount because meet special-product-discount or extra custom-discount

        public virtual object UIObj { get; set; }
        public override string sAmount { get { return (bByProductOrStaticDiscount) ? base.sAmount : RawAmount.ToString("F2"); } }
        [DoNotNotify]
        public override object Tag { get { return DecoratedVAItem.Tag; } set { DecoratedVAItem.Tag = value; } }
        [DoNotNotify]
        public override IList<VAOrderItemByLot> oDeletedItemsByLot { get => DecoratedVAItem.oDeletedItemsByLot; set => DecoratedVAItem.oDeletedItemsByLot = value; }
        public override void SetDefaultUnitPrice()
        {
            DecoratedVAItem.SetDefaultUnitPrice();
        }
        public override decimal DefaultUnitPrice { get => DecoratedVAItem.DefaultUnitPrice; set => throw new NotImplementedException(); }
    }
}