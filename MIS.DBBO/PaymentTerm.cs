using System;
using MyHibernateUtil;

namespace MIS.DBBO
{
    [Serializable]
    public class PaymentTerm : DataElement
    {
        public virtual int ID { get; set; }
        public virtual int DaysDue { get; set; } = 0;
        public virtual double? EarlyPaymentDiscounts { get; set; }
        public virtual int? EarlyPaymentPeriod { get; set; }

        public override int getID()
        {
            return ID;
        }

        public virtual string Desc { get { return ToString(); } }
        public override string ToString()
        {
            if (DaysDue == 0)
                return "Due Upon Receipt";
            string rtnStr = "Net " + DaysDue.ToString() + " Days";
            if ((EarlyPaymentDiscounts ?? 0) > 0 && (EarlyPaymentPeriod ?? 0) > 0 && EarlyPaymentPeriod < DaysDue)
                rtnStr = (double)EarlyPaymentDiscounts + "% " + (int)EarlyPaymentPeriod + " " + rtnStr; //2% 10 Net 30 Day
            return rtnStr;
        }
    }
}
