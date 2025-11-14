using System;


namespace POCO
{
    [Serializable]
    public class FormulationPackageItem
    {
        public virtual int ID { get; set; }
        public virtual Formulation Formulation { get; set; }
        public virtual MESPackageMaterial PackageMaterial { get; set; }
        private bool _isUsedOver = false;
        public virtual bool isUsedOver { get { return _isUsedOver; } set { _isUsedOver = value; } }

        public virtual FormulationPackageItem ShallowCopy()
        {
            return (FormulationPackageItem)this.MemberwiseClone();
        }
    }
}
