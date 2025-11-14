using MyHibernateUtil;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using PropertyChanged;

namespace WebDB.DBBO
{
    [Serializable]
    public class ProductImage : DataElement
    {
        public virtual int ID { get; set; }
        public override int getID() => ID;
        [JsonIgnore]
        public virtual Product oProduct { get; set; }
        private string _productCode = "";
        public virtual string ProductCode { get => oProduct?.ProductCode ?? _productCode; set { _productCode = value; } }
        public virtual bool FrontImage { get; set; } = true;
        public virtual string ImageName { get; set; }
        public virtual string LargeImageName { get; set; }
        public virtual string Flag { get; set; } = "";
        public virtual byte[] NormalImage { get; set; }
        public virtual byte[] LargeImage { get; set; }
        public virtual double Width { get; set; }
        public virtual double Height { get; set; }
        public virtual double LargeWidth { get; set; }
        public virtual double LargeHeight { get; set; }
    }
}
