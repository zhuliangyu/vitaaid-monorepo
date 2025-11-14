using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyHibernateUtil;
using MySystem.Base.Extensions;

namespace WebDB.DBBO
{
  public class OrderData : DataElement
  {
    public virtual int ID { get; set; }
    public override int getID() => ID;
    public virtual string CustomerCode { get; set; } = "";
    public virtual string PONo { get; set; } = "";
    public virtual DateTime OrderDate { get; set; }
    public virtual string PaymentType { get; set; } = "";
    public virtual bool bDropShip { get; set; }
    public virtual string Currency { get; set; } = "";
    public virtual decimal NetSales { get; set; }
    public virtual double dAdjustmentDiscountPercentage { get; set; }
    public virtual string CartDiscountName { get; set; } = "";
    public virtual decimal Adjustment { get; set; }
    public virtual string DispAdjustment { get => Adjustment.ToCurrencyString(true, true, "$"); }
    public virtual decimal ShippingFee { get; set; }
    public virtual bool ShippingByQuote { get; set; } = false;
    public virtual decimal ExtendedAreaSubcharge { get; set; } = 0;
    public virtual decimal SubTotal { get; set; }
    public virtual decimal Total { get; set; }
    public virtual decimal dTaxAmount { get; set; }
    public virtual decimal BalanceDue { get; set; }
    public virtual double TaxRate { get; set; }
    public virtual string TaxTitle { get; set; } = "";
    public virtual decimal dTotalByEarlyPayment { get; set; }
    public virtual decimal dExtraAdjustment { get; set; }
    public virtual string CardNo { get; set; }
    public virtual string CardHolderName { get; set; }
    public virtual decimal UseAmountbyGiftCard { get; set; }
    public virtual int MemberID { get; set; }
    public virtual int VAOrderID { get; set; }
    // memory object
    public virtual OrderItemData[] OrderItems { get; set; }

    //Billing Address
    public virtual string TitleBill { get; set; } = "";
    public virtual string CompanyNameBill { get; set; } = "";
    public virtual string AddrBill { get; set; } = "";
    public virtual string CityBill { get; set; } = "";
    public virtual string ProvinceBill { get; set; } = "";
    public virtual string ZipCodeBill { get; set; } = "";
    public virtual string CountryBill { get; set; } = "";
    public virtual string TelBill { get; set; } = "";
    //Shipping Address
    public virtual string TitleShip { get; set; } = "";
    public virtual string CompanyNameShip { get; set; } = "";
    public virtual string AddrShip { get; set; } = "";
    public virtual string CityShip { get; set; } = "";
    public virtual string ProvinceShip { get; set; } = "";
    public virtual string ZipCodeShip { get; set; } = "";
    public virtual string CountryShip { get; set; } = "";
    public virtual string TelShip { get; set; } = "";

    public virtual string Comment { get; set; } = "";
    // memory object
    public virtual object BillingAddress { get; set; }
    public virtual object ShippingAddress { get; set; }
    public virtual IList<object> Addresses { get => new List<object> { 
      new { Type="BILL", Title=TitleBill, ClinicName=CompanyNameBill, Address=AddrBill, City=CityBill, Province=ProvinceBill, ZipCode=ZipCodeBill, Country=CountryBill, Tel=TelBill},
      new { Type="SHIP", Title=TitleShip, ClinicName=CompanyNameShip, Address=AddrShip, City=CityShip, Province=ProvinceShip, ZipCode=ZipCodeShip, Country=CountryShip, Tel=TelShip}}; }
    public virtual bool IncludeRemoteAreaSurcharge { get => ShippingFee > 0 && ExtendedAreaSubcharge > 0 && ShippingFee >= ExtendedAreaSubcharge; }
    public virtual decimal ShippingFeeNotIncludeRemoteAreaSurcharge
    {
      get
      {
        if (ShippingByQuote) return 0;
        if (IncludeRemoteAreaSurcharge)
          return ShippingFee - ExtendedAreaSubcharge;
        return ShippingFee;
      }
    }
  }
}
