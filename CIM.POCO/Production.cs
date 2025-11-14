using MyHibernateUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using PropertyChanged;
using MySystem.Base;

namespace POCO
{
    [Serializable]
    public class Production : DataElement
    {
        public Production() { }
        public virtual int ID { get; set; }
        public override int getID() => ID;
        public virtual Formulation FormulationObj { get; set; }
        public virtual Route RouteObj { get; set; }
        public virtual string Code { get; set; } = "VA";
        public virtual int Version { get; set; } = 1;
        public virtual string Name { get; set; }
        public virtual int Subversion { get; set; } = 0;
        public virtual string NPN { get; set; }
        public virtual string Description { get; set; }
        public virtual bool isActive { get; set; } = false;
        public virtual bool isOEM { get; set; } = false;
        public virtual bool StabilityTest { get; set; } = true;
        public virtual eFORMTYPE FormType { get; set; } = eFORMTYPE.CAPSULE;
        public virtual IList<EQAssignment> oEQRecipes { get; set; } = new List<EQAssignment>();

        private NSeqList<TestDefinition> _oTestItems;
        public virtual NSeqList<TestDefinition> oTestItems
        {
            get
            {
                if (_oTestItems == null)
                    _oTestItems = new NSeqList<TestDefinition>();
                return _oTestItems;
            }
            set { _oTestItems = value; }
        }

        public virtual double ShelfLife { get; set; } = 48.0;
        public virtual int StorageCondition { get; set; } = 0;
        public virtual string sStorageCondition
        {
            get => (StorageCondition == 0) ? "room temperature" : "refrigerate";
        }

        private int _DailyDose = 1;
        public virtual int DailyDose { get; set; } = 1;
        public virtual double BulkDensity { get; set; } = 1.0;

        //Memory Object
        public virtual StabilityForm oActiveSForm { get; set; }
        public virtual IList<StabilityForm> oPrevSForms { get; set; } = new List<StabilityForm>();
        public virtual IList<ProductPackageReq> oPackageReqs { get; set; }
        public virtual IList<StabilityForm> oSForms { get; set; } = new List<StabilityForm>();
    }
}
