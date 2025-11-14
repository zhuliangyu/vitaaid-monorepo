using System;
using System.ComponentModel;


namespace POCO
{
    [Serializable]
    public class FabRawMaterialApplyBatch : INotifyPropertyChanged
    {
        public virtual int ID { get; set; }
        public virtual int LotID { get; set; }
        public virtual string LotNo { get; set; }
        public virtual FabFormulationItem FormulationItem { get; set; }
        public virtual FabRawMaterialApply RawMaterialApply { get; set; }
        public virtual FabRawMaterialBatch RawMaterialBatch { get; set; }
        public virtual int BatchNo { get; set; }
        public virtual double ReqWeight { get; set; }
        public virtual string displayReqWeight { get { return Math.Round((decimal)ReqWeight, Constant.OPIDIGIT, MidpointRounding.AwayFromZero) + " " + WeightUnit.AbbrName; } }
        public virtual double RealStockWeightBefore { get; set; }
        public virtual string displayStockWeightBefore { get { return Math.Round((decimal)RealStockWeightBefore, Constant.OPIDIGIT, MidpointRounding.AwayFromZero) + " " + WeightUnit.AbbrName; } }
        public virtual double ApplyWeight { get; set; }
        public virtual string displayApplyWeight { get { return Math.Round((decimal)ApplyWeight, Constant.OPIDIGIT, MidpointRounding.AwayFromZero) + " " + WeightUnit.AbbrName; } }
        public virtual double RealStockWeightAfter { get; set; }
        public virtual string displayStockWeightAfter { get { return Math.Round((decimal)RealStockWeightAfter, Constant.OPIDIGIT, MidpointRounding.AwayFromZero) + " " + WeightUnit.AbbrName; } }
        public virtual UnitType WeightUnit { get; set; }
        public virtual string WHRawMaterialName { get; set; }
        public virtual string ScannedBarCode { get; set; }
        public virtual string WHOldCode { get; set; }
        public virtual string BatchCode { get; set; }

        private DateTime _CreatedDate = DateTime.Now;
        public virtual DateTime CreatedDate { get { return _CreatedDate; } set { _CreatedDate = value; } }

        public virtual event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
