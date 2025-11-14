using System;


namespace POCO
{
    [Serializable]
    public class MESPackageMaterial
    {
        public virtual int ID { get; set; }
        public virtual string PackageCode { get; set; }
        public virtual ePACKAGETYPE PackageType { get; set; }

        public virtual string Name { get; set; }
        public virtual string Memo { get; set; }
        private string _Version = "1";
        public virtual string Version { get { return _Version; } set { _Version = value; } }
        private bool _Disposal = false;
        public virtual bool Disposal { get { return _Disposal; } set { _Disposal = value; } }
        public virtual AttachedFile LabelFile { get; set; }
        private double? _Weight = 0;
        public virtual double? Weight { get { return _Weight; } set { _Weight = (value == null) ? 0 : value; } }
        public virtual Supplier Supplier { get; set; }

        //        public virtual double? Weight { get; set; }
        public virtual MESPackageMaterial ShallowCopy()
        {
            return (MESPackageMaterial)this.MemberwiseClone();
        }
    }
}
