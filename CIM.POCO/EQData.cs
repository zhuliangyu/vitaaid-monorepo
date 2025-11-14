using System;
using System.Collections.Generic;
using MyHibernateUtil;

namespace POCO
{
  [Serializable]
  public class EQData : DataElement
  {
    public EQData() { }
    public virtual int ID { get; set; }
    public virtual EQ oEQ { get; set; }
    public virtual string EQName { get => oEQ?.Name ?? ""; set { } }
    public virtual string DataName { get; set; }
    public virtual string DataValue { get; set; }

    public override int getID()
    {
      return ID;
    }
  }
}
