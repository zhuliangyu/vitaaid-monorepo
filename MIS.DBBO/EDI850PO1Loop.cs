using System;
using MyHibernateUtil;

namespace MIS.DBBO
{
    [Serializable]
    public class EDI850PO1Loop : POCOBase
    {
        public virtual int ID { get; set; }
        public virtual VAOrder oVAOrder { get; set; }
        public virtual string VAPONo { get => oVAOrder?.PONo ?? ""; set { } }
        public virtual string ST_TransactionSetControlNumber_02 { get; set; }
        public virtual string BEG_Date_05 { get; set; }
        public virtual string BEG_PurchaseOrderNumber_03 { get; set; }
        public virtual string PO1_AssignedIdentification_01 { get; set; }
        public virtual string PO1_Quantity_02 { get; set; }
        public virtual string PO1_UnitPrice_04 { get; set; }
        public virtual string PO1_UnitorBasisforMeasurementCode_03 { get; set; }
        public virtual string PO1_ProductServiceIDQualifier_06 { get; set; }
        public virtual string PO1_ProductServiceID_07 { get; set; }
        public virtual string PO1_ProductServiceIDQualifier_08 { get; set; }
        public virtual string PO1_ProductServiceID_09 { get; set; }
        public virtual string PID_ItemDescriptionType_01 { get; set; }
        public virtual string PID_ProductProcessCharacteristicCode_02 { get; set; }
        public virtual string PID_Description_05 { get; set; }
        public virtual DateTime UpdatedDate { get; set; } = DateTime.Now;
        public virtual string UpdatedID { get; set; }
        public override int getID()
        {
            return ID;
        }
    }
}
