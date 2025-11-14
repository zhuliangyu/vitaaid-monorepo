using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace POCO
{
    [Serializable]
    public class CurrRMClosedStock_v : INotifyPropertyChanged
    {
        public virtual string ReceivingNo { get; set; }
        public virtual string RawMaterialCode { get; set; }
        private string _RawMaterialName;
        public virtual string RawMaterialName
        {
            get { return _RawMaterialName; }
            set
            {
                _RawMaterialName = value;
                OnPropertyChanged("RawMaterialName");
            }
        }
        public virtual string Supplier { get; set; }
        private string _RawMaterialLotNumber;
        public virtual string RawMaterialLotNumber
        {
            get
            {
                return _RawMaterialLotNumber;
            }
            set
            {
                _RawMaterialLotNumber = value;
                OnPropertyChanged("RawMaterialLotNumber");
            }
        }
        private string _Currency1 = "USD";
        public virtual string Currency1
        {
            get { return _Currency1; }
            set
            {
                _Currency1 = value;
                OnPropertyChanged("Currency1");
            }
        }
        public virtual Decimal UnitCostForeignCurrency { get; set; }

        private double _ExchangeRate = 1;
        public virtual double? ExchangeRate
        {
            get { return _ExchangeRate; }
            set
            {
                _ExchangeRate = (value == null) ? 1 : value.Value;
                OnPropertyChanged("ExchangeRate");
            }
        }

        public virtual DateTime ExchangeRateDate { get; set; }

        private DateTime _ReleaseDate = new DateTime(2050, 12, 31);
        public virtual DateTime ReleaseDate
        {
            get { return _ReleaseDate; }
            set
            {
                _ReleaseDate = value;
                OnPropertyChanged("DispReleaseDate");
            }
        }
        public virtual string DispReleaseDate
        {
            get
            {
                if (bReleased == false)
                    return "Not release";
                else
                    return ReleaseDate.ToShortDateString();
            }
        }
        public virtual bool bReleased
        {
            get { return (ReleaseDate.Year >= 2050) ? false : true; }
        }
        private double? _StockWeight;
        public virtual double? StockWeight
        {
            get { return _StockWeight; }
            set
            {
                _StockWeight = value;
                OnPropertyChanged("StockWeight");
            }
        }
        private double? _SupplyWeight;
        public virtual double? SupplyWeight
        {
            get { return _SupplyWeight; }
            set
            {
                _SupplyWeight = value;
                OnPropertyChanged("SupplyWeight");
            }
        }
        public virtual Decimal SalesPrice { get; set; }
        public virtual Decimal UnitCostTax { get; set; }
        public virtual double UnitCost { get; set; }
        public virtual string DispUnitCost
        {
            get { return (Currency1 + " $" + Math.Round(UnitCost, 2, MidpointRounding.AwayFromZero).ToString() + " /kg"); }
        }
        private DateTime _ExpireDate = new DateTime(2050, 12, 31);
        public virtual DateTime ExpireDate
        {
            get { return _ExpireDate; }
            set
            {
                _ExpireDate = value;
                if (RetestDate.Year == 2050 || RetestDate.Ticks < ExpireDate.Ticks)
                {
                    RetestDate = _ExpireDate;
                    OnPropertyChanged("RetestDate");
                }
                OnPropertyChanged("ExpireDate");
            }
        }
        public virtual string StockLocation { get; set; }
        public virtual string CreatedID { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual string CreatedTime { get; set; }
        public virtual string UpdatedID { get; set; }
        public virtual DateTime UpdatedDate { get; set; }
        public virtual string UpdatedTime { get; set; }
        public virtual double? SafeStock { get; set; }
        private double? _ReserveWeight;
        public virtual double? ReserveWeight
        {
            get { return _ReserveWeight; }
            set
            {
                _ReserveWeight = value;
                OnPropertyChanged("ReserveWeight");
            }
        }
        public virtual string Comment { get; set; }
        private double? _SupplyWeightBox1;
        public virtual double? SupplyWeightBox1
        {
            get { return _SupplyWeight; }
            set
            {
                _SupplyWeight = value;
                OnPropertyChanged("SupplyWeightBox1");
            }
        }
        private int _QuatityofBox1;
        public virtual int QuatityofBox1
        {
            get
            {
                return _QuatityofBox1;
            }
            set
            {
                _QuatityofBox1 = value;
                OnPropertyChanged("QuatityofBox1");
            }
        }
        public virtual double? SupplyWeightBox2 { get; set; }
        public virtual int QuatityofBox2 { get; set; }
        public virtual double? StockWeightRealMesure { get; set; }
        private double? _BagWeight;
        public virtual double? BagWeight
        {
            get { return _BagWeight; }
            set
            {
                _BagWeight = value;
                OnPropertyChanged("BagWeight");
            }
        }

        private string _SpecCode { get; set; }
        public virtual string SpecCode
        {
            get { return _SpecCode; }
            set
            {
                _SpecCode = value;
                OnPropertyChanged("SpecCode");
            }
        }
        public virtual int Status { get; set; }
        public virtual double? StockLastYear { get; set; }
        private DateTime _ClosedDate = new DateTime(2050, 12, 31);
        public virtual DateTime ClosedDate
        {
            get { return _ClosedDate; }
            set
            {
                if (value.Year != 1)
                    _ClosedDate = value;
                OnPropertyChanged("ClosedDate");
            }
        }
        public virtual string DispClosedDate
        {
            get
            {
                return (ClosedDate.Year == 2050) ? "" : ClosedDate.ToString("MM/dd/yyyy");
            }
        }
        private DateTime _DisposalDate = new DateTime(2050, 12, 31);
        public virtual DateTime DisposalDate
        {
            get { return _DisposalDate; }
            set
            {
                if (value.Year != 1)
                    _DisposalDate = value;
                OnPropertyChanged("DisposalDate");
            }
        }
        public virtual string DispDisposalDate
        {
            get
            {
                return (DisposalDate.Year == 2050) ? "" : DisposalDate.ToString("MM/dd/yyyy");
            }
        }

        public virtual MESRawMaterial ShallowCopy()
        {
            return (MESRawMaterial)this.MemberwiseClone();
        }

        public virtual event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }


        // NEW data members for MES system
        public virtual int ID { get; set; }
        public virtual string TotalBuyingWt
        {
            get
            {
                return Math.Round(((SupplyWeightBox1 != null) ? SupplyWeightBox1.Value * QuatityofBox1 : 0) +
                        ((SupplyWeightBox2 != null) ? SupplyWeightBox2.Value * QuatityofBox2 : 0), 2, MidpointRounding.AwayFromZero).ToString() + " kg";
            }
        }
        public virtual RawMaterialCategory RMCategory { get; set; }
        public virtual RawMaterialSpec RMSpec { get; set; }
        public virtual Supplier SupplierObj { get; set; }
        public virtual PurchaseRMReqInfo oPurchaseRMReq { get; set; }
        public virtual string PONumber
        {
            get
            {
                return (oPurchaseRMReq == null) ? "" :
                    (oPurchaseRMReq.oPO == null) ? "" :
                    oPurchaseRMReq.oPO.PONo;
            }
        }

        private double? _GrossWeight = null;
        public virtual double? GrossWeight
        {
            get
            {
                if (_GrossWeight == null)
                {
                    return ((SupplyWeightBox1 != null) ? SupplyWeightBox1.Value : 0) +
                           ((BagWeight != null) ? BagWeight.Value : 0);
                }
                else
                    return _GrossWeight.Value;
            }
            set
            {
                _GrossWeight = value;
            }
        }
        /*public virtual RawMaterialSpecDetail RMSpecDetail { get; set; }
        public virtual string displaySpecDetailParameter
        {
            get
            {
                if (RMSpecDetail.PotencyMethod == ePOTENCYMETHOD.NONE) return "";
                if (RMSpecDetail.PotencyMethod == ePOTENCYMETHOD.CONCENTRATION)
                    return RMSpecDetail.Factor.ToString() + ":1";
                else if (RMSpecDetail.PotencyMethod == ePOTENCYMETHOD.POTENCY)
                {
                    if (RMSpecDetail != null)
                        return RMSpecDetail.displaySpecDetailParameter;
                    else
                        return "";
                }
                else if (RMSpecDetail.PotencyMethod == ePOTENCYMETHOD.PURITY)
                {
                    if (RMSpecDetail != null)
                        return RMSpecDetail.Potency.Value.ToString() + "%";
                    else
                        return "";
                }
                else if (RMSpecDetail.PotencyMethod == ePOTENCYMETHOD.STANDARDIZE)
                    return RMSpecDetail.Potency.ToString() + "% " + RMSpecDetail.ActiveIngredient.Name;
                return "";

            }
        }

        public virtual string displaySimpSpecDetailParameter
        {
            get
            {
                if (RMSpecDetail.PotencyMethod == ePOTENCYMETHOD.NONE) return "";
                if (RMSpecDetail.PotencyMethod == ePOTENCYMETHOD.CONCENTRATION)
                    return RMSpecDetail.Factor.ToString();
                else if (RMSpecDetail.PotencyMethod == ePOTENCYMETHOD.POTENCY)
                {
                    if (RMSpecDetail != null)
                        return RMSpecDetail.displaySpecDetailParameter;
                    else
                        return "";
                }
                else if (RMSpecDetail.PotencyMethod == ePOTENCYMETHOD.PURITY)
                {
                    if (RMSpecDetail != null)
                        return RMSpecDetail.Potency.Value.ToString() + "%";
                    else
                        return "";
                }
                else if (RMSpecDetail.PotencyMethod == ePOTENCYMETHOD.STANDARDIZE)
                    return RMSpecDetail.Potency.ToString();
                return "";

            }
        }*/

        public virtual string dispRealStockWeight
        {
            get
            {
                double dVal = 0;
                dVal += (StockWeight != null && StockWeight.HasValue) ? StockWeight.Value : 0;
                dVal += (ReserveWeight != null && ReserveWeight.HasValue) ? ReserveWeight.Value : 0;
                return Math.Round(dVal, 4, MidpointRounding.AwayFromZero).ToString();
            }
        }

        // transient data member
        private double _tmpStockWeight = Double.MaxValue;
        public virtual double tmpStockWeight { get { return _tmpStockWeight; } set { _tmpStockWeight = value; } }

        // for receiving report
        private bool _bSpcialStorage = false;
        public virtual bool bSpcialStorage
        {
            get { return _bSpcialStorage; }
            set { _bSpcialStorage = value; }
        }
        public virtual string sSpecialStorge { get { return (bSpcialStorage) ? "Yes" : "No"; } }

        private bool _bDamage = true;
        public virtual bool bDamage
        {
            get { return _bDamage; }
            set { _bDamage = value; }
        }
        public virtual string sDamage { get { return (bDamage) ? "Yes" : "No"; } }

        private bool _bSealsIntact = true;
        public virtual bool bSealsIntact
        {
            get { return _bSealsIntact; }
            set { _bSealsIntact = value; }
        }
        public virtual string sSealsIntact { get { return (bSealsIntact) ? "Yes" : "No"; } }

        private bool _bLabelsIntact = true;
        public virtual bool bLabelsIntact
        {
            get { return _bLabelsIntact; }
            set { _bLabelsIntact = value; }
        }
        public virtual string sLabelsIntact { get { return (bLabelsIntact) ? "Yes" : "No"; } }

        private bool _bDeliveredCorrect = true;
        public virtual bool bDeliveredCorrect
        {
            get { return _bDeliveredCorrect; }
            set { _bDeliveredCorrect = value; }
        }
        public virtual string sDeliveredCorrect { get { return (bDeliveredCorrect) ? "Yes" : "No"; } }
        public virtual double QuantityReceived
        {
            get
            {
                return Math.Round((SupplyWeightBox1 != null) ? SupplyWeightBox1.Value * QuatityofBox1 : 0, 1, MidpointRounding.AwayFromZero);
            }
        }
        public virtual string sQRUnit { get { return (oPurchaseRMReq != null && oPurchaseRMReq.BuyingUnit != null) ? oPurchaseRMReq.BuyingUnit.AbbrName : "KG"; } }

        public virtual string SupplierName { get { return (SupplierObj == null) ? "" : SupplierObj.Name; } }
        public virtual string SupplierCode { get { return (SupplierObj == null) ? "" : SupplierObj.Code; } }

        private DateTime _RetestDate = new DateTime(2050, 12, 31);
        public virtual DateTime RetestDate
        {
            get { return _RetestDate; }
            set
            {
                _RetestDate = value;
                OnPropertyChanged("RetestDate");
            }
        }
        public virtual string DispRetestDate
        {
            get
            {
                return (RetestDate.Year == 2050) ? "" : RetestDate.ToString("MM/dd/yyyy");
            }
        }
        private bool _Disposal = false;
        public virtual bool Disposal
        {
            get { return _Disposal; }
            set
            {
                _Disposal = value;
                OnPropertyChanged("Disposal");
            }
        }

        private bool _PriceChecked = false;
        public virtual bool PriceChecked
        {
            get { return _PriceChecked; }
            set
            {
                _PriceChecked = value;
                OnPropertyChanged("PriceChecked");
            }
        }

        public virtual string diffStr(MESRawMaterial oldObj)
        {
            try
            {
                if (oldObj == null) return "";
                string rtnStr = "";
                if (ReceivingNo.Equals(oldObj.ReceivingNo) == false)
                    rtnStr += "[ReceivingNo:" + oldObj.ReceivingNo + "=>" + ReceivingNo + "]";
                if (RawMaterialName.Equals(oldObj.RawMaterialName) == false)
                    rtnStr += "[RawMaterialName:" + oldObj.RawMaterialName + "=>" + RawMaterialName + "]";
                if (RMSpec != null && oldObj.RMSpec != null && RMSpec.ID != oldObj.RMSpec.ID)
                    rtnStr += "[SpecCode:" + oldObj.RMSpec.SpecCode + "=>" + RMSpec.SpecCode + "]";

                if (!((RawMaterialLotNumber == null && oldObj.RawMaterialLotNumber == null) ||
                    (RawMaterialLotNumber != null && oldObj.RawMaterialLotNumber != null && RawMaterialLotNumber.Equals(oldObj.RawMaterialLotNumber) == true))
                    )
                {
                    rtnStr += "[Lot#:" + ((oldObj.RawMaterialLotNumber != null) ? oldObj.RawMaterialLotNumber : "") + "=>" +
                        ((RawMaterialLotNumber != null) ? RawMaterialLotNumber : "") + "]";
                }

                if (SupplyWeightBox1 != oldObj.SupplyWeightBox1)
                    rtnStr += "[SupplyWeightBox1:" + oldObj.SupplyWeightBox1 + "=>" + SupplyWeightBox1 + "]";
                //if (oldObj.BagWeight != null && BagWeight != null && BagWeight.Value != oldObj.BagWeight.Value)
                //  rtnStr += "[BagWeight:" + oldObj.BagWeight.Value + "=>" + BagWeight + "]";
                if (BagWeight != oldObj.BagWeight)
                    rtnStr += "[BagWeight:" + oldObj.BagWeight + "=>" + BagWeight + "]";
                if (this.QuatityofBox1 != oldObj.QuatityofBox1)
                    rtnStr += "[QuatityofBox1#:" + oldObj.QuatityofBox1 + "=>" + QuatityofBox1 + "]";

                if (!((StockLastYear == null && oldObj.StockLastYear == null) ||
                    (StockLastYear != null && oldObj.StockLastYear != null && StockLastYear.Value == oldObj.StockLastYear.Value))
                    )
                {
                    rtnStr += "[StockLastYear:" + ((oldObj.StockLastYear != null) ? oldObj.StockLastYear.Value.ToString() : "") + "=>" +
                        ((StockLastYear != null) ? StockLastYear.Value.ToString() : "") + "]";
                }

                if (Status != oldObj.Status)
                {
                    if (oldObj.Status == 1 && Status == 2)
                        rtnStr += "[Status:" + oldObj.Status + "=>" + Status + ", Sampled]";
                    else
                        rtnStr += "[Status:" + oldObj.Status + "=>" + Status + "]";
                }
                if (UnitCost != oldObj.UnitCost)
                    rtnStr += "[UnitCost:" + oldObj.UnitCost + "=>" + UnitCost + "]";
                if (InvoicePrice != oldObj.InvoicePrice)
                    rtnStr += "[InvoicePrice:" + oldObj.InvoicePrice + "=>" + InvoicePrice + "]";
                if (BuyingUnit != null && oldObj.BuyingUnit != null)
                {
                    if (BuyingUnit.ID != oldObj.BuyingUnit.ID)
                        rtnStr += "[BuyingUnit:" + oldObj.BuyingUnit.AbbrName + "=>" + BuyingUnit.AbbrName + "]";
                }
                else if (BuyingUnit != null && oldObj.BuyingUnit == null)
                    rtnStr += "[BuyingUnit: N/A =>" + BuyingUnit.AbbrName + "]";
                else if (BuyingUnit == null && oldObj.BuyingUnit != null)
                    rtnStr += "[BuyingUnit: " + oldObj.BuyingUnit.AbbrName + "=> N/A]";

                if (!((Currency1 == null && oldObj.Currency1 == null) ||
                    (Currency1 != null && oldObj.Currency1 != null && Currency1.Equals(oldObj.Currency1) == true))
                    )
                {
                    rtnStr += "[Currency:" + ((oldObj.Currency1 != null) ? oldObj.Currency1 : "") + "=>" +
                        ((Currency1 != null) ? Currency1 : "") + "]";
                }

                if (!((ExchangeRate == null && oldObj.ExchangeRate == null) ||
                    (ExchangeRate != null && oldObj.ExchangeRate != null && ExchangeRate.Value == oldObj.ExchangeRate.Value))
                    )
                {
                    rtnStr += "[ExchangeRate:" + ((oldObj.ExchangeRate != null) ? oldObj.ExchangeRate.Value.ToString() : "") + "=>" +
                        ((ExchangeRate != null) ? ExchangeRate.Value.ToString() : "") + "]";
                }

                if (SalesPrice != oldObj.SalesPrice)
                    rtnStr += "[SalesPrice:" + oldObj.SalesPrice + "=>" + SalesPrice + "]";

                if (Disposal != oldObj.Disposal)
                    rtnStr += "[Disposal:" + oldObj.Disposal + "=>" + Disposal + "]";

                if (RetestDate.ToString("yyyyMMdd").Equals(oldObj.RetestDate.ToString("yyyyMMdd")) == false)
                    rtnStr += "[RetestDate:" + oldObj.RetestDate + "=>" + RetestDate + "]";
                if (!((Density == null && oldObj.Density == null) ||
                    (Density != null && oldObj.Density != null && Density.Value == oldObj.Density.Value))
                    )
                {
                    rtnStr += "[Density:" + ((oldObj.Density != null) ? oldObj.Density.Value.ToString() : "") + "=>" +
                        ((Density != null) ? Density.Value.ToString() : "") + "]";
                }
                if (StockLocation != null && StockLocation.Equals(oldObj.StockLocation) == false)
                    rtnStr += "[StockLocation:" + oldObj.StockLocation + "=>" + StockLocation + "]";
                if (bReleased != oldObj.bReleased)
                    rtnStr += "[Release:" + oldObj.bReleased + "=>" + bReleased + "]";
                return rtnStr;
            }
            catch (Exception ex) { throw ex; }
        }

        public virtual bool bOverExpireDate
        {
            get
            {
                DateTime now = DateTime.Now;
                long lExpireVal = ExpireDate.Year * 10000 + ExpireDate.Month * 100 + ExpireDate.Day;
                long lNowVal = now.Year * 10000 + now.Month * 100 + now.Day;
                return (lNowVal > lExpireVal);
            }
        }
        public virtual bool bNeedRetest
        {
            get
            {
                DateTime now = DateTime.Now;
                long lRetestVal = RetestDate.Year * 10000 + RetestDate.Month * 100 + RetestDate.Day;
                long lNowVal = now.Year * 10000 + now.Month * 100 + now.Day;
                return (lNowVal > lRetestVal);
            }
        }

        private double _InvoicePrice = 0;
        public virtual double InvoicePrice
        {
            get { return _InvoicePrice; }
            set
            {
                _InvoicePrice = value;

                if (BuyingUnit != null)
                {
                    if (BuyingUnit.uType == eUNITTYPE.PO_VOLUME && RMSpec != null)
                        UnitCost = Math.Round(InvoicePrice / (BuyingUnit.Multiply * ((RMSpec.Density != null && RMSpec.Density.HasValue) ? RMSpec.Density.Value : 1)), 2, MidpointRounding.AwayFromZero);
                    else if (BuyingUnit.uType == eUNITTYPE.PO_WEIGHT)
                        UnitCost = Math.Round(InvoicePrice / BuyingUnit.Multiply, 2, MidpointRounding.AwayFromZero);
                    OnPropertyChanged("UnitCost");
                }
                OnPropertyChanged("InvoicePrice");
            }
        }
        public virtual string dispInvoicePrice
        {
            get { return (InvoicePrice == 0) ? "" : "$" + InvoicePrice + "/" + (BuyingUnit != null ? BuyingUnit.AbbrName : "kg"); }
        }

        private UnitType _BuyingUnit = null;
        public virtual UnitType BuyingUnit
        {
            get { return _BuyingUnit; }
            set
            {
                _BuyingUnit = value;

                if (BuyingUnit != null)
                {
                    if (BuyingUnit.uType == eUNITTYPE.PO_VOLUME && RMSpec != null)
                        UnitCost = Math.Round(InvoicePrice / (BuyingUnit.Multiply * ((RMSpec.Density != null && RMSpec.Density.HasValue) ? RMSpec.Density.Value : 1)), 2, MidpointRounding.AwayFromZero);
                    else if (BuyingUnit.uType == eUNITTYPE.PO_WEIGHT)
                        UnitCost = Math.Round(InvoicePrice / BuyingUnit.Multiply, 2, MidpointRounding.AwayFromZero);
                    OnPropertyChanged("UnitCost");
                }
                OnPropertyChanged("BuyingUnit");
            }
        }

        public virtual string dispBuyingUnit
        {
            get { return (BuyingUnit != null) ? BuyingUnit.AbbrName : ""; }
        }
        public virtual double? Density { get; set; }

        // memory object
        IList<RawMaterialDetail> _oRMDetails = new List<RawMaterialDetail>();
        public virtual IList<RawMaterialDetail> oRMDetails
        {
            get { return _oRMDetails; }
            set { _oRMDetails = value; }
        }
    }
}
