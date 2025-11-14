using System;
using System.Collections.Generic;
using System.ComponentModel;
using PropertyChanged;

namespace MIS.DBBO
{
    [Serializable]
    public class VAOrderItem : OrderItem, INotifyPropertyChanged
    {
        public override string Unit { get; set; } = "Bottle";
        public virtual decimal StandardPrice { get; set; } = 0;
        public virtual decimal MSRPrice { get; set; } = 0;
        public override string CodeOnInvoice { get; set; } = "";
        public virtual decimal EmployeePrice { get; set; } = 0;
        public virtual decimal USDPrice { get; set; } = 0;
        public override string NameOnInvoice { get; set; }
        public override double OrderQty { get; set; }
        private double? _ShipQty = null;
        public override double? ShipQty { get { if (_ShipQty == null) _ShipQty = OrderQty; return _ShipQty; } set { _ShipQty = value; OnPropertyChanged("ShipQty"); OnPropertyChanged("ShippingByRefriderator"); } }
        //public override double BackorderQty { get; set; } = 0;
        public virtual string DispStandardPrice { get { return StandardPrice.ToString("F"); } }
        public virtual string DispMSRPrice { get { return MSRPrice.ToString("F"); } }
        public virtual string DispEmployeePrice { get { return EmployeePrice.ToString("F"); } }
        public virtual string DispUSDPrice { get { return USDPrice.ToString("F"); } }
        public override double DiscountPercentage
        { get => 0.0; set { } }
        public override decimal UnitPrice { get; set; }
        public virtual bool bDecorated { get; set; } = false;
        public override string ProductCode { get { return Code; } }
        public override string ProductName { get { return Name; } }
        //public override string MESProductCode { get; set; }
        public override DateTime? ExpiredDate { get; set; }
        public override double Weight { get; set; }
        public override string StockLocation { get; set; } = "";
        public override string StockLocationByLot
        {
            get
            {
                string sLocation = "";
                foreach (VAOrderItemByLot oItem in oItemsByLot)
                    sLocation += oItem.StockLocation;
                return sLocation;
            }
        }
        public override string RetailLotNo { get; set; }
        public override int StockCountSnapShot { get; set; }
        public override IList<VAOrderItemByLot> oItemsByLot { get; set; } = new List<VAOrderItemByLot>();
        public override decimal CaculateAmount()
        {
            Amount = decimal.Round(UnitPrice * (decimal)ShipQty, 2, MidpointRounding.AwayFromZero);
            return Amount;
        }
        public override VAGiftItem NewGiftItemBy(AppliedCustomerDiscount _oRefDiscount, double _Qty)
        {
            VAGiftItem oGift = new VAGiftItem
            {
                oVAOrder = this.oVAOrder,
                oOwnerItem = this,
                Name = this.Name,
                CodeOnInvoice = this.Code,
                NameOnInvoice = this.Name,
                Code = this.Code,
                PONo = this.PONo,
                StandardPrice = this.StandardPrice,
                MSRPrice = this.MSRPrice,
                EmployeePrice = this.EmployeePrice,
                USDPrice = this.USDPrice,
                OrderQty = _Qty,
                ShipQty = _Qty,
                oDiscount = _oRefDiscount,
                StockLocation = this.StockLocation,
                RetailLotNo = this.RetailLotNo,
                ExpiredDate = this.ExpiredDate,
                Weight = this.Weight,
                StockCountSnapShot = this.StockCountSnapShot,
                Tag = this.Tag,
                TemperatureSensitive = this.TemperatureSensitive,
                //MESProductCode = this.MESProductCode
            };
            return oGift;
        }
        public virtual int StockCountBottle { get; set; } = 0;
        public virtual int ReservedCountBottle { get; set; } = 0;
        public override bool TemperatureSensitive { get; set; } = false;
        [DoNotNotify]
        public override object Tag { get; set; }
        [DoNotNotify]
        public override IList<VAOrderItemByLot> oDeletedItemsByLot { get; set; } = new List<VAOrderItemByLot>();
        public override void SetDefaultUnitPrice()
        {
            UnitPrice = StandardPrice;
            if (oVAOrder.oAccount == null) return;
            switch (oVAOrder.oAccount.PricePolicy)
            {
                case ePRICEPOLICY.STANDARD_USD:
                    UnitPrice = USDPrice;
                    break;
                case ePRICEPOLICY.MSRP:
                    UnitPrice = MSRPrice;
                    break;
                case ePRICEPOLICY.EMPLOYEE:
                    UnitPrice = EmployeePrice;
                    break;
                case ePRICEPOLICY.MSRP_USD:
                    UnitPrice = decimal.Round(USDPrice * 2, 2, MidpointRounding.AwayFromZero);
                    break;
            }
        }

        public override decimal DefaultUnitPrice
        {
            get
            {
                if (oVAOrder.oAccount == null)
                    return StandardPrice;
                switch (oVAOrder.oAccount.PricePolicy)
                {
                    case ePRICEPOLICY.STANDARD_USD:
                        return USDPrice;
                    case ePRICEPOLICY.MSRP:
                        return MSRPrice;
                    case ePRICEPOLICY.EMPLOYEE:
                        return EmployeePrice;
                    case ePRICEPOLICY.MSRP_USD:
                        return decimal.Round(USDPrice * 2, 2, MidpointRounding.AwayFromZero);
                    default:
                        return StandardPrice;
                }
            }
        }
    }
}

