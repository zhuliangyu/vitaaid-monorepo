using MyHibernateUtil;
using System;

namespace POCO
{
  [Serializable]
  public class MedicineIngredientUsage : POCOBase
  {
    public MedicineIngredientUsage() { }
    public virtual int ID { get; set; }
    public override int getID() => ID;
    public virtual int LotID { get; set; }
    public virtual string LotNo { get; set; }
    public virtual string Code { get; set; }
    public virtual string Name { get; set; }
    public virtual int DailyDose { get; set; }
    public virtual string Specs { get; set; }
    public virtual double DailyAmount { get; set; }
    public virtual string DailyAmountDesc { get; set; }
    public virtual string CategoryName { get; set; }
    public virtual string CategoryCode { get; set; }
  }
}
