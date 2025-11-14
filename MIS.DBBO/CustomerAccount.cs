using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PropertyChanged;
using MyHibernateUtil;

namespace MIS.DBBO
{
  [Serializable]
  public class CustomerAccount : DataElement
  {
    public virtual int ID { get; set; }
    // Basic Info.
    public virtual string CustomerCode { get; set; }
    public virtual string CustomerName { get; set; }
    public virtual string CustomerOwner { get; set; }
    public virtual string CustomerOwnerTitle { get; set; }
    public virtual string ContactPersonalTitle { get; set; }
    public virtual string ContactPersonal { get; set; }
    public virtual string ContactPersonalTel { get; set; }
    public virtual string ContactPersonalEmail { get; set; } // contact email
                                                             //public virtual string ContactManagerTitle { get; set; }
                                                             //public virtual string ContactManager { get; set; }
                                                             //public virtual string ContactManagerTel { get; set; }
                                                             //public virtual string ContactManagerEmail { get; set; }
                                                             // public virtual string CustomerEmail { get; set; } 
    public virtual string InvoiceToCustomer { get; set; } //Email ONLY, Original ONLY, Email and Original
    public virtual string CustomerEmail1 { get; set; }  // invoice email
    public virtual string CustomerEmail2 { get; set; }  // package list email
    public virtual bool IsActive { get; set; } = true;
    public virtual ePRICEPOLICY PricePolicy { get; set; } = ePRICEPOLICY.STANDARD;
    public virtual string CustomerDesc { get { return CustomerCode + ":" + CustomerName; } }
    public virtual bool bEmployee { get => (PricePolicy == ePRICEPOLICY.EMPLOYEE); }
    public virtual bool bDummyAccount { get => CustomerName != null && CustomerName.ToUpper().Equals("DUMMY"); }
    // Sale Rep. and Customer Rep.
    [DoNotNotify]
    public virtual Employee oSalesRep { get; set; } // memory object
    public virtual string SalesRepID { get; set; }
    public virtual string SalesRep { get; set; }
    public virtual string CustomerRepID { get; set; }
    public virtual string CustomerRep { get; set; }

    // Bill Info.
    public virtual IList<CustomerAddress> oAddresses { get; set; } = new List<CustomerAddress>();
    [DoNotNotify]
    public virtual CustomerAddress oBillAddress
    {
      get
      {
        CustomerAddress _oBillAddress = (oAddresses != null && oAddresses.Count > 0) ? oAddresses[0] : null;
        foreach (CustomerAddress oCA in oAddresses)
        {
          if (oCA.DefaultBillingAddress)
          {
            _oBillAddress = oCA;
            break;
          }
        }
        return _oBillAddress;
      }
    }
    public virtual string CustomerPersonal1 { get; set; }
    public virtual string CustomerName1 { get; set; }
    public virtual string CustomerAddress1 { get; set; }
    public virtual string CustomerCity1 { get; set; }
    public virtual string CustomerProvince1 { get; set; }
    public virtual string CustomerPostalCode1 { get; set; }
    public virtual string CustomerAddressSecondLine1 { get { return CustomerCity1 + ", " + CustomerProvince1 + ", " + CustomerPostalCode1; } }
    public virtual string CustomerCountry1 { get; set; }
    public virtual string CustomerTel1 { get; set; }
    public virtual string CustomerFax1 { get; set; }

    // ShipTo Info.		
    [DoNotNotify]
    public virtual CustomerAddress oShipToAddress
    {
      get
      {
        CustomerAddress _oShipToAddress = (oAddresses != null && oAddresses.Count > 0) ? oAddresses[0] : null;
        foreach (CustomerAddress oCA in oAddresses)
        {
          if (oCA.DefaultShippingAddress)
          {
            _oShipToAddress = oCA;
            break;
          }
        }
        return _oShipToAddress;
      }
    }
    public virtual string CustomerPersonal2 { get; set; }
    public virtual string CustomerName2 { get; set; }
    public virtual string CustomerAddress2 { get; set; }
    public virtual string CustomerCity2 { get; set; }
    public virtual string CustomerProvince2 { get; set; }
    public virtual string CustomerPostalCode2 { get; set; }
    public virtual string CustomerAddressSecondLine2 { get { return CustomerCity2 + ", " + CustomerProvince2 + ", " + CustomerPostalCode2; } }
    public virtual string CustomerCountry2 { get; set; }
    public virtual string CustomerTel2 { get; set; }
    public virtual string CustomerFax2 { get; set; }
    // Default shipping method
    public virtual string ShippedVia { get; set; }

    // Payment method
    private string _PaymentTerm = "";
    public virtual string PaymentTerm { get => oPaymentTerm?.Desc ?? ""; set => _PaymentTerm = value; }
    public virtual PaymentTerm oPaymentTerm { get; set; }
    public virtual string PaymentType { get; set; }
    // Credit Card Info.
    [DoNotNotify]
    public virtual CreditCard oCreditCard1 { get; set; } = new CreditCard();
    public virtual string CreditCardType1 { get; set; }
    public virtual string CardHolder1 { get; set; }
    public virtual string CardNumber1 { get; set; }
    public virtual string CreditCardCID1 { get; set; }
    public virtual string CardExpiryDate1 { get; set; }
    [DoNotNotify]
    public virtual CreditCard oCreditCard2 { get; set; } = new CreditCard();
    public virtual string CreditCardType2 { get; set; }
    public virtual string CardHolder2 { get; set; }
    public virtual string CardNumber2 { get; set; }
    public virtual string CreditCardCID2 { get; set; }
    public virtual string CardExpiryDate2 { get; set; }
    [DoNotNotify]
    public virtual string CreditCardNums
    {
      get
      {
        string sRtn = CardNumber1;
        if (string.IsNullOrWhiteSpace(sRtn))
          sRtn = CardNumber2;
        else
          sRtn += Environment.NewLine + CardNumber2;
        return sRtn;
      }
    }
    [DoNotNotify]
    public virtual string CardExpiryDate
    {
      get
      {
        string sRtn = CardExpiryDate1;
        if (string.IsNullOrWhiteSpace(sRtn))
          sRtn = CardExpiryDate2;
        else
          sRtn += Environment.NewLine + CardExpiryDate2;
        return sRtn;
      }
    }
    [DoNotNotify]
    public virtual string CreditCardCID
    {
      get
      {
        string sRtn = CreditCardCID1;
        if (string.IsNullOrWhiteSpace(sRtn))
          sRtn = CreditCardCID2;
        else
          sRtn += Environment.NewLine + CreditCardCID2;
        return sRtn;
      }
    }
    // Note
    public virtual string Comment { get; set; }
    public virtual int OrderCount { get; set; } = 0;
    public virtual bool AutoCommission { get; set; } = true;
    public virtual double? OverridedCommissionRate { get; set; }
    public virtual string POMemo { get; set; }
    public virtual bool DisplayLotNo { get; set; } = false;
    // Memo
    public virtual IList<CustomerMemo> oMemos { get; set; } = new List<CustomerMemo>();
    [DoNotNotify]
    public virtual IList<CustomerDiscount> oCustomerDiscounts { get; set; } = null; //new ObservableCollection<DiscountProgram>();
    [DoNotNotify]
    public virtual IList<DiscountProgram> oDiscountPrograms { get; set; } = null; //new ObservableCollection<DiscountProgram>();
    [DoNotNotify]
    public virtual ObservableCollection<POSInvoice> oInvoiceHistory { get; set; } = null;
    [DoNotNotify]
    public virtual ObservableCollection<POSInvoiceDetailBackOrder> oBackOrders { get; set; } = null;
    public virtual ObservableCollection<VAGiftCard> oGiftCards { get; set; } = new ObservableCollection<VAGiftCard>();
    public virtual decimal AvailableCredit { get => oGiftCards.Sum(x => x.Balance); }
    public virtual decimal EmployeeMonthlyCredit { get; set; } = 0;
    //public virtual string MonthlyCredit { get; set; }
    //public virtual string PromotionPlan { get; set; }
    //public virtual string DiscountPriority { get; set; }
    //public virtual string InvoiceEmail { get; set; }
    public virtual bool bDistributor { get; set; } = false;
    public virtual double PriceRateForDistributor { get; set; } = 1;

    public virtual void syncBillingAddress(CustomerAddress _oBillAddress = null)
    {
      if (_oBillAddress == null)
        _oBillAddress = oBillAddress;
      if (_oBillAddress == null)
      {
        CustomerPersonal1 = "";
        CustomerName1 = "";
        CustomerAddress1 = "";
        CustomerCity1 = "";
        CustomerProvince1 = "";
        CustomerPostalCode1 = "";
        CustomerCountry1 = "";
        CustomerTel1 = "";
        CustomerFax1 = "";
        return;
      }
      CustomerPersonal1 = _oBillAddress.AddressPerson;
      CustomerName1 = _oBillAddress.AddressName;
      CustomerAddress1 = _oBillAddress.Address;
      CustomerCity1 = _oBillAddress.City;
      CustomerProvince1 = _oBillAddress.Province;
      CustomerPostalCode1 = _oBillAddress.PostalCode;
      CustomerCountry1 = _oBillAddress.Country;
      CustomerTel1 = _oBillAddress.Tel;
      CustomerFax1 = _oBillAddress.Fax;
    }
    public virtual void syncShippingAddress(CustomerAddress _oShipToAddress = null)
    {
      if (_oShipToAddress == null)
        _oShipToAddress = oShipToAddress;
      if (_oShipToAddress == null)
      {
        CustomerPersonal2 = "";
        CustomerName2 = "";
        CustomerAddress2 = "";
        CustomerCity2 = "";
        CustomerProvince2 = "";
        CustomerPostalCode2 = "";
        CustomerCountry2 = "";
        CustomerTel2 = "";
        CustomerFax2 = "";
        return;
      }
      CustomerPersonal2 = _oShipToAddress.AddressPerson;
      CustomerName2 = _oShipToAddress.AddressName;
      CustomerAddress2 = _oShipToAddress.Address;
      CustomerCity2 = _oShipToAddress.City;
      CustomerProvince2 = _oShipToAddress.Province;
      CustomerPostalCode2 = _oShipToAddress.PostalCode;
      CustomerCountry2 = _oShipToAddress.Country;
      CustomerTel2 = _oShipToAddress.Tel;
      CustomerFax2 = _oShipToAddress.Fax;
    }
    public virtual void PostProcForCreditCard()
    {
      oCreditCard1 = new CreditCard
      {
        CardCID =CreditCardCID1,
        CardHolder =CardHolder1,
        CardNumber =CardNumber1,
        CardType =CreditCardType1,
        ExpiryDate =CardExpiryDate1
      };
     oCreditCard2 = new CreditCard
      {
        CardCID =CreditCardCID2,
        CardHolder =CardHolder2,
        CardNumber =CardNumber2,
        CardType =CreditCardType2,
        ExpiryDate =CardExpiryDate2
      };

    }
    public virtual void UpdateCreditCardInfo(bool bReadonlyMode = false)
    {
      if (bReadonlyMode == false)
      {
        if (oCreditCard1 == null)
        {
          CreditCardType1 = "";
          CardHolder1 = "";
          CardNumber1 = "";
          CreditCardCID1 = "";
          CardExpiryDate1 = "";
        }
        else
        {
          CreditCardType1 = oCreditCard1.CardType;
          CardHolder1 = oCreditCard1.CardHolder;
          CardNumber1 = oCreditCard1.CardNumber;
          CreditCardCID1 = oCreditCard1.CardCID;
          CardExpiryDate1 = oCreditCard1.ExpiryDate;
        }
        if (oCreditCard2 == null)
        {
          CreditCardType2 = "";
          CardHolder2 = "";
          CardNumber2 = "";
          CreditCardCID2 = "";
          CardExpiryDate2 = "";
        }
        else
        {
          CreditCardType2 = oCreditCard2.CardType;
          CardHolder2 = oCreditCard2.CardHolder;
          CardNumber2 = oCreditCard2.CardNumber;
          CreditCardCID2 = oCreditCard2.CardCID;
          CardExpiryDate2 = oCreditCard2.ExpiryDate;
        }
      }
    }
    public override int getID()
    {
      return ID;
    }
  }
}
