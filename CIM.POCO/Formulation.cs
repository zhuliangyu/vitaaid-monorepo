using MyHibernateUtil;
using MySystem.Base;
using System;
using System.ComponentModel;


namespace POCO
{
  [Serializable]
  public class Formulation : POCOBase
  {
    public Formulation() { }
    public virtual int ID { get; set; }
    public override int getID() => ID;
    //public virtual Production Production { get; set; }
    //public virtual string ProductionCode { get; set; }

    public virtual string FormulationCode { get; set; }

    private NSeqList<FormulationItem> _Items;
    public virtual NSeqList<FormulationItem> Items
    {
      get
      {
        if (_Items == null)
          _Items = new NSeqList<FormulationItem>();
        return _Items;
      }
      set { _Items = value; }
    }
  }
}
