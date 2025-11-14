using System;
using MyHibernateUtil;

namespace MIS.DBBO
{
  [Serializable]
  public class ShippingPolicy : DataElement
  {
    public virtual int ID { get; set; }
    public virtual string PolicyName { get => oSPMaster?.PolicyName ?? ""; set { } }
    public virtual string ParameterName { get; set; }
    public virtual double ParameterValue { get; set; }
    public virtual string ParameterStringValue { get; set; }
    public virtual DateTime ParameterDateValue { get; set; }
    public virtual ShippingPolicyMaster oSPMaster { get; set; }
    public override int getID()
    {
      return ID;
    }
  }
}
