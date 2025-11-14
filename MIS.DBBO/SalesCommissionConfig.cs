using System;
using MyHibernateUtil;

namespace MIS.DBBO
{
    public class SalesCommissionConfig : DataElement
    {
        public virtual int ID { get; set; }
        public virtual string EmpAccount { get; set; }
        public virtual string ShortName { get; set; }
        public virtual int ContractYear { get; set; } = DateTime.Now.Year;
        public virtual double FullCommissionThreshold { get; set; } = 85.0;
        public virtual double FullRateOfFirstOrder { get; set; } = 15.0;
        public virtual string sFullRateOfFirstOrder { get => string.Format("{0}% of the QUOTA", FullCommissionThreshold); }
        public virtual double FullRateOfSecondOrder { get; set; } = 10.0;
        public virtual double FullRateOfOtherOrder { get; set; } = 7.0;
        public virtual double LowerRateOfFirstOrder { get; set; } = 14.0;
        public virtual double LowerRateOfSecondOrder { get; set; } = 9.0;
        public virtual double LowerRateOfOtherOrder { get; set; } = 6.0;
        public virtual decimal Quota1 { get; set; }
        public virtual decimal Quota2 { get; set; }
        public virtual decimal Quota3 { get; set; }
        public virtual decimal Quota4 { get; set; }
        public virtual decimal Quota5 { get; set; }
        public virtual decimal Quota6 { get; set; }
        public virtual decimal Quota7 { get; set; }
        public virtual decimal Quota8 { get; set; }
        public virtual decimal Quota9 { get; set; }
        public virtual decimal Quota10 { get; set; }
        public virtual decimal Quota11 { get; set; }
        public virtual decimal Quota12 { get; set; }

        public virtual decimal LowerQuota1 { get => decimal.Round(Quota1 * (decimal)FullCommissionThreshold * (decimal)0.01, 2, MidpointRounding.AwayFromZero); }
        public virtual decimal LowerQuota2 { get => decimal.Round(Quota2 * (decimal)FullCommissionThreshold * (decimal)0.01, 2, MidpointRounding.AwayFromZero); }
        public virtual decimal LowerQuota3 { get => decimal.Round(Quota3 * (decimal)FullCommissionThreshold * (decimal)0.01, 2, MidpointRounding.AwayFromZero); }
        public virtual decimal LowerQuota4 { get => decimal.Round(Quota4 * (decimal)FullCommissionThreshold * (decimal)0.01, 2, MidpointRounding.AwayFromZero); }
        public virtual decimal LowerQuota5 { get => decimal.Round(Quota5 * (decimal)FullCommissionThreshold * (decimal)0.01, 2, MidpointRounding.AwayFromZero); }
        public virtual decimal LowerQuota6 { get => decimal.Round(Quota6 * (decimal)FullCommissionThreshold * (decimal)0.01, 2, MidpointRounding.AwayFromZero); }
        public virtual decimal LowerQuota7 { get => decimal.Round(Quota7 * (decimal)FullCommissionThreshold * (decimal)0.01, 2, MidpointRounding.AwayFromZero); }
        public virtual decimal LowerQuota8 { get => decimal.Round(Quota8 * (decimal)FullCommissionThreshold * (decimal)0.01, 2, MidpointRounding.AwayFromZero); }
        public virtual decimal LowerQuota9 { get => decimal.Round(Quota9 * (decimal)FullCommissionThreshold * (decimal)0.01, 2, MidpointRounding.AwayFromZero); }
        public virtual decimal LowerQuota10 { get => decimal.Round(Quota10 * (decimal)FullCommissionThreshold * (decimal)0.01, 2, MidpointRounding.AwayFromZero); }
        public virtual decimal LowerQuota11 { get => decimal.Round(Quota11 * (decimal)FullCommissionThreshold * (decimal)0.01, 2, MidpointRounding.AwayFromZero); }
        public virtual decimal LowerQuota12 { get => decimal.Round(Quota12 * (decimal)FullCommissionThreshold * (decimal)0.01, 2, MidpointRounding.AwayFromZero); }

        public override int getID()
        {
            return ID;
        }
    }
}
