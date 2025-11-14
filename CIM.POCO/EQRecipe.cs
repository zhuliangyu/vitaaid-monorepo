using System;
using System.Collections.Generic;
using MyHibernateUtil;
using System.Linq;

namespace POCO
{
    [Serializable]
    public class EQRecipe : DataElement
    {
        public EQRecipe() { }
        public virtual int ID { get; set; }
        public virtual EQ oEQ { get; set; }
        public virtual string EQName { get => oEQ?.Name ?? ""; set { } }
        public virtual string RecipeName { get; set; }
        public virtual string Comment { get; set; }
        public virtual EQDataTemplate Template { get => Parameters?.FirstOrDefault()?.oRecipeParameter?.oTemplate ?? null; }
        public virtual string TemplateName { get => Template?.TemplateName ?? ""; set { } }
        public virtual IList<EQRecipeDetail> Parameters { get; set; } = new List<EQRecipeDetail>();
        public virtual string RecipeInfo { get => String.Join(",", Parameters.Select(x => x.oRecipeParameter.ParameterName + ":" + x.RecipeParameterValue)); }
        public override int getID()
        {
            return ID;
        }
    }
}
