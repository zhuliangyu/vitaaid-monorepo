using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WebDB.DBBO;
using MySystem.Base.Extensions;
using WebDB.DBPO.Extensions;

namespace WebDB.DBPO.ValidationRules
{
    public class TestResultValidationRule : ValidationRule
    {
        public StabilityTestData TestData { get; set; } = null;
        public TestResultValidationRule(StabilityTestData std) : base()
        {
            TestData = std;
        }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult validResult = new ValidationResult(true, null);
            TestData?.Also(x =>
            {
                double numResult = 0;
                if (x.GroupName.StartsWith("Critical Ingredient"))
                {
                    TestData.bValidResult = StabilityTestDataExtension.TryParseNumberResult(value as string, out numResult);
                    if (TestData.bValidResult == false)
                        validResult = new ValidationResult(false, "Example format: 13.5 mcg");
                }
            });
            return validResult;
        }
    }
}
