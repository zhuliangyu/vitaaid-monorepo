using MyHibernateUtil;
using System;
using System.Collections.Generic;


namespace POCO
{
  [Serializable]
  public class Route : DataElement
  {
    public Route() { }
    public virtual int ID { get; set; }
    public override int getID() => ID;
    public virtual Production Production { get; set; }
    public virtual string ProductionCode { get; set; }

    private IList<Process> _Processes = new List<Process>();
    public virtual IList<Process> Processes
    {
      get { return _Processes; }
      set { _Processes = value; }
    } 
  }
}
