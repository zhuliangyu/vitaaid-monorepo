using MySystem.Base.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POCO
{
    public static class LotNoHelper
    {
        public static string MakeLotNo(DateTime dt, string FormType, int SequenceNo)
        {
            return EncodeDT(dt) + FormType + SequenceNo.ToString();
        }

        private static int encodeKey = 123456;
        // LotNo Rule:
        //  MMDDYY
        // +123456
        public static int EncodeDT(DateTime dt)
        {
            return dt.Year - 2000 + dt.Day * 100 + dt.Month * 10000 + encodeKey;
        }
        // LotNo Rule:
        //  MMDDYY
        // +123456
        // YYYY = 2000 + value MOD 100
        // MM   = (int)(value / 10000)
        // DD   = ((int)(value / 100)) MOD 100
        public static DateTime DecodeDT(int value)
        {
            value -= encodeKey;
            if (value < 0) return DateTime.Now;
            return new DateTime(2000 + value % 100, (int)(value / 10000), ((int)(value / 100)) % 100);
        }
        public static DateTime GetDatePart(this Lot oLot) => DecodeDT(oLot.No.Substring(0, 6).ToInt());
        public static DateTime GetDatePart(this VirtualLotInfo oLot) => DecodeDT(oLot.VirtualLotNo.Substring(0, 6).ToInt());
        public static void SetDatePart(this Lot oLot, DateTime value)
        {
            if (value == null) value = DateTime.Now;
            if (oLot.No == null || oLot.No.Length < 6)
                oLot.No = EncodeDT(value).ToString();
            else
                oLot.No = EncodeDT(value).ToString() + oLot.No.Substring(6);
        }
        public static void SetDatePart(this VirtualLotInfo oLot, DateTime value)
        {
            if (value == null) value = DateTime.Now;
            if (oLot.VirtualLotNo == null || oLot.VirtualLotNo.Length < 6)
                oLot.VirtualLotNo = EncodeDT(value).ToString();
            else
                oLot.VirtualLotNo = EncodeDT(value).ToString() + oLot.VirtualLotNo.Substring(6);
        }

        public static string GetFormPart(this Lot oLot) => new string(oLot.No.Substring(6).TakeWhile(x => x < '0' || x > '9').ToArray());
        public static string GetFormPart(this LotSNAP oLot) => new string(oLot.No.Substring(6).TakeWhile(x => x < '0' || x > '9').ToArray());
        public static string GetFormPart(this VirtualLotInfo oLot) => new string(oLot.VirtualLotNo.Substring(6).TakeWhile(x => x < '0' || x > '9').ToArray());
        public static void SetFormPart(this Lot oLot, string value) => oLot.No = oLot.No.Substring(0, 6) + value + oLot.GetSerialPart();
        public static void SetFormPart(this LotSNAP oLot, string value) => oLot.No = oLot.No.Substring(0, 6) + value + oLot.GetSerialPart();
        public static void SetFormPart(this VirtualLotInfo oLot, string value) => oLot.VirtualLotNo = oLot.VirtualLotNo.Substring(0, 6) + value + oLot.GetSerialPart();

        public static string GetSerialPart(this Lot oLot) => new string(oLot.No.Substring(6).SkipWhile(x => x < '0' || x > '9').ToArray());
        public static string GetSerialPart(this LotSNAP oLot) => new string(oLot.No.Substring(6).SkipWhile(x => x < '0' || x > '9').ToArray());
        public static string GetSerialPart(this VirtualLotInfo oLot) => new string(oLot.VirtualLotNo.Substring(6).SkipWhile(x => x < '0' || x > '9').ToArray());

        public static void SetSerialPart(this Lot oLot, string value) => oLot.No = oLot.No.Substring(0, 6) + oLot.GetFormPart() + value;
        public static void SetSerialPart(this LotSNAP oLot, string value) => oLot.No = oLot.No.Substring(0, 6) + oLot.GetFormPart() + value;
        public static void SetSerialPart(this VirtualLotInfo oLot, string value) => oLot.VirtualLotNo = oLot.VirtualLotNo.Substring(0, 6) + oLot.GetFormPart() + value;
        public static bool bValidLotNo(this Lot oLot)
        {
            if (string.IsNullOrWhiteSpace(oLot.No) || oLot.No.Length < 8)
                return false;
            try
            {
                var d = oLot.GetDatePart();
                if (string.IsNullOrWhiteSpace(oLot.GetFormPart()))
                    return false;

                var serialNo = oLot.GetSerialPart();
                if (string.IsNullOrWhiteSpace(serialNo))
                    return false;
                if (serialNo.ToInt() == 0)
                    return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool bValidLotNo(this VirtualLotInfo oLot)
        {
            if (string.IsNullOrWhiteSpace(oLot.VirtualLotNo) || oLot.VirtualLotNo.Length < 8)
                return false;
            try
            {
                var d = oLot.GetDatePart();
                if (string.IsNullOrWhiteSpace(oLot.GetFormPart()))
                    return false;

                var serialNo = oLot.GetSerialPart();
                if (string.IsNullOrWhiteSpace(serialNo))
                    return false;
                if (serialNo.ToInt() == 0)
                    return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
