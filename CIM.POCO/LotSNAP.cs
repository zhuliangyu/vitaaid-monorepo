using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace POCO
{
    [Serializable]
    public class LotSNAP : LotBase
    {
        public virtual string FormPartInNo
        {
            get => this.GetFormPart();
            set
            {
                this.SetFormPart(value);
                OnPropertyChanged("No");
            }
        }
    }
}
