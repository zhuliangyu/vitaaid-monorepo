using MySystem.Base.Extensions;
using POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DBPO
{
    public static class StabilityTestDataHelper
    {
        public static IList<StabilityTestData> SortTestData(IList<StabilityTestData> TestDataList)
        {

            var PhysicalTDs = new List<StabilityTestData>();
            var PerformanceTDs = new List<StabilityTestData>();
            var ChemicalTDs = new List<StabilityTestData>();
            var MicrobiologicalTDs = new List<StabilityTestData>();
            var MiscTDs = new List<StabilityTestData>();
            var CriticalTDs = new List<StabilityTestData>();
            var otherTDs = new List<StabilityTestData>();
            var sortedList = new List<StabilityTestData>();
            TestDataList.Action(x =>
            {
                if (x.GroupName.StartsWith("Physical"))
                    PhysicalTDs.Add(x);
                else if (x.GroupName.StartsWith("Performance"))
                    PerformanceTDs.Add(x);
                else if (x.GroupName.StartsWith("Chemical"))
                    ChemicalTDs.Add(x);
                else if (x.GroupName.StartsWith("Microbiological"))
                    MicrobiologicalTDs.Add(x);
                else if (x.GroupName.StartsWith("Critical"))
                    CriticalTDs.Add(x);
                else if (x.GroupName.StartsWith("Misc"))
                    MiscTDs.Add(x);
                else
                    otherTDs.Add(x);
            });
            sortedList.AddRange(PhysicalTDs.OrderBy(x => x.ID));
            sortedList.AddRange(PerformanceTDs.OrderBy(x => x.ID));
            sortedList.AddRange(ChemicalTDs.OrderBy(x => x.ID));
            sortedList.AddRange(MicrobiologicalTDs.OrderBy(x => x.ID));
            sortedList.AddRange(otherTDs.OrderBy(x => x.GroupName).ThenBy(x => x.ID));
            sortedList.AddRange(MiscTDs.OrderBy(x => x.ID));
            sortedList.AddRange(CriticalTDs);
            return sortedList;
        }
    }
}
