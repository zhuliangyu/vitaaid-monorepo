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
    public class TestSpecValidationRule : ValidationRule
    {
        public StabilityTestData TestData { get; set; } = null;
        public TestSpecValidationRule(StabilityTestData std): base()
        {
            TestData = std;
        }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult validResult = new ValidationResult(true, null);
            TestData?.Also(x =>
            {
                if (x.GroupName.StartsWith("Critical Ingredient"))
                {
                    string SpecUnit;
                    double LowestLimit, HighestLimit;
                    TestData.bValidSpec = StabilityTestDataExtension.bValidDefForCriticalSpec(value as string, out SpecUnit, out LowestLimit, out HighestLimit);
                    if (TestData.bValidSpec == false || ((LowestLimit == 0 && HighestLimit == 0) || LowestLimit > HighestLimit || string.IsNullOrWhiteSpace(SpecUnit)))
                        validResult = new ValidationResult(false, "Example format: 12 mcg 90 ~ 125%");

                }
                else if (x.GroupName.StartsWith("Chemical"))
                {
                    double HighestLimit;
                    TestData.bValidSpec = StabilityTestDataExtension.bValidDefForChemicalSpec(TestData.TestName, value as string, out HighestLimit);
                    if (TestData.bValidSpec == false)
                        validResult = new ValidationResult(false, "Example format: < 10 ppm");

                }
            });
            return validResult;
        }
    }
}
