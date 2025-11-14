using System;
using System.Collections.Generic;
using System.Linq;
using MyHibernateUtil;
using PropertyChanged;
using MySystem.Base.Extensions;

namespace MIS.DBBO
{
    public class CustomerDiscount : DataElement
    {
        public virtual int ID { get; set; }
        public virtual CustomerAccount oCustomer { get; set; }
        public virtual string CustomerCode { get; set; }
        public virtual DiscountProgram oDiscountProgram { get; set; }
        public virtual int UsedCount { get; set; }
        public virtual DateTime? LastUsedDate { get; set; }
        public virtual string LastUsedPONo { get; set; }
        public virtual DateTime? PrevLastUsedDate { get; set; }
        public virtual string PrevLastUsedPONo { get; set; }
        public virtual bool IsActive { get; set; } = true;
        [DoNotNotify]
        public virtual IList<DiscountProductByAccount> oProductsByAccount { get; set; } = null;
        public virtual IList<string> AppliedProducts { get; set; } = new List<string>();
        [DoNotNotify]
        public virtual string sBackupAppliedProducts { get; set; } = null;

        public virtual bool Valid(string PONo, DateTime AppliedDate)
        {
            if (oDiscountProgram.bInvalidFromUIResult)
                return false;
            if (oDiscountProgram.StartDate != null && AppliedDate < oDiscountProgram.StartDate.Value.StartOfDay())
                return false;
            if (oDiscountProgram.ExpiryDate != null && oDiscountProgram.ExpiryDate.Value.EndOfDay() < AppliedDate)
                return false;
            if (oDiscountProgram.UsageLimit == eUSAGERULE.FIRSTBYMONTH && LastUsedDate != null)
            {
                if (PONo.Equals(LastUsedPONo) == true)
                    return true;
                if (LastUsedDate.Value >= AppliedDate || LastUsedDate.Value.ToString("YYYYMM") == AppliedDate.ToString("YYYYMM"))
                    return false;
            }
            if (oDiscountProgram.UsageLimit == eUSAGERULE.ONETIME && UsedCount > 0 && LastUsedPONo != PONo)
                return false;
            return true;
        }
        public virtual bool QualifyDiscount(string sProductCode)
        {
            try
            {
                if (oDiscountProgram.DiscountType == eDISCOUNTTYPE.FIXEDCART_PER || oDiscountProgram.DiscountType == eDISCOUNTTYPE.FIXEDCART_SUB) return true;
                List<string> sProductCodes = new List<string>();
                switch (oDiscountProgram.TargetProductMethod)
                {
                    case ePRODUCTRULE.ALL:
                        return true;
                    case ePRODUCTRULE.SELECTED:
                        sProductCodes.AddRange(oDiscountProgram.oTargetProducts.Select(x => x.ProductCode));
                        break;
                    case ePRODUCTRULE.BYACCOUNT:
                        sProductCodes.AddRange(oProductsByAccount.Select(x => x.ProductCode));
                        break;
                }
                if (sProductCodes == null || sProductCodes.Count == 0)
                    return false;
                foreach (string sCode in sProductCodes)
                    if (sProductCode == sCode)
                        return true;
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override int getID()
        {
            return ID;
        }
    }
}
