using System;

namespace MySystem.Base.Reflection
{
    public class Field
    {
        public Field()
        {
        }

        public Field(string name, Type type)
        {
            this.FieldName = name;
            this.FieldType = type;
        }

        public Field(string name, string sDBType)
        {
            this.FieldName = name;
            string lowerDBType = sDBType.ToLower();
            if (sDBType.Contains("char") || lowerDBType.Contains("text"))
                this.FieldType = typeof(string);
            else if (lowerDBType.Contains("int") || lowerDBType == "bit")
                this.FieldType = typeof(Int32);
            else if (lowerDBType == "numeric" || lowerDBType == "float" || lowerDBType == "real")
                this.FieldType = typeof(Double);
            else if (lowerDBType == "decimal" || lowerDBType.Contains("money"))
                this.FieldType = typeof(Decimal);
            else if (lowerDBType.Contains("date") || lowerDBType.Contains("time"))
                this.FieldType = typeof(DateTime);
            else if (lowerDBType.Contains("binary") || lowerDBType == "timestamp")
                this.FieldType = typeof(Byte[]);
        }

        public string FieldName;

        public Type FieldType;
    }
}