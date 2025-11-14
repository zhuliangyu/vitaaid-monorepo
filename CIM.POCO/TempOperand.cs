using System;


namespace POCO
{
    [Serializable]
    public class TempOperand
    {
        public virtual int ID { get; set; }
        public virtual Process RefMainProcess { get; set; }
        public virtual Step RefStep { get; set; }
        public virtual Operation RefOperation { get; set; }
        public virtual string Name { get; set; }
        public virtual TempOperand ShallowCopy()
        {
            return (TempOperand)this.MemberwiseClone();
        }
    }
}
