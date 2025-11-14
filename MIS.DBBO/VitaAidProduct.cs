using System;
using MyHibernateUtil;
using MySystem.Base.Extensions;

namespace MIS.DBBO
{
    public class VitaAidProduct : VitaAidProductBase
    {
        public virtual string Supplier { get; set; }
        public virtual VitaAidProduct oParentProduct { get; set; }
        public virtual void Refresh()
        {
            OnPropertyChanged("Desc");
        }
        // memory object
        public virtual bool bNACompany { get; set; } = true;// true: NaturoAID, false: VitaAid 
        public virtual bool bVAStock { get => !bNACompany; }
        public virtual bool bQCQuantity { get; set; } = false;
        public virtual bool bWIP { get; set; } = false;
        public virtual object oLot { get; set; }

    }
}
