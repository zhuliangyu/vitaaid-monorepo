using System;

namespace POCO
{
    [Serializable]
    public class ProductList_v
    {
        public virtual int MESPackageSpecID { get; set; }
        public virtual string ProductCode { get; set; }
        public virtual string Name { get; set; }
    }
}
