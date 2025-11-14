using System;
using System.Collections.Generic;
using System.Linq;
using MyHibernateUtil;
using MySystem.Base.Extensions;

namespace MIS.DBBO
{
  [Serializable]
  public class VAOrder : DataElement
  {
    public virtual int ID { get; set; }
    public virtual string PONo { get; set; }
    public virtual string InvoiceNo { get; set; }
    public virtual string AccountNo { get => oAccount?.CustomerCode ?? ""; set { } }
    public virtual DateTime OrderDate { get; set; } = DateTime.Now;
    public virtual DateTime InvoiceDate { get; set; } = DateTime.Now;
    private CustomerAccount _oAccount = null;
    public virtual CustomerAccount oAccount
    {
      get { return _oAccount; }
      set
      {
        _oAccount = value;
        //AccountNo = _oAccount?.CustomerCode;
        Email = _oAccount?.ContactPersonalEmail;
      }
    }

    public virtual string Status { get; set; } = eORDERSTATUS.INIT;
    public virtual string PaymentTerm { get; set; }
    public virtual string PaymentType { get; set; }
    public virtual DateTime DueDate { get; set; } = DateTime.Now;
    public virtual string ShippingMethod { get; set; }
    public virtual string Currency { get; set; }
    public virtual decimal NetSales { get; set; } = 0;
    public virtual string sNetSales { get { return (NetSales < 0) ? "(" + Decimal.Round(NetSales * -1, 2, MidpointRounding.AwayFromZero).ToString() + ")" : Decimal.Round(NetSales, 2, MidpointRounding.AwayFromZero).ToString(); } }
    public virtual double dAdjustmentDiscountPercentage { get; set; } = 0;
    public virtual string cartDiscountName { get; set; } = ""; // memory object for e-commerce shopping cart
    public virtual decimal Adjustment { get; set; } = 0;
    public virtual string sAdjustment { get => (Adjustment * (decimal)-1.0).ToCurrencyString(); }
    public virtual decimal ShippingFee { get; set; } = 0;
    public virtual decimal ExtendedAreaSubcharge { get; set; } = 0;

    public virtual decimal SubTotal { get; set; } = 0;
    public virtual string sSubTotal { get => SubTotal.ToCurrencyString(false); }
    public virtual decimal Total { get; set; } = 0;
    public virtual string sTotal { get => Total.ToCurrencyString(false); }
    public virtual decimal dTaxAmount { get; set; }
    public virtual string sTaxAmount { get => dTaxAmount.ToCurrencyString(); }
    public virtual decimal UseAmountbyGiftCard { get; set; } // Total - BalanceDue; }
    public virtual string sUseAmountbyGiftCard { get => (UseAmountbyGiftCard * (decimal)-1.0).ToCurrencyString(false, true, "$"); }
    public virtual decimal BalanceDue { get; set; }
    public virtual string sBalanceDue { get => Decimal.Round(BalanceDue, 2, MidpointRounding.AwayFromZero).ToString(); }
    public virtual double TaxRate { get; set; } = 0;
    public virtual string TaxTitle { get; set; } = "";
    public virtual string Comment { get; set; }
    public virtual string SalesRep { get; set; }
    public virtual string CardNo { get; set; }
    public virtual string CardHolderName { get; set; }
    //Billing Address
    private CustomerAddress _oBillingAddress = null;

    public virtual CustomerAddress oBillingAddress
    {
      get => _oBillingAddress;
      set
      {
        _oBillingAddress = value;
        if (_oBillingAddress != null)
        {
          try
          {
            TitleBill = _oBillingAddress.AddressName;
            CompanyNameBill = _oBillingAddress.AddressPerson;
            AddrBill = _oBillingAddress.Address;
            CityBill = _oBillingAddress.City;
            ProvinceBill = _oBillingAddress.Province;
            ZipCodeBill = _oBillingAddress.PostalCode;
            CountryBill = _oBillingAddress.Country;
            TelBill = _oBillingAddress.Tel;
          }
          catch (Exception) { }
        }
      }
    }
    public virtual string TitleBill { get; set; }
    public virtual string CompanyNameBill { get; set; }
    public virtual string AddrBill { get; set; }
    public virtual string CityBill { get; set; }
    public virtual string ProvinceBill { get; set; }
    public virtual string ZipCodeBill { get; set; }
    public virtual string CountryBill { get; set; }
    public virtual string TelBill { get; set; }
    //Shipping Address
    private CustomerAddress _oShippingAddress = null;
    public virtual CustomerAddress oShippingAddress
    {
      get => _oShippingAddress;
      set
      {
        _oShippingAddress = value;
        if (_oShippingAddress != null)
        {
          try
          {
            TitleShip = _oShippingAddress.AddressName;
            CompanyNameShip = _oShippingAddress.AddressPerson;
            AddrShip = _oShippingAddress.Address;
            CityShip = _oShippingAddress.City;
            ProvinceShip = _oShippingAddress.Province;
            ZipCodeShip = _oShippingAddress.PostalCode;
            CountryShip = _oShippingAddress.Country;
            TelShip = _oShippingAddress.Tel;
          }
          catch (Exception) { }
        }
      }
    }

    public virtual string TitleShip { get; set; }
    public virtual string CompanyNameShip { get; set; }
    public virtual string AddrShip { get; set; }
    public virtual string CityShip { get; set; }
    public virtual string ProvinceShip { get; set; }
    public virtual string ZipCodeShip { get; set; }
    public virtual string CountryShip { get; set; }
    public virtual string TelShip { get; set; }

    public virtual string Email { get; set; }
    public virtual double? ExchangeRate { get; set; } = 1;
    public virtual string TaxChar { get; set; } = "N";
    public virtual string CouponCode { get; set; }
    public virtual string CustomerPONo { get; set; }
    public virtual string SendEmailStatus { get; set; }
    //public virtual DateTime? AppliedDate { get; set; }
    public virtual decimal SalesActivity { get; set; }
    public virtual decimal? dTotalByEarlyPayment { get; set; } = null;
    public virtual bool ConfirmShipping { get; set; } = false;
    public virtual bool ConfirmMemo { get; set; } = false;
    public virtual bool ShippingByRefriderator { get; set; }
    public virtual decimal dExtraAdjustment { get; set; } = 0;
    public virtual eADJUSTTYPE ExtraAdjustmentType { get; set; } = eADJUSTTYPE.AMOUNT;
    public virtual bool bOverrideProgramedDiscount { get; set; } = false;
    public virtual bool bByMaxDiscount { get; set; } = true;
    public virtual bool bDropShip { get; set; } = false;
    // Fixed Cart Discount
    public virtual IList<AppliedCartDiscount> oCartDiscounts { get; set; } = new List<AppliedCartDiscount>();
    // memory variables
    public virtual string sExtraAdjustmentTitle
    {
      get { return (ExtraAdjustmentType == eADJUSTTYPE.AMOUNT) ? "Adjustment($)" : ((double)dExtraAdjustment).ToString() + "% off Discount"; }
    }
    public virtual decimal ExtraAdjustAmount
    {
      get
      {
        return (dExtraAdjustment == 0)
            ? (decimal)0.0
            : ((ExtraAdjustmentType == eADJUSTTYPE.AMOUNT) ? dExtraAdjustment
                                                           : Decimal.Round((NetSales - Adjustment) * dExtraAdjustment * (decimal)0.01, 2, MidpointRounding.AwayFromZero));
      }
    }
    public virtual string sExtraAdjustment { get => (ExtraAdjustAmount * (decimal)-1.0).ToCurrencyString(); }
    public virtual string PONo850 { get => oAS2VAOrders?.FirstOrDefault()?.PONo850 ?? ""; set { } }
    public virtual IList<AS2VAOrder> oAS2VAOrders { get; set; } = new List<AS2VAOrder>();
    public virtual IList<EDI850PO1Loop> PO1Loop { get; set; } = new List<EDI850PO1Loop>();
    public virtual IList<OrderItem> oOrderItems { get; set; } = new List<OrderItem>();
    public virtual IList<OrderItem> oDeletedItems { get; set; } = new List<OrderItem>();
    public virtual AS2VAOrder oOriginalAS2VAOrder => oAS2VAOrders.Where(x => x.PurposeCode == "00").FirstOrDefault();
    public virtual eORDERTYPE OrderType { get; set; } = eORDERTYPE.NORMAL;
    public virtual bool bOnlineOrder { get; set; } = false;
    public virtual bool ShippingByQuote { get; set; } = false;
    // memory object
    public virtual bool bEDIOrder { get => oAS2VAOrders.Any(); }
    public virtual void delOrderItem(OrderItem oItem)
    {
      if (oDeletedItems.Contains(oItem) == false)
        oDeletedItems.Add(oItem);
      if (oOrderItems.Contains(oItem))
        oOrderItems.Remove(oItem);
    }
    public virtual OrderItem getItemByName(string name)
    {
      foreach (OrderItem oItem in oOrderItems)
      {
        if (oItem.ItemName.Equals(name))
        {
          return oItem;
        }
      }
      return null;
    }
    public virtual OrderItem LastVAOrderItem()
    {
      if (oOrderItems == null || oOrderItems.Count == 0) return null;
      OrderItem oItem = null;
      for (int i = oOrderItems.Count - 1; i >= 0; i--)
      {
        oItem = oOrderItems[i];
        if (oItem is VAGiftItem || oItem is CreditOrderItem)
          continue;
        return oItem;
      }
      return null;
    }
    public virtual void AddOrderItem(OrderItem oItem)
    {
      oOrderItems.Add(oItem);
    }

    public virtual bool bDummy { get => oAccount != null && oAccount.CustomerName != null && oAccount.CustomerName.ToUpper().Equals("DUMMY"); }
    public virtual bool bCheck { get; set; }
    public virtual string sOldPONo { get; set; }

    // memory object
    public virtual IList<VAGiftCard> oGiftCards { get; set; } = new List<VAGiftCard>();
    public virtual IList<VAPayment> oPayments { get; set; } = new List<VAPayment>();
    public virtual IList<GS1ShippingLabel> oShippingLabels { get; set; } = null;
    public virtual int DiffSKUForCARTDiscount { get; set; }
    public virtual double OrderItemsCount { get; set; }
    public virtual decimal OriginalNetSales { get; set; } = 0; // exclude credit discount

    public VAOrder() { }
    public VAOrder(CustomerAccount oCA, string PONo, DateTime InvoiceDate)
    {
      this.PONo = PONo;
      oAccount = oCA;
      this.InvoiceDate = InvoiceDate;
      this.OrderDate = InvoiceDate;
      
      SalesRep = oCA?.SalesRep ?? "";
      PaymentTerm = oCA?.oPaymentTerm?.Desc ?? "";
      PaymentType = oCA?.PaymentType ?? "";
      ShippingMethod = oCA?.ShippedVia ?? "";
      DueDate = InvoiceDate.AddDays(oCA?.oPaymentTerm?.DaysDue ?? 30);
      SalesRep = oCA ?.SalesRep ?? "";
    }


    public virtual bool WithSynearClear()
    {
      try
      {
        foreach (OrderItem oItem in oOrderItems)
        {
          if (oItem is MCOrderItem)
            continue;
          if (oItem is CreditOrderItem)
            continue;
          else if (oItem is VAOrderItem)
          {
            if (oItem.ProductCode.StartsWith("X")) continue; // Sample
            else if (oItem.ProductName.ToLower().Contains("synerclear"))
            {
              return true;
            }
          }
          else if (oItem is DiscountDecorator)
          {
            var oVAItem = (oItem as DiscountDecorator).DecoratedVAItem;
            if (oVAItem.ProductCode.StartsWith("X")) continue; // Sample
            else if (oVAItem.ProductName.ToLower().Contains("synerclear"))
            {
              return true;
            }
          }
        }
        return false;
      }
      catch (Exception)
      {
        throw;
      }
    }

    /*
    public virtual void LoadOrderItem()
    {
        try
        {
            oOrderItems.Clear();
            oDeletedItems.Clear();

            IList<OrderItem> oTmpItems = new List<OrderItem>();

            // DiscountDecorator
            IList<DiscountDecorator> oDecorators = MISDBBO.oSession.GetXObjs<DiscountDecorator>("x.oVAOrder.ID=" + ID + " AND x.bDecorated=0");
            foreach (DiscountDecorator oItem in oDecorators)
                oTmpItems.Add(oItem);

            // VAOrderItem
            IList<VAOrderItem> oVAOrderItems = MISDBBO.oSession.GetXObjs<VAOrderItem>("x.oVAOrder.ID=" + ID + " AND x.bDecorated=0", "ID");
            foreach (VAOrderItem oItem in oVAOrderItems)
                oTmpItems.Add(oItem);

            // MCOrderItem
            IList<MCOrderItem> oMCOrderItems = MISDBBO.oSession.GetXObjs<MCOrderItem>("x.oVAOrder.ID=" + ID, "ID");
            foreach (MCOrderItem oItem in oMCOrderItems)
                oTmpItems.Add(oItem);

            oTmpItems = oTmpItems.OrderBy(x => x.ID).ToList();

            // CreditOrderItem
            IList<CreditOrderItem> oCredits = MISDBBO.oSession.GetXObjs<CreditOrderItem>("x.oVAOrder.ID=" + ID);
            foreach (CreditOrderItem oItem in oCredits)
                oOrderItems.Add(oItem);

            foreach (OrderItem oItem in oTmpItems)
                oOrderItems.Add(oItem);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public virtual void SaveOrder()
    {
        try
        {
            CustomerDiscount oCD = null;
            foreach (OrderItem oItem in oOrderItems)
            {
                oCD = null;
                bool bUpdateUsedCount = (ID == 0 || oItem.ID == 0);
                if (oItem is CreditOrderItem)
                {
                    MISDBBO.oSession.SaveObj(oItem as CreditOrderItem);
                    MISDBBO.oSession.SaveObj((oItem as CreditOrderItem).oDiscount);
                    oCD = (oItem as CreditOrderItem).oDiscount.oRefCustomerDiscount;
                }
                else if (oItem is VAGiftItem)
                {
                    (oItem as VAOrderItem).bDecorated = false;
                    MISDBBO.oSession.SaveObj(oItem as VAGiftItem);
                    MISDBBO.oSession.SaveObj((oItem as VAGiftItem).oDiscount);
                    oCD = (oItem as VAGiftItem).oDiscount.oRefCustomerDiscount;
                }
                else if (oItem is VAOrderItem)
                {
                    (oItem as VAOrderItem).bDecorated = false;
                    MISDBBO.oSession.SaveObj(oItem as VAOrderItem);
                }
                else if (oItem is MCOrderItem)
                {
                    MISDBBO.oSession.SaveObj(oItem as MCOrderItem);
                }
                else
                    ServicesHelper.DoSaveDiscountDecorator(this, oItem as DiscountDecorator, false);
                if (oCD != null)
                {
                    if (bUpdateUsedCount)
                    {
                        oCD.PrevLastUsedDate = oCD.LastUsedDate;
                        oCD.PrevLastUsedPONo = oCD.LastUsedPONo;
                        oCD.LastUsedDate = InvoiceDate;
                        oCD.LastUsedPONo = PONo;
                        oCD.UsedCount++;
                    }
                    MISDBBO.oSession.SaveObj(oCD);
                }
                if (!(oItem is CreditOrderItem) && !(oItem is MCOrderItem))
                {
                    foreach (VAOrderItemByLot oOrderItemByLot in oItem.oItemsByLot)
                        //if (oOrderItemByLot.OrderQty <= 0)
                        //	MISDBBO.oSession.DeleteObj(oOrderItemByLot, true);
                        //else
                        MISDBBO.oSession.SaveObj(oOrderItemByLot);
                    foreach (VAOrderItemByLot oDeletedItemByLot in oItem.oDeletedItemsByLot)
                        MISDBBO.oSession.DeleteObj(oDeletedItemByLot, true);
                }
            }
            if (ID > 0)
            {
                UpdatedDate = DateTime.Now;
                UpdatedID = DataElement.sDefaultUserID;
            }
            MISDBBO.oSession.SaveObj(this);
            foreach (OrderItem oItem in oDeletedItems)
                DeleteOrderItem(oItem);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public virtual void DeleteOrder()
    {
        try
        {
            if (ID == 0) return;
            foreach (OrderItem oItem in oOrderItems)
                DeleteOrderItem(oItem);
            foreach (OrderItem oItem in oDeletedItems)
                DeleteOrderItem(oItem);
            MISDBBO.oSession.DeleteObj(this, true);
            if (string.IsNullOrWhiteSpace(InvoiceNo) == false)
                POSInvoice.DeleteInvoice(InvoiceNo);

        }
        catch (Exception)
        {
            throw;
        }
    }
    public virtual void DeleteOrderItem(OrderItem oItem)
    {
        try
        {
            if (oItem.ID == 0)
                return;
            CustomerDiscount oCD = null;
            if (!(oItem is CreditOrderItem) && !(oItem is MCOrderItem))
                foreach (VAOrderItemByLot oItemByLot in oItem.oItemsByLot)
                    MISDBBO.oSession.DeleteObj(oItemByLot, true);
            if (oItem is CreditOrderItem)
            {
                MISDBBO.oSession.DeleteObj(oItem as CreditOrderItem, true);
                MISDBBO.oSession.DeleteObj((oItem as CreditOrderItem).oDiscount, true);
                oCD = (oItem as CreditOrderItem).oDiscount.oRefCustomerDiscount;
            }
            else if (oItem is VAGiftItem)
            {
                oCD = (oItem as VAGiftItem).oDiscount.oRefCustomerDiscount;
                MISDBBO.oSession.DeleteObj(oItem as VAGiftItem, true);
            }
            else if (oItem is MCOrderItem)
                MISDBBO.oSession.DeleteObj(oItem as MCOrderItem, true);
            else if (oItem is VAOrderItem)
                MISDBBO.oSession.DeleteObj(oItem as VAOrderItem, true);
            else
                ServicesHelper.DoDeleteDiscountDecorator(this, oItem as DiscountDecorator);
            if (oCD != null)
            {
                oCD.LastUsedDate = oCD.PrevLastUsedDate;
                oCD.LastUsedPONo = oCD.PrevLastUsedPONo;
                oCD.UsedCount--;
                if (oCD.UsedCount < 0)
                    oCD.UsedCount = 0;

                if (oCD.UsedCount < 0)
                    oCD.UsedCount = 0;
                MISDBBO.oSession.SaveObj(oCD);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    */
    public virtual void SetPONo(string sPONo)
    {
      try
      {
        PONo = sPONo;
        foreach (OrderItem oItem in oOrderItems)
          oItem.PONo = sPONo;
        foreach (VAPayment oPayment in oPayments)
          oPayment.InvoiceNo = sPONo;
      }
      catch (Exception)
      {
        throw;
      }
    }

    public virtual bool Voided { get; set; } = false;
    public virtual string InternalNotice { get; set; }
    public override int getID()
    {
      return ID;
    }
    //public virtual void DiscountPolicyProcess()
    //{
    //	try
    //	{
    //		try
    //		{
    //			IEnumerable<double> dDiscounts = oOrderItems.Where(x => !(x is CreditOrderItem) && !(x is VAGiftItem)).Select(x => x.DiscountPercentage).Distinct();
    //			dAdjustmentDiscountPercentage = 0;
    //			bool bOuterDiscount = (dDiscounts.Count() == 1 && (dAdjustmentDiscountPercentage = dDiscounts.First()) > 0);
    //			NetSales = 0;
    //			foreach (OrderItem oOI in oOrderItems)
    //			{
    //				if (oOI is DiscountDecorator)
    //				{
    //					if (bOuterDiscount)
    //					{
    //						(oOI as DiscountDecorator).Policy = eDISCOUNTPOLICY.OUTER;
    //						NetSales += oOI.RawAmount;
    //					}
    //					else
    //					{
    //						(oOI as DiscountDecorator).Policy = eDISCOUNTPOLICY.INNER;
    //						NetSales += oOI.Amount;
    //					}
    //				}
    //				else
    //					NetSales += oOI.Amount;
    //			}
    //			if (NetSales < 0)
    //				NetSales = 0;
    //			Adjustment = decimal.Round((NetSales * (decimal)(dAdjustmentDiscountPercentage * 0.01)), 2);
    //			SubTotal = NetSales - Adjustment;
    //			dTaxAmount = decimal.Round(SubTotal * (decimal)TaxRate, 2);
    //			Total = SubTotal + dTaxAmount;
    //		}
    //		catch (Exception)
    //		{
    //			throw;
    //		}
    //	}
    //	catch (Exception)
    //	{

    //		throw;
    //	}
    //}

  }
}
