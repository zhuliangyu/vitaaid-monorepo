using System;
using MyHibernateUtil;

namespace POCO
{
  [Serializable]
  public class EDCData : POCOBase
  {
    public EDCData() { }
    public virtual int ID { get; set; }
    public virtual EQ oEQ { get; set; }
    public virtual string EQName { get => oEQ.Name; set { } }
    public virtual string ItemType { get; set; }
    public virtual string EDCName { get; set; }
    public virtual string EDCValue { get; set; }
    public virtual long TimeStamp { get; set; }
    public virtual string CreatedID { get; set; }
    public virtual DateTime CreatedDate { get; set; }

    public override int getID()
    {
      return ID;
    }
  }
}
