using MIS.DBBO;
using MIS.DBPO;
using MySystem.Base.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vitaaid.com.Models.Account;
//using VA.MIS.APServer.Models.Account;
using WebDB.DBBO;

namespace vitaaid.com.Models.ShoppingCart
{
  public static class OrderDataExtension
  {
    public static OrderData buildFromVAOrder(this OrderData self, VAOrder vaOrder, bool ExcludeEmptyQty = true)
    {
      self.CustomerCode = vaOrder.oAccount.CustomerCode;
      self.PONo = vaOrder.PONo;
      self.OrderDate = vaOrder.InvoiceDate;
      self.Currency = vaOrder.Currency;
      self.NetSales = vaOrder.NetSales;
      self.dAdjustmentDiscountPercentage = vaOrder.dAdjustmentDiscountPercentage;
      self.CartDiscountName = vaOrder.cartDiscountName;
      self.Adjustment = vaOrder.Adjustment;
      self.ShippingFee = vaOrder.ShippingFee;
      self.ExtendedAreaSubcharge = vaOrder.ExtendedAreaSubcharge;

      // SubTotal = 折后商品金额 + 运费（不含税）
      // 98k-6 Coupon要修改Subtotal，需要计算折后商品金额
      self.SubTotal = vaOrder.SubTotal;
      // Total = SubTotal + 税额（含税总金额，最终应付）
      self.Total = vaOrder.Total;
      self.UseAmountbyGiftCard = vaOrder.oPayments.Where(p => p.bPaidByCreditOrGiftCard).Sum(p => p.PayAmount);
      self.dTaxAmount = vaOrder.dTaxAmount;
      self.BalanceDue = vaOrder.BalanceDue;
      self.TaxRate = vaOrder.TaxRate;
      self.TaxTitle = vaOrder.TaxTitle;
      self.dTotalByEarlyPayment = vaOrder.dTotalByEarlyPayment ?? 0;
      self.dExtraAdjustment = vaOrder.dExtraAdjustment;
      self.PaymentType = vaOrder.PaymentType;
      self.bDropShip = vaOrder.bDropShip;
      self.OrderItems = ((ExcludeEmptyQty) ? vaOrder.oOrderItems.Where(x => x is CreditOrderItem || x.ShipQty != 0) : vaOrder.oOrderItems)
                                .Select(x =>
                                            new OrderItemData
                                            {
                                              PONo = x.PONo,
                                              Code = x.CodeOnInvoice,
                                              Name = x.NameOnInvoice,
                                              Qty = (x is CreditOrderItem) ? 0 : (int)x.ShipQty, //x.OrderQty,
                                              Price = (x is CreditOrderItem) ? x.Amount : x.UnitPrice,
                                              Discount = (x is CreditOrderItem) ? 0 : x.DiscountPercentage,
                                              DiscountName = (x is DiscountDecorator) ? (x as DiscountDecorator).oDiscount?.oDiscountProgram.Name ?? "" :
                                                            (x is VAGiftItem) ? (x as VAGiftItem).oDiscount?.oDiscountProgram.Name ?? "" : "",
                                              Amount = x.Amount,
                                              ItemType = (x is CreditOrderItem) ? "CREDIT" :
                                                        (x is VAGiftItem) ? "GIFT" :
                                                        (x is OptionDiscountDecorator) ? "OPTION" :
                                                        (x is DiscountDecorator) ? "DISCOUNT" : "NORMAL",
                                              oOrderData = self
                                            }).ToArray();
      self.Comment = vaOrder.Comment;
      self.CardNo = vaOrder.CardNo;
      self.CardHolderName = vaOrder.CardHolderName;
      self.ShippingByQuote = vaOrder.ShippingByQuote;//InvoiceHelper.IsShippingByQuote(vaOrder.oShippingAddress?.Country ?? "", vaOrder.oBillingAddress?.Country ?? "");
      vaOrder.oBillingAddress?.Also(x =>
      {
        self.BillingAddress = new AddressData(x);
        self.TitleBill = x.AddressName;
        self.CompanyNameBill = x.AddressPerson;
        self.AddrBill = x.Address;
        self.CityBill = x.City;
        self.ProvinceBill = x.Province;
        self.ZipCodeBill = x.PostalCode;
        self.CountryBill = x.Country;
        self.TelBill = x.Tel;

      });
      vaOrder.oShippingAddress?.Also(x =>
      {
        self.ShippingAddress = new AddressData(x);
        self.TitleShip = x.AddressName;
        self.CompanyNameShip = x.AddressPerson;
        self.AddrShip = x.Address;
        self.CityShip = x.City;
        self.ProvinceShip = x.Province;
        self.ZipCodeShip = x.PostalCode;
        self.CountryShip = x.Country;
        self.TelShip = x.Tel;

      });

      return self;
    }
    public static OrderData buildFromPOSInvoice(this OrderData self, POSInvoice oInvoice, VAOrder vaOrder)
    {
      if (oInvoice == null)
        return self.buildFromVAOrder(vaOrder);

      self.CustomerCode = oInvoice.AccountNO;
      self.PONo = oInvoice.InvoiceNo;
      self.OrderDate = oInvoice.InvoiceDate.Value;
      self.Currency = oInvoice.Currency1;
      self.NetSales = oInvoice.NetSales;
      self.dAdjustmentDiscountPercentage = oInvoice.DiscountRate.Value * -1;
      self.CartDiscountName = vaOrder.cartDiscountName;
      self.Adjustment = oInvoice.Adjustment.Value * -1;
      self.ShippingFee = oInvoice.ShipHandling.Value;
      self.ExtendedAreaSubcharge = oInvoice.ExtendedAreaSubcharge;
      self.SubTotal = oInvoice.SubTotal;
      self.Total = oInvoice.Total;
      self.dTaxAmount = oInvoice.dTaxAmount;
      self.BalanceDue = oInvoice.BalanceDue;
      self.UseAmountbyGiftCard = oInvoice.Credits;
      self.TaxRate = oInvoice.TaxRate;
      self.TaxTitle = oInvoice.TaxTitle;
      self.dTotalByEarlyPayment = oInvoice.dTotalByEarlyPayment ?? 0;
      self.dExtraAdjustment = oInvoice.ExtraAdjustmentAmount * -1;
      self.PaymentType = oInvoice.PaymentType;
      self.bDropShip = vaOrder.bDropShip;
      if (oInvoice.InvoiceDate.Value.Year >= 2022)
      {
        self.OrderItems = oInvoice.oInvoiceDetails.Where(x => x.CountShipped != 0).Select(x => new OrderItemData
        {
          PONo = x.InvoiceNO,
          Code = x.ProductCode,
          Name = x.ProductName,
          Qty = (x.ItemType == "CREDIT") ? 0 : (int)x.CountShipped,
          Price = (x.ItemType == "CREDIT") ? x.UnitPrice.Value * -1 : x.UnitPrice.Value,
          Discount = (x.ItemType == "CREDIT") ? 0 : ((x.Discount.HasValue) ? x.Discount.Value : 0),
          DiscountName = x.DiscountName,
          Amount = x.NetSales,
          ItemType = x.ItemType,
          oOrderData = self
        }).ToArray();
      }
      else
      {
        self.OrderItems = vaOrder.oOrderItems.Where(x => x is CreditOrderItem || x.ShipQty != 0).Select(x => new OrderItemData
        {
          PONo = x.PONo,
          Code = x.CodeOnInvoice,
          Name = x.NameOnInvoice,
          Qty = (x is CreditOrderItem) ? 0 : (int)x.ShipQty,
          Price = (x is CreditOrderItem) ? x.Amount : x.UnitPrice,
          Discount = (x is CreditOrderItem) ? 0 : x.DiscountPercentage,
          DiscountName = (x is DiscountDecorator) ? (x as DiscountDecorator).oDiscount?.oDiscountProgram.Name ?? "" :
                (x is VAGiftItem) ? (x as VAGiftItem).oDiscount?.oDiscountProgram.Name ?? "" : "",
          Amount = x.Amount,
          ItemType = (x is CreditOrderItem) ? "CREDIT" :
            (x is VAGiftItem) ? "GIFT" :
            (x is OptionDiscountDecorator) ? "OPTION" :
            (x is DiscountDecorator) ? "DISCOUNT" : "NORMAL",
          oOrderData = self
        }).ToArray();

      }
      self.Comment = oInvoice.Comment;
      self.CardNo = vaOrder.CardNo;
      self.CardHolderName = vaOrder.CardHolderName;
      self.ShippingByQuote = vaOrder.ShippingByQuote;//InvoiceHelper.IsShippingByQuote(oInvoice.ShipToCountry, oInvoice.BillToCountry);
      self.BillingAddress = new AddressData(null).Also(x =>
      {
        x.DefaultBillingAddress = true;
        x.DefaultShippingAddress = false;
        x.AddressPerson = oInvoice.BillToPerson;
        x.AddressName = oInvoice.BillToName;
        x.Address = oInvoice.BillToAddress;
        x.City = oInvoice.BillToCity;
        x.Province = oInvoice.BillToProvince;
        x.PostalCode = oInvoice.BillToPostCode;
        x.Country = oInvoice.BillToCountry;
        x.Tel = oInvoice.BillToTel;
        x.Fax = oInvoice.BillToFax;
      });
      self.Also(x =>
      {
        x.TitleBill = oInvoice.BillToPerson;
        x.CompanyNameBill = oInvoice.BillToName;
        x.AddrBill = oInvoice.BillToAddress;
        x.CityBill = oInvoice.BillToCity;
        x.ProvinceBill = oInvoice.BillToProvince;
        x.ZipCodeBill = oInvoice.BillToPostCode;
        x.CountryBill = oInvoice.BillToCountry;
        x.TelBill = oInvoice.BillToTel;
      });
      self.ShippingAddress = new AddressData(null).Also(x =>
      {
        x.DefaultBillingAddress = false;
        x.DefaultShippingAddress = true;
        x.AddressPerson = oInvoice.ShipToPerson;
        x.AddressName = oInvoice.ShipToName;
        x.Address = oInvoice.ShipToAddress;
        x.City = oInvoice.ShipToCity;
        x.Province = oInvoice.ShipToProvince;
        x.PostalCode = oInvoice.ShipToPostCode;
        x.Country = oInvoice.ShipToCountry;
        x.Tel = oInvoice.ShipToTel;
        x.Fax = oInvoice.ShipToFax;
      });
      self.Also(x =>
      {
        x.TitleShip = oInvoice.ShipToPerson;
        x.CompanyNameShip = oInvoice.ShipToName;
        x.AddrShip = oInvoice.ShipToAddress;
        x.CityShip = oInvoice.ShipToCity;
        x.ProvinceShip = oInvoice.ShipToProvince;
        x.ZipCodeShip = oInvoice.ShipToPostCode;
        x.CountryShip = oInvoice.ShipToCountry;
        x.TelShip = oInvoice.ShipToTel;
      });

      return self;
    }

  }
}
