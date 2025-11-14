using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Properties;
using NHibernate.Transform;

namespace MyHibernateUtil
{
    [Serializable]
    public class DictionaryResultTransformer : IResultTransformer
    {

        public DictionaryResultTransformer()
        {

        }

        #region IResultTransformer Members

        public IList TransformList(IList collection)
        {
            return collection;
        }

        public object TransformTuple(object[] tuple, string[] aliases)
        {
            var result = new Dictionary<string, object>();
            for (int i = 0; i < aliases.Length; i++)
            {
                result[aliases[i]] = tuple[i];
            }
            return result;
        }

        #endregion
    }
}
