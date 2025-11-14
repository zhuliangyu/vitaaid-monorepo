using MyHibernateUtil;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace WebDB.DBBO
{
  [Serializable]
  public class Product : DataElement
  {
    public virtual int ID { get; set; }
    public override int getID() => ID;
    public virtual int OldID { get; set; }
    public virtual string Category { get; set; }
    public virtual string Category1 { get; set; }
    [Required]
    public virtual string ProductName { get; set; }
    public virtual string Function { get; set; }
    [Required]
    public virtual string ProductCode { get; set; }
    public virtual string Tags { get; set; }
    public virtual string Description { get; set; }
    public virtual string Indications { get; set; }
    public virtual string Combination { get; set; }
    public virtual string Suggest { get; set; }
    public virtual string ProductSheet { get; set; }
    public virtual string image { get; set; }
    public virtual DateTime TimeStamp { get; set; }
    [Required]
    public virtual string Size { get; set; }
    public virtual string Size2 { get; set; }
    public virtual string Caution { get; set; }
    public virtual string NPN { get; set; }
    public virtual bool Featured { get; set; }
    public virtual string SupplementFact { get; set; }
    public virtual eWEBSITE VisibleSite { get; set; } = eWEBSITE.ALL;
    public virtual bool Published { get; set; } = false;
    public virtual string AdditionalInfo { get; set; }

    public virtual IList<Ingredient> oIngredients { get; set; }
    public virtual IList<int> VitaaidCategory { get; set; } = new List<int>();
    public virtual IList<int> AllergyCategory { get; set; } = new List<int>();
    public virtual IList<ProductImage> oProductImages { get; set; }
    public virtual string RepresentativeImage { get; set; } = "";
    public virtual string RepresentativeLargeImage { get; set; } = "";
    //Medicinal Ingredients
    [JsonIgnore]
    public virtual IList<Ingredient> oMedicinalIngredients { get => oIngredients?.Where(x => x.GroupNo != 5)?.ToList() ?? new List<Ingredient>(); }

    //Non-medicinal Ingredients
    [JsonIgnore]
    public virtual Ingredient oNMI { get => oIngredients?.Where(x => x.GroupNo == 5)?.FirstOrDefault(); }
    [JsonIgnore]
    public virtual object Tag { get; set; }
    // transient data
    public virtual int StockCount { get; set; }
    public virtual decimal UnitPrice { get; set; }
    public virtual int ServingSize { get; set; } = 1;
    public virtual string ServingUnit { get; set; } = "Capsule";
    public virtual string sServingSize { get => string.Format("{0} {1}(s)", ServingSize, ServingUnit); }
    public virtual int ServingsPerContainer { get; set; } = 1;
  }
}
