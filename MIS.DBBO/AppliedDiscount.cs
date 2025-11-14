using MyHibernateUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIS.DBBO
{
  public class AppliedDiscount: DataElement
  {
    public virtual int ID { get; set; }
    public override int getID()
    {
      return ID;
    }
    public virtual DiscountProgram oDiscountProgram { get; set; }
    public virtual CustomerAccount oCustomer { get; set; }
    public virtual string CustomerCode { get => oCustomer?.CustomerCode ?? ""; set { } }
    public virtual VAOrder oOrder { get; set; }
    public virtual DateTime? AppliedDate { get; set; }
    public virtual bool bApplied { get; set; } = true;
  }
}
