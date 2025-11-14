using MyHibernateUtil;
using System;
using System.ComponentModel;


namespace POCO
{
    [Serializable]
    public class VirtualLotInfo : DataElement
    {
        public VirtualLotInfo() {}
        public VirtualLotInfo(string fCode, string pCode, string name, int servings, int iRefPeriodMonth, int RefQuanity, int iFP) 
        {
            FormulationCode = fCode;
            ProductCode = pCode;
            ProductName = name;
            Servings = servings;
            RefPeriodByMonth = iRefPeriodMonth;
            this.RefQuanity = RefQuanity;
            ReqPeriodByMonth = iFP;
        }
        public virtual int ID { get; set; }
        public override int getID() => ID;
        public virtual string VirtualLotNo { get; set; }
        public virtual string FormulationCode { get; set; }
        public virtual string ProductCode { get; set; }
        public virtual string ProductName { get; set; }
        public virtual int RefQuanity { get; set; }
        public virtual int RefPeriodByMonth { get; set; } = 6;
        public virtual int RefPeriodByInvoice { get; set; } = 6;
        public virtual string sRefPeriod
        {
            get => (RefPeriodByMonth == RefPeriodByInvoice) ? RefPeriodByMonth.ToString() : RefPeriodByInvoice + "(" + RefPeriodByMonth + ")";
        }
        public virtual int ReqPeriodByMonth { get; set; } = 0;

        public virtual int ReqQuanity { get; set; } = 0;

        public virtual int Servings { get; set; } = 0;
        public virtual int ProductSize { get => (Servings <= 0) ? 0 : (int)Math.Ceiling((double)(TotalServings / Servings)); set { } }
        public virtual string sProductSize => (Servings <= 0) ? "" : Math.Ceiling((double)(TotalServings / Servings)).ToString();

        private int _TotalServings = 0;
        public virtual int TotalServings
        {
            get
            {
                return _TotalServings;
            }
            set
            {
                _TotalServings = value;
                if (oBindFabLot != null)
                    oBindFabLot.LotSize = _TotalServings;
                OnPropertyChanged("TotalServings");
            }
        }

        public virtual int ShortServings => (((ReqQuanity * Servings) / 1000) + 1) * 1000;
        public virtual eVirtualLotStatus Status { get; set; } = eVirtualLotStatus.ANALYSIS;

        public virtual bool bAbort { get; set; } = false;

        public virtual bool bAnalysis => (Status == eVirtualLotStatus.ANALYSIS && bProduction == false);
        public virtual bool bProduction => !(oBindFabLot?.No?.StartsWith("V") ?? true);
        public virtual bool bByForecast { get; set; }
        public virtual bool Urgent { get; set; }
        public virtual Lot oBindFabLot { get; set; }
        public virtual int Sequence { get => oBindFabLot?.Sequence ?? 0; 
            set {
                if (oBindFabLot != null)
                    oBindFabLot.Sequence = value;}
        }
        // memory data
        public virtual int StockCountButtle { get; set; }
        public virtual int SemiProductBottle { get; set; }
        public virtual int QCQuantity { get; set; }
        public virtual bool bEmergency
        {
            get
            {
                if (Urgent == true ||
                    StockCountButtle == 0 ||
                    RefPeriodByMonth == 0 ||
                    (RefQuanity / RefPeriodByMonth) * 4 >= (StockCountButtle + QCQuantity)                    
                    )
                    return true;
                return false;
            }
        }
        public virtual int PRLevel
        {
            get
            {
                int iReqPerMonByRef = (RefPeriodByMonth > 0) ? (int)Math.Ceiling((float)RefQuanity / RefPeriodByMonth) : 99999;
                int iReqPerMonByReq = (ReqPeriodByMonth > 0) ? (int)Math.Ceiling((float)ReqQuanity / ReqPeriodByMonth) : 0;
                int iReqPerMon = Math.Max(iReqPerMonByRef, iReqPerMonByReq);
                int iTotalStockCount = StockCountButtle + QCQuantity;
                int iLevel =
                    (iTotalStockCount <= (int)(iReqPerMon * 1.5) || Urgent) ? 2 :
                    (iTotalStockCount <= (iReqPerMon * 4)) ? 1 : 0;
                return iLevel;
            }
        }
    }
}
