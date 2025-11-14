using System;
using System.ComponentModel;
using MyHibernateUtil;
using PropertyChanged;

namespace WebDB.DBBO
{
    [Serializable]
    public class ProductCodeInfo : DataElement
    {
        public ProductCodeInfo() { }
        public virtual int ID { get; set; }
        public override int getID() => ID;
        public virtual string ProductCode { get; set; }
        public virtual string FormulationCode { get; set; }
        public virtual bool IsOEM { get; set; } = false;
        public virtual int Servings { get; set; } = 0;
        public virtual int SampleServings { get; set; } = 0;

        public virtual bool TemperatureSensitive { get; set; } = false;
    }
}
