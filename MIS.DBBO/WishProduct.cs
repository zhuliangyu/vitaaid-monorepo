using MyHibernateUtil;
using System;

namespace MIS.DBBO
{
  public class WishProduct : DataElement
  {
    public virtual int ID { get; set; }
    public override int getID() => ID;
    public virtual CustomerAccount oCustomer { get; set; }
    public virtual string CustomerCode { get => oCustomer?.CustomerCode ?? ""; set { } }
    public virtual string CustomerName { get; set; }// => oCustomer?.CustomerName ?? ""; set { } }
    public virtual string CustomerEmail { get; set; }// => oCustomer?.CustomerEmail1 ?? ""; set { } }

    public virtual string ProductCode { get; set; }
    public virtual string ProductName { get; set; }
    public virtual int Qty { get; set; }

    public virtual string SendEmailStatus { get; set; } = "";
    public virtual DateTime? SendEmailDate { get; set; }
    public virtual bool bCheck { get; set; } = false; // memory object, UI purpose
    public virtual string Sales { get; set; } // memory object
    public virtual string SalesEmail { get; set; } // memory object
  }
}

