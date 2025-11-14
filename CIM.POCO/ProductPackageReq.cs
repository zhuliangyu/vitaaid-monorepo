using MyHibernateUtil;
using System;
using System.ComponentModel;
using PropertyChanged;

namespace POCO
{
    [Serializable]
    public class ProductPackageReq : DataElement
    {
        public virtual int ID { get; set; }
        public override int getID() => ID;
        public virtual MESPackageSpec oPackageSpec { get; set; }
        public virtual string PackageCode { get => oPackageSpec?.PackageCode ?? ""; set { } }
        public virtual ePACKAGETYPE PackageType { get => oPackageSpec?.PackageType ?? ePACKAGETYPE.CAPSULE; set { } }
        public virtual string PackageName { get => oPackageSpec?.Name ?? ""; set { } }
        public virtual string CategoryCode { get => oPackageSpec?.PackageCode?.Substring(0, 6) ?? ""; set { } }
        public virtual int GroupNo { get; set; }
        public virtual Production oProduct { get; set; }
        public virtual bool SpecificSupplier { get; set; }

        public virtual string ScannedBarCode { get; set; }

        // memory object, PackageCostDetail
        public virtual object oPackageCost { get; set; }

    }
}
