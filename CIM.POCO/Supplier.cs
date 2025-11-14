using System;


namespace POCO
{
    [Serializable]
    public class Supplier
    {
        public Supplier() { }
        public virtual int ID { get; set; }
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
        public virtual bool Active { get; set; } = true;
        public virtual string dispActive { get { return (Active) ? "V" : "X"; } }
        public virtual Supplier ShallowCopy()
        {
            return (Supplier)this.MemberwiseClone();
        }
    }
}
