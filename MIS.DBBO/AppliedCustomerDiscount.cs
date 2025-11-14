using System;
using System.Collections.Generic;
using System.Linq;
using MyHibernateUtil;
using PropertyChanged;
using MySystem.Base.Extensions;

namespace MIS.DBBO
{
  public class AppliedCustomerDiscount : AppliedDiscount
  {
    public virtual bool IsActive { get; set; } = true;
    public virtual CustomerDiscount oRefCustomerDiscount { get; set; }
    public virtual int UsedCount { get; set; }

    public AppliedCustomerDiscount() : base() { }
    public AppliedCustomerDiscount(CustomerDiscount oCD, VAOrder oOrder)//DateTime? _AppliedDate)
    {
      oRefCustomerDiscount = oCD;
      oCustomer = oCD.oCustomer;
      CustomerCode = oCD.CustomerCode;
      oDiscountProgram = oCD.oDiscountProgram;
      UsedCount = oCD.UsedCount;
      IsActive = oCD.IsActive;
      AppliedDate = oOrder.InvoiceDate;
      this.oOrder = oOrder;
    }
  }
}
