using MyHibernateUtil;
using System;
using PropertyChanged;
using Newtonsoft.Json;

namespace WebDB.DBBO
{
    [Serializable]
    public class StabilityTestData : DataElement
    {        
        public virtual string GroupName { get; set; }
        public virtual string TestName { get; set; }
        public virtual string TestDesc { get; set; }
        public virtual string TestSpec { get; set; }
        public virtual string TestMethod { get; set; }
        public virtual int ID { get; set; }
        public override int getID() => ID;
        [JsonIgnore]
        public virtual StabilityForm oStabilityForm { get; set; }
        public virtual string Result0 { get; set; }
        public virtual string TestUser0 { get; set; }
        public virtual DateTime TestDate0 { get; set; } = SysVal.NilDate;
        public virtual string RecordedBy0 { get { return (TestDate0.Year == SysVal.NilDate.Year) ? "" : TestUser0; } }//(TestDate0.Year == SysVal.NilDate.Year) ? "" : TestUser0 + " " + TestDate0.ToString("MMM/dd/yyyy"); } }
        public virtual string Result12 { get; set; }
        public virtual string TestUser12 { get; set; }
        public virtual DateTime TestDate12 { get; set; } = SysVal.NilDate;
        public virtual string RecordedBy12 { get { return (TestDate12.Year == SysVal.NilDate.Year) ? "" : TestUser12; } }//(TestDate12.Year == SysVal.NilDate.Year) ? "" : TestUser12 + " " + TestDate12.ToString("MMM/dd/yyyy"); } }
        public virtual string Result24 { get; set; }
        public virtual string TestUser24 { get; set; }
        public virtual DateTime TestDate24 { get; set; } = SysVal.NilDate;
        public virtual string RecordedBy24 { get { return (TestDate24.Year == SysVal.NilDate.Year) ? "" : TestUser24; } }//(TestDate24.Year == SysVal.NilDate.Year) ? "" : TestUser24 + " " + TestDate24.ToString("MMM/dd/yyyy"); } }
        public virtual string Result36 { get; set; }
        public virtual string TestUser36 { get; set; }
        public virtual DateTime TestDate36 { get; set; } = SysVal.NilDate;
        public virtual string RecordedBy36 { get { return (TestDate36.Year == SysVal.NilDate.Year) ? "" : TestUser36; } }//(TestDate36.Year == SysVal.NilDate.Year) ? "" : TestUser36 + " " + TestDate36.ToString("MMM/dd/yyyy"); } }
        public virtual string Result48 { get; set; }
        public virtual string TestUser48 { get; set; }
        public virtual DateTime TestDate48 { get; set; } = SysVal.NilDate;
        public virtual string RecordedBy48 { get { return (TestDate48.Year == SysVal.NilDate.Year) ? "" : TestUser48; } }//(TestDate48.Year == SysVal.NilDate.Year) ? "" : TestUser48 + " " + TestDate48.ToString("MMM/dd/yyyy"); } }
        public virtual string sSuperscript { get; set; } = "";
        public virtual int Sequence { get; set; }
        public virtual eWEBSITE VisibleSite { get; set; } =  eWEBSITE.ALL;
        public virtual double LowestLimit { get; set; } = 0;
        public virtual double HighestLimit { get; set; } = 0;
        public virtual double NumericResult { get; set; } = 0;
        public virtual string SpecUnit { get; set; } = "";

        // Memory Object
        [JsonIgnore]
        public virtual object Tag { get; set; }
        public virtual bool bValidSpec { get; set; } = true;
        public virtual bool bValidResult { get; set; } = true;
    }
}
