using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MySystem.Base.Extensions
{
  public static class IListExtension
  {
    public static void Swap<T>(this IList<T> self, int idx1, int idx2)
    {
      try
      {
        if (idx1 == idx2) return;
        if (idx1 < 0 || idx1 >= self.Count) return;
        if (idx2 < 0 || idx2 >= self.Count) return;

        T tmp = self[idx1];
        self[idx1] = self[idx2];
        self[idx2] = tmp;
      }
      catch (Exception ex) { throw ex; }
    }
    private static Random rng = new Random(Environment.TickCount);

    public static IList<T> Shuffle<T>(this IList<T> list)
    {
      int n = list.Count;
      while (n > 1)
      {
        n--;
        int k = rng.Next(n + 1);
        T value = list[k];
        list[k] = list[n];
        list[n] = value;
      }
      return list;
    }
    public static IEnumerable asObservableCollection(this IList source)
    {
      var listItemTypes = source.GetType().GetCompatibleItemTypes();
      var oc = (IList)Activator.CreateInstance(typeof(ObservableCollection<>).MakeGenericType(new[] { listItemTypes[0] }));
      foreach (var o in source)
        oc.Add(o);
      return oc as IEnumerable;
    }
    //public static ObservableCollection<T> asObservableCollection<T>(this IList source)
    //{
    //    //var listItemTypes = source.GetType().GetCompatibleItemTypes();
    //    ObservableCollection<T> oc = (ObservableCollection<T>)Activator.CreateInstance(typeof(ObservableCollection<T>));//.MakeGenericType(new[] { listItemTypes[0] }));
    //    foreach (var o in source)
    //        oc.Add((T)o);
    //    return oc;
    //}

    public static ObservableCollection<T> asObservableCollection<T>(this IList<T> source)
    {
      //var listItemTypes = source.GetType().GetCompatibleItemTypes();
      ObservableCollection<T> oc = (ObservableCollection<T>)Activator.CreateInstance(typeof(ObservableCollection<T>));//.MakeGenericType(new[] { listItemTypes[0] }));
      foreach (var o in source)
        oc.Add((T)o);
      return oc;
    }

    public static void ForEach<T>(this IList query, Action<T> method)
    {
      foreach (T item in query)
      {
        method(item);
      }
    }

    public static void ForEachWithIndex<T>(this IList enumerable, Action<T, int> handler)
    {
      int idx = 0;
      foreach (T item in enumerable)
        handler(item, idx++);
    }

    public static void foreachReverse<T>(this IList enumerable, Action<T, int> handler)
    {
      for (int idx = enumerable.Count - 1; idx >= 0; idx--)
        handler((T)enumerable[idx], idx);
    }

    /// <summary>
    /// Do the action for all enum elements
    /// </summary>
    /// <param name="source">The enum</param>
    /// <param name="action">What to do</param>
    /// <returns>Reference to the enum</returns>
    /// <remarks>NOTE: Unfortunately there is no such extension in LINQ</remarks>
    public static IList Action<T>(this IList source, Action<T> action)
    {
      foreach (T element in source)
      {
        action(element);
      }

      return source;
    }

    public static T UniqueOrDefault<T>(this System.Collections.Generic.IEnumerable<T> list) => UniqueOrDefault<T>(list.ToList<T>());

    public static T UniqueOrDefault<T>(this System.Collections.Generic.IList<T> list)
    {
      int size = list.Count;
      if (size == 0)
      {
        return default(T);
      }
      T first = list[0];
      for (int i = 1; i < size; i++)
      {
        if (list[i].Equals(first))
        {
          throw new Exception("query did not return a unique result: " + list.Count.ToString());
        }
      }
      return first;
    }
  }
}