using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using MyHibernateUtil;
using MySystem.Base;

namespace MIS.DBBO
{
  public class DiscountProgram : DataElement
  {
    public virtual int ID { get; set; }
    public virtual string Name { get; set; }
    public virtual string CouponCode { get; set; }
    public virtual bool bExtraCartDiscount { get => (string.IsNullOrWhiteSpace(CouponCode)) ? false : (CouponCode.ToUpper() == "EXTRACARTDISCOUNT"); }
    public virtual eDISCOUNTTYPE DiscountType { get; set; }
    public virtual double? DiscountAmount { get; set; }
    public virtual int MinDifferentSKUs { get; set; } = 0;
    public virtual decimal MinimumSpend { get; set; } = 0;
    public virtual double MinimumQuantity { get; set; } = 0;
    public virtual eTARGETSKUSTRATEGY TargetSKUStrategy { get; set; } = eTARGETSKUSTRATEGY.ANY;
    public virtual double? DiscountAmountFirstOrder { get; set; }
    public virtual int? MinDifferentSKUsFirstOrder { get; set; }
    public virtual double? MinimumSpendFirstOrder { get; set; } = 0;
    public virtual double? MinimumQuantityFirstOrder { get; set; } = 0;
    public virtual eTARGETSKUSTRATEGY TargetSKUStrategyFirstOrder { get; set; } = eTARGETSKUSTRATEGY.ANY;
    public virtual DateTime? StartDate { get; set; }
    public virtual DateTime? ExpiryDate { get; set; }
    public virtual eACCOUNTRULE TargetAccountMethod { get; set; } = eACCOUNTRULE.ALL;
    //public virtual string TargetAccount { get; set; }
    public virtual string TargetAccountExclude { get; set; }
    public virtual ePRODUCTRULE TargetProductMethod { get; set; } = ePRODUCTRULE.ALL;
    //public virtual string TargetProduct { get; set; }
    public virtual string TargetProductExclude { get; set; }
    public virtual eUSAGERULE UsageLimit { get; set; } = eUSAGERULE.UNLIMITED;
    public virtual int UsedCount { get; set; } = 0;
    public virtual DateTime? LastUsedDate { get; set; }
    public virtual string Comment { get; set; }
    public virtual string sSeparator { get; set; } = "";
    public virtual IList<DiscountTargetProduct> oTargetProducts { get; set; } = new List<DiscountTargetProduct>();
    public virtual IList<DiscountTargetAccount> oTargetAccounts { get; set; } = new List<DiscountTargetAccount>();
    public virtual bool Deferred { get; set; } = false;
    public static void TargetProdToAppliedProd(IEnumerable<DiscountTargetProduct> oProdList, IList<string> sAppliedProducts)
    {
      sAppliedProducts.Clear();
      foreach (var oDTP in oProdList)
      {
        if (oDTP.iState != eOPSTATE.DELETE)
          sAppliedProducts.Add(oDTP.ToTagString());
      }
    }

    public static void AppliedProdToTargetProd<T>(DiscountProgram oDP, IList<string> sAppliedProducts, IList<T> oTargetList, string sCreatedID) where T : DiscountTargetProduct, new()
    {
      string sProdCode = "", sProdName = "";
      string sUpperProdCode = "", sUpperProdName = "";
      foreach (T oDTP in oTargetList)
        oDTP.iState = eOPSTATE.DELETE;
      bool bFind = false;
      foreach (string sTag in sAppliedProducts)
      {
        DiscountTargetProduct.DecodeTagString(sTag, out sProdCode, out sProdName);
        sUpperProdCode = sProdCode.ToUpper();
        sUpperProdName = sProdName.ToUpper();
        bFind = false;
        foreach (T oDTP in oTargetList)
          if (oDTP.ProductCode.ToUpper().Equals(sUpperProdCode) &&
              oDTP.ProductName.ToUpper().Equals(sUpperProdName))
          {
            oDTP.iState = (oDTP.ID == 0) ? eOPSTATE.NEW : eOPSTATE.INIT;
            bFind = true;
            break;
          }
        if (bFind == false)
        {
          oTargetList.Add(new T
          {
            iState = eOPSTATE.NEW,
            oDiscountProgram = oDP,
            DiscountName = oDP.Name,
            ProductCode = sProdCode,
            ProductName = sProdName,
            CreatedID = sCreatedID,
            CreatedDate = DateTime.Now
          });
        }
      }
    }

    //public virtual void AppliedProdToTargetProd(IEnumerable<DiscountTargetProduct> oProdList)
    //      {
    //          string sProdCode = "", sProdName = "";
    //          string sUpperProdCode = "", sUpperProdName = "";
    //          foreach (DiscountTargetProduct oDTP in oProdList)
    //			oDTP.iState = eSTATE.DELETE;
    //          bool bFind = false;
    //          foreach (string sTag in AppliedProducts)
    //          {
    //		DiscountTargetProduct.DecodeTagString(sTag, out sProdCode, out sProdName);
    //              sUpperProdCode = sProdCode.ToUpper();
    //              sUpperProdName = sProdName.ToUpper();
    //              bFind = false;
    //              foreach (DiscountTargetProduct oDTP in oProdList)
    //                  if (oDTP.ProductCode.ToUpper().Equals(sUpperProdCode) &&
    //                      oDTP.ProductName.ToUpper().Equals(sUpperProdName))
    //                  {
    //                      oDTP.iState = (oDTP.ID == 0) ? eSTATE.NEW : eSTATE.NONE;
    //                      bFind = true;
    //                      break;
    //                  }
    //		if (bFind == false)
    //		{					
    //			if (oProdList.GetType().GetGenericArguments()[0] == typeof(DiscountProductByAccount))						
    //				oProductsByAccount.Add(new DiscountProductByAccount {
    //					iState = eSTATE.NEW,
    //					oDiscountProgram = this,
    //					DiscountName = Name,
    //					ProductCode = sProdCode,
    //					ProductName = sProdName,
    //					CreatedID = UAMDB.sLoginID,
    //					CreatedDate = DateTime.Now
    //				});
    //			else
    //				oTargetProducts.Add(new DiscountTargetProduct {
    //				iState = eSTATE.NEW,
    //				oDiscountProgram = this,
    //				DiscountName = Name,
    //				ProductCode = sProdCode,
    //				ProductName = sProdName,
    //				CreatedID = UAMDB.sLoginID,
    //				CreatedDate = DateTime.Now
    //			});
    //		}
    //          }
    //      }
    public virtual void NewTargetProdsByAppliedProds(string sCreatedID)
    {
      string sProdCode = "", sProdName = "";
      oTargetProducts.Clear();

      foreach (string sTag in AppliedProducts)
      {
        DiscountTargetProduct.DecodeTagString(sTag, out sProdCode, out sProdName);
        oTargetProducts.Add(new DiscountTargetProduct
        {
          iState = eOPSTATE.NEW,
          oDiscountProgram = this,
          DiscountName = Name,
          ProductCode = sProdCode,
          ProductName = sProdName,
          CreatedID = sCreatedID,
          CreatedDate = DateTime.Now
        });
      }
    }

    public virtual void TargetAcntToAppliedAcnt(string sPlit)
    {
      AppliedAccounts.Clear();
      string sAcntTag = "";
      sSeparator = sPlit;
      foreach (DiscountTargetAccount oDTA in oTargetAccounts)
      {
        if (oDTA.iState != eOPSTATE.DELETE)
        {
          sAcntTag = oDTA.CustomerCode + sPlit + oDTA.oCustomerAccount.CustomerName;
          AppliedAccounts.Add(oDTA.CustomerCode + sSeparator + oDTA.oCustomerAccount.CustomerName);
        }
      }
    }

    public virtual IList<string> DecodeAppliedAccounts()
    {
      IList<string> sDecodeAccounts = new List<string>();
      int idx = 0;
      foreach (string s in AppliedAccounts)
      {
        if ((idx = s.IndexOf(sSeparator)) > 0)
          sDecodeAccounts.Add(s.Substring(0, idx));
        else
          sDecodeAccounts.Add(s);
      }

      return sDecodeAccounts;
    }

    public virtual void AppliedAcntToTargetAcnt(Func<string, CustomerAccount> GetCAFunction, string sCreatedID)
    {
      string sUpperAcntCode = ""; ;
      foreach (DiscountTargetAccount oDTA in oTargetAccounts)
        oDTA.iState = eOPSTATE.DELETE;
      bool bFind = false;
      CustomerAccount oCA = null;
      IList<string> sDecodeAccounts = DecodeAppliedAccounts();
      foreach (string sTag in sDecodeAccounts)
      {
        sUpperAcntCode = sTag.ToUpper();
        bFind = false;
        foreach (DiscountTargetAccount oDTA in oTargetAccounts)
          if (oDTA.CustomerCode.ToUpper().Equals(sUpperAcntCode))
          {
            oDTA.iState = (oDTA.ID == 0) ? eOPSTATE.NEW : eOPSTATE.INIT;
            bFind = true;
            break;
          }
        if (bFind == false)
        {
          oCA = GetCAFunction(sTag); //MISDB.Session.GetXObjs<CustomerAccount>("CustomerCode = '" + sTag + "'")[0];
          oTargetAccounts.Add(new DiscountTargetAccount
          {
            iState = eOPSTATE.NEW,
            oDiscountProgram = this,
            DiscountName = this.Name,
            oCustomerAccount = oCA,
            CustomerCode = sTag,
            CustomerName = oCA.CustomerName,
            CreatedID = sCreatedID,
            CreatedDate = DateTime.Now
          });
        }
      }
    }
    public virtual void NewTargetAcntsByAppliedAcnts(Func<string, CustomerAccount> GetCAFunction, string sCreatedID)
    {
      oTargetAccounts.Clear();
      CustomerAccount oCA;
      foreach (string sTag in AppliedAccounts)
      {
        oCA = GetCAFunction(sTag);//MISDB.Session.GetXObjs<CustomerAccount>("CustomerCode = '" + sTag + "'")[0];
        oTargetAccounts.Add(new DiscountTargetAccount
        {
          iState = eOPSTATE.NEW,
          oDiscountProgram = this,
          DiscountName = this.Name,
          oCustomerAccount = oCA,
          CustomerCode = sTag,
          CustomerName = oCA.CustomerName,
          CreatedID = sCreatedID,
          CreatedDate = DateTime.Now
        });
      }
    }

    public virtual IList<string> AppliedProducts { get; set; } = new List<string>();
    public virtual IList<string> AppliedAccounts { get; set; } = new List<string>();
    public virtual string sBackupAppliedProducts { get; set; }
    public virtual string sBackupAppliedAccounts { get; set; }
    public virtual bool bFirstOrderRule
    {
      get
      {
        return (DiscountAmountFirstOrder != null && (double)DiscountAmountFirstOrder.Value > 0) ? true : false;
      }
    }
    public virtual string DispStartDate
    {
      get
      {
        if (StartDate == null || StartDate == SysVal.NilDate)
          return "";
        return StartDate.Value.ToShortDateString();
      }
    }
    public virtual string DispExpiryDate
    {
      get
      {
        if (ExpiryDate == null || ExpiryDate == SysVal.NilDate)
          return "";
        return ExpiryDate.Value.ToShortDateString();
      }
    }
    public virtual bool ValidCoupon(string sCouponCode)
    {
      if (string.IsNullOrWhiteSpace(CouponCode))
        return true;
      if (bExtraCartDiscount)
        return true;
      return (CouponCode.ToUpper() == (sCouponCode?.ToUpper() ?? ""));
    }
    public virtual bool bMeetItemCondition(double Qty, decimal UnitPrice)
    {
      return (Qty >= MinimumQuantity && (UnitPrice * (decimal)Qty >= MinimumSpend));
    }
    public virtual bool bMeetCARTCondition(int DiffSKUForCARTDiscount, decimal dNetSalesForCARTDiscount, double dTotalQtyForCARTDiscount)
    {
      // 98k-7 购物车折扣条件检查
      return (DiffSKUForCARTDiscount >= MinDifferentSKUs &&
              dNetSalesForCARTDiscount >= MinimumSpend &&
              dTotalQtyForCARTDiscount >= MinimumQuantity);
    }
    public virtual double DiscountPercentage()
    {
      if (DiscountType == eDISCOUNTTYPE.FIXEDCART_PER || DiscountType == eDISCOUNTTYPE.FIXEDPRODUCT_PER)
        return (double)DiscountAmount;
      if (DiscountType == eDISCOUNTTYPE.FIXEDPRODUCT_FREE)
        return (double)decimal.Round((decimal)(DiscountAmount * 100 / (MinimumQuantity + DiscountAmount)), 2, MidpointRounding.AwayFromZero);
      return 0;
    }
    // memory object for UI
    public virtual bool bInvalidFromUIResult { get; set; } = false;

    public virtual string Summary
    {
      get
      {
        string sSummary = "";
        sSummary = "Name:" + Name + "\n";
        sSummary += "Type:" + DiscountType.GetType().GetMember(DiscountType.ToString()).FirstOrDefault()?.GetCustomAttribute<DescriptionAttribute>()?.Description + "\n"; ;
        sSummary += "Discount amount:" + DiscountAmount + ((DiscountType == eDISCOUNTTYPE.FIXEDCART_PER || DiscountType == eDISCOUNTTYPE.FIXEDPRODUCT_PER) ? "% OFF" :
                                                          (DiscountType == eDISCOUNTTYPE.FIXEDCART_SUB) ? " dollar(s) OFF" : " bottle(s) FREE") + "\n";
        sSummary += "Start date:" + StartDate.ToString() + "\n"; ;
        sSummary += "Expiry date:" + ExpiryDate.ToString() + "\n"; ;
        if (TargetSKUStrategy == eTARGETSKUSTRATEGY.EACH)
        {
          sSummary += "Rule:" + "Minimun " + MinDifferentSKUs + " different SKUs with minumun " +
              ((MinimumQuantity > 0) ? MinimumQuantity + " bottle(s) " : MinimumSpend + " dollar(s) ") +
              "each selected product" + "\n";
        }
        else if (TargetSKUStrategy == eTARGETSKUSTRATEGY.ANY)
        {
          if (DiscountType == eDISCOUNTTYPE.FIXEDPRODUCT_PER)
            sSummary += "Rule:" + "Minimun " +
            ((MinimumQuantity > 0) ? MinimumQuantity + " bottle(s) " : MinimumSpend + " dollar(s) ") +
            " on any selected product" + "\n";
          else if (DiscountType == eDISCOUNTTYPE.FIXEDPRODUCT_FREE)
          {
            if (MinimumQuantity > 0)
              sSummary += "Rule:" + "Minimun " + MinimumQuantity + " bottle(s) on any selected product. Order quantity or selected item must be equal to " + (double)MinimumQuantity + " or its multiples to be eligible.\n";
            else
              sSummary += "Rule:" + "Minimun " + MinimumSpend + " dollar(s) on any selected product. Order spend be equal to " + (double)MinimumSpend + " or its multiples to be eligible.\n";
          }
        }
        else if (TargetSKUStrategy == eTARGETSKUSTRATEGY.SAME)
        {
          if (MinimumQuantity > 0)
            sSummary += "Rule:" + "on " + MinimumQuantity + "+ bottle(s) of same product.\n";
          else
            sSummary += "Rule:" + "on " + MinimumQuantity + "+ dollar(s) of same product.\n";
        }
        if (DiscountAmountFirstOrder != null && (double)DiscountAmountFirstOrder > 0)
        {
          sSummary += "First order rule(optional):\n";
          sSummary += "\tDiscount amount:" + DiscountAmountFirstOrder + ((DiscountType == eDISCOUNTTYPE.FIXEDCART_PER || DiscountType == eDISCOUNTTYPE.FIXEDPRODUCT_PER) ? "% OFF" :
                                                            (DiscountType == eDISCOUNTTYPE.FIXEDCART_SUB) ? " dollar(s) OFF" : " bottle(s) FREE") + "\n";
          if (TargetSKUStrategyFirstOrder == eTARGETSKUSTRATEGY.EACH)
          {
            sSummary += "\tRule:" + "Minimun " + MinDifferentSKUsFirstOrder + " different SKUs with minumun " +
                ((MinimumQuantityFirstOrder != null && (double)MinimumQuantityFirstOrder > 0) ? MinimumQuantityFirstOrder + " bottle(s) " : MinimumSpendFirstOrder + " dollar(s) ") +
                "each selected product" + "\n";
          }
          else if (TargetSKUStrategyFirstOrder == eTARGETSKUSTRATEGY.ANY)
          {
            if (DiscountType == eDISCOUNTTYPE.FIXEDPRODUCT_PER)
              sSummary += "\tRule:" + "Minimun " +
              ((MinimumQuantityFirstOrder != null && (double)MinimumQuantityFirstOrder > 0) ? MinimumQuantityFirstOrder + " bottle(s) " : MinimumSpendFirstOrder + " dollar(s) ") +
              " on any selected product" + "\n";
            else if (DiscountType == eDISCOUNTTYPE.FIXEDPRODUCT_FREE)
            {
              if (MinimumQuantityFirstOrder != null && (double)MinimumQuantityFirstOrder > 0)
                sSummary += "\tRule:" + "Minimun " + MinimumQuantityFirstOrder + " bottle(s) on any selected product. Order quantity or selected item must be equal to " + (double)MinimumQuantityFirstOrder + " or its multiples to be eligible.\n";
              else
                sSummary += "\tRule:" + "Minimun " + MinimumSpendFirstOrder + " dollar(s) on any selected product. Order spend be equal to " + (double)MinimumSpendFirstOrder + " or its multiples to be eligible.\n";
            }
          }
          else if (TargetSKUStrategyFirstOrder == eTARGETSKUSTRATEGY.SAME)
          {
            if (MinimumQuantityFirstOrder != null && (double)MinimumQuantityFirstOrder > 0)
              sSummary += "\tRule:" + "on " + MinimumQuantityFirstOrder + "+ bottle(s) of same product.\n";
            else
              sSummary += "\tRule:" + "on " + MinimumQuantityFirstOrder + "+ dollar(s) of same product.\n";
          }
        }
        sSummary += "Usage Limit:" + UsageLimit.GetType().GetMember(UsageLimit.ToString()).FirstOrDefault()?.GetCustomAttribute<DescriptionAttribute>()?.Description + "\n"; ;
        sSummary += "Product Limit:" + TargetProductMethod.GetType().GetMember(TargetProductMethod.ToString()).FirstOrDefault()?.GetCustomAttribute<DescriptionAttribute>()?.Description + "\n";
        //if (TargetProductMethod == ePRODUCTRULE.SELECTED)
        //    sSummary += "Selected Products:" + TargetProduct + "\n"; ;
        //sSummary += "Account Limit:" + TargetAccountMethod.GetType().GetMember(TargetAccountMethod.ToString()).FirstOrDefault()?.GetCustomAttribute<DescriptionAttribute>()?.Description + "\n";
        //if (TargetAccountMethod == eACCOUNTRULE.SELECTED)
        //    sSummary += "Selected Accounts:" + TargetAccount + "\n"; ;

        return sSummary;
      }
    }

    public override int getID()
    {
      return ID;
    }
  }
}
