using System;
using MyHibernateUtil;

namespace MIS.DBBO
{
    [Serializable]
    public class CanadaTax : POCOBase
    {
        public virtual int ID { get; set; }
        public virtual string ProvinceID { get; set; }
        public virtual string ProvinceName { get; set; }
        public virtual string CountryName { get; set; }
        public virtual string FromProvionceID { get; set; }
        public virtual string TaxIDfromProvionce { get; set; }
        public virtual string TaxID { get; set; }
        public virtual double TaxRateHST { get; set; }
        public virtual double TaxRateGST { get; set; }
        public virtual double TaxRatePST { get; set; }
        public virtual double TaxRateReserve { get; set; }
        public virtual string Status { get; set; }
        public virtual string CreatedID { get; set; }
        public virtual DateTime CreatedDate { get; set; } = DateTime.Now;
        public virtual string CreatedTime { get; set; }
        public virtual string UpdatedID { get; set; }
        public virtual DateTime UpdatedDate { get; set; }
        public virtual string UpdatedTime { get; set; }
        public virtual string Comment { get; set; }

        public override int getID()
        {
            return ID;
        }
    }
}
