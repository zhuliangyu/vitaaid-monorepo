using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using MyHibernateUtil;
using PropertyChanged;
namespace POCO
{
    [Serializable]
    public class PackagePurchaseOrder : DataElement
    {
        public virtual int ID { get; set; }
        public override int getID() => ID;
        public virtual string PONo { get; set; }
        public virtual ePurchaseStatus Status { get; set; } = ePurchaseStatus.PROCESSING;
        public virtual CompanyAbbreviation oVendor { get; set; }
        private DateTime _PODate = new DateTime(2050, 12, 31);
        public virtual DateTime PODate
        {
            get => _PODate; 
            set
            {
                _PODate = value;
                DeliveryDate = value.AddDays(14);
                if (oVendor?.CompanyCountry?.Equals("CANADA") == true)
                    DeliveryDate = value.AddDays(7);
                OnPropertyChanged("PODate");
                OnPropertyChanged("DeliveryDate");
            }
        }
        public virtual string displayPODate => (PODate.Year == 2050) ? "" : PODate.ToShortDateString();
        public virtual string ShippingAddr { get; set; } = "";

        public virtual string ShippingMethod { get; set; } = "";

        private double _SubTotal = 0;
        public virtual double SubTotal
        {
            get { return _SubTotal; }
            set
            {
                _SubTotal = value;
                TotalCost = _SubTotal + ShippingCost + OtherFees;
                OnPropertyChanged("SubTotal");
                OnPropertyChanged("TotalCost");
            }
        }

        private double _ShippingCost = 0;
        public virtual double ShippingCost
        {
            get { return _ShippingCost; }
            set
            {
                _ShippingCost = value;
                TotalCost = SubTotal + _ShippingCost + OtherFees;
                OnPropertyChanged("ShippingCost");
                OnPropertyChanged("TotalCost");
            }
        }

        public virtual double TotalCost { get; set; }

        public virtual string PaymentTerm { get; set; } = "30 DAYS";
        public virtual DateTime DeliveryDate { get; set; } = new DateTime(2050, 12, 31);
        public virtual string displayDeliveryDate => (DeliveryDate.Year == 2050) ? "" : DeliveryDate.ToShortDateString();
        private double _OtherFees = 0;
        public virtual double OtherFees
        {
            get { return _OtherFees; }
            set
            {
                _OtherFees = value;
                TotalCost = SubTotal + ShippingCost + _OtherFees;
                OnPropertyChanged("OtherFees");
                OnPropertyChanged("TotalCost");
            }
        }
        public virtual string Comment { get; set; } = "Please attach CERTIFICATE OF ANALYSIS and expired date not less than 18 months.";
        public virtual string TrackingNo { get; set; }

        public virtual bool ShippingMethodConfirmed { get; set; } = false;
        public virtual bool ShippingAddressConfirmed { get; set; } = false;
        public virtual bool ETAConfirmed { get; set; } = false;
        public virtual void CalculatorSubTotal()
        {
            SubTotal = PurchaseReqList?.Sum(x => x.TotalPrice) ?? 0;
        }
        public virtual IList<PackagePurchaseReq> PurchaseReqList { get; set; } = new List<PackagePurchaseReq>();
        public virtual string ShippingMethodLabel => "Shipping Method";

        public virtual string PDFFile { get; set; }
    }
}
