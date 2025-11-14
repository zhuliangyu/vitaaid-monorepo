using System;
using MyHibernateUtil;

namespace MIS.DBBO
{
    public class DiscountTargetProduct : POCOBase
    {
        public virtual int ID { get; set; }
        public virtual DiscountProgram oDiscountProgram { get; set; }
        public virtual string DiscountName { get; set; }
        private string _sProductCode = "";
        private string _sProductName = "";
        public virtual string ProductCode { get { return _sProductCode; } set { _sProductCode = value; } }
        public virtual string ProductName { get { return _sProductName; } set { _sProductName = value; } }
        public virtual DateTime CreatedDate { get; set; }
        public virtual string CreatedID { get; set; }

        public virtual string ToTagString()
        {
            return DiscountTargetProduct.ToTagString(ProductCode, ProductName);
        }
        public virtual void DecodeTagString(string sTagString)
        {
            DiscountTargetProduct.DecodeTagString(sTagString, out _sProductCode, out _sProductName);
        }
        public static string ToTagString(string sProductCode, string sProductName)
        {
            return sProductName + " [" + sProductCode + "]";
        }
        public static void DecodeTagString(string sTagString, out string sProductCode, out string sProductName)
        {
            sProductCode = ""; sProductName = "";
            string[] sTokens = sTagString.Split('[');
            if (sTokens.Length == 2)
            {
                sProductName = sTokens[0].Trim();
                sProductCode = sTokens[1].Substring(0, sTokens[1].IndexOf(']')).Trim();
            }
        }

        public override int getID()
        {
            return ID;
        }
    }
}
