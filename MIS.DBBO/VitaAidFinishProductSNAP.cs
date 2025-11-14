using MyHibernateUtil;
using System;

namespace MIS.DBBO
{
    public class VitaAidFinishProductSNAP : VitaAidProduct
    {
        public virtual int ReservedCount { get; set; }
        public virtual bool IsRelabel { get; set; }
        public virtual double GramWeightPerBottle { get; set; }
        public virtual string ReleasedID { get; set; }
        public virtual DateTime ReleasedDate { get; set; } = SysVal.NilDate;

        //memory object
        public virtual object oProductInfo { get; set; }
        public virtual string sFormulationName { get; set; }
        public virtual int iIndex { get; set; }
        public virtual int Size { get; set; }
        public virtual string dispSize { get { return (Size > 0) ? Size.ToString() : ""; } }
        private double _dBOTTLECost = 0;
        private double _dCAPCost = 0;
        private double _dLABELCost = 0;
        private double _dCAPSULECost = 0;
        private double _dSCOOPSCost = 0;
        private double _dDESICCANTCost = 0;
        public virtual bool bIncludeCAP { get; set; } = false;
        public virtual bool bValidLabeCost { get { return ((dLABELCost <= 0 || dLABELCost > 0.5) ? false : true); } }
        public virtual double dBOTTLECost { get { return _dBOTTLECost; } set { _dBOTTLECost = value; OnPropertyChanged("dBOTTLECost"); } }
        public virtual double dCAPCost { get { return _dCAPCost; } set { _dCAPCost = value; OnPropertyChanged("dCAPCost"); } }
        public virtual double dLABELCost
        {
            get { return _dLABELCost; }
            set
            {
                _dLABELCost = value; OnPropertyChanged("dLABELCost");
            }
        }
        public virtual double dCAPSULECost { get { return _dCAPSULECost; } set { _dCAPSULECost = value; OnPropertyChanged("dCAPSULECost"); } }
        public virtual double dSCOOPSCost { get { return _dSCOOPSCost; } set { _dSCOOPSCost = value; OnPropertyChanged("dSCOOPSCost"); } }
        public virtual double dDESICCANTCost { get { return _dDESICCANTCost; } set { _dDESICCANTCost = value; OnPropertyChanged("dDESICCANTCost"); } }
        public virtual double dRawMaterialCost { get; set; }
        public virtual double dPackageCost
        {
            get
            {
                return (dBOTTLECost + ((bIncludeCAP) ? 0 : dCAPCost) + dLABELCost + dCAPSULECost + dSCOOPSCost + dDESICCANTCost);
            }
        }
        public virtual int iBOTTLECnt { get; set; }
        public virtual int iCAPCnt { get; set; }
        public virtual int iLABELCnt { get; set; }
        public virtual int iCAPSULECnt { get; set; }
        public virtual int iSCOOPSCnt { get; set; }
        public virtual int iDESICCANTCnt { get; set; }
        public virtual void ResetPackCost()
        {
            dBOTTLECost = dCAPCost = dLABELCost = dCAPSULECost = dSCOOPSCost = dDESICCANTCost = 0;
            iBOTTLECnt = iCAPCnt = iLABELCnt = iCAPSULECnt = iSCOOPSCnt = iDESICCANTCnt = 0;
            bIncludeCAP = false;
        }
        public virtual string DisplayCreatedDate { get { return CreatedDate.ToString("yyyy, MM/dd"); } }
        public virtual string DisplayUpdatedDate { get { return UpdatedDate.ToString("yyyy, MM/dd"); } }
        public virtual string DisplayExpireDate { get { return ExpiredDate.ToString("yyyy, MM/dd"); } }

        // memory object
        public virtual object Tag { get; set; }
    }
}
