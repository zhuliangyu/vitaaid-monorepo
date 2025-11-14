using MyHibernateUtil;

namespace POCO
{
  public class EQAssignment : DataElement
  {
    public virtual int ID { get; set; }
    public virtual string ProcessOPID { get; set; }
    public virtual Production oProduction { get; set; }
    public virtual string ProductCode { get => oProduction?.Code; set { } }
    public virtual int ProductVersion { get => oProduction?.Version ?? 0; set { } }
    public virtual string ProductName { get => oProduction?.Name; set { } }
    public virtual EQ oEQ { get; set; }
    public virtual string EQName { get => oEQ?.Name ?? ""; set { } }
    public virtual string EQTypeName { get => oEQ?.EQType.ToString() ?? ""; set { } }
    public virtual EQRecipe oEQRecipe { get; set; }
    public virtual string RecipeName { get => oEQRecipe?.RecipeName ?? ""; set { } }
    public override int getID()
    {
      return ID;
    }
  }
}
