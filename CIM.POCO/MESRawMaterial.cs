using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using PropertyChanged;
namespace POCO
{
    [Serializable]
    public class MESRawMaterial : VAMESRawMaterial
    {
        public virtual string QStockLocation { get; set; } = "";
        public virtual string DoubleCheckBy { get; set; } = "";
        // memory object
        public virtual IList<RawMaterialDetail> oRMDetails { get; set; } = new List<RawMaterialDetail>();
        public virtual ObservableCollection<MedicineIngredientUsage> oMIUs { get; set; } = new ObservableCollection<MedicineIngredientUsage>();
        public virtual string bDailyDose { get => oMIUs.Any() ? "V" : ""; }
    }
}
