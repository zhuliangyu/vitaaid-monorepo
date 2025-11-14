using MyHibernateUtil;
using System;

namespace MIS.DBBO
{
  [Serializable]
  public class ProductApply_v : POCOBase
  {
    public virtual int ID { get; set; }
    public override int getID() => ID;
    public virtual string CustomerCode { get; set; }
    public virtual string CustomerName { get; set; }
    public virtual string CustomerEmail1 { get; set; }
    public virtual string CustomerTel1 { get; set; }
    public virtual string ProductCode { get; set; }
    //public virtual string MESProductCode { get; set; }
    public virtual string ProductName { get; set; }
    public virtual int ApplyCountBottle { get; set; }
    public virtual DateTime ApplyDate { get; set; }
    public virtual string LotInfo { get; set; }
  }
}
