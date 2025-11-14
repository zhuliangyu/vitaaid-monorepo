using MyHibernateUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using PropertyChanged;
using System.Linq;

namespace POCO
{
    [Serializable]
    public class RecallDetail : DataElement
    {
        public virtual int ID { get; set; }
        public override int getID() => ID;
        public virtual Recall oRecall { get; set; }
        public virtual string ComplaintNo { get => oRecall?.ComplaintNo ?? ""; set { } }
        public virtual RecallLot oRecallLot { get; set; }
        public virtual string LotNo { get => oRecallLot?.LotNo ?? ""; set { } }
        public virtual string ProductCode { get; set; }
        public virtual CustomerRecall oCustomer { get; set; }
        public virtual string CustomerCode { get => oCustomer?.CustomerCode ?? ""; set { } }
        public virtual int RecallQty { get; set; } = 0;
        public virtual int CollectedQty { get; set; } = 0;
        public virtual IList<XactHistoryRecall> oXacts { get; set; } = new List<XactHistoryRecall>();
        public virtual Dictionary<string, int> getShippingQty()
        {
            var rtnVal = oXacts.Select(x => new { ApplyData = x.ApplyDate.ToString("MMM-dd-yyyy"), x.ApplyCountBottle })
                  .GroupBy(x => x.ApplyData)
                  .Select(x => new { x.Key, ShippingQty = x.Sum(y => y.ApplyCountBottle) })
                  .ToDictionary(x => x.Key, y => y.ShippingQty);
            //Dictionary<string, int> rtnVal = new Dictionary<string, int>();
            //if (oXactHistorys == null || oXactHistorys.Count == 0) return rtnVal;
            //string sKey;
            //foreach (XactHistoryRecall oXHR in oXactHistorys)
            //{
            //    sKey = oXHR.ApplyDate.ToString("MMM-dd-yyyy");
            //    if (rtnVal.ContainsKey(sKey))
            //        rtnVal[sKey] += oXHR.ApplyCountBottle;
            //    else
            //        rtnVal.Add(sKey, oXHR.ApplyCountBottle);
            //}
            return rtnVal;
        }
        public virtual bool bEditable { get; set; }

    }
}
