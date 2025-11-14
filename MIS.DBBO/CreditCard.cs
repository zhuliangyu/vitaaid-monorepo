using System.Text.RegularExpressions;

namespace MIS.DBBO
{
  public class CreditCard
  {
    private string _CartType = "";
    public virtual string CardType {
      get
      {
        if (string.IsNullOrEmpty(_CartType))
          _CartType = FindType(CardNumber);
        return _CartType;
      }
      set 
      {
        _CartType = value;
      }
    }
    public virtual string CardHolder { get; set; }
    public virtual string CardNumber { get; set; }
    public virtual string CardNumber2 { get => FormatCardNumber(CardNumber); set { } }
    public virtual string ExpiryDate { get; set; }
    public virtual string ExpiryDate2 { get => (string.IsNullOrEmpty(ExpiryDate)) ? "" : ExpiryDate.Replace("/", ""); set { } }
    public virtual string CardCID { get; set; }
    public virtual void CopyTo(CreditCard oTarget)
    {
      oTarget.CardType = CardType;
      oTarget.CardHolder = CardHolder;
      oTarget.CardNumber = CardNumber;
      oTarget.ExpiryDate = ExpiryDate;
      oTarget.CardCID = CardCID;
    }
    public static string FormatCardNumber(string CardNumber)
    {
      return (string.IsNullOrEmpty(CardNumber)) ? "" : CardNumber.Replace("-", " ");
    }
    public static string ExpiryDate_RemoveExtraChar(string ExpiryDate)
    {
      return (string.IsNullOrEmpty(ExpiryDate)) ? "" : ExpiryDate.Replace("/", "");
    }
    public static string FindType(string CardNumber)
    {
      if (string.IsNullOrEmpty(CardNumber))
        return "";
      //https://www.regular-expressions.info/creditcard.html
      if (Regex.Match(CardNumber, @"^4[0-9]{12}(?:[0-9]{3})?$").Success)
      {
        return "VISA";
      }

      if (Regex.Match(CardNumber, @"^(?:5[1-5][0-9]{2}|222[1-9]|22[3-9][0-9]|2[3-6][0-9]{2}|27[01][0-9]|2720)[0-9]{12}$").Success)
      {
        return "MASTER";
      }

      if (Regex.Match(CardNumber, @"^3[47][0-9]{13}$").Success)
      {
        return "AE";
      }
      return ("");
    }

  }
}
