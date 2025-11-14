using MyHibernateUtil;
using MySystem.Base;
using System;
using System.Collections.Generic;

namespace POCO
{
  [Serializable]
  public class Operation : POCOBase, IMESSequence
  {
    public Operation() { }
    public virtual int ID { get; set; }
    public override int getID() => ID;
    public virtual int Sequence { get; set; }
    public virtual eOPERATION? OperationType { get; set; }
    public virtual string Parameter1 { get; set; }
    public virtual string Parameter2 { get; set; }
    public virtual string Note { get; set; }
    public virtual int? MeshSize { get; set; }
    public virtual eKEEPIN? KeepInContainer { get; set; }

    private NSeqList<Operand> _Operands;
    public virtual NSeqList<Operand> Operands
    {
      get
      {
        if (_Operands == null)
          _Operands = new NSeqList<Operand>();
        return _Operands;
      }
      set { _Operands = value; }
    }
  }
}
