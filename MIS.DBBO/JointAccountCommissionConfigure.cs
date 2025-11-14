namespace MIS.DBBO
{
  public class JointAccountCommissionConfigure : SharedCommissionConfigure
  {
    public virtual Employee oJointAccount { get; set; } = null;
    public virtual string JointAccount { get; set; }
    public virtual string JointAccountName
    {
      get => oJointAccount?.ShortName ?? "";
      set { }
    }
    //memory object
    public virtual int refereneCnt { get; set; } = 0;
  }
}
