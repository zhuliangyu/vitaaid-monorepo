using System.ComponentModel;

namespace MIS.DBBO
{
  public enum eADJUSTTYPE
  {
    AMOUNT = 1,
    PERCENTAGE = 2
  }
  public enum eDECORATORTYPE
  {
    DISCOUNT = 0,
    TAX
  }
  public enum eDISCOUNTTYPE
  {
    [Description("FIXED CART % DISCOUNT")]
    FIXEDCART_PER = 1,
    [Description("FIXED CART SUBTOTAL DISCOUNT")]
    FIXEDCART_SUB = 2,
    [Description("FIXED PRODUCTS % DISCOUNT")]
    FIXEDPRODUCT_PER = 3,
    [Description("FIXED PRODUCTS FREE DISCOUNT")]
    FIXEDPRODUCT_FREE = 4
  }
  public enum eTARGETSKUSTRATEGY
  {
    [Description("ANY \"SELECTED\" SKU")]
    ANY = 0,
    [Description("EACH \"SELECTED\" SKU")]
    EACH = 1,
    [Description("SAME SKU")]
    SAME = 2
  }
  public enum eACCOUNTRULE
  {
    [Description("ALL ACCOUNTS")]
    ALL = 1,
    [Description("SELECTED ACCOUNTS")]
    SELECTED = 2,
  }
  public enum ePRODUCTRULE
  {
    [Description("ALL PRODUCTS")]
    ALL = 1,
    [Description("SELECTED PRODUCTS")]
    SELECTED = 2,
    [Description("PRODUCTS WILL BE CUSTOMIZED BY ACCOUNT")]
    BYACCOUNT = 3
  }
  public enum eUSAGERULE
  {
    [Description("UNLIMITED")]
    UNLIMITED = 1,
    [Description("ONE TIME USE")]
    ONETIME = 2,
    [Description("FIRST ORDER EVERY MONTH")]
    FIRSTBYMONTH = 3
  }
  public enum ePRICEPOLICY
  {
    [Description("STANDARD PRICE")]
    STANDARD = 0,
    [Description("MSR PRICE")]
    MSRP,
    [Description("STANDARD PRICE(USD)")]
    STANDARD_USD,
    [Description("MSR PRICE(USD)")]
    MSRP_USD,
    [Description("EMPLOYEE PRICE")]
    EMPLOYEE
  }
  public enum eORDERTYPE
  {
    NORMAL = 0,
    CREDIT_MEMO = 1,
    REFUND = 2
  }
  public enum eCREDITTYPE
  {
    EMPLOYEE_CREDIT = 0,
    CUSTOMER_CREDIT = 1,
    GIFTCARD = 2
  }
  public enum eVAPAYMENT
  {
    [Description("Employee Credit")]
    EMPLOYEE_CREDIT = 0,
    [Description("Customer Credit")]
    CUSTOMER_CREDIT = 1,
    [Description("Gift Card")]
    GIFT_CARD = 2,
    [Description("Credit Card")]
    CREDIT_CARD = 3,
    [Description("Cheque")]
    CHEQUE = 4,
    [Description("By Wire")]
    BY_WIRE = 5,
    [Description("Cash")]
    CASH = 6
  }
  public enum eINVOICESTATUS
  {
    [Description("Unpaid")]
    UNPAID = 0,
    [Description("Partially paid")]
    PARTIAL = 1,
    [Description("Declined")]
    DECLINED = 2,
    //[Description("Overdue")]
    //OVERDUE = 3,
    [Description("Paid")]
    PAID = 4,
    [Description("Confirmed")]
    CONFIRMED = 9,
  }
}
