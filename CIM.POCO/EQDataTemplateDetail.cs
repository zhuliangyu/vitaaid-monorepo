using System;
using System.ComponentModel;
using MyHibernateUtil;

namespace POCO
{
  [Serializable]
  public class EQDataTemplateDetail : DataElement
  {
    public EQDataTemplateDetail() { }
    public virtual int ID { get; set; }
    public virtual string ParameterName { get; set; }
    public virtual string Description { get; set; }
    public virtual EQDataTemplate oTemplate { get; set; }
    public virtual string TemplateName { get => oTemplate?.TemplateName ?? ""; set { } }
    public virtual eEQDataItemType ItemType { get; set; } = eEQDataItemType.RECIPE;
    public override int getID()
    {
      return ID;
    }
  }
}
