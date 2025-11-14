using POCO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CIM.DBPO
{
  public class ReProcessValidationRule : ValidationRule
  {
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
      if (value == null || !(value is eREPROCESS) || (eREPROCESS)value == eREPROCESS.UNDEFINED)
        return new ValidationResult(false, "Please select one");
      else
        return new ValidationResult(true, null);
    }
  }
}
