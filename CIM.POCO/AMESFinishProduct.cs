using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace POCO
{
    [Serializable]
    public class AMESFinishProduct
    {
        public virtual string SupplyCode { get; set; }
        public virtual string ProductCode { get; set; }
        public virtual string ProductName { get; set; }
        public virtual string LotNumber { get; set; }
        public virtual int StockCountBottle { get; set; }
        public virtual int SupplyCountBottle { get; set; }
        public virtual decimal SalesPrice { get; set; }
        public virtual string DisplaySalesPrice
        {
            get { return Math.Round(SalesPrice, 2).ToString(); }
        }
        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime UpdatedDate { get; set; }
        public virtual DateTime ExpireDate { get; set; }
        public virtual string DisplayCreatedDate { get { return CreatedDate.ToString("yyyy, MM/dd"); } }
        public virtual string DisplayUpdatedDate { get { return UpdatedDate.ToString("yyyy, MM/dd"); } }
        public virtual string DisplayExpireDate { get { return ExpireDate.ToString("yyyy, MM/dd"); } }
        public virtual string StockLocation { get; set; }
        //public virtual string CapsulesOrGrams { get; set; }
        public virtual int CapsulesPerBottle { get; set; }
        public virtual string Comment { get; set; }
        public virtual double StockLastYear { get; set; }
        // memory object for showing retest date image
        public virtual object oProductInfo { get; set; }
        public virtual object oLot { get; set; }
        public virtual int iIndex { get; set; }
        public virtual int iServings { get; set; }
        public virtual int Size { get; set; }
        public virtual string dispSize { get { return (Size > 0) ? Size.ToString() : ""; } }
        private double _dBOTTLECost = 0;
        private double _dCAPCost = 0;
        private double _dLABELCost = 0;
        private double _dCAPSULECost = 0;
        private double _dSCOOPSCost = 0;
        private double _dDESICCANTCost = 0;

        public virtual double dBOTTLECost { get { return _dBOTTLECost; } set { _dBOTTLECost = value; OnPropertyChanged("dBOTTLECost");  } }
        public virtual double dCAPCost { get { return _dCAPCost; } set { _dCAPCost = value; OnPropertyChanged("dCAPCost"); } }
        public virtual double dLABELCost { get { return _dLABELCost; } set { _dLABELCost = value; OnPropertyChanged("dLABELCost"); } }
        public virtual double dCAPSULECost { get { return _dCAPSULECost; } set { _dCAPSULECost = value; OnPropertyChanged("dCAPSULECost"); } }
        public virtual double dSCOOPSCost { get { return _dSCOOPSCost; } set { _dSCOOPSCost = value; OnPropertyChanged("dSCOOPSCost"); } }
        public virtual double dDESICCANTCost { get { return _dDESICCANTCost; } set { _dDESICCANTCost = value; OnPropertyChanged("dDESICCANTCost"); } }
        public virtual double dRawMaterialCost { get; set; }
        public virtual double dPackageCost
        {
            get
            {
                return (dBOTTLECost + dCAPCost + dLABELCost + dCAPSULECost + dSCOOPSCost + dDESICCANTCost);
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
        }

        public virtual event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

    }
}
