using MyHibernateUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace POCO
{
    [Serializable]
    public class Recall : DataElement
    {
        public virtual int ID { get; set; }
        public override int getID() => ID;
        public virtual string ComplaintNo { get; set; }
        public virtual string PotentialHazard { get; set; }
        public virtual string ProblemDesc { get; set; }
        public virtual string ImpactLevel { get; set; }
        public virtual string HarmLevel { get; set; }
        public virtual IList<CustomerRecall> oCustomers { get; set; } = new List<CustomerRecall>();
        public virtual IList<RecallLot> oRecallLots { get; set; } = new List<RecallLot>();        
        public virtual string LotNo { get => (oRecallLots.Any())? oRecallLots.Select(x => x.LotNo).Aggregate((a, b) => a + "," + b) : ""; set { } }
        public virtual string ProductName
        {
            get
            {
                string name = (oRecallLots.Any()) ? oRecallLots.Select(x => x.ProductName).Distinct().Aggregate((a, b) => a + "," + b) : "";
                if (string.IsNullOrWhiteSpace(name) || name.Length < 395)
                    return name;
                return name.Substring(0, 395) + " ...";
            }
            set { }
        }
        public virtual string ProductCode { get => (oRecallLots.Any()) ? oRecallLots.Select(x => x.ProductCode).Distinct().Aggregate((a, b) => a + "," + b) : ""; set { } }
        public virtual int CollectedQty { get => oRecallLots.SelectMany(x => x.oRecallDetails).Sum(x => x.CollectedQty); }
        public virtual int TotalRecallQty { get => oRecallLots.SelectMany(x => x.oRecallDetails).Sum(x => x.RecallQty); }
        public virtual string DispCollectedRate
        {
            get
            {
                if (oCustomers == null || oCustomers.Count == 0) return "N/A";
                int iTotalRecallQty = TotalRecallQty, iCollectedQty = CollectedQty;
                if (iTotalRecallQty == 0) return "N/A";
                return iCollectedQty.ToString() + "/" + iTotalRecallQty.ToString() + "=" + 
                    Math.Round(iCollectedQty * 100.0 / iTotalRecallQty, 2, MidpointRounding.AwayFromZero).ToString() + "%";
            }

        }
    }
}
