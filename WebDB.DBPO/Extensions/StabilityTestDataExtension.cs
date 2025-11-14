using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDB.DBBO;
using MySystem.Base.Extensions;

namespace WebDB.DBPO.Extensions
{
    public static class StabilityTestDataExtension
    {
        static string[] TargetOfChemicalTest = new[] { "LEAD", "CADMIUM", "ARSENIC", "TOTAL MERCURY" };

        public static void TestLimitInfoFromSpec(this StabilityTestData self)
        {
            if (self.GroupName.StartsWith("Critical Ingredient"))
                self.TestLimitInfoFromCriticalSpec();
            else if (self.GroupName.StartsWith("Chemical"))
                self.TestLimitInfoFromChemicalSpec();
        }

        // example : 12 mcg 90 ~ 125%
        public static bool TryParseNumberResult(string TestResult, out double numResult)
        {
            numResult = 0;
            try
            {
                var num = TestResult.Split(' ')[0].Trim();//.ToArray().TakeWhile(x => x == '.' || (x >= '0' && x <= '9')).ToArray();
                if (num.Length == 0) 
                    return false;
                return Double.TryParse(num, out numResult);
            }
            catch (Exception)
            {
                return false;
            }
        }
        private static void TestLimitInfoFromCriticalSpec(this StabilityTestData self)
        {
            try
            {
                string SpecUnit;
                double LowestLimit, HighestLimit;
                bValidDefForCriticalSpec(self.TestSpec, out SpecUnit, out LowestLimit, out HighestLimit);
                self.SpecUnit = SpecUnit;
                self.LowestLimit = LowestLimit;
                self.HighestLimit = HighestLimit;

                double numResult = 0;
                if (TryParseNumberResult(self.Result0, out numResult))// = Double.Parse(new string(self.Result0.ToArray().TakeWhile(x => x == '.' || (x >= '0' && x <= '9')).ToArray()));
                    self.NumericResult = numResult;
            }
            catch (Exception)
            {
                return;
            }

        }
        public static bool bValidDefForCriticalSpec(string TestSpec, out string SpecUnit, out double LowestLimit, out double HighestLimit)
        {
            SpecUnit = "";
            LowestLimit = 0;
            HighestLimit = 0;
            try
            {
                if (TestSpec.Contains('~') == false && TestSpec.Contains('%') == false)
                    return false;
                string firstPart = "", secondPart = "";
                TestSpec.Split('~').Also(x =>
                {
                    firstPart = x[0].Trim();  // 12 mcg 90
                    secondPart = x[1].Trim(); // 125%
                });

                var idxOfLowestLimit = firstPart.LastIndexOfAny(new[] { ' ', '\n', '\t' });
                var idxOfUnit = firstPart.IndexOf(' ');
                var lowestLimitPercent = Double.Parse(firstPart.Substring(idxOfLowestLimit + 1)); // 90
                var HighestLimitPercent = Double.Parse(secondPart.Substring(0, secondPart.IndexOf('%')).Trim()); // 125

                var specValue = Double.Parse(firstPart.Substring(0, idxOfUnit)); // 12
                SpecUnit = firstPart.Substring(idxOfUnit + 1, idxOfLowestLimit - idxOfUnit).Trim();
                LowestLimit = Math.Round(specValue * lowestLimitPercent * 0.01, 2);
                HighestLimit = Math.Round(specValue * HighestLimitPercent * 0.01, 2);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        private static void TestLimitInfoFromChemicalSpec(this StabilityTestData self)
        {
            try
            {
                double HighestLimit;
                bValidDefForChemicalSpec(self.TestName, self.TestSpec, out HighestLimit);
                self.HighestLimit = HighestLimit;

                double numResult = 0;
                if (TryParseNumberResult(self.Result0, out numResult))// = Double.Parse(new string(self.Result0.ToArray().TakeWhile(x => x == '.' || (x >= '0' && x <= '9')).ToArray()));
                    self.NumericResult = numResult;
            }
            catch (Exception)
            {
                return;
            }
        }
        public static bool bValidDefForChemicalSpec(string TestName, string TestSpec, out double HighestLimit)
        {
            HighestLimit = 0.0;
            try
            {
                if (TargetOfChemicalTest.Contains(TestName.ToUpper()) == false)
                    return true;

                if (TestSpec.StartsWith("<") == false)
                    return false;

                string firstPart = TestSpec.Substring(1).Trim().Split(' ')[0];
                HighestLimit = Double.Parse(firstPart);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
