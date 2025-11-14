using System;
using PropertyChanged;
namespace MIS.DBBO
{
    public class MCOrderItem : OrderItem
    {
        public override string CodeOnInvoice { get; set; }
        public override string NameOnInvoice { get; set; }
        public override decimal UnitPrice { get; set; }
        public override string Unit { get; set; }
        public override double Weight { get; set; }
        public override string ProductCode { get => ItemCode; }
        public override string ProductName { get => ItemName; }
        public override double OrderQty { get; set; }
        private double? _shipQty = 0;
        public override double? ShipQty { get { return OrderQty; } set { _shipQty = value; } }
        public override string sDesc { get { return NameOnInvoice; } }
        public override void SetDefaultUnitPrice() { }
        public override double DiscountPercentage { get; set; }

        public override decimal CaculateAmount()
        {
            Amount = Decimal.Round((decimal)OrderQty * UnitPrice, 2, MidpointRounding.AwayFromZero);
            return Amount;
        }
        [DoNotNotify]
        public override object Tag { get; set; }
    }
}
