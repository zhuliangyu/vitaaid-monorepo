using System;
using System.Collections.Generic;
using System.Linq;
using MyHibernateUtil;
using PropertyChanged;
using MySystem.Base.Extensions;
using System.Diagnostics;

namespace MIS.DBBO
{
  public class AppliedCartDiscount : AppliedDiscount
  {
    public virtual bool IsActive { get; set; } = true;
    public virtual CustomerDiscount oRefCustomerDiscount { get; set; }

    public AppliedCartDiscount() : base() { }
    public AppliedCartDiscount(CustomerDiscount oCD, VAOrder oOrder)//DateTime? _AppliedDate)
    {
      oRefCustomerDiscount = oCD;
      oCustomer = oCD.oCustomer;
      CustomerCode = oCD.CustomerCode;
      oDiscountProgram = oCD.oDiscountProgram;
      IsActive = oCD.IsActive;
      AppliedDate = oOrder.InvoiceDate;// _AppliedDate;
      this.oOrder = oOrder;
      Debug.Assert(this.oDiscountProgram.DiscountType == eDISCOUNTTYPE.FIXEDCART_PER);
    }
  }
}
