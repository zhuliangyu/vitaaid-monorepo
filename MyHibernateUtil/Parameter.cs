using PropertyChanged;

namespace MyHibernateUtil
{
    public class Parameter: POCOBase
    {
        public virtual int ID { get; set; }

        public virtual string AppName { get; set; }
        public virtual string Category { get; set; }
        public virtual string ConfigName { get; set; }
        public virtual double? ConfigVal1 { get; set; }
        public virtual string ConfigVal2 { get; set; } = "";

        public override int getID()
        {
            return ID;
        }

        public static explicit operator int(Parameter oVal) => (oVal.ConfigVal1 != null && oVal.ConfigVal1.HasValue) ? (int)(double)oVal.ConfigVal1 : 0;
        public static explicit operator bool(Parameter oVal) => (oVal.ConfigVal1 != null && oVal.ConfigVal1.HasValue) ? ((int)(double)oVal.ConfigVal1 == 1) : false;
    }
}
