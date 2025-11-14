using MIS.DBBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySystem.Base.Extensions;
using MyHibernateUtil;

namespace MIS.DBPO
{
  public static class ShippingPolicyExtension
  {
    public static IList<ShippingPolicyMaster> FetchAllShippingPolicies(SessionProxy oSession)
    {
      try
      {
        return oSession.QueryDataElement<ShippingPolicyMaster>()
                       .Where(x => x.IsActive)
                       .ToList()
                       .Also(results =>
                       {
                         results.Action(oSPM => oSPM.oPolicyContext = oSession.QueryDataElement<ShippingPolicy>().Where(x => x.oSPMaster == oSPM).ToList());
                       });
      }
      catch (Exception)
      {
        throw;
      }
    }
    public static bool IsValidShippingPolicy(this ShippingPolicyMaster self, VAOrder oOrder)
    {
      try
      {
        if (!self.IsActive || !self.IsValidByDate(oOrder.OrderDate) || !self.IsValidByCouponCode(oOrder.CouponCode))
          return false;

        string Country = oOrder.CountryShip.ToUpper();
        ShippingPolicy P_Next_Free_Shipping_DATE = null;
        if (self.PolicyType == eSHIPPINGPOLICYTYPE.FREE_SHIPPING_DAY_CA)
          P_Next_Free_Shipping_DATE = self.oPolicyContext.Where(x => x.ParameterName == "P_Next_Free_Shipping_DATE").FirstOrDefault();

        return self.PolicyType switch
        {
          eSHIPPINGPOLICYTYPE.CANADA => (Country == "CANADA" || Country == "CA"),
          eSHIPPINGPOLICYTYPE.USA => (Country == "UNITED STATES" || Country == "USA"),
          eSHIPPINGPOLICYTYPE.FREE_SHIPPING_DAY_CA => (Country == "CANADA" || Country == "CA") && P_Next_Free_Shipping_DATE != null && P_Next_Free_Shipping_DATE.ParameterDateValue.SameDay(oOrder.OrderDate),
          _ => false
        };
      }
      catch (Exception)
      {
        throw;
      }
    }
    public static bool CaculateShippingFeeByPolicy(this ShippingPolicyMaster self, VAOrder oOrder, out decimal fee)
    {
      fee = 0;
      try
      {
        if (!self.IsValidShippingPolicy(oOrder))
          return false;

        return self.PolicyType switch
        {
          eSHIPPINGPOLICYTYPE.CANADA => self.CaculateShippingFeeCA_Area(oOrder, out fee),
          eSHIPPINGPOLICYTYPE.USA => self.CaculateShippingFeeUSA_Area(oOrder, out fee),
          eSHIPPINGPOLICYTYPE.FREE_SHIPPING_DAY_CA => self.CaculateShippingFeeFreeShippingDay_CAArea(oOrder, out fee),
          _ => false
        };
      }
      catch (Exception)
      {
        throw;
      }
    }
    private static bool CaculateShippingFeeFreeShippingDay_CAArea(this ShippingPolicyMaster self, VAOrder oOrder, out decimal ShippingFee)
    {
      ShippingFee = 0M;
      try
      {
        //if (!self.IsValidShippingPolicy(oOrder))
        //  return false;
        return self.CaculateShippingFeeCA_Area(oOrder, out ShippingFee);
      }
      catch (Exception)
      {
        throw;
      }
    }
    private static bool CaculateShippingFeeCA_Area(this ShippingPolicyMaster self, VAOrder oOrder, out decimal ShippingFee)
    {
      ShippingFee = 0M;
      try
      {
        //if (!self.IsValidShippingPolicy(oOrder))
        //  return false;

        string Province = oOrder.ProvinceShip?.ToUpper() ?? "";
        
        var oSPDict = self.oPolicyContext;
        ShippingPolicy P_Canada_Free_Shipping_Express = null;
        ShippingPolicy P_Canada_Free_Shipping_Express_Province_STR = null;
        ShippingPolicy P_Canada_Free_Shipping = null;
        ShippingPolicy P_Canada_Free_Shipping_Province_STR = null;
        ShippingPolicy P_Canada_Shipping_In_BC = null;
        ShippingPolicy P_Canada_Shipping_WO_Synerclear_Out_BC = null;
        ShippingPolicy P_Canada_Shipping_With_Synerclear_Out_BC = null;
        ShippingPolicy P_LowestAmountForExtraCharge = null;
        ShippingPolicy P_ExtraChargeFee = null;
        oSPDict.Action(x =>
        {
          if (x.ParameterName == "P_Canada_Free_Shipping_Express")
            P_Canada_Free_Shipping_Express = x;
          else if (x.ParameterName == "P_Canada_Free_Shipping_Express_Province_STR")
            P_Canada_Free_Shipping_Express_Province_STR = x;
          else if (x.ParameterName == "P_Canada_Free_Shipping")
            P_Canada_Free_Shipping = x;
          else if (x.ParameterName == "P_Canada_Free_Shipping_Province_STR")
            P_Canada_Free_Shipping_Province_STR = x;
          else if (x.ParameterName == "P_Canada_Shipping_In_BC")
            P_Canada_Shipping_In_BC = x;
          else if (x.ParameterName == "P_Canada_Shipping_WO_Synerclear_Out_BC")
            P_Canada_Shipping_WO_Synerclear_Out_BC = x;
          else if (x.ParameterName == "P_Canada_Shipping_With_Synerclear_Out_BC")
            P_Canada_Shipping_With_Synerclear_Out_BC = x;
          else if (x.ParameterName == "P_LowestAmountForExtraCharge")
            P_LowestAmountForExtraCharge = x;
          else if (x.ParameterName == "P_ExtraChargeFee")
            P_ExtraChargeFee = x;
        });
        if ( P_Canada_Free_Shipping_Express == null ||
             P_Canada_Free_Shipping_Express_Province_STR == null ||
             P_Canada_Free_Shipping == null ||
             P_Canada_Free_Shipping_Province_STR == null ||
             P_Canada_Shipping_In_BC == null ||
             P_Canada_Shipping_WO_Synerclear_Out_BC == null ||
             P_Canada_Shipping_With_Synerclear_Out_BC == null ||
             P_LowestAmountForExtraCharge == null ||
             P_ExtraChargeFee == null)
          return false;

        // Order over CDN $350 to ON, QC, NS, NB, NF, and PE (Next Day 1-2 Day Delivery)
        if (oOrder.SalesActivity >= (decimal)P_Canada_Free_Shipping_Express.ParameterValue &&
            !string.IsNullOrWhiteSpace(P_Canada_Free_Shipping_Express_Province_STR.ParameterStringValue) &&
             P_Canada_Free_Shipping_Express_Province_STR.ParameterStringValue.Contains(Province))
          return true;  // condition of free shipping: true

        // Order over CDN $250 to BC, AB, SK, MB (1-2 Day Delivery)
        if (oOrder.SalesActivity >= (decimal)P_Canada_Free_Shipping.ParameterValue &&
            !string.IsNullOrWhiteSpace(P_Canada_Free_Shipping_Province_STR.ParameterStringValue) &&
             P_Canada_Free_Shipping_Province_STR.ParameterStringValue.Contains(Province))
          return true; // condition of free shipping: true

        //Orders under $250 CAD are subject to the following fees:
        bool bWithSynerClear = oOrder.WithSynearClear();

        //o $12.50 CAD for shipments to areas within BC
        if (Province == "BC")
          ShippingFee = (decimal)P_Canada_Shipping_In_BC.ParameterValue;
        //o $12.50 CAD for shipments (WITHOUT SynerClear ® ) to all other provinces in Canada
        else if (Province != "BC" && bWithSynerClear == false)
          ShippingFee = (decimal)P_Canada_Shipping_WO_Synerclear_Out_BC.ParameterValue;
        //o $16 CAD for shipments (WITH SynerClear ® ) to all other provinces outside BC in Canada
        else if (Province != "BC" && bWithSynerClear == true)
          ShippingFee = (decimal)P_Canada_Shipping_With_Synerclear_Out_BC.ParameterValue;

        //Orders under $50 are subject to an additional $5 handling fee.
        if (oOrder.SalesActivity < (decimal)P_LowestAmountForExtraCharge.ParameterValue)
          ShippingFee += (decimal)P_ExtraChargeFee.ParameterValue;
        
        return true;
      }
      catch (Exception)
      {
        throw;
      }
    }
    private static bool CaculateShippingFeeUSA_Area(this ShippingPolicyMaster self, VAOrder oOrder, out decimal ShippingFee)
    {
      ShippingFee = 0M;
      try
      {
        //if (!self.IsValidShippingPolicy(oOrder))
        //  return false;

        string Province = oOrder.ProvinceShip?.ToUpper() ?? "";

        bool bAlaskaHawaii = (Province == "HI" || Province == "AK") ? true : false;

        var oSPDict = self.oPolicyContext;
        ShippingPolicy P_US_Free_Shipping = null;
        ShippingPolicy P_US_AKHI_Free_Shipping = null;
        ShippingPolicy P_US_Shipping = null;
        ShippingPolicy P_LowestAmountForExtraCharge = null;
        ShippingPolicy P_ExtraChargeFee = null;
        oSPDict.Action(x =>
        {
          if (x.ParameterName == "P_US_Free_Shipping")
            P_US_Free_Shipping = x;
          else if (x.ParameterName == "P_US_AKHI_Free_Shipping")
            P_US_AKHI_Free_Shipping = x;
          else if (x.ParameterName == "P_US_Shipping")
            P_US_Shipping = x;
          else if (x.ParameterName == "P_LowestAmountForExtraCharge")
            P_LowestAmountForExtraCharge = x;
          else if (x.ParameterName == "P_ExtraChargeFee")
            P_ExtraChargeFee = x;          
        });
        if (P_US_Free_Shipping == null || P_US_AKHI_Free_Shipping == null ||
             P_US_Shipping == null || P_LowestAmountForExtraCharge == null ||
             P_ExtraChargeFee == null)
          return false;

        //Orders over USD $350 in Continental United States (USD $500 in Alaska and Hawaii) will be regular-shipped free of charge.
        if (oOrder.SalesActivity >= (bAlaskaHawaii ? (decimal)P_US_AKHI_Free_Shipping.ParameterValue 
                                                   : (decimal)P_US_Free_Shipping.ParameterValue)) 
          return true;  // condition of free shipping: true

        //Orders under $350 USD for US orders are subject to the following fees:
        //o $20 USD for all US destinations.
        ShippingFee = (decimal)P_US_Shipping.ParameterValue;

        //Orders under $50 are subject to an additional $5 handling fee.
        if (oOrder.SalesActivity < (decimal)P_LowestAmountForExtraCharge.ParameterValue)
          ShippingFee += (decimal)P_ExtraChargeFee.ParameterValue;

        return true;
      }
      catch (Exception)
      {
        throw;
      }
    }

  }
}
