using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using MyHibernateUtil;

namespace MIS.DBBO
{
    public class TraidingPartnerConfig : POCOBase
    {
        public virtual int ID { get; set; } = 0;
        // VENDOR NUMBERS
        public virtual string FILE_NAME { get; set; } = "";
        public virtual string VENDOR_NUMBER { get; set; } = "NAA";
        public virtual string EDI_QUALIFIER { get; set; } = "ZZ";
        public virtual string EDI_ISA_ID { get; set; } = "";
        public virtual string EDI_GS_ID { get; set; } = "";
        public virtual string INTERCHANGE_STANDARDS_ID { get; set; } = "U"; //ISA11 
        public virtual string INTERCHANGE_VERSION_ID { get; set; } = "5010"; //ISA12
        public virtual string ACT_REQUESTED { get; set; } = "0"; //ISA14, 0: No Acknowledgement, 1: Request Acknowledgement
        public virtual string USAGE_INDICATOR { get; set; } = "P"; //ISA15, T: Test Data	P: Production Data
        public virtual string ISA_CONTROL_NUM { get; set; } = "0";
        public virtual string GS_CONTROL_NUM { get; set; } = "0";
        public virtual string ST_CONTROL_NUM { get; set; } = "0";
        public virtual string CONTENT => $"{VENDOR_NUMBER}*{EDI_QUALIFIER}*{EDI_ISA_ID}*{EDI_GS_ID}*{INTERCHANGE_STANDARDS_ID}*{INTERCHANGE_VERSION_ID}*{ACT_REQUESTED}*{USAGE_INDICATOR}*{ISA_CONTROL_NUM}*{GS_CONTROL_NUM}*{ST_CONTROL_NUM}";

        public override string ToString() => CONTENT;
        public virtual bool OFF_LINE_MODE { get; set; } = false;

        public override int getID()
        {
            return ID;
        }
    }
}
