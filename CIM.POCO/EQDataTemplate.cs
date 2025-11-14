using System;
using System.Collections.Generic;
using System.Linq;
using MyHibernateUtil;

namespace POCO
{
  [Serializable]
  public class EQDataTemplate : DataElement
  {
    public EQDataTemplate() { }
    public virtual int ID { get; set; }
    public virtual string TemplateName { get; set; }
    public virtual eEQTYPE EQType { get; set; }
    public virtual string Description { get; set; }
    private IList<EQDataTemplateDetail> _TemplateRecipeDetails = null;
    private IList<EQDataTemplateDetail> _TemplateEDCDetails = null;
    public virtual IList<EQDataTemplateDetail> TemplateRecipeDetails { get => (_TemplateRecipeDetails != null) ? _TemplateRecipeDetails : new List<EQDataTemplateDetail>(); set { _TemplateRecipeDetails = value; } }
    public virtual IList<EQDataTemplateDetail> TemplateEDCDetails { get => (_TemplateEDCDetails != null) ? _TemplateEDCDetails : new List<EQDataTemplateDetail>(); set { _TemplateEDCDetails = value; } }
    public virtual IList<EQDataTemplateDetail> TemplateDetails { get => TemplateRecipeDetails.Concat(TemplateEDCDetails).ToList(); }
    public override int getID()
    {
      return ID;
    }

  }
}
