using MyHibernateUtil;
using System;
using System.ComponentModel;


namespace POCO
{
    [Serializable]
    public class XactHistoryRecall : POCOBase
    {
        public virtual int ID { get; set; }
        public override int getID() => ID;
        public virtual Recall oRecall { get; set; }
        public virtual CustomerRecall oCustomer { get; set; }
        public virtual string CustomerCode { get => oCustomer.CustomerCode; set { } }
        public virtual string CustomerName { get => oCustomer.CustomerName; set { } }
        public virtual string ComplaintNo { get => oRecall.ComplaintNo; set { } }
        public virtual RecallDetail oRecallDetail { get; set; }
        public virtual DateTime ApplyDate { get; set; }
        //public virtual string ApplyTime { get; set; }
        public virtual int ApplyCountBottle { get; set; }
        public virtual string ProductCode { get => oRecallDetail?.ProductCode ?? ""; set { } }
        public virtual string DispApplyDate {  get => ApplyDate.ToShortDateString(); }
    }
}
