using System;
using System.Collections.Generic;
using MyHibernateUtil;

namespace MIS.DBBO
{
    [Serializable]
    public class AS2Log : POCOBase
    {
        public virtual int ID { get; set; }
        public virtual string XactCode { get; set; }
        public virtual string XactCtrlNum { get; set; }
        public virtual DateTime CreatedDate { get; set; } = DateTime.Now;
        public virtual string Note { get; set; }
        public virtual string FileName { get; set; }
        public virtual string AS2Type { get; set; }
        public virtual IList<AS2VAOrder> Groups { get; set; } = null;

        public override int getID()
        {
            return ID;
        }
    }
}
