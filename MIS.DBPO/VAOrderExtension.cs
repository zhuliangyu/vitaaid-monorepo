using MIS.DBBO;
using System;
using System.Collections.Generic;
using System.Linq;
using MyHibernateUtil;
using static MIS.DBPO.DBPOServiceHelper;
using MySystem.Base.Extensions;
using WebDB.DBBO;
using System.Text.Json;

namespace MIS.DBPO
{
  // This temporary class is used to parse the JSON string from the database
  public class FIXED_AMOUNT_PERCENTAGE_OFF_JSON
  {
    public String value { get; set; }
  }

  public static class VAOrderExtension
  {
    public static void buildCartDiscounts(this VAOrder self)
    {
      try
      {
        if (self.ID == 0) // new order
          self.oCartDiscounts.Clear();

        // 当buildCartDiscounts多次访问的时候,避免重复创建 AppliedCartDiscount 对象
        var existingCartDiscounts = new List<AppliedCartDiscount>(self.oCartDiscounts);
        self.oCartDiscounts.Clear();

        self.oAccount.oCustomerDiscounts.Where(x => x.oDiscountProgram.DiscountType == eDISCOUNTTYPE.FIXEDCART_PER)
            .Action(x =>
            {
              // 尝试在 existingCartDiscounts 中找到曾经已经存储过的折扣计划(oDiscountProgram.ID)的记录。
              // 如果找到，就复用该旧对象（并从 existingCartDiscounts 中移除，避免重复使用）。
              // 如果找不到，就创建一个新的 AppliedCartDiscount。
              var oAppliedCartDiscount = existingCartDiscounts.Where(oCartDiscounts => oCartDiscounts.oDiscountProgram.ID == x.oDiscountProgram.ID)
                                      .FirstOrDefault()?.Also(oCartDiscount => existingCartDiscounts.Remove(oCartDiscount))
                                      ?? new AppliedCartDiscount
                                      {
                                        AppliedDate = self.InvoiceDate,
                                        bApplied = false,
                                        IsActive = true,
                                        oCustomer = self.oAccount,
                                        oDiscountProgram = x.oDiscountProgram,
                                        oRefCustomerDiscount = x,
                                        oOrder = self,
                                        iState = MySystem.Base.eOPSTATE.NEW
                                      };
              self.oCartDiscounts.Add(oAppliedCartDiscount);
            });
      }
      catch (Exception)
      {
        throw;
      }
    }
    //public static void LoadOrderItem(this VAOrder self) => self.LoadOrderItem(MISDB[eST.SESSION0]);
    public static void LoadOrderItem(this VAOrder self, SessionProxy oSession)
    {
      try
      {
        self.oOrderItems.Clear();
        self.oDeletedItems.Clear();

        IList<OrderItem> oTmpItems = new List<OrderItem>();

        // DiscountDecorator
        IList<DiscountDecorator> oDecorators =
            oSession.QueryDataElement<DiscountDecorator>()
                                      .Where(x => x.oVAOrder == self && x.bDecorated == false)
                                      .ToList();
        //MISDBBO.oSession.GetXObjs<DiscountDecorator>("x.oVAOrder.ID=" + order.ID + " AND x.bDecorated=0");
        foreach (DiscountDecorator oItem in oDecorators)
          oTmpItems.Add(oItem);

        // VAOrderItem
        IList<VAOrderItem> oVAOrderItems = oSession.QueryDataElement<VAOrderItem>()
                                      .Where(x => x.oVAOrder == self && x.bDecorated == false)
                                      .ToList();
        //MISDBBO.oSession.GetXObjs<VAOrderItem>("x.oVAOrder.ID=" + order.ID + " AND x.bDecorated=0", "ID");
        foreach (VAOrderItem oItem in oVAOrderItems)
          oTmpItems.Add(oItem);

        // MCOrderItem
        IList<MCOrderItem> oMCOrderItems = oSession.QueryDataElement<MCOrderItem>()
                                      .Where(x => x.oVAOrder == self)
                                      .ToList();
        //MISDBBO.oSession.GetXObjs<MCOrderItem>("x.oVAOrder.ID=" + order.ID, "ID");
        foreach (MCOrderItem oItem in oMCOrderItems)
          oTmpItems.Add(oItem);

        oTmpItems = oTmpItems.OrderBy(x => x.ID).ToList();

        // CreditOrderItem
        IList<CreditOrderItem> oCredits = oSession.QueryDataElement<CreditOrderItem>()
                                      .Where(x => x.oVAOrder == self)
                                      .ToList();
        //MISDBBO.oSession.GetXObjs<CreditOrderItem>("x.oVAOrder.ID=" + order.ID);
        foreach (CreditOrderItem oItem in oCredits)
          self.oOrderItems.Add(oItem);

        foreach (OrderItem oItem in oTmpItems)
          self.oOrderItems.Add(oItem);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    public static OrderItem CreateOrderItem(this VAOrder self, CustomerAccount oCA, VAOrderItem oItem)
    {
      try
      {
        // create options of discount for the VAOrderItem
        if (oItem.oItemsByLot == null)
          oItem.oItemsByLot = new List<VAOrderItemByLot>();

        if (string.IsNullOrWhiteSpace(oItem.FromBONo) == false) // Backorder
          return oItem;

        OptionDiscountDecorator oOptionDiscount = new OptionDiscountDecorator
        {
          oDecoratedItem = oItem,
          oVAOrder = self,
          PONo = oItem.PONo
        };
        if (oCA != null && oCA.oCustomerDiscounts != null)
        {
          foreach (CustomerDiscount oCD in oCA.oCustomerDiscounts)
          {
            if (oCD.oDiscountProgram.ValidCoupon(self.CouponCode) == false)
              continue;
            if (oCD.oDiscountProgram.DiscountType == eDISCOUNTTYPE.FIXEDCART_PER || oCD.oDiscountProgram.DiscountType == eDISCOUNTTYPE.FIXEDCART_SUB ||
                oCD.Valid(self.PONo, self.InvoiceDate) == false)
              continue;
            if (oCD.QualifyDiscount(oItem.ItemCode)) // QualifyDiscount by product
              oOptionDiscount.oOptions.Add(new AppliedCustomerDiscount(oCD, self));// AppliedDate));
          }
        }
        if (oOptionDiscount.oOptions.Count > 0)
          return oOptionDiscount;
        else
          return oItem;

      }
      catch (Exception)
      {

        throw;
      }
    }

    // EAS: Extended Area Surcharge
    //public static void RebuildOrderItem(this VAOrder self, Dictionary<string, ShippingPolicy> oSPDict,
    //  bool bRecreate = true, bool bAutoAddEAS = false) => self.RebuildOrderItem(oSPDict, MISDB[eST.SESSION0], bRecreate, bAutoAddEAS);


    public static void RebuildOrderItem(this VAOrder self, IList<ShippingPolicyMaster> oShoppingPolicies,
      SessionProxy oVAMISSession, bool bRecreate = true, bool bAutoAddEAS = true, string couponCode = "")
    {
      try
      {
        // Create Order Item for the account
        List<OrderItem> oNewOrderItemsAfterProcessing = new List<OrderItem>();
        // 免费不用付钱的商品
        List<CreditOrderItem> oNewCreditItems = new List<CreditOrderItem>();

        // 处理后的购物车商品
        OrderItem oNewOrderItemAfterProcessing = null;
        decimal dNetSalesForCARTDiscount = 0;
        int DiffSKUForCARTDiscount = 0;
        double dTotalQtyForCARTDiscount = 0;
        OrderItem oTmpObj = null;

        // 从原购物车中删除 CreditOrderItem 和 VAGiftItem, 因为它们不会参与折扣计算
        for (int i = self.oOrderItems.Count() - 1; i >= 0; i--)
        {
          oTmpObj = self.oOrderItems[i];
          if (oTmpObj is CreditOrderItem || oTmpObj is VAGiftItem)
          {
            self.oDeletedItems.Add(oTmpObj);
            self.oOrderItems.RemoveAt(i);
            continue;
          }
        }

        // 遍历购物车中的商品，根据ExistingOrderItem创建NewOrderItem
        foreach (OrderItem oExistingOrderItem in self.oOrderItems)
        {
          if (oExistingOrderItem is MCOrderItem)
          {
            oNewOrderItemAfterProcessing = oExistingOrderItem;
            oNewOrderItemAfterProcessing.CaculateAmount();
          }
          else
          {
            if (oExistingOrderItem is VAOrderItem)
              oNewOrderItemAfterProcessing = self.CreateOrderItem(self.oAccount, oExistingOrderItem as VAOrderItem);
            else if (bRecreate &&
                string.IsNullOrWhiteSpace((oExistingOrderItem as DiscountDecorator).DecoratedVAItem.FromBONo)) // not backorder
              oNewOrderItemAfterProcessing = self.CreateOrderItem(self.oAccount, (oExistingOrderItem as DiscountDecorator).DecoratedVAItem);
            else
              oNewOrderItemAfterProcessing = oExistingOrderItem;
            oNewOrderItemAfterProcessing.CaculateAmount();

            // 计算折后商品金额和商品数量
            dNetSalesForCARTDiscount += oNewOrderItemAfterProcessing.Amount;
            dTotalQtyForCARTDiscount += oNewOrderItemAfterProcessing.OrderQty;
          }
          oNewOrderItemsAfterProcessing.Add(oNewOrderItemAfterProcessing);

          // 当前相同的商品品类共计购买数量
          DiffSKUForCARTDiscount = oNewOrderItemsAfterProcessing.Where(x => !(x is MCOrderItem) && !x.IsSampleProduct && x.OrderQty > 0).Select(x => x.ProductCode).Distinct().Count();

          if (oNewOrderItemAfterProcessing is DiscountDecorator)
          {
            // merge gift obj
            VAGiftItem oNewGiftObj = null;
            VAGiftItem oDeletedGift = null;
            for (int i = 0; i < oNewOrderItemAfterProcessing.oGifts.Count(); i++)
            {
              oNewGiftObj = oNewOrderItemAfterProcessing.oGifts[i] as VAGiftItem;
              // 从原购物车中删除的 GiftItem 中找到相同的商品
              for (int j = self.oDeletedItems.Count() - 1; j >= 0; j--)
              {
                oDeletedGift = self.oDeletedItems[j] as VAGiftItem;
                if (oDeletedGift != null && oDeletedGift.oOwnerItem == oNewGiftObj.oOwnerItem)
                {
                  oDeletedGift.PONo = oNewGiftObj.PONo;
                  oDeletedGift.OrderQty = oNewGiftObj.OrderQty;
                  oDeletedGift.ShipQty = oNewGiftObj.ShipQty;
                  oDeletedGift.StockLocation = oNewGiftObj.StockLocation;
                  oDeletedGift.RetailLotNo = oNewGiftObj.RetailLotNo;
                  oDeletedGift.ExpiredDate = oNewGiftObj.ExpiredDate;
                  oDeletedGift.StockCountSnapShot = oNewGiftObj.StockCountSnapShot;
                  oNewOrderItemAfterProcessing.oGifts[i] = oDeletedGift;
                  self.oDeletedItems.RemoveAt(j);
                }
              }
            }
            foreach (OrderItem oGift in oNewOrderItemAfterProcessing.oGifts)
              oNewOrderItemsAfterProcessing.Add(oGift);
          }
        }

        // PROCESSING eDISCOUNTTYPE.FIXEDCART_SUB 
        self.oAccount?.oCustomerDiscounts
            ?.Where(x => x.oDiscountProgram.DiscountType == eDISCOUNTTYPE.FIXEDCART_SUB &&
                        x.oDiscountProgram.ValidCoupon(self.CouponCode) && x.Valid(self.PONo, self.InvoiceDate) &&
                        x.oDiscountProgram.DiscountAmount > 0 && !x.oDiscountProgram.bInvalidFromUIResult &&
                        // 98k-7 购物车折扣条件检查
                        x.oDiscountProgram.bMeetCARTCondition(DiffSKUForCARTDiscount, dNetSalesForCARTDiscount, dTotalQtyForCARTDiscount)
                        )
            ?.Action(oCustomerDiscount =>
            {
              // merge credit obj
              self.oDeletedItems.Where(x => x is CreditOrderItem && (x as CreditOrderItem).oDiscount.oRefCustomerDiscount == oCustomerDiscount)
                                .ToList()
                                .Also(xs =>
                                {
                                  if (xs.Any())
                                    xs.Action(x =>
                                            {
                                              self.oDeletedItems.Remove(x);
                                              oNewCreditItems.Add(x as CreditOrderItem);
                                            });
                                  else
                                    oNewCreditItems.Add(
                                                new CreditOrderItem
                                                {
                                                  Name = oCustomerDiscount.oDiscountProgram.Name,
                                                  CodeOnInvoice = oCustomerDiscount.oDiscountProgram.CouponCode,
                                                  NameOnInvoice = oCustomerDiscount.oDiscountProgram.Name,
                                                  PONo = self.PONo,
                                                  Credit = (decimal)oCustomerDiscount.oDiscountProgram.DiscountAmount,
                                                  oVAOrder = self,
                                                  oDiscount = new AppliedCustomerDiscount(oCustomerDiscount, self)
                                                });
                                });
            });

        // 清空购物车中原来的商品
        self.oOrderItems.Clear();
        // 将处理后的 CreditOrderItem 添加到购物车
        oNewCreditItems.ForEach(x => self.oOrderItems.Add(x));
        // 将处理后的购物车商品添加到购物车
        self.ShippingByRefriderator = false;
        oNewOrderItemsAfterProcessing.ForEach(x =>
        {
          if (x.ShippingByRefriderator)
            self.ShippingByRefriderator = true;
          self.oOrderItems.Add(x);
        });
        self.ProcessSummary(oShoppingPolicies, oVAMISSession, bAutoAddEAS, couponCode);
      }
      catch (Exception)
      {

        throw;
      }
    }
    public static DateTime NewVersionDate = new DateTime(2021, 07, 20, 23, 59, 59);
    // EAS: Extended Area Surcharge
    public static void ProcessSummary(this VAOrder self, IList<ShippingPolicyMaster> oSPs,
                                      bool bAutoAddEAS = false) => self.ProcessSummary(oSPs, MISDB[eST.SESSION0], bAutoAddEAS);


    public static void ProcessSummary(this VAOrder self, IList<ShippingPolicyMaster> oSPs, SessionProxy oSession,
                                      bool bAutoAddEAS = false, string couponCode = "")
    {
      try
      {
        self.NetSales = 0;
        decimal NetSalesOFMCProduct = 0;
        decimal NetSalesOfSpecialDiscount = 0;
        decimal NetSalesOthers = 0;
        decimal CreditsFromDiscountProgram = 0;
        self.OriginalNetSales = 0;
        self.OrderItemsCount = 0;
        double shippingItemCount = 0;
        foreach (OrderItem oOrderItem in self.oOrderItems)
        {
          if (oOrderItem is MCOrderItem)
            NetSalesOFMCProduct += oOrderItem.Amount;
          else if (oOrderItem is CreditOrderItem)
            CreditsFromDiscountProgram += oOrderItem.RawAmount;
          else if (oOrderItem.bByProductOrStaticDiscount)
          {
            NetSalesOfSpecialDiscount += oOrderItem.Amount;
            self.OrderItemsCount += oOrderItem.OrderQty;
            shippingItemCount += oOrderItem.ShipQty.ToDouble();
          }
          else
          {
            NetSalesOthers += oOrderItem.RawAmount;
            self.OrderItemsCount += oOrderItem.OrderQty;
            shippingItemCount += oOrderItem.ShipQty.ToDouble();
          }
        }

        self.OriginalNetSales = NetSalesOfSpecialDiscount + NetSalesOthers + NetSalesOFMCProduct;

        if (self.OriginalNetSales > 0 && CreditsFromDiscountProgram != 0)
        {
          self.NetSales = self.OriginalNetSales + CreditsFromDiscountProgram;
          if (self.NetSales < 0)
            self.NetSales = 0;
        }
        else
          self.NetSales = self.OriginalNetSales;

        self.DiffSKUForCARTDiscount = self.oOrderItems.Where(x => !(x is MCOrderItem) && !(x is VAGiftItem) && !(x is CreditOrderItem) && !x.IsSampleProduct && x.OrderQty > 0).Select(x => x.ProductCode).Distinct().Count();
        if (self.oCartDiscounts.IsNullOrEmpty())
        {
          if (!self.bOverrideProgramedDiscount)
          {
            self.dAdjustmentDiscountPercentage = 0.0;
            self.cartDiscountName = "";
          }
        }
        // Discount program 减少总价
        // 对于新的订单, 而且订单不允许手动干预的情况下
        else if (self.InvoiceDate <= NewVersionDate || !self.bOverrideProgramedDiscount)
        { 
          // 使用最大的折扣力度
          if (self.bByMaxDiscount)
          {
            var oAppliedCartDiscount = self.oCartDiscounts.Where(x => x.oDiscountProgram.bMeetCARTCondition(self.DiffSKUForCARTDiscount, self.NetSales, self.OrderItemsCount) &&
                                                          x.oRefCustomerDiscount.Valid(self.PONo, self.InvoiceDate) &&
                                                          x.oDiscountProgram.ValidCoupon(self.CouponCode))
                                              .OrderByDescending(x => x.oDiscountProgram.DiscountAmount)
                                              .ToList().FirstOrDefault();

            self.dAdjustmentDiscountPercentage = oAppliedCartDiscount?.oDiscountProgram?.DiscountAmount ?? 0.0; //oPrograms.Max(x => x.oDiscountProgram.DiscountAmount) ?? 0.0;
            self.cartDiscountName = oAppliedCartDiscount?.oDiscountProgram?.Name ?? "";
            if (oAppliedCartDiscount != null)
              oAppliedCartDiscount.bApplied = true;
            //oPrograms.Where(x => x.oDiscountProgram.DiscountAmount == self.dAdjustmentDiscountPercentage).FirstOrDefault()?.oDiscountProgram?.Name ?? "";
          }
          else
          {
            var oAppliedCartDiscount = self.oCartDiscounts.Where(x => x.bApplied).FirstOrDefault();
            self.dAdjustmentDiscountPercentage = oAppliedCartDiscount?.oDiscountProgram?.DiscountAmount ?? 0.0;
            self.cartDiscountName = oAppliedCartDiscount?.oDiscountProgram?.Name ?? "";
            //self.dAdjustmentDiscountPercentage =
            //    self.oCartDiscounts.Where(x => x.bApplied).FirstOrDefault()?.oDiscountProgram?.DiscountAmount ?? 0.0;
          }
        }

        // 根据 Discount program 计算折扣金额
        decimal dAdjustmentByDiscountProgram = decimal.Round(((self.InvoiceDate <= NewVersionDate) ? NetSalesOthers : self.NetSales) * (decimal)(self.dAdjustmentDiscountPercentage * 0.01), 2, MidpointRounding.AwayFromZero);

        // 根据Coupon 计算折扣金额
        decimal dAdjustmentByCoupon = GetCouponAdjustmentAmount(couponCode, self);
        // coupon vs discount program 拿折扣力度最大的那个金额作为最终的折扣金额
        if(dAdjustmentByCoupon > dAdjustmentByDiscountProgram) {
          self.Adjustment = dAdjustmentByCoupon;
          self.cartDiscountName = "Coupon";
        }
        else {
          self.Adjustment = dAdjustmentByDiscountProgram;
          // self.cartDiscountName already assigned before
        }

        // discount amount calculation by % or a fixed amount
        if (self.ExtraAdjustmentType == eADJUSTTYPE.AMOUNT)
          // 固定金额折扣
          //self.Adjustment 是根据Discount program 计算的折扣金额
          //self.dExtraAdjustment 是手动干预的折扣金额
          self.SalesActivity = decimal.Round(self.NetSales - self.Adjustment - self.dExtraAdjustment, 2, MidpointRounding.AwayFromZero);
        else
          // 百分比折扣
          self.SalesActivity = decimal.Round((self.NetSales - self.Adjustment) * (1 - self.dExtraAdjustment * (decimal)0.01), 2, MidpointRounding.AwayFromZero);

        // 处理运费计算
        if (self.Status == eORDERSTATUS.INIT && !string.IsNullOrWhiteSpace(self.CountryShip) && oSPs != null)
        {
          self.ExtendedAreaSubcharge = 0;
          if (self.OrderItemsCount <= 0 || shippingItemCount <= 0 || self.OriginalNetSales < 0)
          {
            self.ShippingFee = 0;
            self.ShippingByQuote = false;
          }
          else
          {
            self.CaculateShippingFeeByPolicy(oSPs);
            if (!self.ShippingByQuote && bAutoAddEAS &&
                !self.oAccount.bEmployee && !self.oAccount.bDummyAccount)
            {
              self.ExtendedAreaSubcharge = self.oShippingAddress.ExtendedAreaSubcharge(oSession);
              self.ShippingFee += self.ExtendedAreaSubcharge;
            }
          }
        }

        self.SubTotal = self.SalesActivity + self.ShippingFee;
        self.dTaxAmount = decimal.Round(self.SubTotal * (decimal)self.TaxRate, 2, MidpointRounding.AwayFromZero);
        self.Total = decimal.Round(self.SubTotal + self.dTaxAmount, 2, MidpointRounding.AwayFromZero);

        //if (self.oAccount != null && self.oAccount.bEmployee && self.bDummy == false && self.Total < 0)
        //{
        //    self.Total = 0;
        //}
        if (self.Total > 0 && self.oAccount != null && self.oAccount.oPaymentTerm != null &&
            self.oAccount.oPaymentTerm.EarlyPaymentDiscounts != null && self.oAccount.oPaymentTerm.EarlyPaymentDiscounts.Value > 0 &&
            self.oAccount.oPaymentTerm.EarlyPaymentPeriod != null && self.oAccount.oPaymentTerm.EarlyPaymentPeriod.Value <= self.oAccount.oPaymentTerm.DaysDue)
        {
          decimal subtotalEaylyPayment = self.SalesActivity * (decimal)(1 - 0.01 * self.oAccount.oPaymentTerm.EarlyPaymentDiscounts.Value) + self.ShippingFee;
          decimal dTaxAmountEaylyPayment = decimal.Round(subtotalEaylyPayment * (decimal)self.TaxRate, 2, MidpointRounding.AwayFromZero);
          self.dTotalByEarlyPayment = decimal.Round(subtotalEaylyPayment + dTaxAmountEaylyPayment, 2, MidpointRounding.AwayFromZero);
        }
        else
          self.dTotalByEarlyPayment = null;
        if (self.Total >= 0)
        {
          IList<VAPayment> oNewPayments = new List<VAPayment>();
          if (self.OrderType == eORDERTYPE.CREDIT_MEMO)
          {
            self.RemoveCollectedCredits()
              ?.Also(x => oNewPayments.Add(x));
            self.OrderType = eORDERTYPE.NORMAL;
          }
          else
          {
            self.OrderType = eORDERTYPE.NORMAL;
            var oCreditOrGiftPayments = self.oPayments.Where(x => x.bPaidByCreditOrGiftCard).ToList();
            (oCreditOrGiftPayments.Sum(x => x.PayAmount) - self.Total).Also(diff =>
            {
              if (diff > 0) // refund
              {
                oCreditOrGiftPayments.GroupBy(p => p.PaymentNote)
                              .OrderBy(gp => gp.First().PaymentMethod)
                              .ToList()
                              .Also(gp =>
                                      {
                                        for (int idx = gp.Count - 1; idx >= 0 && diff > 0; idx--)
                                        {
                                          var refundAmount = Math.Min(gp[idx].Sum(p => p.PayAmount), diff);
                                          if (refundAmount > 0)
                                          {
                                            self.oGiftCards.Where(g => g.Code == gp[idx].Key)
                                                                    .UniqueOrDefault()
                                                                    ?.Also(giftCard => oNewPayments.Add(giftCard.refund(refundAmount, self.PONo)));
                                            diff -= refundAmount;
                                          }
                                        }
                                      });
              }
              else if (diff < 0)
              {// charge from gift/credit
                var chargeTotalAmounts = diff * -1;
                for (int idx = 0; idx < self.oGiftCards.Count && chargeTotalAmounts > 0; idx++)
                {
                  self.oGiftCards[idx].charge(chargeTotalAmounts, self.PONo, oSession)?.Also(newPayment =>
                                {
                                  oNewPayments.Add(newPayment);
                                  chargeTotalAmounts -= newPayment.PayAmount;
                                });
                }
              }
            });
          }
          oNewPayments.Action(n => self.oPayments.Add(n));
          self.OptimizePayments();
        }
        else
        {
          if (self.OrderType == eORDERTYPE.NORMAL)
            self.OrderType = eORDERTYPE.CREDIT_MEMO;
          self.CreditRefundMemoProcess();
        }
        self.UseAmountbyGiftCard = self.oPayments?.Where(x => x.bPaidByCreditOrGiftCard).Sum(x => x.PayAmount) ?? 0;
        //self.BalanceDue = self.Total - (self.oPayments?.Where(x => x.PaymentStatus == null || (x.PaymentStatus != "DECLINED" && x.PaymentStatus != "VOID"))?.Sum(x => x.PayAmount) ?? 0);//self.UseAmountbyGiftCard;
        self.BalanceDue = self.Total - (self.oPayments?.Where(x => x.bPaidByCreditOrGiftCard)?.Sum(x => x.PayAmount) ?? 0);//self.UseAmountbyGiftCard;
      }
      catch (Exception)
      {
        throw;
      }
    }

    private static decimal GetCouponAdjustmentAmount(string couponCode, VAOrder oOrder)
    {
      var oSession = WebDBServer[eST.SESSION0];
      List<HubCoupon> HubCoupons = oSession.Query<HubCoupon>().ToList();
      HubCoupon oCoupon = HubCoupons.FirstOrDefault(x => x.Code == couponCode);
      
      // early return when coupon is invalid
      if (oCoupon == null || !oCoupon.IsActive || oCoupon.StartDate > DateTime.Now || (oCoupon.EndDate != null && oCoupon.EndDate < DateTime.Now) )
        return 0;

      List<HubCouponRule> HubCouponRules = oSession.Query<HubCouponRule>().Where(x => x.oCoupon.ID == oCoupon.ID).ToList();
      List<HubCouponAction> HubCouponActions = oSession.Query<HubCouponAction>().Where(x => x.oCoupon.ID == oCoupon.ID).ToList();
      // push rules and actions to oCoupon
      oCoupon.Rules = HubCouponRules;
      oCoupon.Actions = HubCouponActions;

      foreach (var HubCouponAction in HubCouponActions)
      {
        decimal amountOff = 0;
        var jsonStringFromDB = HubCouponAction.ActionDetails;
        switch (HubCouponAction.ActionType)
        {
          case "FIXED_AMOUNT_OFF":
            var obj_amount_off = JsonSerializer.Deserialize<FIXED_AMOUNT_PERCENTAGE_OFF_JSON>(jsonStringFromDB);
            amountOff = decimal.Parse(obj_amount_off.value);
            break;
          case "PERCENTAGE_OFF":
            var obj_percent_off = JsonSerializer.Deserialize<FIXED_AMOUNT_PERCENTAGE_OFF_JSON>(jsonStringFromDB);
            amountOff = oOrder.NetSales * decimal.Parse(obj_percent_off.value) * 0.01m;
            break;
        }
        return amountOff;
      }
      return 0;
    }

    public static decimal CaculateShippingFeeByPolicy(this VAOrder self, IList<ShippingPolicyMaster> oSPs)
    {
      try
      {
        self.ShippingByQuote = false;
        self.ShippingFee = 0;

        if (self.oAccount == null) return 0;
        if (!self.oOrderItems.Any()) return 0;
        if (self.oAccount.bEmployee || self.oAccount.bDummyAccount) return 0;

        ShippingPolicyMaster oAppliedSP = null;
        oSPs.Where(x => x.IsValidShippingPolicy(self))
            .GroupBy(x => x.Priority)
            .OrderByDescending(x => x.Key)
            .FirstOrDefault()
            ?.Also(x =>
            {
              if (x.Count() == 1)
                oAppliedSP = x.First();
            });

        if (oAppliedSP == null)
          self.ShippingByQuote = true;

        decimal ShippingFee = 0M;
        if (!(oAppliedSP?.CaculateShippingFeeByPolicy(self, out ShippingFee) ?? false))
          self.ShippingByQuote = true;

        self.ShippingFee = ShippingFee;
        return ShippingFee;
      }
      catch (Exception)
      {
        throw;
      }
    }

    public static void CreditRefundMemoProcess(this VAOrder self)
    {
      try
      {
        var customerCredits = self.oGiftCards.Where(x => x.CreditType == eCREDITTYPE.CUSTOMER_CREDIT).UniqueOrDefault();
        if (customerCredits == null) return;
        IList<VAPayment> oNewPayments = new List<VAPayment>();
        self.oPayments.Where(x => x.bPaidByCreditOrGiftCard).GroupBy(p => p.PaymentNote)
                      .Also(gp =>
                      {
                        gp.Where(g => g.Sum(p => p.PayAmount) > 0)
                                  .Action(g =>
                                  {
                                    var returnAmount = g.Sum(p => p.PayAmount);
                                    self.oGiftCards.Where(c => c.Code == g.Key)
                                            .UniqueOrDefault()
                                            .Also(giftCard => oNewPayments.Add(giftCard.refund(returnAmount, self.PONo)));
                                  });
                        if (self.OrderType == eORDERTYPE.CREDIT_MEMO)
                        {
                          (gp.Select(g => g.Sum(p => p.PayAmount)).Where(x => x < 0).Sum(x => x) - self.Total)
                                    .Also(diff =>
                                    {
                                      if (diff > 0)
                                      { // add credits to account
                                        var refundPayment = customerCredits.refund(diff, self.PONo);
                                        customerCredits.Amount += diff;
                                        oNewPayments.Add(refundPayment);
                                      }
                                      else if (diff < 0)
                                      { // remove credits from account

                                        VAPayment oPayment = customerCredits.removeCredit(diff * -1, self.PONo);
                                        oNewPayments.Add(oPayment);
                                      }
                                    });
                        }
                        else // self.OrderType == eORDERTYPE.REFUND)
                        {
                          gp.Select(g => g.Sum(p => p.PayAmount)).Where(x => x < 0).Sum(x => x)
                                  .Also(totalCollectCredits =>
                                  {
                                    if (totalCollectCredits != 0)
                                    {// remove credits from account
                                      VAPayment oPayment = customerCredits.removeCredit(totalCollectCredits * -1, self.PONo);
                                      oNewPayments.Add(oPayment);
                                    }
                                  });
                        }
                      });
        oNewPayments.Action(n => self.oPayments.Add(n));
        self.OptimizePayments();
      }
      catch (Exception)
      {

        throw;
      }
    }
    public static VAPayment RemoveCollectedCredits(this VAOrder self)
    {
      var customerCredits = self.oGiftCards.Where(x => x.CreditType == eCREDITTYPE.CUSTOMER_CREDIT).UniqueOrDefault();
      return self.oPayments.Where(x => x.bPaidByCreditOrGiftCard && x.PaymentMethod == eVAPAYMENT.CUSTOMER_CREDIT)
                           .Sum(x => x.PayAmount)
                           .Let<decimal, VAPayment>(totalCollectedCredits =>
                           {
                             if (totalCollectedCredits >= 0)
                               return null;
                             return customerCredits.removeCredit(totalCollectedCredits * -1, self.PONo);
                           });
    }

    public static void OptimizePayments(this VAOrder self)
    {
      var refinePayments = self.oPayments.Where(x => x.ID == 0 && x.bPaidByCreditOrGiftCard)
                                        .GroupBy(x => x.PaymentNote)
                                        .SelectMany(x =>
                                        {
                                          var totalPayAmount = x.Sum(p => p.PayAmount);
                                          if (totalPayAmount == 0)
                                            return new List<VAPayment>();
                                          else
                                          {
                                            var refPayment = x.First();
                                            return new List<VAPayment>
                                                {
                                                          new VAPayment
                                                          {
                                                              PayAmount = totalPayAmount,
                                                              Currency = refPayment.Currency,
                                                              ExchangeRate = refPayment.ExchangeRate,
                                                              PaymentType = refPayment.PaymentType,
                                                              PaymentNote = refPayment.PaymentNote,
                                                              PaymentMethod = refPayment.PaymentMethod,
                                                              BalanceDueBeforePaying = refPayment.BalanceDueBeforePaying,
                                                              PaymentStatus = refPayment.PaymentStatus,
                                                              BalanceDue = refPayment.BalanceDueBeforePaying - totalPayAmount
                                                          }.Also(x => x.copyInvoiceData(refPayment))
                                                };
                                          }
                                        }).ToList();
      self.oPayments = self.oPayments.Where(x => x.ID > 0 || !x.bPaidByCreditOrGiftCard)
                                     .ToList()
                                     .Also(x => x.AddRange(refinePayments));

    }
    public static void WriteNoteToGiftCards(this VAOrder self)
    {
      self.oPayments.Where(x => x.ID == 0 && x.bPaidByCreditOrGiftCard)
                    .GroupBy(x => x.PaymentNote)
                    .Action(x =>
                    {
                      self.oGiftCards.Where(g => x.Key == g.Code)
                                     .UniqueOrDefault()
                                    ?.Also(g => g.UpdateLastUsedNote(x.Sum(y => y.PayAmount), self.PONo));
                    });
    }
    //public static void SaveOrder(this VAOrder order) => order.SaveOrder(MISDB[eST.SESSION0]);
    public static void SaveOrder(this VAOrder order, SessionProxy oSession)
    {
      try
      {
        CustomerDiscount oCD = null;
        foreach (OrderItem oItem in order.oOrderItems)
        {
          oCD = null;
          bool bUpdateUsedCount = (order.ID == 0 || oItem.ID == 0);
          if (oItem is CreditOrderItem)
          {
            oSession.SaveObj(oItem as CreditOrderItem);
            oSession.SaveObj((oItem as CreditOrderItem).oDiscount);
            oCD = (oItem as CreditOrderItem).oDiscount.oRefCustomerDiscount;
          }
          else if (oItem is VAGiftItem)
          {
            (oItem as VAOrderItem).bDecorated = false;
            oSession.SaveObj(oItem as VAGiftItem);
            oSession.SaveObj((oItem as VAGiftItem).oDiscount);
            // oCD = (oItem as VAGiftItem).oDiscount.oRefCustomerDiscount; // have saved in the VAOrderItem
          }
          else if (oItem is VAOrderItem)
          {
            (oItem as VAOrderItem).bDecorated = false;
            oSession.SaveObj(oItem as VAOrderItem);
          }
          else if (oItem is MCOrderItem)
          {
            oSession.SaveObj(oItem as MCOrderItem);
          }
          else
            order.DoSaveDiscountDecorator(oItem as DiscountDecorator, false, oSession);
          if (oCD != null)
          {
            if (bUpdateUsedCount)
            {
              oCD.PrevLastUsedDate = oCD.LastUsedDate;
              oCD.PrevLastUsedPONo = oCD.LastUsedPONo;
              oCD.LastUsedDate = order.InvoiceDate;
              oCD.LastUsedPONo = order.PONo;
              oCD.UsedCount++;
            }
            oSession.SaveObj(oCD);
          }
          if (!(oItem is CreditOrderItem) && !(oItem is MCOrderItem))
          {
            foreach (VAOrderItemByLot oOrderItemByLot in oItem.oItemsByLot)
              //if (oOrderItemByLot.OrderQty <= 0)
              //	MISDBBO.oSession.DeleteObj(oOrderItemByLot, true);
              //else
              oSession.SaveObj(oOrderItemByLot);
            foreach (VAOrderItemByLot oDeletedItemByLot in oItem.oDeletedItemsByLot)
              oSession.DeleteObj(oDeletedItemByLot, true);
          }
        }
        order.oCartDiscounts.Action(x =>
        {
          if (x.iState == MySystem.Base.eOPSTATE.DELETE)
            oSession.DeleteObj(x);
          else
          {
            bool bSaveCD = false;
            if (x.bApplied)
            {
              if (x.oRefCustomerDiscount.LastUsedPONo != order.PONo)
              {
                x.oRefCustomerDiscount.PrevLastUsedDate = x.oRefCustomerDiscount.LastUsedDate;
                x.oRefCustomerDiscount.PrevLastUsedPONo = x.oRefCustomerDiscount.LastUsedPONo;
                x.oRefCustomerDiscount.LastUsedDate = order.InvoiceDate;
                x.oRefCustomerDiscount.LastUsedPONo = order.PONo;
                x.oRefCustomerDiscount.UsedCount++;
                bSaveCD = true;
              }
            }
            else
            {
              if (x.oRefCustomerDiscount.LastUsedPONo == order.PONo)
              {
                x.oRefCustomerDiscount.LastUsedDate = x.oRefCustomerDiscount.PrevLastUsedDate;
                x.oRefCustomerDiscount.LastUsedPONo = x.oRefCustomerDiscount.PrevLastUsedPONo;
                x.oRefCustomerDiscount.UsedCount--;
                if (x.oRefCustomerDiscount.UsedCount < 0)
                  x.oRefCustomerDiscount.UsedCount = 0;
                bSaveCD = true;
              }
            }
            if (x.oRefCustomerDiscount.ID == 0 || bSaveCD)
              oSession.SaveObj(x.oRefCustomerDiscount);
            oSession.SaveObj(x);
          }
        });
        order.oPayments.Where(x => x.PayAmount != 0 && !string.IsNullOrWhiteSpace(x.InvoiceNo)).Action(x => x.SaveObj(oSession));
        order.oGiftCards.Where(x => x.ID > 0 || (x.ID == 0 && x.Amount != 0)).Action(x => x.SaveObj(oSession));
        if (order.ID > 0)
        {
          order.UpdatedDate = DateTime.Now;
          order.UpdatedID = DataElement.sDefaultUserID;
        }
        oSession.SaveObj(order);
        foreach (OrderItem oItem in order.oDeletedItems)
          order.DeleteOrderItem(oItem, oSession);

      }
      catch (Exception)
      {
        throw;
      }
    }
    public static void DeleteOrder(this VAOrder order, SessionProxy oSession)
    {
      try
      {
        if (order.ID == 0) return;
        foreach (OrderItem oItem in order.oOrderItems)
          order.DeleteOrderItem(oItem, oSession);
        foreach (OrderItem oItem in order.oDeletedItems)
          order.DeleteOrderItem(oItem, oSession);
        oSession.DeleteObj(order, true);
        if (string.IsNullOrWhiteSpace(order.InvoiceNo) == false)
          InvoiceHelper.DeleteInvoice(oSession, order);
        order.oCartDiscounts.Action(x =>
        {
          if (x.bApplied)
          {
            x.oRefCustomerDiscount.LastUsedDate = x.oRefCustomerDiscount.PrevLastUsedDate;
            x.oRefCustomerDiscount.LastUsedPONo = x.oRefCustomerDiscount.PrevLastUsedPONo;
            x.oRefCustomerDiscount.UsedCount--;
            if (x.oRefCustomerDiscount.UsedCount < 0)
              x.oRefCustomerDiscount.UsedCount = 0;
            oSession.SaveObj(x.oRefCustomerDiscount);
          }
          oSession.DeleteObj(x);
        });
        order.RefundAllCreditGiftPayments(oSession);
        order.oPayments.Action(x => oSession.DeleteObj(x));
      }
      catch (Exception)
      {
        throw;
      }
    }
    public static void RefundAllCreditGiftPayments(this VAOrder order, SessionProxy oSession)
    {
      try
      {
        order.oPayments.Where(x => x.bPaidByCreditOrGiftCard)
                       .Action(x =>
                       {
                         oSession.QueryDataElement<VAGiftCard>()
                                 .Where(g => g.Code == x.PaymentNote).ToList()
                                 .FirstOrDefault()
                                ?.Also(gift =>
                                 {
                                   var refundRec = gift.refund(x.PayAmount, order.PONo);
                                   //oSession.SaveObj(refundRec);
                                   oSession.SaveObj(gift);
                                 });
                       });
      }
      catch (Exception)
      {

        throw;
      }
    }
    //public static void DeleteOrderItem(this VAOrder order, OrderItem oItem) => order.DeleteOrderItem(oItem, MISDB[eST.SESSION0]);
    public static void DeleteOrderItem(this VAOrder order, OrderItem oItem, SessionProxy oSession)
    {
      try
      {
        if (oItem.ID == 0)
          return;
        CustomerDiscount oCD = null;
        if (!(oItem is CreditOrderItem) && !(oItem is MCOrderItem))
        {
          foreach (VAOrderItemByLot oItemByLot in oItem.oItemsByLot)
            oSession.DeleteObj(oItemByLot, true);
        }
        if (oItem is CreditOrderItem)
        {
          oSession.DeleteObj(oItem as CreditOrderItem, true);
          oSession.DeleteObj((oItem as CreditOrderItem).oDiscount, true);
          oCD = (oItem as CreditOrderItem).oDiscount.oRefCustomerDiscount;
        }
        else if (oItem is VAGiftItem)
        {
          oCD = (oItem as VAGiftItem).oDiscount.oRefCustomerDiscount;
          oSession.DeleteObj(oItem as VAGiftItem, true);
        }
        else if (oItem is MCOrderItem)
          oSession.DeleteObj(oItem as MCOrderItem, true);
        else if (oItem is VAOrderItem)
          oSession.DeleteObj(oItem as VAOrderItem, true);
        else
          order.DoDeleteDiscountDecorator(oItem as DiscountDecorator, oSession);
        if (oCD != null)
        {
          oCD.LastUsedDate = oCD.PrevLastUsedDate;
          oCD.LastUsedPONo = oCD.PrevLastUsedPONo;
          oCD.UsedCount--;
          if (oCD.UsedCount < 0)
            oCD.UsedCount = 0;

          if (oCD.UsedCount < 0)
            oCD.UsedCount = 0;
          oSession.SaveObj(oCD);
        }
      }
      catch (Exception)
      {
        throw;
      }
    }

    //public static POSInvoice ToInvoice(this VAOrder order, bool bOverwrite = true, bool bSave = true)
    //  => order.ToInvoice(MISDB[eST.SESSION0], MISDB[eST.READONLY], bOverwrite, bSave);
    public static POSInvoice SaveInvoice(this VAOrder order, SessionProxy oSession)//, bool bOverwrite = true, bool bSave = true)
    {
      try
      {
        if (order == null) return null;
        order.InvoiceNo = order.PONo;
        POSInvoice oPOSInvoice = null;
        bool bTotalChanged = true;
        if (order.ID > 0)
          oPOSInvoice = oSession.QueryDataElement<POSInvoice>().Where(x => x.oOrder.ID == order.ID).UniqueOrDefault();
        if (oPOSInvoice == null)
          oPOSInvoice = new POSInvoice();
        else
        {
          if (oPOSInvoice.Total == order.Total)
            bTotalChanged = false;
        }

        order.SyncToInvoice(oSession, oPOSInvoice);
        if (!order.Voided)
        {
          if (order.OrderType == eORDERTYPE.CREDIT_MEMO)
            oPOSInvoice.InvoiceStatus = eINVOICESTATUS.CONFIRMED;
          else if (order.oAccount.bEmployee)
          {
            if (order.BalanceDue == 0)
              oPOSInvoice.InvoiceStatus = eINVOICESTATUS.CONFIRMED;
            else if (bTotalChanged)
              oPOSInvoice.InvoiceStatus = eINVOICESTATUS.UNPAID;
          }
          else if (oPOSInvoice.ID > 0 && bTotalChanged)
          {
            if (oPOSInvoice.Total == order.oPayments.Where(x => x.bPaidByCreditOrGiftCard).Sum(x => x.PayAmount))
              oPOSInvoice.InvoiceStatus = eINVOICESTATUS.CONFIRMED;
            else
              oPOSInvoice.InvoiceStatus = eINVOICESTATUS.UNPAID;
            //var due = oSession.QueryDataElement<VAPayment>()
            //                  .Where(y => y.oInvoice == oPOSInvoice && y.PaymentStatus != "DECLINED" && y.PaymentStatus != "VOID")
            //                  .ToList()
            //                  .Sum(y => y.PayAmount);
          }
        }
        oSession.SaveObj(oPOSInvoice);

        return oPOSInvoice;
      }
      catch (Exception Ex)
      {
        throw Ex;
      }
    }

    //private static POSInvoice CreateAndSaveInvoice(this VAOrder order, string sInvoiceNo, bool bSaveInvoice)
    //  => order.CreateAndSaveInvoice(sInvoiceNo, bSaveInvoice, MISDB[eST.SESSION0]);
    private static void SyncToInvoice(this VAOrder order, SessionProxy oSession, POSInvoice oInvoice)
    {
      oInvoice.InvoiceNo = order.InvoiceNo;
      oInvoice.AccountNO = order.AccountNo;
      oInvoice.Adjustment = order.Adjustment * -1;
      oInvoice.BillToAddress = order.AddrBill;
      oInvoice.BillToCity = order.CityBill;
      oInvoice.BillToCountry = order.CountryBill;
      oInvoice.BillToName = order.CompanyNameBill;
      oInvoice.BillToPerson = order.TitleBill;
      oInvoice.BillToPostCode = order.ZipCodeBill;
      oInvoice.BillToProvince = order.ProvinceBill;
      oInvoice.BillToTel = order.TelBill;
      oInvoice.ShipToAddress = order.AddrShip;
      oInvoice.ShipToCity = order.CityShip;
      oInvoice.ShipToCountry = order.CountryShip;
      oInvoice.ShipToName = order.CompanyNameShip;
      oInvoice.ShipToPerson = order.TitleShip;
      oInvoice.ShipToPostCode = order.ZipCodeShip;
      oInvoice.ShipToProvince = order.ProvinceShip;
      oInvoice.ShipToTel = order.TelShip;
      oInvoice.Currency1 = order.Currency;
      oInvoice.DiscountRate = order.dAdjustmentDiscountPercentage * -1;
      oInvoice.DueDate = order.DueDate;
      oInvoice.ExchangeRate = order.ExchangeRate;
      oInvoice.InvoiceDate = order.OrderDate;
      oInvoice.Tax = order.TaxChar;
      oInvoice.GST = (order.TaxChar == "G") ? order.TaxRate : 0;
      oInvoice.HST = (order.TaxChar == "H") ? order.TaxRate : 0;
      oInvoice.PST = 0;
      oInvoice.PaymentType = order.PaymentType;
      oInvoice.PaymentTerm = order.PaymentTerm;
      oInvoice.PurchaseNO = order.PONo;
      oInvoice.ShipHandling = order.ShippingFee;
      oInvoice.ExtendedAreaSubcharge = order.ExtendedAreaSubcharge;
      oInvoice.SalesRep = order.SalesRep;
      oInvoice.ShippedVia = order.ShippingMethod;
      oInvoice.NetSales = order.NetSales;
      oInvoice.SubTotal = order.SubTotal;
      oInvoice.TaxTitle = order.TaxTitle;
      oInvoice.TaxRate = order.TaxRate;
      oInvoice.dTaxAmount = order.dTaxAmount;
      oInvoice.Total = order.Total;
      oInvoice.dTotalByEarlyPayment = order.dTotalByEarlyPayment;
      oInvoice.Credits = order.UseAmountbyGiftCard;
      oInvoice.BalanceDue = order.BalanceDue;
      oInvoice.ExtraAdjustmentAmount = order.ExtraAdjustAmount * -1;
      oInvoice.ExtraAdjustmentTitle = order.sExtraAdjustmentTitle;
      oInvoice.CustomerPONo = order.CustomerPONo;
      oInvoice.OrderType = order.OrderType;
      oInvoice.Comment = order.Comment;
      oInvoice.SalesActivity = decimal.Round(order.NetSales - order.Adjustment, 2, MidpointRounding.AwayFromZero);
      oInvoice.Voided = order.Voided;
      oInvoice.InternalNotice = order.InternalNotice;
      oInvoice.oOrder = order;

      // recreate Invoice Items
      oInvoice.oInvoiceDetails.Action(x => oSession.DeleteObj(x));
      oInvoice.oInvoiceDetails.Clear();
      foreach (OrderItem oItem in order.oOrderItems)
      {
        var oDetail = oItem.ToInvoiceDetail(oSession, oInvoice);
        oSession.SaveObj(oDetail);
        oInvoice.oInvoiceDetails.Add(oDetail);
      }
    }
    /*
    private static POSInvoice CreateAndSaveInvoice(this VAOrder order, string sInvoiceNo, bool bSaveInvoice, SessionProxy oSession)
    {
      try
      {
        POSInvoice oInvoice = new POSInvoice
        {
          InvoiceNo = sInvoiceNo,
          AccountNO = order.AccountNo,
          Adjustment = order.Adjustment * -1,
          BillToAddress = order.AddrBill,
          BillToCity = order.CityBill,
          BillToCountry = order.CountryBill,
          BillToName = order.CompanyNameBill,
          BillToPerson = order.TitleBill,
          BillToPostCode = order.ZipCodeBill,
          BillToProvince = order.ProvinceBill,
          BillToTel = order.TelBill,
          ShipToAddress = order.AddrShip,
          ShipToCity = order.CityShip,
          ShipToCountry = order.CountryShip,
          ShipToName = order.CompanyNameShip,
          ShipToPerson = order.TitleShip,
          ShipToPostCode = order.ZipCodeShip,
          ShipToProvince = order.ProvinceShip,
          ShipToTel = order.TelShip,
          Currency1 = order.Currency,
          DiscountRate = order.dAdjustmentDiscountPercentage * -1,
          DueDate = order.DueDate,
          ExchangeRate = order.ExchangeRate,
          InvoiceDate = order.OrderDate,
          Tax = order.TaxChar,
          GST = (order.TaxChar == "G") ? order.TaxRate : 0,
          HST = (order.TaxChar == "H") ? order.TaxRate : 0,
          PST = 0,
          PaymentType = order.PaymentType,
          PaymentTerm = order.PaymentTerm,
          PurchaseNO = order.PONo,
          ShipHandling = order.ShippingFee,
          ExtendedAreaSubcharge = order.ExtendedAreaSubcharge,
          SalesRep = order.SalesRep,
          ShippedVia = order.ShippingMethod,
          NetSales = order.NetSales,
          SubTotal = order.SubTotal,
          TaxTitle = order.TaxTitle,
          TaxRate = order.TaxRate,
          dTaxAmount = order.dTaxAmount,
          Total = order.Total,
          dTotalByEarlyPayment = order.dTotalByEarlyPayment,
          Credits = order.UseAmountbyGiftCard,
          BalanceDue = order.BalanceDue,
          ExtraAdjustmentAmount = order.ExtraAdjustAmount * -1,
          ExtraAdjustmentTitle = order.sExtraAdjustmentTitle,
          CustomerPONo = order.CustomerPONo,
          OrderType = order.OrderType,
          Comment = order.Comment
        };
        oInvoice.SalesActivity = decimal.Round(order.NetSales - order.Adjustment, 2, MidpointRounding.AwayFromZero);

        POSInvoiceDetail oDetail = null;
        oInvoice.oInvoiceDetails.Clear();
        foreach (OrderItem oItem in order.oOrderItems)
        {
          oDetail = oItem.ToInvoiceDetail(oSession, oInvoice);
          if (bSaveInvoice)
            oSession.SaveObj(oDetail);
          oInvoice.oInvoiceDetails.Add(oDetail);
        }
        if (bSaveInvoice)
          oSession.SaveObj(oInvoice);
        if (order.InvoiceNo == null || order.InvoiceNo.Equals(sInvoiceNo) == false)
        {
          order.InvoiceNo = sInvoiceNo;
        }
        return oInvoice;

      }
      catch (Exception Ex)
      {
        throw Ex;
      }
    }
    */
    //public static void DoSaveAppliedCustomerDiscount(this VAOrder order, DiscountDecorator oDecoratorObj, AppliedCustomerDiscount oACD) =>
    //  order.DoSaveAppliedCustomerDiscount(oDecoratorObj, oACD, MISDB[eST.SESSION0]);
    public static void DoSaveAppliedCustomerDiscount(this VAOrder order, DiscountDecorator oDecoratorObj, AppliedCustomerDiscount oACD, SessionProxy oSession)
    {
      try
      {
        bool bUpdateUsedCount = (order.ID == 0 || oDecoratorObj.ID == 0 || oACD.ID == 0);
        oSession.SaveObj(oACD);
        bool bSave = false;

        if (oACD.oRefCustomerDiscount.ID == 0)
          bSave = true;
        if (oACD.bApplied)
        {
          if (bUpdateUsedCount)
          {
            oACD.oRefCustomerDiscount.PrevLastUsedDate = oACD.oRefCustomerDiscount.LastUsedDate;
            oACD.oRefCustomerDiscount.PrevLastUsedPONo = oACD.oRefCustomerDiscount.LastUsedPONo;
            oACD.oRefCustomerDiscount.LastUsedDate = DateTime.Now;
            oACD.oRefCustomerDiscount.LastUsedPONo = order.PONo;
            oACD.oRefCustomerDiscount.UsedCount++;
          }
          bSave = true;
        }
        if (bSave)
          oSession.SaveObj(oACD.oRefCustomerDiscount);
      }
      catch (Exception)
      {

        throw;
      }
    }
    //public static void DoSaveDiscountDecorator(this VAOrder order, DiscountDecorator oDecoratorObj, bool bDecorated) => order.DoSaveDiscountDecorator(oDecoratorObj, bDecorated, MISDB[eST.SESSION0]);
    public static void DoSaveDiscountDecorator(this VAOrder order, DiscountDecorator oDecoratorObj, bool bDecorated, SessionProxy oSession)
    {
      try
      {
        oDecoratorObj.bDecorated = bDecorated;
        if (oDecoratorObj is OptionDiscountDecorator)
        {
          foreach (AppliedCustomerDiscount oACD in (oDecoratorObj as OptionDiscountDecorator).oOptions)
            order.DoSaveAppliedCustomerDiscount(oDecoratorObj, oACD, oSession);
          oSession.SaveObj(oDecoratorObj as OptionDiscountDecorator);
        }
        else
        {
          if (oDecoratorObj.oDiscount != null)
            order.DoSaveAppliedCustomerDiscount(oDecoratorObj, oDecoratorObj.oDiscount, oSession);
          oSession.SaveObj(oDecoratorObj as DiscountDecorator);
        }

        if (oDecoratorObj.oDecoratedItem is DiscountDecorator)
        {
          order.DoSaveDiscountDecorator(oDecoratorObj.oDecoratedItem as DiscountDecorator, true, oSession);
        }
        else if (oDecoratorObj.oDecoratedItem is VAOrderItem)
        {
          (oDecoratorObj.oDecoratedItem as VAOrderItem).bDecorated = true;
          oSession.SaveObj(oDecoratorObj.oDecoratedItem as VAOrderItem);
        }
      }
      catch (Exception)
      {

        throw;
      }
    }
    //public static void DoDeleteAppliedCustomerDiscount(this VAOrder order, AppliedCustomerDiscount oACD)
    //  => order.DoDeleteAppliedCustomerDiscount(oACD, MISDB[eST.SESSION0]);
    public static void DoDeleteAppliedCustomerDiscount(this VAOrder order, AppliedCustomerDiscount oACD, SessionProxy oSession)
    {
      try
      {
        oSession.DeleteObj(oACD, true);
        bool bSave = false;

        if (oACD.oRefCustomerDiscount.ID == 0)
          bSave = false;
        if (oACD.bApplied)
        {
          oACD.oRefCustomerDiscount.LastUsedDate = oACD.oRefCustomerDiscount.PrevLastUsedDate;
          oACD.oRefCustomerDiscount.LastUsedPONo = oACD.oRefCustomerDiscount.PrevLastUsedPONo;
          oACD.oRefCustomerDiscount.UsedCount--;
          if (oACD.oRefCustomerDiscount.UsedCount < 0)
            oACD.oRefCustomerDiscount.UsedCount = 0;
          bSave = true;
        }
        if (bSave)
          oSession.SaveObj(oACD.oRefCustomerDiscount);
      }
      catch (Exception)
      {

        throw;
      }
    }
    //public static void DoDeleteDiscountDecorator(this VAOrder order, DiscountDecorator oDecoratorObj)
    //  => order.DoDeleteDiscountDecorator(oDecoratorObj, MISDB[eST.SESSION0]);
    public static void DoDeleteDiscountDecorator(this VAOrder order, DiscountDecorator oDecoratorObj, SessionProxy oSession)
    {
      try
      {
        if (oDecoratorObj is OptionDiscountDecorator)
        {
          foreach (AppliedCustomerDiscount oACD in (oDecoratorObj as OptionDiscountDecorator).oOptions)
            order.DoDeleteAppliedCustomerDiscount(oACD, oSession);
          (oDecoratorObj as OptionDiscountDecorator).oOptions.Clear();
          oSession.DeleteObj(oDecoratorObj as OptionDiscountDecorator, true);
        }
        else
        {
          if (oDecoratorObj.oDiscount != null)
            order.DoDeleteAppliedCustomerDiscount(oDecoratorObj.oDiscount, oSession);
          oDecoratorObj.oDiscount = null;
          oSession.DeleteObj(oDecoratorObj as DiscountDecorator, true);
        }

        if (oDecoratorObj.oDecoratedItem is DiscountDecorator)
          order.DoDeleteDiscountDecorator(oDecoratorObj.oDecoratedItem as DiscountDecorator, oSession);
        else if (oDecoratorObj.oDecoratedItem is VAOrderItem)
          oSession.DeleteObj(oDecoratorObj.oDecoratedItem as VAOrderItem, true);
      }
      catch (Exception)
      {

        throw;
      }
    }

    //public static void SetShipped(this VAOrder oOrder, ORMServer MISDBServer, bool bAutoXferToVitaaid, ORMServer VADBServer)
    //  => oOrder.SetShipped(bAutoXferToVitaaid, MISDBServer[eST.SESSION0], VADBServer[eST.SESSION0]);
    public static void SetShipped(this VAOrder oOrder, bool bAutoXferToVitaaid, SessionProxy oSession, SessionProxy oVMSession)
    {
      try
      {
        if (oOrder.Status == eORDERSTATUS.SHIP)
          return;
        POSInvoice oInvoice = null;
        //POSInvoiceDetailBackOrder oBackorder = null;
        IList<POSInvoiceDetailBackOrder> oBOs = null;

        if (string.IsNullOrWhiteSpace(oOrder.InvoiceNo))
          oInvoice = oOrder.SaveInvoice(oSession);
        else if (oOrder.bDummy == false)
          oInvoice = oSession.QueryDataElement<POSInvoice>()
                             .Where(x => x.oOrder.ID == oOrder.ID)
                             .UniqueOrDefault();//InvoiceHelper.LoadInvoice(oSession, oOrder.InvoiceNo);

        if (oInvoice != null)
        {
          foreach (POSInvoiceDetail oInvoiceDetail in oInvoice.oInvoiceDetails)//oInvoice.oItemDetails)
          {
            if (oInvoiceDetail.CountOrder < 0) continue;
            if (oInvoiceDetail.CountShipped < oInvoiceDetail.CountOrder)
            {
              //oBackorder = new POSInvoiceDetailBackOrder(oInvoiceDetail);
              //oBackorder.CustomerEmail1 = oOrder.oAccount.CustomerEmail1;
              //oBackorder.SalesRep = oOrder.oAccount.SalesRep;
              //oBackorder.CustomerOwner = oOrder.oAccount.CustomerOwner;
              //oSession.SaveObj(oBackorder);
            }
            else
            {
              oBOs = oSession.QueryDataElement<POSInvoiceDetailBackOrder>()
                             .Where(x => x.ProductCode == oInvoiceDetail.ProductCode && x.AccountNO == oInvoiceDetail.AccountNO)
                             .ToList();
              //.GetXObjs<POSInvoiceDetailBackOrder>("x.ProductCode='" + oInvoiceDetail.ProductCode + "' AND x.AccountNO='" + oInvoiceDetail.AccountNO + "'");
              foreach (POSInvoiceDetailBackOrder oBO in oBOs)
                oSession.DeleteObj(oBO);
            }
          }
        }
        oOrder.Status = eORDERSTATUS.SHIP;
        oSession.SaveObj(oOrder);
        FinishProductApply oApply = null;
        IList<VitaAidFinishProduct> oFinishProducts = null;
        int StockCountBottleAfter = 0;
        foreach (OrderItem oItem in oOrder.oOrderItems)
        {
          if (oItem is CreditOrderItem) continue;
          if (oItem is MCOrderItem &&
              oItem.ProductCode.StartsWith("VA-") == false && oItem.ProductCode.StartsWith("XVA") == false &&
              oItem.ProductCode.StartsWith("SP-") == false && oItem.ProductCode.StartsWith("XSP") == false)
            continue;
          foreach (VAOrderItemByLot oItemByLot in oItem.oItemsByLot)
          {
            if (oItemByLot.ShipQty <= 0 || string.IsNullOrWhiteSpace(oItemByLot.RetailLotNo)) continue;

            //MISDBServer.oSession.GetXObjs<VitaAidFinishProduct>("x.SupplyCode='" + oItemByLot.SupplyCode + "'").ToList().FirstOrDefault();
            oFinishProducts = oSession.QueryDataElement<VitaAidFinishProduct>()
                                                  .Where(x => x.ProductCode == oItemByLot.Code &&
                                                              x.RetailLotNo == oItemByLot.RetailLotNo &&
                                                              x.Salable && x.IsDisposal == false && x.StockCount > 0)
                                                  .OrderBy(x => x.ExpiredDate)
                                                  .ToList();
            oFinishProducts.Where(x => x.SupplyCode == oItemByLot.SupplyCode)
                           .ToList()
                           .Also(x =>
                            {
                              if (x.IsNullOrEmpty())
                                return;
                              x.AddRange(oFinishProducts.Where(x => x.SupplyCode != oItemByLot.SupplyCode));
                              oFinishProducts = x;
                            });

            if (oFinishProducts.IsNullOrEmpty()) return;
            int ShipQty = (int)oItemByLot.ShipQty;

            oFinishProducts.Action(oFinishProduct =>
            {
              if (ShipQty <= 0) return;
              var qty = Math.Min(oFinishProduct.StockCount, ShipQty);
              StockCountBottleAfter = oFinishProduct.StockCount - qty;
              if (StockCountBottleAfter < 0)
                StockCountBottleAfter = 0;
              oApply = new FinishProductApply
              {
                InvoiceNo = oOrder.InvoiceNo,
                SupplyCode = oFinishProduct.SupplyCode,
                ProductCode = oItemByLot.Code,
                ProductName = oItemByLot.Name,
                MESProductCode = oFinishProduct.MESProductCode,
                LotNumber = oItemByLot.RetailLotNo,
                StockCountBottleBefore = oFinishProduct.StockCount,
                ApplyCountBottle = qty,
                StockCountBottleAfter = StockCountBottleAfter,
                SalesPrice = oItem.UnitPrice,
                StockLocation = oItemByLot.StockLocation,
                Status = "I",
                AccountNO = oOrder.AccountNo,
                ApplyID = DataElement.sDefaultUserID,
                ApplyDate = DateTime.Now
              };
              oSession.SaveObj(oApply);
              oFinishProduct.StockCount = StockCountBottleAfter;
              oFinishProduct.UpdatedID = DataElement.sDefaultUserID;
              oFinishProduct.UpdatedDate = DateTime.Now;
              oSession.SaveObj(oFinishProduct);
              ShipQty -= qty;
              if (bAutoXferToVitaaid)
                oItemByLot.XferToVitaaidWH(oFinishProduct, oVMSession, qty);
            });

          }
        }

      }
      catch (Exception)
      {
        throw;
      }
    }
    public static void SetTaxRateByAddress(this VAOrder oOrder, SessionProxy oSession, CustomerAddress oAddress)
    {
      oAddress?.TaxInfo(oSession).Also(x =>
      {
        oOrder.TaxRate = x.TaxRate;
        oOrder.TaxTitle = x.TaxTitle;
        oOrder.TaxChar = x.TaxChar;
      });
    }
  }
}
