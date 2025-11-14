using MyHibernateUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POCO
{
    public class PackagePRNotification: POCOBase
    {
        public virtual int ID { get; set; }
        public override int getID() => ID;
        public virtual string PackageCode { get; set; }
        public virtual string CategoryCode { get; set; }
        public virtual ePACKAGETYPE PackageType { get; set; }
        public virtual string PackageName { get; set; }
        public virtual int Qty { get; set; }
        public virtual int dStockCount { get; set; }
        public virtual string AskLotInfo { get; set; }
        public virtual DateTime CreatedDate { get; set; } = DateTime.Now;
        public virtual string CreatedID { get; set; } = DataElement.sDefaultUserID;
        public virtual DateTime NotifyDate { get; set; } = DateTime.Now;
        public virtual string NotifyID { get; set; } = DataElement.sDefaultUserID;
        public virtual string NotiftyCode { get => (PackageType == ePACKAGETYPE.LABEL) ? PackageCode : CategoryCode; }

        // memory data
        public virtual bool isLable { get => PackageType == ePACKAGETYPE.LABEL; }
        public virtual bool bEnough { get => Qty == 0 || dStockCount >= Qty; }
        public virtual bool bCheck { get; set; } = false;
        public override string ToString() => string.Format("Type: {0}\nCode: {1}\nQty: {2}\nStock: {3}\nName: {4}\nAskingProductInfo: {5}\n", 
                                PackageType.ToString(), PackageCode, Qty, dStockCount, PackageName, AskLotInfo);
    }
}
