using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MyHibernateUtil;
using static MySystem.Base.Extensions.DateTimeExtension;

namespace MIS.DBBO
{
    public class VAGiftCard : DataElement
    {
        public virtual int ID { get; set; }
        public virtual string Name { get; set; }
        public virtual string Code { get; set; }
        public virtual decimal Amount { get; set; } = 0;
        public virtual decimal Balance { get; set; } = 0;
        public virtual System.DateTime StartDate { get; set; } = DateTime.Now.StartOfDay();
        public virtual System.DateTime ExpiryDate { get; set; } = NilDate;
        public virtual int UsageLimit { get; set; } = Int32.MaxValue;
        public virtual int UsedCount { get; set; } = 0;
        public virtual bool Valid { get; set; } = true;
        public virtual System.DateTime? LastUsedDate { get; set; }
        public virtual string Comment { get; set; } = "";
        public virtual string AccountNo { get; set; }
        public virtual string LastUsedNote { get; set; } = "";
        public virtual string LastUsedOn { get; set; } = "";
        public virtual eCREDITTYPE CreditType { get; set; } = eCREDITTYPE.CUSTOMER_CREDIT;
        public virtual string Currency { get; set; } = "CAD";
        public virtual bool bValidCard(DateTime applyDate)
        {
            return (Valid && UsedCount < UsageLimit && Balance > 0 && StartDate <= applyDate && applyDate <= ExpiryDate.EndOfDay());
        }
        public virtual bool bValidCard()
        {
            return bValidCard(DateTime.Now); //(Valid && UsedCount < UsageLimit && Balance > 0 && StartDate <= DateTime.Now && DateTime.Now <= ExpiryDate);
        }

        // memory object
        public virtual decimal UseAmount { get; set; } = 0;
        public virtual string sUseAmount { get { return "(" + UseAmount + ")"; } }
        public virtual decimal NewBalance { get { return Balance - UseAmount; } }
        public virtual string sUsageLimit { get => (UsageLimit == Int32.MaxValue) ? "UNLIMITED" : UsageLimit.ToString(); }
        public virtual string sExpiryDate { get => ExpiryDate.ToStr("MM/dd/yyyy"); }
        public virtual ObservableCollection<VAPayment> oPayments { get; set; }

        public override int getID()
        {
            return ID;
        }


        public static string GiftCardCodeForEmployeeCredit(CustomerAccount oCA, string refPONo)
        {
            try
            {
                if (oCA.bEmployee == false || string.IsNullOrWhiteSpace(refPONo))
                    return "";
                return refPONo.Substring(1, 6) + oCA.CustomerCode;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public static string GiftCardCodeForEmployeeCredit(CustomerAccount oCA, DateTime refDate)
        {
            try
            {
                if (oCA.bEmployee == false)
                    return "";
                return refDate.ToString("yyyyMM") + oCA.CustomerCode;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public static VAGiftCard NewGiftCardForEmployeeCredit(CustomerAccount oCA, string sGiftCardCode, int year, int month)
        {
            try
            {
                return new VAGiftCard
                {
                    CreditType = eCREDITTYPE.EMPLOYEE_CREDIT,
                    Name = "Employee Credit",
                    Code = sGiftCardCode,
                    AccountNo = oCA.CustomerCode,
                    Amount = oCA.EmployeeMonthlyCredit,
                    Balance = oCA.EmployeeMonthlyCredit,
                    StartDate = new DateTime(year, month, 1, 0, 0, 0),
                    ExpiryDate = (new DateTime(year, month, 1, 23, 59, 59)).AddMonths(1).AddDays(-1),
                    UsageLimit = 1,
                    UsedCount = 0,
                    Valid = true,
                };
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
