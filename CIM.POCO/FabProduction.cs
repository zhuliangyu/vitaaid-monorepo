using MyHibernateUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using PropertyChanged;

namespace POCO
{
    [Serializable]
    public class FabProduction : DataElement
    {
        public FabProduction() { }
        public virtual int ID { get; set; }
        public override int getID() => ID;
        public virtual FabFormulation FormulationObj { get; set; }
        public virtual Route RouteObj { get; set; }
        public virtual string Code { get; set; } = "VA";
        public virtual int Version { get; set; } = 1;
        public virtual string Name { get; set; }
        public virtual int Subversion { get; set; } = 0;
        public virtual string NPN { get; set; }
        public virtual string Description { get; set; }
        public virtual IList<FabEQAssignment> oEQRecipes { get; set; } = new List<FabEQAssignment>();
        public virtual double ShelfLife { get; set; } = 48.0;
        public virtual int StorageCondition { get; set; } = 0;
        public virtual string sStorageCondition { get => (StorageCondition == 0) ? "room temperature" : "refrigerator"; set { } }

        public virtual int DailyDose { get; set; } = 1;
        private double _BulkDensity = 1.0;
        public virtual double BulkDensity
        {
            get { return _BulkDensity; }
            set
            {
                _BulkDensity = value;
                OnPropertyChanged("BulkDensity");
            }
        }
    }
}
