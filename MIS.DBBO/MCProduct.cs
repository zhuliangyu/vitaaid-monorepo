using System;
using MyHibernateUtil;

namespace MIS.DBBO
{
    [Serializable]
    public class MCProduct : DataElement
    {
        public virtual int ID { get; set; }
        public virtual string Name { get; set; }
        public virtual string Code { get; set; } = "MC-";
        public virtual decimal UnitPrice { get; set; }
        public virtual string Unit { get; set; }
        public virtual double Weight { get; set; }
        public virtual string Remark { get; set; }
        public override int getID()
        {
            return ID;
        }
    }
}
