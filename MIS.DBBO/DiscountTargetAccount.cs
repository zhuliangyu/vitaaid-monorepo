using System;
using MyHibernateUtil;

namespace MIS.DBBO
{
    public class DiscountTargetAccount : POCOBase
    {
        public virtual int ID { get; set; }
        public virtual DiscountProgram oDiscountProgram { get; set; }
        public virtual string DiscountName { get; set; }
        public virtual CustomerAccount oCustomerAccount { get; set; }
        public virtual string CustomerCode { get; set; }
        public virtual string CustomerName { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual string CreatedID { get; set; }

        public override int getID()
        {
            return ID;
        }
    }
}
