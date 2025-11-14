using MyHibernateUtil;

namespace MIS.DBBO
{
    public class SharedCommissionConfigure : DataElement
    {
        public virtual int ID { get; set; }
        public virtual Employee oSalesRep { get; set; }
        public virtual string SalesRep
        {
            get => oSalesRep?.ShortName ?? "";
            set { }
        }
        public virtual string SalesRepAccount { get; set; }
        public virtual double Rate { get; set; } = 0.0;

        public override int getID()
        {
            return ID;
        }
    }
}
