using MyHibernateUtil;
using System;


namespace POCO
{
  [Serializable]
  public class FFI_PRReq : POCOBase
  {
    public virtual int ID { get; set; }
    public override int getID() => ID;
    public FFI_PRReq() { }
    public FFI_PRReq(FabFormulationItem ffi, PurchaseRMReqInfo pi, VirtualLotInfo vli)
    {
      oFabFormulationItem = ffi;
      oPurchaseRMReqInfo = pi;
      oVirtualLotInfo = vli;
    }
    public virtual FabFormulationItem oFabFormulationItem { get; set; }
    public virtual PurchaseRMReqInfo oPurchaseRMReqInfo { get; set; }
    public virtual VirtualLotInfo oVirtualLotInfo { get; set; }

    public virtual EQ ShallowCopy()
    {
      return (EQ)this.MemberwiseClone();
    }
  }
}
