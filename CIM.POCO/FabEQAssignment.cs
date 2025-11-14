using MyHibernateUtil;

namespace POCO
{
  public class FabEQAssignment : DataElement
  {
    public virtual int ID { get; set; }
    public virtual string ProcessOPID { get; set; }
    public virtual FabProduction oFabProduction { get; set; }
    public virtual string ProductCode { get => oFabProduction?.Code; set { } }
    public virtual int ProductVersion { get => oFabProduction?.Version ?? 0; set { } }
    public virtual string ProductName { get => oFabProduction?.Name; set { } }
    public virtual EQ oEQ { get; set; }
    public virtual string EQName { get => oEQ?.Name; set { } }
    public virtual string EQTypeName { get => oEQ?.EQType.ToString() ?? ""; set { } }
    public virtual EQRecipe oEQRecipe { get; set; }
    public virtual string RecipeName { get => oEQRecipe?.RecipeName ?? ""; set { } }
    public virtual Lot oLot { get; set; }
    public virtual string LotNo { get => oLot?.No ?? ""; set { } }
    public override int getID()
    {
      return ID;
    }
  }
}
