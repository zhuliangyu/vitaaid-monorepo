using MyHibernateUtil;
using System;
using System.Collections.Generic;
using System.Linq;

namespace POCO
{
  [Serializable]
  public class SPCSheet: DataElement
  {
    public SPCSheet() { }
    public virtual int ID { get; set; }
    public virtual int SheetNo { get; set; }
    public virtual Lot oLot { get; set; }
    public virtual PackageMaterial oPackageMaterial { get; set; }
    private double _CapsuleWt = 0.0;
    public virtual double CapsuleWt { get { return _CapsuleWt; } set { _CapsuleWt = value; StandardWt = value + StdRmWt; } }
    private double _StdRmWt = 0.0;
    public virtual double StdRmWt { get { return _StdRmWt; } set { _StdRmWt = value; StandardWt = CapsuleWt + value; } }
    public virtual double StandardWt { get; set; } = 0.0;
    public virtual string Speed { get; set; } = "";
    public virtual double TargetLSLWt { get; set; } = 0.0;
    public virtual double TargetUSLWt { get; set; } = 0.0;
    public virtual double TargetLCLWt { get; set; } = 0.0;
    public virtual double TargetUCLWt { get; set; } = 0.0;
    public virtual double VarLSLWt { get; set; } = 0.0;
    public virtual double VarUSLWt { get; set; } = 0.0;
    public virtual double VarLCLWt { get; set; } = 0.0;
    public virtual double VarUCLWt { get; set; } = 0.0;
    public virtual double MinWt { get; set; } = double.MaxValue;
    public virtual double AvgWt { get; set; } = 0.0;
    public virtual double MaxWt { get; set; } = 0.0;
    private int _AbnormalSpecCnt { get; set; } = 0;
    public virtual int AbnormalCtlCnt { get; set; } = 0;
    public virtual int AbnormalSpecCnt { get { return _AbnormalSpecCnt; } set { _AbnormalSpecCnt = value; } }
    public virtual int VarAbnormalCnt
    {
      get
      {
        return _AbnormalSpecCnt % 1000;
      }
    }
    public virtual int VarCriticialCnt
    {
      get
      {
        return ((int)(_AbnormalSpecCnt / 1000)) * 1000;
      }
    }
    public virtual bool isAvgAlarm { get => isTypeOf(eSheetType.AVG_ALARM); }
    public virtual bool isVarAlarm { get => isTypeOf(eSheetType.VAR_ALARM); }
    public virtual string DispDate { get { return CreatedDate.ToShortDateString(); } }
    public virtual string DispTime { get { return CreatedDate.ToString("HH:mm"); } }
    private double _dAvgPercent = -1;
    public virtual double dAvgPercent
    {
      get
      {
        if (_dAvgPercent == -1)
        {
          if (SPCValues == null || StandardWt <= 0 || AvgWt <= 0 || CapsuleWt <= 0 || (StandardWt - CapsuleWt) <= 0)
            return -1;
          _dAvgPercent = Math.Round((AvgWt - CapsuleWt) / (StandardWt - CapsuleWt) * 100, 0, MidpointRounding.AwayFromZero);
        }
        return _dAvgPercent;
      }
    }
    public virtual string sAvgPercent
    {
      get
      {
        if (dAvgPercent == -1)
          return "";
        if (AvgWt < TargetLSLWt || AvgWt > TargetUSLWt)
          return dAvgPercent.ToString() + "!";
        else if (AvgWt < TargetLCLWt || AvgWt > TargetUCLWt)
          return dAvgPercent.ToString() + "*";
        else
          return dAvgPercent.ToString();
      }
    }
    public virtual eSheetType SheetType { get; set; } = 0;
    public virtual void SetSheetType(eSheetType type)
    {
      SheetType |= type;
    }
    public virtual void ClearSheetType(eSheetType type)
    {
      SheetType &= ~type;
    }
    public virtual bool isTypeOf(eSheetType type)
    {
      return ((SheetType & type) == type);
    }
    public virtual bool isPilotRunSPC
    {
      get
      {
        return isTypeOf(eSheetType.PILOT_RUN);
      }
    }
    public virtual bool isNormalSPC
    {
      get
      {
        return !isPilotRunSPC;
      }
    }
    public virtual void ClearAlarmInfo()
    {
      SheetType = SheetType & ~eSheetType.AVG_ALARM & ~eSheetType.VAR_ALARM;

    }
    public virtual void updateStatistic()
    {
      try
      {
        //if (SPCValues.Count == 0)
        //{
        MinWt = double.MaxValue;
        MaxWt = 0.0;
        AvgWt = 0;
        VarUCLWt = 0;
        VarLCLWt = 0;
        VarUSLWt = 0;
        VarLSLWt = 0;
        _AbnormalSpecCnt = 0;
        AbnormalCtlCnt = 0;
        if (SPCValues.Count == 0)
          return;
        //}
        double dAmount = 0;
        foreach (SPCValue oValue in SPCValues)
        {
          dAmount += oValue.Value;
          if (oValue.Value > MaxWt)
            MaxWt = oValue.Value;
          if (oValue.Value < MinWt)
            MinWt = oValue.Value;
        }
        AvgWt = Math.Round(dAmount / SPCValues.Count, 2, MidpointRounding.AwayFromZero);

        VarUCLWt = Math.Round(CapsuleWt + (AvgWt - CapsuleWt) * (1 + oLot.VarControlLimit * 0.01), 2, MidpointRounding.AwayFromZero);
        VarLCLWt = Math.Round(CapsuleWt + (AvgWt - CapsuleWt) * (1 - oLot.VarControlLimit * 0.01), 2, MidpointRounding.AwayFromZero);
        VarUSLWt = Math.Round(CapsuleWt + (AvgWt - CapsuleWt) * (1 + oLot.VarToleranceLimit * 0.01), 2, MidpointRounding.AwayFromZero);
        VarLSLWt = Math.Round(CapsuleWt + (AvgWt - CapsuleWt) * (1 - oLot.VarToleranceLimit * 0.01), 2, MidpointRounding.AwayFromZero);
        double dCriticalLSLWt = Math.Round(CapsuleWt + (AvgWt - CapsuleWt) * 0.75, 2, MidpointRounding.AwayFromZero);
        double dCriticalUSLWt = Math.Round(CapsuleWt + (AvgWt - CapsuleWt) * 1.25, 2, MidpointRounding.AwayFromZero);

        foreach (SPCValue oValue in SPCValues)
        {
          if (oValue.Value > VarUCLWt || oValue.Value < VarLCLWt)
            AbnormalCtlCnt++;
          if (oValue.Value > VarUSLWt || oValue.Value < VarLSLWt)
            _AbnormalSpecCnt++;
          if (oValue.Value >= dCriticalUSLWt || oValue.Value <= dCriticalLSLWt)
            _AbnormalSpecCnt += 1000;
        }

        // clear alarm info
        if (SPCValues.Count <= 20 || SPCValues.Count == 60)
          ClearAlarmInfo();
        // check point
        if (bCheckPoint)
        {
          if (AvgWt > TargetUSLWt || AvgWt < TargetLSLWt)
            SetSheetType(eSheetType.AVG_ALARM);
          if (_AbnormalSpecCnt >= ((SPCValues.Count == 20) ? 3 : 7))
            SetSheetType(eSheetType.VAR_ALARM);

        }
      }
      catch (Exception)
      {
      }
    }

    private IList<SPCValue> _SPCValues;
    public virtual IList<SPCValue> SPCValues
    {
      get
      {
        if (_SPCValues == null)
          _SPCValues = new List<SPCValue>();
        return _SPCValues;
      }
      set { _SPCValues = value; }
    }
    public virtual bool bCheckPoint { get => (SPCValues.Count == 20 || SPCValues.Count == 60); }
    public virtual bool bFinished { get => SPCValues.Count == (isTypeOf(eSheetType.VAR_ALARM) ? 60 : 20); }
    public virtual bool bAlarm { get => isTypeOf(eSheetType.AVG_ALARM) || isTypeOf(eSheetType.VAR_ALARM); }
    public virtual bool bWarning { get => VarAbnormalCnt >= 1 && VarAbnormalCnt < 3; }
    public virtual long TimeStamp { get; set; }
    public virtual IList<EDCData> EDCDataList { get; set; } = new List<EDCData>();
    public virtual string EQName { get => EDCDataList?.FirstOrDefault()?.EQName ?? ""; }
    public virtual string EQDataTemplateName { get => EDCDataList?.FirstOrDefault()?.oEQ?.EQDataTemplateName ?? ""; }

    public virtual string ExtraInfo { get; set; } = "";
    public virtual string OPID { get; set; } = "";

    public override int getID()
    {
      return ID;
    }
  }
}
