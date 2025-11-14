using System;
using System.Collections.Generic;
using System.Dynamic;

namespace MySystem.Base.Reflection
{
    public class DynamicClass : DynamicObject
    {
        private Dictionary<string, KeyValuePair<Type, object>> _fields;

        public DynamicClass(List<Field> fields)
        {
            _fields = new Dictionary<string, KeyValuePair<Type, object>>();
            fields.ForEach(x => _fields.Add(x.FieldName,
                new KeyValuePair<Type, object>(x.FieldType, null)));
        }

        public DynamicClass(List<Field> fields, IList<object> values)
        {
            _fields = new Dictionary<string, KeyValuePair<Type, object>>();
            for (int idx = 0; idx < fields.Count; idx++)
                _fields.Add(fields[idx].FieldName, new KeyValuePair<Type, object>(fields[idx].FieldType, idx < values.Count ? values[idx] : null));
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (_fields.ContainsKey(binder.Name))
            {
                var type = _fields[binder.Name].Key;
                if (value.GetType() == type)
                {
                    _fields[binder.Name] = new KeyValuePair<Type, object>(type, value);
                    return true;
                }
                else throw new Exception("Value " + value + " is not of type " + type.Name);
            }
            return false;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = _fields[binder.Name].Value;
            return true;
        }
    }
}