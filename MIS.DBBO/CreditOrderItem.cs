namespace MIS.DBBO
{
  // for eDISCOUNTTYPE.FIXEDCART_SUB
  public class CreditOrderItem : OrderItem
  {
    public virtual AppliedCustomerDiscount oDiscount { get; set; }
    public override string CodeOnInvoice { get; set; }
    public override string NameOnInvoice { get; set; }
    public override string Unit { get { return ""; } }
    private decimal _Credit { get; set; }
    public virtual decimal Credit { get { return _Credit; } set { _Credit = value; CaculateAmount(); } }
    public override decimal CaculateAmount()
    {
      Amount = (-1 * Credit);
      RawAmount = Amount;
      return Amount;
    }
    public override string ItemCode { get { return "Credit"; } }
    public override string ProductCode { get => ItemCode; }
    public override string ProductName { get => ItemName; }
    public override decimal RawAmount { get; set; }
    public override string sDesc { get { return ProductName; } }
    public override string sQty { get { return ""; } }
    public override string sUnitPrice { get { return ""; } }
    public override string sShipQty { get { return ""; } }
    public override string sBackorderQty { get => ""; }
    //public override string sAmount { get { return "(" + Credit.ToString("F2") + ")"; } }
    public override void SetDefaultUnitPrice() { }
    public override object Tag { get; set; }
    public override string sDiscountPercentage { get => ""; set { } }
  }
}
