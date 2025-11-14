using System;


namespace MIS.DBBO
{
    [Serializable]
    public class SupplyCode_v
    {
        public virtual int ID { get; set; }
        public virtual string SupplyCode { get; set; }
        public virtual string sType { get; set; }
    }
}
