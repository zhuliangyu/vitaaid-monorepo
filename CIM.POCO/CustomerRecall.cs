using MyHibernateUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using PropertyChanged;


namespace POCO
{
    [Serializable]
    public class CustomerRecall : DataElement
    {
        public virtual int ID { get; set; }
        public override int getID() => ID;
        public virtual Recall oRecall { get; set; }
        public virtual string CustomerCode { get; set; }
        public virtual string CustomerName { get; set; }
        public virtual string CustomerEmail { get; set; }
        public virtual string NotifyMethod { get; set; }
        public virtual string CustomerTel { get; set; }
        private string _SendEmailStatus = "";
        public virtual string SendEmailStatus
        {
            get { return _SendEmailStatus; }
            set
            {
                _SendEmailStatus = value;
                OnPropertyChanged("SendEmailStatus");
                OnPropertyChanged("DispNotifyStatus");
            }
        }
        public virtual string DispNotifyStatus { get => (SendEmailStatus == "SUCCESS!") ? "SENT" : SendEmailStatus; }
        public virtual string RecallStatus { get; set; }
        public virtual DateTime RecallDate { get; set; } = SysVal.NilDate;
        public virtual string sRecallDate { get => (RecallDate.Year >= SysVal.NilDate.Year || RecallDate.Year < 2000) ? "" : RecallDate.ToString("MMM-dd-yyyy"); }
        public virtual IList<RecallDetail> oRecallDetails { get; set; } = new List<RecallDetail>();
        public virtual Dictionary<string, string> getVarValues()
        {
            Dictionary<string, string> rtnVal = new Dictionary<string, string>();
            if (oRecall == null) return rtnVal;

            rtnVal.Add("{Complaint#}", oRecall.ComplaintNo);
            rtnVal.Add("{CustomerName}", CustomerName);
            rtnVal.Add("{RecallDate}", (RecallDate.Year >= SysVal.NilDate.Year || RecallDate.Year < 2015) ? DateTime.Now.ToString("MMM-dd-yyyy") : RecallDate.ToString("MMM-dd-yyyy"));
            rtnVal.Add("{Problem}", oRecall.ProblemDesc);
            rtnVal.Add("{PotentialHazard}", oRecall.PotentialHazard);
            rtnVal.Add("{PatientDistributorLevel}", oRecall.ImpactLevel);
            rtnVal.Add("{Symptom}", oRecall.HarmLevel);

            var info = "Product Name\tCode\tLotNo\n";
            info += oRecallDetails.Select(x => x.oRecallLot.ProductName + "\t" + x.ProductCode + "\t" + x.LotNo)
                              .Aggregate((a, b) => a + "\n" + b);
            rtnVal.Add("{info}", info);
            return rtnVal;
        }
        public virtual string sLetterFileName
        {
            get
            {
                if (oRecall == null) return "-" + CustomerCode;
                return oRecall.ComplaintNo + "-" + CustomerCode;
            }
        }

        // Memory Object
        private bool _bEditable = false;
        public virtual bool bEditable { get { return _bEditable; } set { _bEditable = value; } }
        private bool _bCheck = false;
        public virtual bool bCheck { get { return _bCheck; } set { _bCheck = value; OnPropertyChanged("bCheck"); } }
        public virtual int iTotalRecallQty
        {
            get
            {
                return oRecallDetails.SelectMany(x => x.oXacts).Sum(x => x.ApplyCountBottle);
                //if (oXactHistorys == null || oXactHistorys.Count == 0)
                //    return 0;
                //int iTotal = 0;
                //foreach (XactHistoryRecall xhr in oXactHistorys)
                //    iTotal += xhr.ApplyCountBottle;
                //return iTotal;
            }
        }
        public virtual string TotalRecallQty { get => iTotalRecallQty.ToString(); }

        private IList<string> _NotifyMethods = null;
        public virtual IList<string> NotifyMethods
        {
            get
            {
                if (_NotifyMethods == null)
                {
                    _NotifyMethods = new List<string>();
                    _NotifyMethods.Add("Mail");
                    if (CustomerEmail != null && CustomerEmail.Length > 0)
                    {
                        _NotifyMethods.Add(CustomerEmail);
                    }
                }
                return _NotifyMethods;
            }
        }
    }
}
