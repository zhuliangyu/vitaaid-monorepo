namespace MIS.DBBO
{
    public class DiscountProductByAccount : DiscountTargetProduct
    {
        public virtual CustomerAccount oCustomerAccount { get; set; }
        public virtual string CustomerCode { get; set; }
        public virtual string CustomerName { get; set; }
    }
}
