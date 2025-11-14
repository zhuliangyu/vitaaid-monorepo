using MyHibernateUtil;
using System;


namespace POCO
{
    [Serializable]
    public class OrderNoControl: POCOBase
    {
        public virtual int ID { get; set; }
        public override int getID() => ID;
        public OrderNoControl() { }
        public OrderNoControl(string sVC, int iYear, int iSNo)
        {
            VendorCode = sVC;
            Year = iYear;
            SerierNo = iSNo;
        }
        public virtual string VendorCode { get; set; }
        public virtual int Year { get; set; }
        public virtual int SerierNo { get; set; }
        public virtual string ToPONo() => VendorCode + Year.ToString().Substring(1, 3) + "-" + SerierNo.ToString("D3");

    }
}
