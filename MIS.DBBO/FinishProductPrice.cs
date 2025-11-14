using System;
using MyHibernateUtil;

namespace MIS.DBBO
{
    [Serializable]
    public class FinishProductPrice : DataElement
    {
        public virtual int ID { get; set; }
        public virtual string ProductCode { get; set; }
        public virtual string ProductName { get; set; }
        public virtual decimal StandardPrice { get; set; }
        public virtual decimal MSRPrice { get; set; }
        public virtual decimal EmployeePrice { get; set; }
        public virtual decimal USDPrice { get; set; }
        public virtual double USDExchangeRateFactor { get; set; } = 1.0;
        public virtual string UPC { get; set; }
        public virtual string Comment { get; set; }

        public override int getID()
        {
            return ID;
        }
    }
}
