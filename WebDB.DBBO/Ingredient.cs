using MyHibernateUtil;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using PropertyChanged;
using MySystem.Base.Extensions;

namespace WebDB.DBBO
{
  [Serializable]
  public class Ingredient : DataElement
  {
    public virtual int ID { get; set; }
    public override int getID() => ID;
    public virtual string Name { get; set; }
    public virtual string LabelClaim { get; set; }
    public virtual string LabelClaimUS { get; set; }
    public virtual string GenerateLabelClaimByServiceSize(int ServingSize)
    {
      try
      {
        if (ServingSize == 1)
          return LabelClaim;
        else if (ServingSize > 1 && !string.IsNullOrWhiteSpace(LabelClaim))
        {
          var tokens = LabelClaim.Split(' ');
          tokens[0] = tokens.First().Let(t => (float.Parse(t.Trim().Replace(",", "")) * ServingSize).ToString());
          return tokens.Aggregate((a, b) => a + " " + b);
        }
        return "";
      }
      catch (Exception)
      {
        return "";
      }
    }
    public virtual string AdditionalInfo { get; set; }
    [JsonIgnore]
    public virtual Product oProduct { get; set; }
    public virtual string ProductCode { get => oProduct?.ProductCode ?? ""; set { } }
    public virtual int GroupNo { get; set; }
    public virtual string RawMaterialCategory { get; set; }
    public virtual int Sequence { get; set; }
    // memory object
    [JsonIgnore]
    public virtual object Tag { get; set; }
  }
}
