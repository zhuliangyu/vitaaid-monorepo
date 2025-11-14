namespace MIS.DBBO
{
    public class VAGiftItem : VAOrderItem
    {
        public virtual AppliedCustomerDiscount oDiscount { get; set; }
        public virtual OrderItem oOwnerItem { get; set; }
        public override string ItemName => "(FREE) " + base.ItemName;
        public override decimal RawAmount { get { return 0; } }
        public override decimal Amount { get { return 0; } }
        public override double DiscountPercentage { get; set; } = 100;
        public override string sDiscountPercentage { get { return "100%"; } }
        public override decimal CaculateAmount() { return 0; }
        public override bool bByProductOrStaticDiscount { get { return true; } }
        //public override string MESProductCode { get; set; }
        //public override double BackorderQty { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
