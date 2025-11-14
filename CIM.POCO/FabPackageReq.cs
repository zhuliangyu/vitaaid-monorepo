using MyHibernateUtil;
using System;
using System.ComponentModel;
using PropertyChanged;

namespace POCO
{
    [Serializable]
    public class FabPackageReq : DataElement
    {
        public virtual int ID { get; set; }
        public override int getID() => ID;
        public virtual MESPackageSpec oPackageSpec { get; set; }
        public virtual string PackageCode { get => oPackageSpec?.PackageCode ?? ""; set { } }
        public virtual string CategoryCode { get => oPackageSpec?.PackageCode?.Substring(0, 6) ?? ""; set { } }
        public virtual ePACKAGETYPE PackageType { get => oPackageSpec?.PackageType ?? ePACKAGETYPE.CAPSULE; set { } }
        public virtual string PackageName { get => oPackageSpec?.Name ?? ""; set { } }
        public virtual int GroupNo { get; set; }
        public virtual int Qty { get; set; }
        public virtual Lot oLot { get; set; }
        public virtual bool FixedQuantity { get; set; }
        public virtual bool SpecificSupplier { get; set; }
        public virtual string ScannedBarCode { get; set; }
        public virtual string dispPkgType
        {
            get
            {
                switch (PackageType)
                {
                    case ePACKAGETYPE.BOTTLE:
                        return "BOTTLE";
                    case ePACKAGETYPE.CAP:
                        return "CAP";
                    case ePACKAGETYPE.CAPSULE:
                        return "CAPSULE";
                    case ePACKAGETYPE.DESICCANT:
                        return "DESICCANT";
                    case ePACKAGETYPE.LABEL:
                        return "LABEL";
                    case ePACKAGETYPE.SCOOPS:
                        return "SCOOPS";
                    default:
                        return "UNKNOWN";
                }
            }
        }

        public virtual double? dStockCount { get; set; } = null;
        // memory data
        public virtual bool bEnough { get; set; } = false;
    }
}
