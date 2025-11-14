using NHibernate;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHibernateUtil.Extensions
{
  public static class IQueryExtensions
  {
    public static IList<T> ToTupleList<T>(this IQuery self)
    {
      return self.SetResultTransformer(Transformers.AliasToBeanConstructor(typeof(T).GetConstructors()[0])).List<T>();
    }
    public static ObservableCollection<T> ToObservableCollection<T>(this IQueryable<T> self)
    {
      return new ObservableCollection<T>(self.ToList<T>());
    }

    /*
    public static IList<(T1, T2)> ToList<T1, T2>(this IQuery self)
    {
        var constructor = typeof((T1, T2)).GetConstructor(new[] { typeof(T1), typeof(T2) });
        return self.SetResultTransformer(Transformers.AliasToBeanConstructor(constructor)).List<(T1, T2)>();
    }
    public static IList<(T1, T2, T3)> ToList<T1, T2, T3>(this IQuery self)
    {
        var constructor = typeof((T1, T2, T3)).GetConstructor(new[] { typeof(T1), typeof(T2), typeof(T3) });
        return self.SetResultTransformer(Transformers.AliasToBeanConstructor(constructor)).List<(T1, T2, T3)>();
    }

    public static IList ToList<T1, T2, T3>(this IQuery self)
    {
        var constructor = typeof(Tuple<T1, T2, T3>).GetConstructor(new[] { typeof(T1), typeof(T2), typeof(T3) });
        return self.SetResultTransformer(Transformers.AliasToBeanConstructor(constructor)).List();
    }
    public static IList ToList<T1, T2, T3, T4>(this IQuery self)
    {
        var constructor = typeof(Tuple<T1, T2, T3, T4>).GetConstructor(new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) });
        return self.SetResultTransformer(Transformers.AliasToBeanConstructor(constructor)).List();
    }
    public static IList ToList<T1, T2, T3, T4, T5>(this IQuery self)
    {
        var constructor = typeof(Tuple<T1, T2, T3, T4, T5>).GetConstructor(new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5) });
        return self.SetResultTransformer(Transformers.AliasToBeanConstructor(constructor)).List();
    }
    */
  }
}
