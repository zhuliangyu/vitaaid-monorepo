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
    public class Lot : LotBase
    {
        public virtual DateTime DatePartInNo
        {
            get => this.GetDatePart();
            set
            {
                this.SetDatePart(value);
                OnPropertyChanged("No");
            }
        }
        public virtual string FormPartInNo
        {
            get => this.GetFormPart();
            set
            {
                this.SetFormPart(value);
                OnPropertyChanged("No");
            }
        }
        public virtual string SearialPartInNo
        {
            get => this.GetSerialPart();
            set
            {
                this.SetSerialPart(value);
                OnPropertyChanged("SearialPartInNo");
                OnPropertyChanged("No");
            }
        }
        public virtual Lot newSubGroup(int GroupNo)
        {
            Lot subGroup = new Lot()
            {
                No = this.No,
                LotSize = this.LotSize,
                BatchCount = this.BatchCount,
                FabProduction = this.FabProduction,
                ModelingProduction = this.ModelingProduction,
                OPID = this.OPID,
                GroupCode = Lot.MakeGroupCode(this.No, GroupNo),
                GroupSize = this.GroupSize,
                GroupBatchCount = this.GroupBatchCount,
                GroupStartOPID = this.GroupStartOPID,
                GroupEndOPID = this.GroupEndOPID,
                GroupRevision = this.GroupRevision,
                oParentLot = this,
                AutoMerge = this.AutoMerge,
                PreparedDate = this.PreparedDate,
                PreparedPMFinish = this.PreparedPMFinish,
                PreparedPMOPID = this.PreparedPMOPID,
                ReProducedFrom = this.ReProducedFrom,
                RetailLotNo = this.RetailLotNo,
                SampleBatch = this.SampleBatch,
                WipStatus = this.WipStatus,
                Status = this.Status,
                PreStatus = this.PreStatus,
                TargetControlLimit = this.TargetControlLimit,
                TargetToleranceLimit = this.TargetToleranceLimit,
                VarControlLimit = this.VarControlLimit,
                VarToleranceLimit = this.VarToleranceLimit,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };
            SubGroupList.Add(subGroup);
            return subGroup;
        }
        public virtual void AddTo(List<Lot> TargetCol)
        {
            try
            {
                Sequence = TargetCol.Count + 1;
                TargetCol.Add(this);
            }
            catch (Exception ex) { throw ex; }
        }
    }
}

