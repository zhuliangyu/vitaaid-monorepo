using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MyHibernateUtil;
using MySystem.Base;

namespace MIS.DBBO
{
    [Serializable]
    public class GS1ShippingLabel : POCOBase
    {
        public virtual int ID { get; set; }
        public virtual AS2Log oAS2Log { get; set; }
        public virtual string PONo850 { get; set; }
        public virtual VAOrder oVAOrder { get; set; }
        public virtual string VAPONo { get => oVAOrder?.PONo ?? ""; set { } }
        public virtual DateTime CreatedDate { get; set; } = DateTime.Now;
        public virtual string CreatedID { get; set; }
        public virtual string SSCC { get; set; }
        public virtual double GrossWeight { get; set; }
        public virtual string UnitOfMeasurement { get; set; } = "LB";
        public virtual string CarrierCode { get; set; } = "USPS";
        public virtual string CarrierRouting { get; set; } = "United States Postal Service";
        public virtual string TrackingNumber { get; set; }
        public virtual IList<EDIHLItem> oEDIHLItems { get; set; } = new List<EDIHLItem>();

        public virtual string ShipToName { get; set; } = "";
        public virtual string ShipToLocationNum { get; set; } = "";
        public virtual string ShipToAddress1 { get; set; } = "";
        public virtual string ShipToAddress2 { get; set; } = "";
        public virtual string ShipToCityName { get; set; } = "";
        public virtual string ShipToState { get; set; } = "";
        public virtual string ShipToPostalCode { get; set; } = "";
        public virtual string ShipToCountry { get; set; } = "";
        public virtual string BillOfLading { get; set; } = "";
        public virtual int CountOfPack { get; set; }
        public virtual int TotalOfPack { get; set; }
        public virtual string ItemCode { get; set; } = "";
        public virtual string ItemDesc { get; set; } = "";
        public virtual int QuanityPerCarton { get; set; }
        public virtual string CONTENT => $"{SSCC}*{GrossWeight}*{UnitOfMeasurement}*{CarrierCode}*{CarrierRouting}*{TrackingNumber}";
        public override string ToString() => CONTENT;
        // memory data
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
        public virtual void UpdateCalculatedData()
        {
            try
            {
                var activeItems = oEDIHLItems.Where(x => x.IsDelete == false);
                ItemCode = (activeItems.Count() == 1) ? activeItems.First().Code : "MIXED";
                ItemDesc = (activeItems.Count() == 1) ? activeItems.First().Name : "MIXED";
                QuanityPerCarton = activeItems.Sum(x => x.ShippedQty);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public virtual string generateLableZPL(string GS1CompanyPrefix)  //  = "081474100"
        {
            try
            {
                string zplTemplate = File.ReadAllText("emerson-template.prn");

                // shipping to, Postal Code & Bar Code
                zplTemplate = zplTemplate.Replace("{N102}", ShipToName);
                zplTemplate = zplTemplate.Replace("{N104}", string.IsNullOrWhiteSpace(ShipToLocationNum) ? "" : "#" + ShipToLocationNum);
                zplTemplate = zplTemplate.Replace("{N301}", ShipToAddress1);
                zplTemplate = zplTemplate.Replace("{N401}", ShipToCityName);
                zplTemplate = zplTemplate.Replace("{N402}", ShipToState);
                zplTemplate = zplTemplate.Replace("{N403}", ShipToPostalCode);
                zplTemplate = zplTemplate.Replace("{N404}", ShipToCountry);

                // Carrier Information
                zplTemplate = zplTemplate.Replace("{TD503}", CarrierCode);
                if (string.IsNullOrEmpty(BillOfLading) == false)
                    zplTemplate = zplTemplate.Replace("{REF02}", "B/L: " + BillOfLading);
                else
                    zplTemplate = zplTemplate.Replace("{REF02}", "Pro: " + TrackingNumber);
                zplTemplate = zplTemplate.Replace("{HLLoopIdx}", CountOfPack.ToString());
                zplTemplate = zplTemplate.Replace("{HLLoopCount}", TotalOfPack.ToString());

                // Customer/Retailer Information
                zplTemplate = zplTemplate.Replace("{PRF01}", PONo850);
                zplTemplate = zplTemplate.Replace("{LIN03}", ItemCode);
                zplTemplate = zplTemplate.Replace("{PID05}", ItemDesc);
                zplTemplate = zplTemplate.Replace("{SN102}", QuanityPerCarton.ToString());

                // Serialized Shipping Container Code - SSCC
                zplTemplate = zplTemplate.Replace("{MAN02}", SSCC17(GS1CompanyPrefix));
                zplTemplate = zplTemplate.Replace("{SSCC18}", SSCC18(GS1CompanyPrefix));
                return zplTemplate;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual string SSCC17(string GS1CompanyPrefix) => "0" + GS1CompanyPrefix + SSCC;
        public virtual string SSCC18(string GS1CompanyPrefix)
        {
            string sSSCC17 = SSCC17(GS1CompanyPrefix);
            if (sSSCC17.Length != 17)
                return "ERROR";
            var ary = sSSCC17.ToArray();
            int[] SSCCMultiply = new int[] { 3, 1, 3, 1, 3, 1, 3, 1, 3, 1, 3, 1, 3, 1, 3, 1, 3 };
            var sum = 0;
            for (int i = 0; i < 17; i++)
                sum += (ary[i] - '0') * SSCCMultiply[i];
            var modValue = (sum % 10);
            return sSSCC17 + ((modValue == 0) ? 0 : 10 - modValue).ToString();
        }
        public virtual string GS1SSCC(string GS1CompanyPrefix) => "00" + SSCC18(GS1CompanyPrefix);
        public override int getID()
        {
            return ID;
        }

        // memory object
        public virtual object oTS850 { get; set; } = null;
    }
}
