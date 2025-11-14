using System;
using MyHibernateUtil;

namespace MIS.DBBO
{
    [Serializable]
    public abstract class POSItemDetailBase : DataElement
    {
        public virtual int ID { get; set; }
        public virtual string InvoiceNO { get; set; }
        public virtual string AccountNO { get; set; }
        public virtual string ProductCode { get; set; }
        public virtual string ProductName { get; set; }
        //public virtual string MESProductCode { get; set; }
        public virtual double? CountOrder { get; set; }
        public virtual double? CountShipped { get; set; }
        public virtual string CountUnit { get; set; }
        public virtual decimal? UnitPrice { get; set; }
        public virtual double? Discount { get; set; }
        public virtual string Tax { get; set; }
        public virtual double? GST { get; set; }
        public virtual double? PST { get; set; }
        public virtual double? HST { get; set; }
        public virtual short? PackingListNO { get; set; }
        public virtual string CreatedTime { get; set; }
        public virtual string UpdatedTime { get; set; }
        public virtual string Comment { get; set; }
        public virtual decimal NetSales { get; set; } = 0;

        public override int getID()
        {
            return ID;
        }
    }
}
