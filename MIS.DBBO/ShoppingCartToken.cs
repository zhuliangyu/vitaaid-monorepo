using MyHibernateUtil;

namespace MIS.DBBO
{
    public class ShoppingCartToken : DataElement
    {
    public virtual int ID { get; set; }
    public override int getID() => ID;
    public virtual string AppName { get; set; }
    public virtual string RefreshToken { get; set; }
    public virtual string Account { get; set; }
    public virtual string CustomerCode { get; set; }
    public virtual string StoreName { get; set; }
    public virtual int Expired { get; set; }
    public virtual string HostName { get; set; }
  }
}
