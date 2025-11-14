using System;
using System.Linq;
using System.Collections.Generic;
using MySystem.Base.Extensions;

namespace MIS.DBBO
{
  public class OptionDiscountDecorator : DiscountDecorator
  {
    public virtual IList<AppliedCustomerDiscount> oOptions { get; set; } = new List<AppliedCustomerDiscount>();
    public virtual bool bByMaxDiscount { get; set; } = true;
    public override decimal CaculateAmount()
    {
      try
      {
        Remark = "";
        if (oOptions == null || oOptions.Count == 0)
          return (Amount = oDecoratedItem.CaculateAmount());
        if (bByMaxDiscount)
        {
          oDiscount = null;
          double dVal = 0, dMaxDiscount = 0;
          foreach (AppliedCustomerDiscount oACD in oOptions)
          {
            oACD.bApplied = false;
            if (oACD.oDiscountProgram.bMeetItemCondition((double)OrderQty, UnitPrice))
            {
              if ((dVal = oACD.oDiscountProgram.DiscountPercentage()) > dMaxDiscount)
              {
                dMaxDiscount = dVal;
                oDiscount = oACD;
                DiscountType = oACD.oDiscountProgram.DiscountType;
              }
            }
            else if (oACD.oDiscountProgram.DiscountType == eDISCOUNTTYPE.FIXEDPRODUCT_FREE && oGifts != null && oGifts.Count > 0)
              oGifts.Clear();
          }
          if (oDiscount != null)
            oDiscount.bApplied = true;
        }
        else //if(oOptions.Where(x => x.bApplied).IsNullOrEmpty())
             //return (Amount = oDecoratedItem.CaculateAmount());
             //else 
        {
          var oAppliedDiscount = oOptions.Where(x => x.bApplied).FirstOrDefault();
          if (oAppliedDiscount == null)
            oDiscount = null;
          else if (oDiscount != oAppliedDiscount) 
          { 
            oDiscount = oAppliedDiscount;
            DiscountType = oAppliedDiscount.oDiscountProgram.DiscountType;
          };
        }
        return base.CaculateAmount();
      }
      catch (Exception)
      {

        throw;
      }
    }

  }
}
