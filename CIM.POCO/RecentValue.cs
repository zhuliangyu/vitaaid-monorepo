using MyHibernateUtil;
using System;


namespace POCO
{
    [Serializable]
    public class RecentValue : POCOBase, IEquatable<RecentValue>
    {
        public RecentValue() { }
        public RecentValue(string tag, string str)
        {
            Tag = tag;
            sValue = str;
            sZipValue = (sValue != null && sValue.Length > 0) ? sValue.Replace(" ", string.Empty).Replace("\t", string.Empty).ToLower() : "";
        }
        public virtual int ID { get; set; }
        public virtual string Tag { get; set; }
        public virtual string sValue { get; set; }
        public virtual string sZipValue { get; set; }
        public virtual int RefCnt { get; set; } = 1;

        public virtual bool Equals(RecentValue other)
        {
            if (other == null) return false;
            return sZipValue.Equals(other.sZipValue);
        }
        public override int getID() => ID;
    }
}
