using System;
using System.Collections.Generic;
using MyHibernateUtil;

namespace POCO
{
  [Serializable]
  public class EQ : DataElement
  {
    public EQ() { }
    public virtual int ID { get; set; }
    public virtual string Name { get; set; }
    public virtual string Description { get; set; }
    public virtual string SerialNo { get; set; }
    public virtual string Model { get; set; }
    public virtual string DefaultRecipeID { get; set; }
    public virtual eEQTYPE EQType { get; set; } = eEQTYPE.ENCAPSULATION;
    public virtual int SubType { get; set; } = 1;
    public virtual string Status { get; set; }
    public virtual EQDataTemplate oEQDataTemplate { get; set; }
    public virtual string EQDataTemplateName { get => oEQDataTemplate?.TemplateName ?? ""; set { } }
    public virtual string Info { get => Name + ":" + SerialNo + ":" + EQType.ToString(); set { } }
    public virtual IList<EQRecipe> RecipeList { get; set; }
    public virtual IList<EQData> EQDataList { get; set; }

    public override int getID()
    {
      return ID;
    }
  }
}
