using MyHibernateUtil;
using MySystem.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using MySystem.Base.Extensions;

namespace POCO
{
  [Serializable]
  public class StabilityForm : DataElement
  {
    public virtual int ID { get; set; }
    public override int getID() => ID;
    public virtual Production oProduct { get; set; }
    public virtual Lot oLot { get; set; }
    public virtual string Code
    {
      get
      {
        return (oProduct != null) ? oProduct.Code :
          (oLot != null && oLot.FabProduction != null) ? oLot.FabProduction.Code : "";
      }
    }
    public virtual string ProductName
    {
      get
      {
        return (oProduct != null) ? oProduct.Name :
          (oLot != null && oLot.FabProduction != null) ? oLot.FabProduction.Name : "";
      }
    }
    public virtual string Location { get; set; }
    public virtual string LotNumber { get; set; }
    public virtual DateTime ProduceDate { get; set; } = DateTime.Now;
    public virtual string sProduceDate { get { return ProduceDate.ToString("MMM-yyyy"); } }
    public virtual DateTime ExpiryDate { get; set; } = DateTime.Now;
    public virtual string sExpiryDate { get { return ExpiryDate.ToString("MMM-yyyy"); } }
    public virtual string Servings { get; set; }
    public virtual string PackageForm { get; set; }
    public virtual string PackageClosure { get; set; }
    public virtual string PackageCap { get; set; } = "";
    public virtual string PackageBottle { get; set; } = "";
    public virtual string PackageDesiccant { get; set; }
    public virtual string PackageCotton { get; set; }
    public virtual string Comment { get; set; }
    public virtual string sTemplateFile { get; set; } = "";
    public virtual bool bRebuild { get; set; } = false;
    public virtual NSeqList<StabilityTestData> delTestData { get; set; } = new NSeqList<StabilityTestData>();
    public virtual void LoadTestTemplate(string sFile = "StabilityFormTemplate.xml")
    {
      try
      {
        sTemplateFile = sFile;
        bRebuild = false;
        if (File.Exists(sTemplateFile))
        {
          XmlSerializer deserializer = new XmlSerializer(typeof(NSeqList<StabilityTestData>));
          TextReader textReader = new StreamReader(sTemplateFile);
          if (oTestData == null || oTestData.Count == 0)
            oTestData = (NSeqList<StabilityTestData>)(deserializer.Deserialize(textReader));
          else
          {
            NSeqList<StabilityTestData> oTemplateTD = (NSeqList<StabilityTestData>)(deserializer.Deserialize(textReader));
            foreach (StabilityTestData oSTD in oTestData)
            {
              foreach (StabilityTestData oTemp in oTemplateTD)
                if (oSTD.TestName.Equals(oTemp.TestName))
                {
                  oSTD.SpecChoice = new List<string>(oTemp.SpecChoice);
                  oSTD.ResultChoice = new List<string>(oTemp.ResultChoice);
                  oSTD.bSpecEditable = oTemp.bSpecEditable;
                  oTemplateTD.Remove(oTemp);
                  break;
                }
            }
          }
          textReader.Close();
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public virtual void RebuildTestData(bool bRebuildMode, string newTemplateFile = null)
    {
      try
      {
        if (oTestData == null || oTestData.Count == 0)
          return;
        if (newTemplateFile == null)
          newTemplateFile = sTemplateFile;
        if (File.Exists(newTemplateFile))
        {
          XmlSerializer deserializer = new XmlSerializer(typeof(NSeqList<StabilityTestData>));
          TextReader textReader = new StreamReader(newTemplateFile);
          NSeqList<StabilityTestData> newOrderData = new NSeqList<StabilityTestData>();
          NSeqList<StabilityTestData> oTemplateTD = (NSeqList<StabilityTestData>)(deserializer.Deserialize(textReader));
          bool bFound = false;
          int iProgress = TestProgress();
          foreach (StabilityTestData oTemp in oTemplateTD)
          {
            bFound = false;
            foreach (StabilityTestData oSTD in oTestData)
            {
              var noSpaceTestName = oSTD.TestName.Replace(" ", "").ToUpper();
              if (noSpaceTestName.Equals(oTemp.TestName.Replace(" ", "").ToUpper()))
              {
                oSTD.TestDesc = oTemp.TestDesc;
                oSTD.TestMethod = oTemp.TestMethod;
                bFound = true;
              }
              else if (string.IsNullOrWhiteSpace(oTemp.OldTestName) == false && noSpaceTestName.Equals(oTemp.OldTestName.Replace(" ", "").ToUpper()))
              {
                bFound = true;
                oSTD.TestName = oTemp.TestName;
                oSTD.TestDesc = oTemp.TestDesc;
                oSTD.TestMethod = oTemp.TestMethod;
              }
              if (bFound)
              {
                newOrderData.Add(oSTD);
                oTestData.Remove(oSTD);
                break;
              }
            }
            if (bFound == false)
            {
              if (oTemp.ResultChoice != null && oTemp.ResultChoice.Count > 0)
              {
                if (iProgress >= 0)
                  oTemp.Result0 = (bRebuildMode) ? "N/A" : oTemp.ResultChoice[0];
                if (iProgress >= 12)
                  oTemp.Result12 = (bRebuildMode) ? "N/A" : oTemp.ResultChoice[0];
                if (iProgress >= 24)
                  oTemp.Result24 = (bRebuildMode) ? "N/A" : oTemp.ResultChoice[0];
                if (iProgress >= 36)
                  oTemp.Result36 = (bRebuildMode) ? "N/A" : oTemp.ResultChoice[0];
                if (iProgress >= 48)
                  oTemp.Result48 = (bRebuildMode) ? "N/A" : oTemp.ResultChoice[0];
              }
              newOrderData.Add(oTemp);
            }
          }

          NSeqList<StabilityTestData> oRemainObjs = new NSeqList<StabilityTestData>();
          foreach (StabilityTestData oSTD in oTestData)
          {
            if (oSTD.GroupName.Contains("Critical Ingredient") == true)
              oRemainObjs.Add(oSTD);
            else
              delTestData.Add(oSTD);
          }
          oTestData.Clear();
          foreach (StabilityTestData oSTD in newOrderData)
          {
            oTestData.Add(oSTD);
          }
          foreach (StabilityTestData oSTD in oRemainObjs)
          {
            oTestData.Add(oSTD);
          }
          oTestData.ReassignSeq();
          textReader.Close();
          bRebuild = true;
        }
      }
      catch (Exception)
      {
      }
    }

    public virtual NSeqList<StabilityTestData> oTestData { get; set; } = new NSeqList<StabilityTestData>();
    public virtual StabilityTestData addTestData(string sGroup, string sName, string sDesc, string sSpec, string sMethod)
    {
      StabilityTestData oSTD = new StabilityTestData() { oStabilityForm = this, GroupName = sGroup, TestName = sName, TestDesc = sDesc, TestSpec = sSpec, TestMethod = sMethod };
      oTestData.Add(oSTD);
      return oSTD;
    }
    public virtual bool Published { get; set; } = false;

    //public virtual bool IsClosed { get { return (ReviewedBy != null && ReviewedBy.Length > 0) ? true : false; } }
    public virtual bool IsClosed { get { return (Status == eStabilityTestStatus.CLOSED); } }
    public virtual string ReviewedBy { get; set; }
    //private DateTime _ReviewedDate = SysVal.NilDate;
    public virtual DateTime ReviewedDate { get; set; } = SysVal.NilDate; // { get { return _ReviewedDate; } set { _ReviewedDate = value; } }
    public virtual string DispReviewedDate { get { return (ReviewedDate.Year == SysVal.NilDate.Year) ? "" : ReviewedDate.ToString("MMM/dd/yyyy"); } }
    public virtual string DispReviewedBy
    {
      get { return ((ReviewedBy == null) ? "" : ReviewedBy + ", Date:" + DispReviewedDate/* + "       Result:" + ReviewedResult*/); }
    }
    public virtual string sSuperscriptDesc { get; set; }
    public virtual string ReviewedResult { get; set; }
    public virtual eStabilityTestStatus Status { get; set; }
    public virtual string sStatus { get { return Status.ToString(); } }
    public virtual string RecordedBy0
    {
      get
      {
        return (TestDate0 == SysVal.NilDate) ? "" : RecordedByUser0 + " " + DispTestDate0;
        //string sVal = "";
        //if (oTestData != null)
        //{
        //    long ticks = 0;
        //    foreach (StabilityTestData oSTD in oTestData)
        //    {
        //        if (oSTD.TestDate0.Year != SysVal.NilDate.Year && oSTD.TestDate0.Ticks > ticks)
        //        {
        //            sVal = oSTD.RecordedBy0 + " " + TestDate0.ToString("MMM/dd/yyyy");
        //            ticks = oSTD.TestDate0.Ticks;
        //        }
        //    }
        //}
        //return sVal;
      }
    }
    public virtual string RecordedBy12
    {
      get
      {
        return (TestDate12 == SysVal.NilDate) ? "" : RecordedByUser12 + " " + DispTestDate12;
        //string sVal = "";
        //if (oTestData != null)
        //{
        //    long ticks = 0;
        //    foreach (StabilityTestData oSTD in oTestData)
        //    {
        //        if (oSTD.TestDate12.Year != SysVal.NilDate.Year && oSTD.TestDate12.Ticks > ticks)
        //        {
        //            sVal = oSTD.RecordedBy12 + " " + TestDate12.ToString("MMM/dd/yyyy");
        //            ticks = oSTD.TestDate12.Ticks;
        //        }
        //    }
        //}
        //return sVal;
      }
    }
    public virtual string RecordedBy24
    {
      get
      {
        return (TestDate24 == SysVal.NilDate) ? "" : RecordedByUser24 + " " + DispTestDate24;
        //string sVal = "";
        //if (oTestData != null)
        //{
        //    long ticks = 0;
        //    foreach (StabilityTestData oSTD in oTestData)
        //    {
        //        if (oSTD.TestDate24.Year != SysVal.NilDate.Year && oSTD.TestDate24.Ticks > ticks)
        //        {
        //            sVal = oSTD.RecordedBy24 + " " + TestDate24.ToString("MMM/dd/yyyy");
        //            ticks = oSTD.TestDate24.Ticks;
        //        }
        //    }
        //}
        //return sVal;
      }
    }
    public virtual string RecordedBy36
    {
      get
      {
        return (TestDate36 == SysVal.NilDate) ? "" : RecordedByUser36 + " " + DispTestDate36;
        //string sVal = "";
        //if (oTestData != null)
        //{
        //    long ticks = 0;
        //    foreach (StabilityTestData oSTD in oTestData)
        //    {
        //        if (oSTD.TestDate36.Year != SysVal.NilDate.Year && oSTD.TestDate36.Ticks > ticks)
        //        {
        //            sVal = oSTD.RecordedBy36 + " " + TestDate36.ToString("MMM/dd/yyyy");
        //            ticks = oSTD.TestDate36.Ticks;
        //        }
        //    }
        //}
        //return sVal;
      }
    }
    public virtual string RecordedBy48
    {
      get
      {
        return (TestDate48 == SysVal.NilDate) ? "" : RecordedByUser48 + " " + DispTestDate48;
        //string sVal = "";
        //if (oTestData != null)
        //{
        //    long ticks = 0;
        //    foreach (StabilityTestData oSTD in oTestData)
        //    {
        //        if (oSTD.TestDate48.Year != SysVal.NilDate.Year && oSTD.TestDate48.Ticks > ticks)
        //        {
        //            sVal = oSTD.RecordedBy48 + " " + TestDate48.ToString("MMM/dd/yyyy");
        //            ticks = oSTD.TestDate48.Ticks;
        //        }
        //    }
        //}
        //return sVal;
      }
    }

    public virtual string RecordedByUser0
    {
      get
      {
        string sVal = "";
        if (oTestData != null)
        {
          long ticks = 0;
          foreach (StabilityTestData oSTD in oTestData)
          {
            if (oSTD.TestDate0.Year != SysVal.NilDate.Year && oSTD.TestDate0.Ticks > ticks)
            {
              sVal = oSTD.RecordedBy0;
              ticks = oSTD.TestDate0.Ticks;
            }
          }
        }
        return sVal;
      }
      set
      {
        if (oTestData != null)
        {
          foreach (StabilityTestData oSTD in oTestData)
          {
            if (oSTD.TestDate0.Year != SysVal.NilDate.Year)
              oSTD.TestUser0 = value;
          }
        }
      }
    }
    public virtual string RecordedByUser12
    {
      get
      {
        string sVal = "";
        if (oTestData != null)
        {
          long ticks = 0;
          foreach (StabilityTestData oSTD in oTestData)
          {
            if (oSTD.TestDate12.Year != SysVal.NilDate.Year && oSTD.TestDate12.Ticks > ticks)
            {
              sVal = oSTD.RecordedBy12;
              ticks = oSTD.TestDate12.Ticks;
            }
          }
        }
        return sVal;
      }
      set
      {
        if (oTestData != null)
        {
          foreach (StabilityTestData oSTD in oTestData)
          {
            if (oSTD.TestDate12.Year != SysVal.NilDate.Year)
              oSTD.TestUser12 = value;
          }
        }
      }
    }
    public virtual string RecordedByUser24
    {
      get
      {
        string sVal = "";
        if (oTestData != null)
        {
          long ticks = 0;
          foreach (StabilityTestData oSTD in oTestData)
          {
            if (oSTD.TestDate24.Year != SysVal.NilDate.Year && oSTD.TestDate24.Ticks > ticks)
            {
              sVal = oSTD.RecordedBy24;
              ticks = oSTD.TestDate24.Ticks;
            }
          }
        }
        return sVal;
      }
      set
      {
        if (oTestData != null)
        {
          foreach (StabilityTestData oSTD in oTestData)
          {
            if (oSTD.TestDate24.Year != SysVal.NilDate.Year)
              oSTD.TestUser24 = value;
          }
        }
      }
    }
    public virtual string RecordedByUser36
    {
      get
      {
        string sVal = "";
        if (oTestData != null)
        {
          long ticks = 0;
          foreach (StabilityTestData oSTD in oTestData)
          {
            if (oSTD.TestDate36.Year != SysVal.NilDate.Year && oSTD.TestDate36.Ticks > ticks)
            {
              sVal = oSTD.RecordedBy36;
              ticks = oSTD.TestDate36.Ticks;
            }
          }
        }
        return sVal;
      }
      set
      {
        if (oTestData != null)
        {
          foreach (StabilityTestData oSTD in oTestData)
          {
            if (oSTD.TestDate36.Year != SysVal.NilDate.Year)
              oSTD.TestUser36 = value;
          }
        }
      }
    }
    public virtual string RecordedByUser48
    {
      get
      {
        string sVal = "";
        if (oTestData != null)
        {
          long ticks = 0;
          foreach (StabilityTestData oSTD in oTestData)
          {
            if (oSTD.TestDate48.Year != SysVal.NilDate.Year && oSTD.TestDate48.Ticks > ticks)
            {
              sVal = oSTD.RecordedBy48;
              ticks = oSTD.TestDate48.Ticks;
            }
          }
        }
        return sVal;
      }
      set
      {
        if (oTestData != null)
        {
          foreach (StabilityTestData oSTD in oTestData)
          {
            if (oSTD.TestDate48.Year != SysVal.NilDate.Year)
              oSTD.TestUser48 = value;
          }
        }
      }
    }


    public virtual void CopyFrom(StabilityForm oSrc)
    {
      Location = oSrc.Location;
      LotNumber = oSrc.LotNumber;
      ProduceDate = oSrc.ProduceDate;
      ExpiryDate = oSrc.ExpiryDate;
      Servings = oSrc.Servings;
      PackageForm = oSrc.PackageForm;
      PackageClosure = oSrc.PackageClosure;
      PackageCap = oSrc.PackageCap;
      PackageBottle = oSrc.PackageBottle;
      PackageDesiccant = oSrc.PackageDesiccant;
      PackageCotton = oSrc.PackageCotton;
      Comment = oSrc.Comment;

      for (int i = 0; i < oTestData.Count; i++)
        oTestData[i].CopyFrom(oSrc.oTestData[i]);
      CreatedDate = oSrc.CreatedDate;
      CreatedID = oSrc.CreatedID;
      ReviewedBy = oSrc.ReviewedBy;
      ReviewedDate = oSrc.ReviewedDate;
      UpdatedDate = oSrc.UpdatedDate;
      UpdatedID = oSrc.UpdatedID;
      SOPRev = oSrc.SOPRev;

    }
    public virtual void BackupTestData()
    {
      try
      {
        foreach (StabilityTestData oSTD in oTestData)
          oSTD.backupResult();
      }
      catch (Exception)
      {

        throw;
      }
    }
    public virtual void UpdateTestUserInfo(string sUser, DateTime dtNow, bool bUpdateFormTestDate)
    {
      try
      {
        foreach (StabilityTestData oSTD in oTestData)
          oSTD.UpdateTestUserInfo(sUser, dtNow, bUpdateFormTestDate, ref _TestDate0, ref _TestDate12,
              ref _TestDate24, ref _TestDate36, ref _TestDate48);
      }
      catch (Exception)
      {

        throw;
      }
    }
    /*
    public virtual int StabilityStudyStatus()
    {
        try
        {
            foreach (StabilityTestData oSTD in oTestData)
                if (oSTD.Result0 == null || oSTD.Result0.Length == 0)
                    return 0;
            foreach (StabilityTestData oSTD in oTestData)
                if (oSTD.Result12 == null || oSTD.Result12.Length == 0)
                    return 12;
            foreach (StabilityTestData oSTD in oTestData)
                if (oSTD.Result24 == null || oSTD.Result24.Length == 0)
                    return 24;
            foreach (StabilityTestData oSTD in oTestData)
                if (oSTD.Result36 == null || oSTD.Result36.Length == 0)
                    return 36;
            foreach (StabilityTestData oSTD in oTestData)
                if (oSTD.Result48 == null || oSTD.Result48.Length == 0)
                    return 48;
            return 9999;
        }
        catch (Exception)
        {
            return -1;
        }
    }
    */

    public virtual int TestProgress()
    {
      try
      {
        foreach (StabilityTestData oSTD in oTestData)
          if (oSTD.Result0 == null || oSTD.Result0.Length == 0)
            return 0;
        foreach (StabilityTestData oSTD in oTestData)
          if (oSTD.Result12 == null || oSTD.Result12.Length == 0)
            return 12;
        foreach (StabilityTestData oSTD in oTestData)
          if (oSTD.Result24 == null || oSTD.Result24.Length == 0)
            return 24;
        foreach (StabilityTestData oSTD in oTestData)
          if (oSTD.Result36 == null || oSTD.Result36.Length == 0)
            return 36;
        foreach (StabilityTestData oSTD in oTestData)
          if (oSTD.Result48 == null || oSTD.Result48.Length == 0)
            return 48;
        return 9999;
      }
      catch (Exception)
      {
        return -1;
      }
    }

    public virtual void UpdateStatus()
    {
      try
      {
        if (Status == eStabilityTestStatus.REVIEW || Status == eStabilityTestStatus.CLOSED)
          return;
        int iProgress = TestProgress();
        switch (iProgress)
        {
          case 0:
            Status = eStabilityTestStatus.T0;
            break;
          case 12:
            Status = eStabilityTestStatus.T12;
            break;
          case 24:
            Status = eStabilityTestStatus.T24;
            break;
          case 36:
            Status = eStabilityTestStatus.T36;
            break;
          case 48:
            Status = eStabilityTestStatus.T48;
            break;
          case 9999:
            Status = eStabilityTestStatus.TESTING;
            break;
          default:
            Status = eStabilityTestStatus.INIT;
            break;
        }
      }
      catch (Exception)
      {
      }
    }
    private DateTime _TestDate0 = SysVal.NilDate;
    public virtual DateTime TestDate0 { get { return _TestDate0; } set { _TestDate0 = value; } }
    private DateTime _TestDate12 = SysVal.NilDate;
    public virtual DateTime TestDate12 { get { return _TestDate12; } set { _TestDate12 = value; } }
    private DateTime _TestDate24 = SysVal.NilDate;
    public virtual DateTime TestDate24 { get { return _TestDate24; } set { _TestDate24 = value; } }
    private DateTime _TestDate36 = SysVal.NilDate;
    public virtual DateTime TestDate36 { get { return _TestDate36; } set { _TestDate36 = value; } }
    private DateTime _TestDate48 = SysVal.NilDate;
    public virtual DateTime TestDate48 { get { return _TestDate48; } set { _TestDate48 = value; } }
    public virtual string DispTestDate0
    {
      get
      {
        string sVal = (TestDate0 == SysVal.NilDate) ?
            "" :
            TestDate0.ToString("MMM/dd/yyyy");
        return sVal;
      }
    }
    public virtual string DispTestDate12
    {
      get
      {
        return (TestDate12 == SysVal.NilDate) ? "" : TestDate12.ToString("MMM/dd/yyyy");
      }
    }
    public virtual string DispTestDate24
    {
      get
      {
        return (TestDate24 == SysVal.NilDate) ? "" : TestDate24.ToString("MMM/dd/yyyy");
      }
    }
    public virtual string DispTestDate36
    {
      get
      {
        return (TestDate36 == SysVal.NilDate) ? "" : TestDate36.ToString("MMM/dd/yyyy");
      }
    }
    public virtual string DispTestDate48
    {
      get
      {
        return (TestDate48 == SysVal.NilDate) ? "" : TestDate48.ToString("MMM/dd/yyyy");
      }
    }
    public virtual double ShelfLife { get; set; }
    public virtual string DispShelfLife
    {
      get { return (ShelfLife > 0) ? ShelfLife.ToString() + " months" : "NA"; }
    }

    public virtual int Version { get; set; } = 1;
    public virtual string sVersion { get { return "Version " + Version.ToString(); } }
    public virtual StabilityForm oNextVersion { get; set; }
    public virtual StabilityForm oPrevVersion { get; set; }
    public virtual string SOPRev { get; set; } = "03";
    public override object ShallowCopy()
    {
      StabilityForm oSForm = (StabilityForm)this.MemberwiseClone();
      oSForm.oTestData = new NSeqList<StabilityTestData>();
      foreach (StabilityTestData oSTD in oTestData)
        oSForm.oTestData.Add(((StabilityTestData)oSTD.ShallowCopy()).Also(x => x.ID = 0));
      return oSForm;
    }

    // memory object
    public virtual double OldShelfLife { get; set; }
    public virtual bool bCheck { get; set; } = false;
  }
}
