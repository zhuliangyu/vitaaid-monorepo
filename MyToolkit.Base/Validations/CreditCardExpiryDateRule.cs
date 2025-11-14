using System;
using System.Globalization;
using System.Windows.Controls;

namespace MyToolkit.Base.Validations
{
    public class CreditCardExpiryDateRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is string && string.IsNullOrEmpty((string)value) == false)
            {
                try
                {
                    string[] sVals = ((string)value).Split('/');
                    int iVal = 0;
                    if (sVals.Length != 2 ||
                        //(sVals[0][0] != '0' && sVals[0][0] != '1') ||
                        Int32.TryParse(sVals[0], out iVal) == false ||
                        iVal < 1 || iVal > 12 ||
                        Int32.TryParse(sVals[1], out iVal) == false)
                        return new ValidationResult(false, "Expiration date is invalid ");
                }
                catch (Exception)
                {
                    return new ValidationResult(false, "Expiration date is invalid ");
                }
            }
            return new ValidationResult(true, null);
        }
    }
}
