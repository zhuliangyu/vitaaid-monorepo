using MyHibernateUtil;
using MySystem.Base;
using System;
using System.Collections.Generic;


namespace POCO
{
  [Serializable]
  public class Process : DataElement
  {
    public Process() { }
    public virtual int ID { get; set; }
    public override int getID() => ID;
    public virtual Route Route { get; set; }
    public virtual string Sequence { get; set; }
    public virtual int LoopCount { get; set; }
    public virtual int Skippable { get; set; }
    public virtual ePROCESSTYPE ProcessType { get; set; }
    public virtual string ProcessCode { get; set; }

    private NSeqList<Step> _Steps;
    public virtual NSeqList<Step> Steps
    {
      get
      {
        if (_Steps == null)
          _Steps = new NSeqList<Step>();
        return _Steps;
      }
      set { _Steps = value; }
    }


    private IList<Process> _SubProcesses = new List<Process>();
    public virtual IList<Process> SubProcesses
    {
      get { return _SubProcesses; }
      set { _SubProcesses = value; }
    }

    private IList<TempOperand> _TempOperands = new List<TempOperand>();
    public virtual IList<TempOperand> TempOperands
    {
      get { return _TempOperands; }
      set { _TempOperands = value; }
    }
  }
}
