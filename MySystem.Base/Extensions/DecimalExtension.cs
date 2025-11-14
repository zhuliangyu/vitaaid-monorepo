using System;

namespace MySystem.Base.Extensions
{
    public static class DecimalExtension
    {
        public static string ToCurrencyString(this Decimal self, bool EmptyStrIfZero = true, bool NegativeInParentheses = true, string Symbol = "")
        {
            if (EmptyStrIfZero && self == 0)
                return "";

            string tmpStr = (self >= 0) ? decimal.Round(self, 2, MidpointRounding.AwayFromZero).ToString("F2")
                                        : decimal.Round(self * -1, 2, MidpointRounding.AwayFromZero).ToString("F2");
            if (self >= 0)
                return Symbol + tmpStr;
            else
            {
                if (NegativeInParentheses)
                    return "(" + Symbol + tmpStr + ")";
                else
                    return "-" + Symbol + tmpStr;
            }
        }
    }
}