using MyHibernateUtil;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebDB.DBBO
{
  public class OrderItemData: DataElement
  {
    public virtual int ID { get; set; }
    public override int getID() => ID;
    public virtual string PONo { get; set; } = "";
    public virtual string Code { get; set; } = "";
    public virtual string Name { get; set; } = "";
    public virtual int Qty { get; set; }
    public virtual decimal Price { get; set; }
    public virtual double Discount { get; set; }
    public virtual string DiscountName { get; set; } = "";
    public virtual decimal Amount { get; set; }
    public virtual string ItemType { get; set; } = "";
    [JsonIgnore]
    public virtual OrderData oOrderData { get; set; }
  }
}
