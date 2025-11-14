using MyHibernateUtil;
using MySystem.Base;
using System;
using System.Collections.Generic;

namespace POCO
{
  [Serializable]
  public class SPC : POCOBase, IMESSequence
  {
    public SPC() { }
    public virtual int ID { get; set; }
    public override int getID() => ID;
    public virtual int Sequence { get; set; }
    /*
    private IList<Step> _Steps = new List<Step>();
    public virtual IList<Step> Steps
    {
        get { return _Steps; }
        set { _Steps = value; }
    }
    */
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
    //public virtual int Sequence { get; set; }
    public virtual int MultiTimes { get; set; }
    public virtual int ValueMethod { get; set; } /*1: keyin, 2:Calculated */
    public virtual string Name { get; set; }

    private NSeqList<SPCItem> _SPCItems;
    public virtual NSeqList<SPCItem> SPCItems
    {
      get
      {
        if (_SPCItems == null)
          _SPCItems = new NSeqList<SPCItem>();
        return _SPCItems;
      }
      set { _SPCItems = value; }
    }
  }
}
