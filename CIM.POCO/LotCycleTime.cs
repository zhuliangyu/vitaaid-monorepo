using MyHibernateUtil;
using System;

namespace POCO
{
  [Serializable]
  public class LotCycleTime : POCOBase
  {
    public virtual int LotID { get; set; }
    public override int getID() => LotID;
    public virtual string Code { get; set; }
    public virtual int Size { get; set; }
    public virtual string LotNo { get; set; }
    public virtual string ProductName { get; set; }
    public virtual DateTime LotStart { get; set; }
    public virtual DateTime LotEnd { get; set; }
    public virtual int Days { get; set; }
    public override string ToString() => String.Format("{0}({1})", Days, Size);
  }
}
