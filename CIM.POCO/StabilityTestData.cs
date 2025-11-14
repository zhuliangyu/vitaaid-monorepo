using MyHibernateUtil;
using MySystem.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using PropertyChanged;
namespace POCO
{
  [Serializable]
  public class StabilityTestData : DataElement
  {
    public virtual string GroupName { get; set; }
    public virtual string TestName { get; set; }
    // memory data for rebuild test form
    public virtual string OldTestName { get; set; } = "";
    public virtual string TestDesc { get; set; }
    public virtual string TestSpec { get; set; }
    public virtual string TestMethod { get; set; }
    public virtual bool bSpecEditable { get; set; } = false;
    public virtual List<string> SpecChoice { get; set; } = new List<string>();// { "1E3 cfu", "1E4 cfu" };
    public virtual List<string> ResultChoice { get; set; } = new List<string>();// Constant.PASSCHOICE;
    public virtual int Sequence { get; set; }
    public virtual void AddTo(List<StabilityTestData> TargetCol)
    {
      try
      {
        Sequence = TargetCol.Count + 1;
        TargetCol.Add(this);
      }
      catch (Exception ex) { throw ex; }
    }
    public static void RearrangeSeq(List<StabilityTestData> ObjCol)
    {
      try
      {
        if (ObjCol == null) return;
        for (int i = 1; i <= ObjCol.Count; i++)
          ObjCol[i].Sequence = i;
      }
      catch (Exception ex) { throw ex; }
    }

    [XmlIgnore]
    public virtual int ID { get; set; }
    public override int getID() => ID;
    [XmlIgnore]
    public virtual StabilityForm oStabilityForm { get; set; }
    [XmlIgnore]
    public virtual string Result0 { get; set; }
    [XmlIgnore]
    public virtual string TestUser0 { get; set; }
    [XmlIgnore]
    public virtual DateTime TestDate0 { get; set; } = SysVal.NilDate;
    [XmlIgnore]
    public virtual string RecordedBy0 { get { return (TestDate0.Year == SysVal.NilDate.Year) ? "" : TestUser0; } }//(TestDate0.Year == SysVal.NilDate.Year) ? "" : TestUser0 + " " + TestDate0.ToString("MMM/dd/yyyy"); } }

    [XmlIgnore]
    public virtual string Result12 { get; set; }
    [XmlIgnore]
    public virtual string TestUser12 { get; set; }
    [XmlIgnore]
    public virtual DateTime TestDate12 { get; set; } = SysVal.NilDate;
    [XmlIgnore]
    public virtual string RecordedBy12 { get { return (TestDate12.Year == SysVal.NilDate.Year) ? "" : TestUser12; } }//(TestDate12.Year == SysVal.NilDate.Year) ? "" : TestUser12 + " " + TestDate12.ToString("MMM/dd/yyyy"); } }

    [XmlIgnore]
    public virtual string Result24 { get; set; }
    [XmlIgnore]
    public virtual string TestUser24 { get; set; }
    [XmlIgnore]
    public virtual DateTime TestDate24 { get; set; } = SysVal.NilDate;
    [XmlIgnore]
    public virtual string RecordedBy24 { get { return (TestDate24.Year == SysVal.NilDate.Year) ? "" : TestUser24; } }//(TestDate24.Year == SysVal.NilDate.Year) ? "" : TestUser24 + " " + TestDate24.ToString("MMM/dd/yyyy"); } }

    [XmlIgnore]
    public virtual string Result36 { get; set; }
    [XmlIgnore]
    public virtual string TestUser36 { get; set; }
    [XmlIgnore]
    public virtual DateTime TestDate36 { get; set; } = SysVal.NilDate;
    [XmlIgnore]
    public virtual string RecordedBy36 { get { return (TestDate36.Year == SysVal.NilDate.Year) ? "" : TestUser36; } }//(TestDate36.Year == SysVal.NilDate.Year) ? "" : TestUser36 + " " + TestDate36.ToString("MMM/dd/yyyy"); } }

    [XmlIgnore]
    public virtual string Result48 { get; set; }
    [XmlIgnore]
    public virtual string TestUser48 { get; set; }
    [XmlIgnore]
    public virtual DateTime TestDate48 { get; set; } = SysVal.NilDate;
    [XmlIgnore]
    public virtual string RecordedBy48 { get { return (TestDate48.Year == SysVal.NilDate.Year) ? "" : TestUser48; } }//(TestDate48.Year == SysVal.NilDate.Year) ? "" : TestUser48 + " " + TestDate48.ToString("MMM/dd/yyyy"); } }


    public virtual void CopyFrom(StabilityTestData oSrc)
    {
      if (bSpecEditable)
        TestSpec = oSrc.TestSpec;
      Result0 = oSrc.Result0;
      TestUser0 = oSrc.TestUser0;
      TestDate0 = oSrc.TestDate0;
      Result12 = oSrc.Result12;
      TestUser12 = oSrc.TestUser12;
      TestDate12 = oSrc.TestDate12;
      Result24 = oSrc.Result24;
      TestUser24 = oSrc.TestUser24;
      TestDate24 = oSrc.TestDate24;
      Result36 = oSrc.Result36;
      TestUser36 = oSrc.TestUser36;
      TestDate36 = oSrc.TestDate36;
      Result48 = oSrc.Result48;
      TestUser48 = oSrc.TestUser48;
      TestDate48 = oSrc.TestDate48;
    }

    public override object ShallowCopy()
    {
      //   public virtual List<string> SpecChoice { get; set; } = new List<string>();// { "1E3 cfu", "1E4 cfu" };
      //public virtual List<string> ResultChoice { get; set; } = new List<string>();// Constant.PASSCHOICE;

      StabilityTestData oSTD = (StabilityTestData)this.MemberwiseClone();
      oSTD.SpecChoice = new List<string>();
      foreach (string s in this.SpecChoice)
        oSTD.SpecChoice.Add(s);
      oSTD.ResultChoice = new List<string>();
      foreach (string s in this.ResultChoice)
        oSTD.ResultChoice.Add(s);
      return oSTD;
    }

    [XmlIgnore]
    public virtual string OldResult0 { get; set; }
    [XmlIgnore]
    public virtual string OldResult12 { get; set; }
    [XmlIgnore]
    public virtual string OldResult24 { get; set; }
    [XmlIgnore]
    public virtual string OldResult36 { get; set; }
    [XmlIgnore]
    public virtual string OldResult48 { get; set; }
    [XmlIgnore]
    public virtual string sSuperscript { get; set; } = "";

    public virtual void backupResult()
    {
      OldResult0 = Result0;
      OldResult12 = Result12;
      OldResult24 = Result24;
      OldResult36 = Result36;
      OldResult48 = Result48;
    }

    public virtual void UpdateTestUserInfo(string sUser, DateTime dtNow, bool bUpdateFormTestDate,
       ref DateTime dtFormTestDate0, ref DateTime dtFormTestDate12, ref DateTime dtFormTestDate24,
       ref DateTime dtFormTestDate36, ref DateTime dtFormTestDate48)
    {
      try
      {
        if (!((OldResult0 == null && Result0 == null) ||
            (OldResult0 != null && Result0 != null && OldResult0.Equals(Result0) == true)))
        {
          TestUser0 = sUser;
          TestDate0 = dtNow;
          if (bUpdateFormTestDate)
            dtFormTestDate0 = TestDate0;
        }
        if (!((OldResult12 == null && Result12 == null) ||
            (OldResult12 != null && Result12 != null && OldResult12.Equals(Result12) == true)))
        {
          TestUser12 = sUser;
          TestDate12 = dtNow;
          if (bUpdateFormTestDate)
            dtFormTestDate12 = TestDate12;
        }
        if (!((OldResult24 == null && Result24 == null) ||
            (OldResult24 != null && Result24 != null && OldResult24.Equals(Result24) == true)))
        {
          TestUser24 = sUser;
          TestDate24 = dtNow;
          if (bUpdateFormTestDate)
            dtFormTestDate24 = TestDate24;
        }
        if (!((OldResult36 == null && Result36 == null) ||
            (OldResult36 != null && Result36 != null && OldResult36.Equals(Result36) == true)))
        {
          TestUser36 = sUser;
          TestDate36 = dtNow;
          if (bUpdateFormTestDate)
            dtFormTestDate36 = TestDate36;
        }
        if (!((OldResult48 == null && Result48 == null) ||
            (OldResult48 != null && Result48 != null && OldResult48.Equals(Result48) == true)))
        {
          TestUser48 = sUser;
          TestDate48 = dtNow;
          if (bUpdateFormTestDate)
            dtFormTestDate48 = TestDate48;
        }
      }
      catch (Exception)
      {
      }
    }
  }
}
