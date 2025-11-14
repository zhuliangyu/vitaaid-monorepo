using MyHibernateUtil;
using System;
using System.ComponentModel;


namespace POCO
{
  [Serializable]
  public class FabProcessLog : POCOBase
  {
    public FabProcessLog() { }
    public virtual int ID { get; set; }
    public override int getID() => ID;

    private int _LogLevel = 0;
    public virtual int LogLevel
    {
      get { return _LogLevel; }
      set
      {
        _LogLevel = value;
        if (value >= 0 && value < Constant.sLogLevel.Length)
          Type = Constant.sLogLevel[value];
      }
    }

    public virtual string Type { get; set; }
    public virtual Lot oLot { get; set; }
    public virtual string LotNo { get; set; }
    public virtual string OPID { get; set; }
    public virtual string ProductName { get; set; }
    public virtual string Message { get; set; }

    private DateTime _CreatedDate = DateTime.Now;
    public virtual DateTime CreatedDate { get { return _CreatedDate; } set { _CreatedDate = value; } }
    public virtual string CreatedID { get; set; }

    private bool _bProcessed = false;
    public virtual bool bProcessed
    {
      get { return _bProcessed; }
      set
      {
        _bProcessed = value;
        OnPropertyChanged("bProcessed");
      }
    }
    private DateTime? ProcessedDate { get; set; }
    public virtual string ProcessedID { get; set; }
    public virtual string Client { get; set; }
    public virtual string Comment { get; set; }
    public virtual string Category { get; set; }
  }
}
