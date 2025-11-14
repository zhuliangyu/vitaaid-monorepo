using MyHibernateUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using PropertyChanged;

namespace POCO
{
    [Serializable]
    public class UnitType : DataElement
    {
        public virtual int ID { get; set; }
        public override int getID() => ID;
        public virtual string Name { get; set; } = "";
        private string _AbbrName;
        public virtual string AbbrName { get; set; } = "";
        public virtual eUNITTYPE uType { get; set; }
        public virtual double Multiply { get; set; } = 1;
        public virtual string Comment { get; set; }
        public override string ToString()
        {
            return AbbrName;
        }

        public virtual string ToCmbItemStr
        {
            get
            {
                return AbbrName + ": " + Name;
            }
        }
    }
}
