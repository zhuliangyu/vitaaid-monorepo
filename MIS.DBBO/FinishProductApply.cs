using System;
using MyHibernateUtil;

namespace MIS.DBBO
{
    [Serializable]
    public class FinishProductApply : POCOBase
    {
        public virtual int ID { get; set; }
        public virtual int ApplySequenceNumber { get; set; }
        public virtual string InvoiceNo { get; set; }
        public virtual string SupplyCode { get; set; }
        public virtual string ProductCode { get; set; }
        public virtual string ProductName { get; set; }
        public virtual string MESProductCode { get; set; }
        public virtual string LotNumber { get; set; }
        public virtual int StockCountBottleBefore { get; set; }
        public virtual int ApplyCountBottle { get; set; }
        public virtual int StockCountBottleAfter { get; set; }
        public virtual Decimal SalesPrice { get; set; }

        public virtual string StockLocation { get; set; }
        public virtual string Status { get; set; }
        public virtual string AccountNO { get; set; }
        public virtual string ApplyID { get; set; }
        public virtual DateTime ApplyDate { get; set; }

        public override int getID()
        {
            return ID;
        }
    }
}
