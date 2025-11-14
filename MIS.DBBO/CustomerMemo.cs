using System;
using MyHibernateUtil;
using PropertyChanged;

namespace MIS.DBBO
{
    [Serializable]
    public class CustomerMemo : DataElement
    {
        public virtual int ID { get; set; }
        public virtual CustomerAccount oCustomer { get; set; }
        public virtual string CustomerCode { get; set; }
        public virtual string Memo { get; set; }

        [DoNotNotify]
        public virtual bool bEmpty
        {
            get
            {
                return string.IsNullOrWhiteSpace(Memo);
            }
        }
        public override int getID()
        {
            return ID;
        }
        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(Memo) ? "" : Memo;
        }
    }
}
