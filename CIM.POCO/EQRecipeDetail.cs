using System;
using MyHibernateUtil;

namespace POCO
{
    [Serializable]
    public class EQRecipeDetail : DataElement
    {
        public EQRecipeDetail() { }
        public virtual int ID { get; set; }
        public virtual EQRecipe oEQRecipe { get; set; }
        public virtual string RecipeName { get => oEQRecipe?.RecipeName; set { } }
        public virtual EQDataTemplateDetail oRecipeParameter { get; set; }
        public virtual string RecipeParameterName { get => oRecipeParameter?.ParameterName ?? ""; set { } }
        public virtual String RecipeParameterValue { get; set; }
        public override int getID()
        {
            return ID;
        }
    }
}
