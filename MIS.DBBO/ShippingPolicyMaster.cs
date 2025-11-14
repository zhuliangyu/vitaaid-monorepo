using System;
using System.Collections.Generic;
using MyHibernateUtil;
using MySystem.Base.Extensions;
using static MySystem.Base.Extensions.DateTimeExtension;

namespace MIS.DBBO
{
  [Serializable]
  public class ShippingPolicyMaster : DataElement
  {
    public virtual int ID { get; set; }
    public virtual string PolicyType { get; set; }
    public virtual string PolicyName { get; set; }
    public virtual string CouponCode { get; set; }
    public virtual DateTime? StartDate { get; set; }
    public virtual DateTime? ExpiryDate { get; set; }
    public virtual string Comment { get; set; }
    public virtual int Priority { get; set; } = 0;
    public virtual IList<ShippingPolicy> oPolicyContext { get; set; } = new List<ShippingPolicy>();
    public virtual bool IsActive { get; set; } = true;
    public virtual bool IsValidByDate(DateTime date)
    {
      if (StartDate.HasValue && !StartDate.Value.IsNil() && date < StartDate.Value.StartOfDay())
        return false;
      if (ExpiryDate.HasValue && !ExpiryDate.Value.IsNil() && date > ExpiryDate.Value.EndOfDay())
        return false;
      return true;
    }
    public virtual bool IsValidByCouponCode(string Code)
    {
      if (string.IsNullOrWhiteSpace(CouponCode))
        return true;
      return (CouponCode.ToUpper() == (Code?.ToUpper() ?? ""));
    }
    public override int getID()
    {
      return ID;
    }
    //memory object
    public virtual object Tag { get; set; }
  }
}
