using System;
using MyHibernateUtil;
using MySystem.Base;

namespace MIS.DBBO
{
    [Serializable]
    public class EDIHLItem : POCOBase
    {
        public virtual int ID { get; set; }
        public virtual AS2Log oAS2Log { get; set; }
        public virtual string PONo850 { get; set; }
        public virtual VAOrder oVAOrder { get; set; }
        public virtual string VAPONo { get => oVAOrder?.PONo ?? ""; set { } }
        public virtual GS1ShippingLabel oGS1ShippingLabel { get; set; }
        public virtual string SSCC { get => oGS1ShippingLabel?.SSCC ?? ""; set { } }
        public virtual string TrackingNumber { get => oGS1ShippingLabel?.TrackingNumber ?? ""; set { } }
        public virtual VAOrderItemByLot oOrderItemByLot { get; set; }
        public virtual string Name { get => oOrderItemByLot?.oItemOwner?.ItemName ?? ""; set { } }
        public virtual string Code { get => oOrderItemByLot?.oItemOwner.ItemCode ?? ""; set { } }
        public virtual string RetailLotNo { get => oOrderItemByLot?.RetailLotNo ?? ""; set { } }
        public virtual DateTime ExpiredDate { get => oOrderItemByLot?.ExpiredDate ?? new DateTime(2050, 12, 31); set { } }
        public virtual int ShippedQty { get; set; }
        public virtual string LIN01 { get; set; }
        public virtual string CONTENT => $"{ShippedQty}";
        public override string ToString() => CONTENT;
        // memory data
        public virtual object Tag { get; set; } = (int)0;
        public virtual eOPSTATE prevState { get; set; }
        public virtual bool IsDelete
        {
            get => iState == eOPSTATE.DELETE;
            set
            {
                if (value == false)
                    iState = prevState;
                else
                {
                    prevState = iState;
                    iState = eOPSTATE.DELETE;
                }
            }
        }
        public virtual string BackupData { get; set; } = "";
        public virtual DateTime CreatedDate { get; set; } = DateTime.Now;
        public virtual string CreatedID { get; set; }
        public override int getID()
        {
            return ID;
        }

        // memory object
        public virtual object oTS850 { get; set; } = null;
    }
}
