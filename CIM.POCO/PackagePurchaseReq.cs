using MyHibernateUtil;
using System;
using System.ComponentModel;
using PropertyChanged;

namespace POCO
{
    [Serializable]
    public class PackagePurchaseReq : DataElement
    {
        public virtual int ID { get; set; }
        public override int getID() => ID;
        public virtual string OrderNo { get; set; }
        public virtual string OldOrderNo { get; set; }
        public virtual ePurchaseStatus Status { get; set; }
        public virtual ePACKAGETYPE PackageType { get; set; }
        public virtual string CategoryCode { get; set; }
        private MESPackageSpec _PMSpec;
        public virtual MESPackageSpec PMSpec
        {
            get { return _PMSpec; }
            set
            {
                _PMSpec = value;
                OnPropertyChanged("displaySupplier");
                OnPropertyChanged("ItemDescriptionForPO");
            }
        }
        private double _SizePerBox = 0;
        public virtual double SizePerBox
        {
            get { return _SizePerBox; }
            set
            {
                _SizePerBox = value;
                UpdateTotalPrice();
                OnPropertyChanged("SizePerBox");
            }
        }
        private int _BuyingBox = 0;
        public virtual int BuyingBox
        {
            get { return _BuyingBox; }
            set
            {
                _BuyingBox = value;
                UpdateTotalPrice();
                OnPropertyChanged("BuyingBox");
            }
        }
        public virtual double BuyingPiece => SizePerBox * BuyingBox;
        public virtual string dispBuyingUnit => PackageType switch
                                                {
                                                    ePACKAGETYPE.BOTTLE    => SizePerBox.ToString(),
                                                    ePACKAGETYPE.CAP       => SizePerBox.ToString(),
                                                    ePACKAGETYPE.CAPSULE   => SizePerBox.ToString() + " M",
                                                    ePACKAGETYPE.DESICCANT => SizePerBox.ToString(),
                                                    ePACKAGETYPE.LABEL     => SizePerBox.ToString(),
                                                    ePACKAGETYPE.SCOOPS    => SizePerBox.ToString(),
                                                    _                      => SizePerBox.ToString()
                                                };

        public virtual string dispBuyingQty => (PackageType == ePACKAGETYPE.CAPSULE) ? (BuyingPiece * 1000).ToString() : BuyingPiece.ToString();
        public virtual UnitType CostCurrency { get; set; }
        private double _UnitCost = 0;
        public virtual double UnitCost
        {
            get { return _UnitCost; }
            set
            {
                _UnitCost = value;
                UpdateTotalPrice();
                OnPropertyChanged("UnitCost");
                OnPropertyChanged("TotalPrice");
            }
        }
        private void UpdateTotalPrice()
        {
            TotalPrice = Math.Round(SizePerBox * BuyingBox * UnitCost, 2, MidpointRounding.AwayFromZero);
            //if (PackageType == ePACKAGETYPE.CAPSULE)
            //    TotalPrice /= 1000.0;
        }
        public virtual double TotalPrice { get; set; } = 0;
        public virtual PackagePurchaseOrder oPO { get; set; }
        public virtual DateTime PurchaseDate { get; set; } = new DateTime(2050, 12, 31);
        private double _ReceivedWeight = 0;
        public virtual double ReceivedWeight { get; set; } = 0;
        private double _ReceivedQty = 0;
        public virtual double ReceivedQty { get; set; }
        public virtual string dispReceivedQty => (PackageType == ePACKAGETYPE.CAPSULE) ? ReceivedQty.ToString() + " M" : ReceivedQty.ToString();
        public virtual string dispTotalPrice => TotalPrice.ToString("C2");
        public virtual string displayPurchaseDate => (PurchaseDate.Year == 2050) ? "" : PurchaseDate.ToShortDateString();
        public virtual bool bOverNormalReceiving => (oPO != null) ? DateTime.Now > oPO.DeliveryDate : DateTime.Now.Subtract(PurchaseDate).Days >= 30 ? true : false;
        public virtual bool bOverWarningReceiving => DateTime.Now.Subtract(PurchaseDate).Days >= 90 ? true : false;
        public virtual string displaySupplier => PMSpec?.Supplier?.Name ?? "";
        public virtual string ItemDescriptionForPO => PMSpec?.Name ?? "";
        public virtual string dispUnitCost => UnitCost.ToString("C2");
        public virtual bool bDelete { get; set; } = false;
    }
}
