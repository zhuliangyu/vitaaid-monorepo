using System;
using MyHibernateUtil;

namespace MIS.DBBO
{
    [Serializable]
    public class AS2VAOrder : POCOBase
    {
        public virtual int ID { get; set; }
        public virtual AS2Log oAS2Log { get; set; }
        public virtual string PONo850 { get; set; }
        public virtual string PODate850 { get; set; }
        public virtual VAOrder oVAOrder { get; set; }
        public virtual string VAPONo { get => oVAOrder?.PONo ?? ""; set { } }
        public virtual string PurposeCode { get; set; } // 00:Original, 01:Cancellation, 05:Replace
        public virtual DateTime CreatedDate { get; set; } = DateTime.Now;
        public virtual string CreatedID { get; set; }
        public virtual DateTime? DateOf855 { get; set; }
        public virtual string DispDateOf855 { get => DateOf855?.ToString() ?? ""; }
        public virtual string MemoOf855 { get; set; }
        public virtual DateTime? DateOf856 { get; set; }
        public virtual string DispDateOf856 { get => DateOf856?.ToString() ?? ""; }
        public virtual string MemoOf856 { get; set; }
        public virtual DateTime? DateOf810 { get; set; }
        public virtual string DispDateOf810 { get => DateOf810?.ToString() ?? ""; }
        public virtual string MemoOf810 { get; set; }
        public override int getID()
        {
            return ID;
        }

        // memory object
        public virtual object oTS850 { get; set; } = null;
        public virtual object oTS860 { get; set; } = null;
    }
}
