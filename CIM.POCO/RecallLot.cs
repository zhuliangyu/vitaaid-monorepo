using MyHibernateUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using PropertyChanged;
using System.Linq;

namespace POCO
{
    [Serializable]
    public class RecallLot : DataElement
    {
        public virtual int ID { get; set; }
        public override int getID() => ID;
        public virtual Recall oRecall { get; set; }
        public virtual string ComplaintNo { get => oRecall?.ComplaintNo ?? ""; set { } }
        public virtual string LotNo { get; set; } = "";
        public virtual string ProductCode { get; set; } = "";
        public virtual string ProductName { get; set; } = "";
        public virtual IList<RecallDetail> oRecallDetails { get; set; } = new List<RecallDetail>();
        public virtual int CollectedQty { get => oRecallDetails.Sum(x => x.CollectedQty); }
        public virtual int TotalRecallQty { get => oRecallDetails.Sum(x => x.RecallQty); }

        // memory obj
        public virtual string sLabelFileByLot { get; set; } = "";
        public virtual IList<string> sLabelFiles { get; set; } = new List<string>();
    }
}
