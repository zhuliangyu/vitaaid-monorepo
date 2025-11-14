using MyHibernateUtil;
using System;

namespace POCO
{
  [Serializable]
  public class RMApplyLog : POCOBase
  {
    public virtual int ID { get; set; }
    public override int getID() => ID;
    public virtual string SpecCode { get; set; }
    public virtual string ReceivingNo { get; set; }
    public virtual string LotNo { get; set; }
    public virtual string ProductCode { get; set; }
    public virtual int FormulationItemID { get; set; }
    public virtual int FormulationItemSeq { get; set; }
    public virtual string LogType { get; set; }
    public virtual double ReserveWeight { get; set; }
    public virtual double RollbackWeight { get; set; }
    public virtual double BeforeStockWeight { get; set; }
    public virtual double AfterStockWeight { get; set; }
    public virtual double BeforeReserveWeight { get; set; }
    public virtual double AfterReserveWeight { get; set; }
    public virtual double ApplyWeight { get; set; }
    public virtual DateTime CreatedDate { get; set; }
    public virtual string CreatedID { get; set; }
    public virtual string Comment { get; set; }
    public virtual string GroupCode { get; set; }
  }
}
