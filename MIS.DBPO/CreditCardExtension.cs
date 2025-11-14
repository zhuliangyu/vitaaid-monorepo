
using MIS.DBBO;
using System.Text.RegularExpressions;
using MySystem.Base.Extensions;

namespace MIS.DBPO
{
  public static class CreditCardExtension
  {
    public static bool CardNumEqual(this CreditCard self, string num2)
    {
      var s1 = CreditCard.FormatCardNumber(self.CardNumber).Replace(" ", "");
      var s2 = CreditCard.FormatCardNumber(num2).Replace(" ", "");
      return s1 == s2;
    }
    public static bool Equals(this CreditCard self, string CardNumber, string ExpirtDate, string CID)
    {
      return  self.CardNumEqual(CardNumber) &&
              self.ExpiryDate2.LogicalEqual(CreditCard.ExpiryDate_RemoveExtraChar(ExpirtDate)) &&
              self.CardCID.LogicalEqual(CID);
    }
  }
}
