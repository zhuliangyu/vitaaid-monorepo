using System.Globalization;
using System.Windows.Controls;

namespace MyToolkit.Base.Validations
{
    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
                return new ValidationResult(false, "Field is required.");
            if (value is string && ((string)value).Length == 0)
                return new ValidationResult(false, "Field is required.");
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
                ? new ValidationResult(false, "Field is required.")
                : ValidationResult.ValidResult;
        }
    }
}
