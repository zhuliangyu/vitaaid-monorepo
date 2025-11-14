using System.Windows.Controls;

namespace MyToolkit.Base.Validations
{
    public class NoEmptySelectionValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value == null)
                return new ValidationResult(false, "Please select one");
            else
                return new ValidationResult(true, null);
        }
    }
}
