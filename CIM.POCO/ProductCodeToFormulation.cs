using System;
using System.ComponentModel;
using MyHibernateUtil;
using PropertyChanged;

namespace POCO
{
  [Serializable]
  public class ProductCodeToFormulation : DataElement
  {
    public ProductCodeToFormulation() { }
    public virtual int ID { get; set; }
    public override int getID() => ID;
    public virtual string ProductCode { get; set; }
    public virtual string FormulationCode { get; set; }
    public virtual bool IsOEM { get; set; } = false;
    public virtual int Servings { get; set; } = 0;
    public virtual int SampleServings { get; set; } = 0;

    public virtual bool TemperatureSensitive { get; set; } = false;
    public virtual bool CAProduct { get; set; } = true;
    public virtual int ServingSize { get; set; } = 1;
    public virtual string ServingUnit { get; set; } = "Capsule";
    public virtual string sServingSize { get => string.Format("{0} {1}(s)", ServingSize, ServingUnit); }
    public virtual int ServingsPerContainer { get; set; } = 1;
    // memory data
    //public virtual Production oProduction { get; set; }
    public virtual int MESPackageSpecID { get; set; } = 0;
    public virtual string ProductName { get; set; } = "";
    public virtual int FormulationID { get; set; } = 0;
    public virtual string FormulationName { get; set; } = "";
    public virtual string NPN { get; set; } = "";
    public virtual int Version { get; set; }
    public virtual string ProductNameOrFormulationName { get => string.IsNullOrWhiteSpace(ProductName) ? FormulationName : ProductName; }
    public virtual int LabelFileID { get; set; }
    public virtual AttachedFile LabelFile { get; set; } = null;
  }
}
