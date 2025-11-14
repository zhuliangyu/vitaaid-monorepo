using MyHibernateUtil;
using MySystem.Base;
using System;
using System.Collections.Generic;

namespace POCO
{
  [Serializable]
  public class SPCItem : POCOBase, IMESSequence
  {
    public virtual int ID { get; set; }
    public override int getID() => ID;
    public virtual int Sequence { get; set; }
    public virtual string Name { get; set; }
    public virtual eVALUETYPE ValueType { get; set; }
    public virtual UnitType Unit { get; set; }
    public virtual SPC ComputedBySPC { get; set; }
    public virtual string ComputedBySPCItems { get; set; }
    public virtual string ValueOperation { get; set; }
  }
}
