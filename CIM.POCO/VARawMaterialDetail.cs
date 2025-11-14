using MyHibernateUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using PropertyChanged;

namespace POCO
{
    [Serializable]
    public class VARawMaterialDetail : DataElement
    {
        public virtual int ID { get; set; }
        public override int getID() => ID;

        public virtual RawMaterialSpec RawMaterialSpec { get; set; }

        public virtual string ReceivingNo { get; set; } = "";

        public virtual int BoxNumber { get; set; }

        public virtual DateTime RetestDate { get; set; }

        public virtual string Barcode { get; set; }
        public virtual bool IsPrinted { get; set; }
        public virtual bool InWH { get; set; } = true;
        public virtual string RawMaterialLotNumber { get; set; }
        public virtual double? SupplyWeightBox { get; set; }
        public virtual double? BagWeight { get; set; }
        public virtual double? GrossWeight { get; set; }
        public virtual double? StockWeight { get; set; }
        public virtual double? ReserveWeight { get; set; }
        public virtual string StockLocation { get; set; }
        public virtual bool Disposal { get; set; } = false;
        public virtual double? SafeStock { get; set; }
        public virtual string Comment { get; set; }
        public virtual bool Calibration { get; set; }
        public virtual string CalibrationID { get; set; }
        public virtual DateTime? CalibrationDate { get; set; }
        public virtual string OldRawMaterialCode { get; set; }
        public virtual string RawMaterialName { get => RawMaterialSpec?.RawMaterialName ?? ""; set { } }
        public virtual string ShortRetestDate
        {
            get
            {
                return RetestDate.ToString("yyMM");
            }
            set
            {

                int year, month;
                if (Int32.TryParse(value.Substring(0, 2), out year) && Int32.TryParse(value.Substring(2, 2), out month))
                {
                    year = 2000 + Int32.Parse(value.Substring(0, 2));
                    month = Int32.Parse(value.Substring(2, 2));
                    RetestDate = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
                }
            }
        }
        
        public virtual void AutoGenBarcode()
        {
            if (RawMaterialSpec == null) return;
            Barcode = RawMaterialSpec.SpecCode + "*" + ReceivingNo + "*" + BoxNumber.ToString("D2") + "*" + ShortRetestDate + "00";
            OnPropertyChanged("Barcode");
        }
        
        public virtual VAMESRawMaterial oOwner { get; set; }
        private static string diffDouble(double? oldVal, double? newVal)
        {
            try
            {
                string rtnStr = "";
                if (oldVal == null || oldVal.HasValue == false)
                    rtnStr = "null";
                else
                    rtnStr = oldVal.Value.ToString();
                if (newVal == null || newVal.HasValue == false)
                    rtnStr += "=>null";
                else
                    rtnStr += "=>" + newVal.Value.ToString();
                return rtnStr;
            }
            catch (Exception ex) { throw ex; }
        }

        public virtual string diffStr(VARawMaterialDetail oldObj)
        {
            try
            {
                string rtnStr = "";
                if (GrossWeight != oldObj.GrossWeight)
                    rtnStr += "[GrossWeight:" + diffDouble(oldObj.GrossWeight, GrossWeight) + "]";
                if (BagWeight != oldObj.BagWeight)
                    rtnStr += "[BagWeight:" + diffDouble(oldObj.BagWeight, BagWeight) + "]";
                if (StockLocation != null && StockLocation.Equals(oldObj.StockLocation) == false)
                    rtnStr += "[StockLocation:" + oldObj.StockLocation + "=>" + StockLocation + "]";
                if (InWH != oldObj.InWH)
                    rtnStr += "[InWH:" + oldObj.InWH.ToString() + "=>" + InWH.ToString() + "]";
                if (IsPrinted != oldObj.IsPrinted)
                    rtnStr += "[IsPrinted:" + oldObj.IsPrinted.ToString() + "=>" + IsPrinted.ToString() + "]";
                if (SupplyWeightBox != oldObj.SupplyWeightBox)
                    rtnStr += "[SupplyWeightBox:" + diffDouble(oldObj.SupplyWeightBox, SupplyWeightBox) + "]";
                if (BoxNumber != oldObj.BoxNumber)
                    rtnStr += "[BoxNum:" + oldObj.BoxNumber + "=>" + BoxNumber + "]";
                if (RetestDate.Ticks != oldObj.RetestDate.Ticks)
                    rtnStr += "[RetestDate:" + oldObj.RetestDate.ToString("yyMM") + "=>" + RetestDate.ToString("yyMM") + "]";
                if (Disposal != oldObj.Disposal)
                    rtnStr += "[Disposal:" + oldObj.Disposal + "=>" + Disposal + "]";
                return rtnStr;
            }
            catch (Exception ex) { throw ex; }
        }
        public virtual IList<PrintRawMaterialLabelLog> oPrintLabelLogs { get; set; } = new List<PrintRawMaterialLabelLog>();
    }
}
