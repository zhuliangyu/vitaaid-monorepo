using MyHibernateUtil;
using MySystem.Base;
using System;
using System.Collections.Generic;

namespace POCO
{
  [Serializable]
  public class Operand : POCOBase, IMESSequence
  {
    public virtual int ID { get; set; }
    public override int getID() => ID;
    public virtual int Sequence { get; set; }
    public virtual TempOperand TempOperand { get; set; }
    public virtual RawMaterialSpec RawMaterialOperand { get; set; }
    public virtual string Parameter { get; set; }
    public virtual int? Proportions { get; set; }
    public virtual int? MeshSize { get; set; }
    public virtual eOPERAND OperandType { get; set; }
  }
}
