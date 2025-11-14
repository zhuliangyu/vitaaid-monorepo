using MyHibernateUtil;
using MySystem.Base;
using System;
using System.Collections.Generic;

namespace POCO
{
  [Serializable]
  public class Step : POCOBase, IMESSequence
  {
    public Step() { }
    public virtual int ID { get; set; }
    public override int getID() => ID;
    public virtual int Sequence { get; set; }
    public virtual Process Process { get; set; }
    public virtual Recipe Recipe { get; set; }
    public virtual EQ EQ { get; set; }
    //public virtual int Sequence { get; set; }
    public virtual string Description { get; set; }
    /*
    private IList<SPC> _SPCs = new List<SPC>();
    public virtual IList<SPC> SPCs {
        get { return _SPCs; }
        set { _SPCs = value; }
    }
     */
    private NSeqList<SPC> _SPCs;
    public virtual NSeqList<SPC> SPCs
    {
      get
      {
        if (_SPCs == null)
          _SPCs = new NSeqList<SPC>();
        return _SPCs;
      }
      set { _SPCs = value; }
    }
  }
}
