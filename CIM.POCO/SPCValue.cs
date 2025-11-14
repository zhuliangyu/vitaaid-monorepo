using MyHibernateUtil;
using System;


namespace POCO
{
  [Serializable]
  public class SPCValue : POCOBase
  {
    public virtual int ID { get; set; }
    public virtual SPCSheet oSPCSheet { get; set; }
    public virtual double Value { get; set; } = 0.0;
    public virtual DateTime CreatedDate { get; set; } = DateTime.Now;

    public override int getID()
    {
      return ID;
    }

  }
}
