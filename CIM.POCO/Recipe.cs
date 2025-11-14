using MyHibernateUtil;
using MySystem.Base;
using System;
using System.Collections.Generic;


namespace POCO
{
  [Serializable]
  public class Recipe : POCOBase
  {
    public Recipe() { }
    public virtual int ID { get; set; }
    public override int getID() => ID;
    public virtual string Name { get; set; }

    private IList<Step> _Steps = new List<Step>();
    public virtual IList<Step> Steps
    {
      get { return _Steps; }
      set { _Steps = value; }
    }


    private NSeqList<Operation> _Operations;
    public virtual NSeqList<Operation> Operations
    {
      get
      {
        if (_Operations == null)
          _Operations = new NSeqList<Operation>();
        return _Operations;
      }
      set { _Operations = value; }
    }

    public virtual string EQRecipe { get; set; }
  }
}
