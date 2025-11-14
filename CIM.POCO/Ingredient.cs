using MyHibernateUtil;
using System;


namespace POCO
{
  [Serializable]
  public class Ingredient : POCOBase
  {
    public Ingredient() { }
    public virtual int ID { get; set; }
    public override int getID() => ID;
    public virtual string Name { get; set; }
  }
}
