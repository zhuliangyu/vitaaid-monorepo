using System;
using System.Collections.Generic;
using System.ComponentModel;
using MyHibernateUtil;

namespace POCO
{
  [Serializable]
  public class LotComment : DataElement
  {
    public static Dictionary<string, string> OPIDDesc = new Dictionary<string, string>
    {
      {"1000", "PREPARE" },
      {"1100", "DISPENSE" },
      {"1150", "DISPENSE-QA" },
      {"1500", "BLENDING" },
      {"2500", "ENCAPSULATION" },
      {"3500", "POLISHING" },
      {"4500", "BOTTLE FILLING" },
      {"5500", "PACKAGING" },
      {"FINISH", "QA SUMMARY" }
    };
    public static string GenerateOPIDDetail(string OPID) => string.Format("{0} - {1}", OPID, OPIDDesc[OPID]);
    public LotComment() { }
    public virtual int ID { get; set; }
    public override int getID() => ID;

    private string _Type = "";
    public virtual string Type { get { return _Type; } set { _Type = value; } }
    public virtual Lot oLot { get; set; }
    public virtual string LotNo { get => oLot?.No ?? ""; set { } }
    private string _OPID = "";
    public virtual string OPID { get { return _OPID; }
      set {
        _OPID = value;
        if (!string.IsNullOrWhiteSpace(value))
          _OPIDDetail = GenerateOPIDDetail(value.Trim());
        OnPropertyChanged("OPID");
        OnPropertyChanged("OPIDDetail");
      }
    }
    
    private string _OPIDDetail = null;
    public virtual string OPIDDetail { 
      get { return _OPIDDetail; } 
      set { 
        _OPIDDetail = value;
        if (!string.IsNullOrWhiteSpace(value))
          _OPID = value.Split('-')[0].Trim();
        OnPropertyChanged("OPID");
        OnPropertyChanged("OPIDDetail");
      }
    }

    //public virtual string ProductCode { get; set; }
    //public virtual Production oProduct { get; set; }
    public virtual string ProductName { get; set; }
    public virtual string FormulationCode { get; set; }

    private string _Comment = "";
    public virtual string Comment
    {
      get
      {
        return _Comment;
      }
      set
      {
        _Comment = value;
        OnPropertyChanged("Comment");
      }
    }
    // memory
    public virtual string[] TypeList { get; set; } = new List<string>().ToArray();
    public virtual string DispUpdatedDate { get => UpdatedDate.ToString("MMM/dd/yy HH:mm"); }
  }
}
