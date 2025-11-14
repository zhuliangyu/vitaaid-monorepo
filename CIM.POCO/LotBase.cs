using MyHibernateUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using PropertyChanged;
using static POCO.LotNoHelper;
using MySystem.Base.Extensions;

namespace POCO
{
    [Serializable]
    public class LotBase : DataElement
    {
        public LotBase() { }
        public static List<string> OPIDsBeforeBlending = new List<string> { "PLAN", "INIT", "1000", "1100" };
        public virtual int ID { get; set; }
        public override int getID() => ID;
        public virtual string No { get; set; }
        //private string _LabelLotNo = "";
        //public virtual string LabelLotNo { get { return _LabelLotNo; }
        //    set
        //    {
        //        _LabelLotNo = value;
        //        OnPropertyChanged("LabelLotNo");
        //    }
        //}
        private int _LotSize = 0;
        public virtual int LotSize
        {
            get { return _LotSize; }
            set
            {
                _LotSize = value;
                OnPropertyChanged("LotSize");
                if (bIndependant && IsRoot)
                {
                    GroupSize = value;
                    OnPropertyChanged("GroupSize");
                }
            }
        }
        public virtual int BatchCount { get; set; } = 1;

        public virtual bool SampleBatch { get; set; } = false;
        public virtual Production ModelingProduction { get; set; }
        public virtual FabProduction FabProduction { get; set; }
        public virtual string BriefInfo
        {
            get
            {
                return No + "|" + FabProduction.Code + "|" + FabProduction.Name;
            }
        }

        public virtual string WipStatus { get; set; }
        public virtual eLOTSTATUS2 PreStatus { get; set; } = eLOTSTATUS2.INIT;

        public virtual eLOTSTATUS2 Status { get; set; }
        public virtual void UpdateStatusByOPID()
        {
            try
            {
                UpdateStatus(GetStatusByOPID());
            }
            catch (Exception ex) { throw ex; }
        }

        public virtual eLOTSTATUS2 GetStatusByOPID()
        {
            try
            {
                if (OPID == null) return eLOTSTATUS2.INIT;
                if (OPID.Equals("INIT"))
                    return eLOTSTATUS2.INIT;
                else if (OPID.Equals("1000"))
                    return eLOTSTATUS2.PREPARE;
                else if (OPID.Equals("1100"))
                    return eLOTSTATUS2.DISPENSE;
                else if (OPID.Equals("7500"))
                    return eLOTSTATUS2.SHIPPING;
                else
                    return eLOTSTATUS2.PRODUCE;
            }
            catch (Exception ex) { throw ex; }
        }

        public virtual void UpdateStatus(eLOTSTATUS2 status)
        {
            PreStatus = Status;
            Status = status;
        }


        public virtual void SetLotNo(DateTime dt, string FormType, int SequenceNo)
        {
            No = MakeLotNo(dt, FormType, SequenceNo);
        }
        public virtual eFORMTYPE FormType { get => ModelingProduction?.FormType ?? eFORMTYPE.CAPSULE; }

        //public virtual IList<FabRawMaterialApply> fabRMApplyList { get; set; }
        public virtual string OPID { get; set; }
        public virtual double TotalReqWeight { get; set; }
        public virtual double TotalApplyWeight { get; set; } = 0.0;
        //public virtual double GroupWeight { get; set; } = 0;
        public virtual double VirtualGroupWeight
        {
            get
            {
                if (OPID == "PLAN" || OPID == "INIT" || OPID == "1000" || OPID == "1100")
                    return Math.Round(TotalReqWeight * GroupSize / LotSize, Constant.OPIDIGIT, MidpointRounding.AwayFromZero);
                else
                    return TotalApplyWeight * GroupSize / LotSize;
            }
        }

        public virtual DateTime PreparedDate { get; set; } = DateTime.Now.AddDays(1);
        public virtual string PreparedDateShort { get { return PreparedDate.ToString("MM/dd/yyyy"); } }

        public virtual string PreparedPMOPID { get; set; } = "2500";
        public virtual bool PreparedPMFinish { get; set; } = false;
        public virtual bool bEnough
        {
            get
            {
                if (OPID == "INIT")
                {
                    foreach (FabFormulationItem ffi in FabProduction.FormulationObj.Items)
                        if (ffi.bEnough == false)
                        {
                            return false;
                        }
                    return true;
                }
                else
                    return true;
            }
        }
        public virtual bool bDeletable { get => bIndependant && OPID != "FINISH"; }
        public virtual double TargetControlLimit { get; set; } = 5;
        public virtual double TargetToleranceLimit { get; set; } = 10;
        public virtual double VarControlLimit { get; set; } = 5;
        public virtual double VarToleranceLimit { get; set; } = 10;
        public virtual Lot oParentLot { get; set; }
        public virtual string GroupCode { get; set; }
        public virtual int GroupSize { get; set; } = 0;
        public virtual int GroupBatchCount { get; set; }
        public virtual string GroupStartOPID { get; set; }
        public virtual string GroupEndOPID { get; set; }
        public virtual string Comment { get; set; }
        public virtual int GroupRevision { get; set; } = 0;
        public virtual string RetailLotNo { get; set; }
        public virtual bool TrialRun { get; set; } = false;
        public virtual bool AutoMerge { get; set; } = true;
        public virtual bool Urgent { get; set; } = false;
        public virtual bool IsRoot { get => (oParentLot == null); }
        public static string ToRootGroupCode(string No) => string.IsNullOrWhiteSpace(No) ? "" : No + "X" + "0".PadLeft(13 - No.Length - 1, '0');
        public virtual string RootGroupCode { get => ToRootGroupCode(No); }
        public virtual string DisplayGroupCode
        {
            get => string.IsNullOrWhiteSpace(GroupCode) ? RootGroupCode : GroupCode;
        }
        public virtual string ReProducedFrom { get; set; } = "";
        public virtual IList<Lot> SubGroupList { get; set; } = new List<Lot>();
        private int _opno = -1;
        public virtual int OPNo()
        {
            if (_opno == -1)
            {
                if (Int32.TryParse(OPID, out _opno))
                    return _opno;
                else
                    _opno = OPID == "PLAN" ? 0 :
                            OPID == "INIT" ? 1 : 9999;
            }
            return _opno;       
        }
        public virtual int GlobalOPNo()
        {
            var iOPNo = OPNo();
            int tmp = 9999;
            foreach(var x in SubGroupList)
            {
                tmp = x.GlobalOPNo();
                if (tmp < iOPNo)
                    iOPNo = tmp;
            };
            return iOPNo;
        }
        public virtual bool OPIDInGroup(string sOPID)
        {
            if (OPID == sOPID)
                return true;
            else
            {
                foreach (var s in SubGroupList) {
                    if (s.OPIDInGroup(sOPID))
                        return true;
                }
                return false;
            }
        }
        public virtual void InitialGroup()
        {
            GroupCode = RootGroupCode;
            GroupSize = LotSize;
            GroupBatchCount = BatchCount;
            RetailLotNo = No;
        }
        public virtual bool bIndependant { get => (SubGroupList?.Count ?? 0) == 0; }
        // memory object
        public virtual StabilityForm oSForm { get; set; }
        public virtual IList<StabilityForm> oPrevSForms { get; set; } = new List<StabilityForm>();
        public virtual IList<FabPackageReq> oPackageReqs { get; set; }
        public virtual bool IsClosedCOA { get => oSForm?.IsClosed ?? false; }
        public virtual string Code { get => ModelingProduction?.Code ?? FabProduction.Code; }
        public virtual string Name { get => ModelingProduction?.Name ?? FabProduction.Name; }
        public virtual int Version { get => ModelingProduction?.Version ?? FabProduction.Version; }
        public virtual string sVersion 
        {
            get { return (FabProduction != null) ? FabProduction.Version.ToString() + "-" + FabProduction.Subversion.ToString() : ""; }
        }
        public virtual int StorageCondition { get => ModelingProduction?.StorageCondition ?? FabProduction.StorageCondition; }
        public virtual string sStorageCondition
        {
            get
            {
                try
                {
                    return (ModelingProduction.StorageCondition == 0) ? "room temperature" : "refrigerate";
                }
                catch (Exception)
                {
                    return (FabProduction.StorageCondition == 0) ? "room temperature" : "refrigerate"; ;
                }
            }
        }
        public static string MakeGroupCode(string No, int GroupNo)
        {
            string sDummy = "0";
            string sGrouoNo = GroupNo.ToString();
            return No + "X" + sDummy.PadLeft(13 - No.Length - sGrouoNo.Length - 1, '0') + sGrouoNo;
        }

        public virtual bool IsEmptyRawMaterialReq
        {
            get
            {
                if (OPID == "PLAN") return true;
                if (this.FabProduction == null || this.FabProduction.FormulationObj == null) return true;
                if (OPID == "INIT")
                    foreach (var item in this.FabProduction.FormulationObj.RawMaterialReqItems)
                        if (item.ReqWeight > 0 || item.ApplyWeight > 0)
                            return false;
                return true;

            }
        }
        public virtual bool BeforeBlending { get; set; } = false;
        public virtual bool Withdrawable { get => (WipStatus != Constant.PROC) && (OPID.Equals("1000") || OPID.Equals("1100"));  }

        // Memory object
        public virtual IList<PMDetailApplyLog> oLabelPMs { get; set; }

        // add IMESSequence Feature
        public virtual int Sequence { get; set; }
        public static void RearrangeSeq(List<Lot> ObjCol)
        {
            try
            {
                if (ObjCol == null) return;
                for (int i = 1; i <= ObjCol.Count; i++)
                    ObjCol[i].Sequence = i;
            }
            catch (Exception ex) { throw ex; }
        }

    }
}

