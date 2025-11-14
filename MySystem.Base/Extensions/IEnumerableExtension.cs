using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MySystem.Base.Extensions
{
    public static class IEnumerableExtension
    {
        /// <summary>
        /// Returns minimum base types of items accepted by the enumerable.
        /// Note that Enumerable class may actually implement several generic IEnumerable interfaces and accept several different item types
        /// (e.g. by doing conversions internally)
        /// </summary>
        /// <param name="list"></param>
        /// <param name="listItemCandidate"></param>
        /// <returns></returns>
        public static IReadOnlyList<Type> GetCompatibleItemsTypes(this IEnumerable source)
        {
            var listItemTypes = source.GetType().GetCompatibleItemTypes();

            return listItemTypes;
        }

        public static IEnumerable<TSource> DefaultIfEmpty<TSource>(this IEnumerable<TSource> source, Func<TSource> getDefaultValue)
        {
            if (source == null)
                throw new ArgumentException("source");
            else
                return DefaultIfEmptyIterator<TSource>(source, getDefaultValue);
        }

        private static IEnumerable<TSource> DefaultIfEmptyIterator<TSource>(IEnumerable<TSource> source, Func<TSource> getDefaultValue)
        {
            using (IEnumerator<TSource> enumerator = source.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    do
                    {
                        yield return enumerator.Current;
                    }
                    while (enumerator.MoveNext());
                }
                else
                    yield return getDefaultValue();
            }
        }

        public static IEnumerable<object> OfType(
            this IEnumerable source,
            Type type,
            bool treatNullableAsEquivalent = false)
        {
            foreach (object obj in source)
            {
                if (obj == null)
                    continue;

                if (obj.GetType().IsTypeEquivalentTo(type, treatNullableAsEquivalent))
                    yield return obj;
                else if (obj.GetType().ImplementsOrExtends(type))
                    yield return obj;
            }
        }

        public static IEnumerable<IEnumerable<T>> Chunkify<T>(
           this IEnumerable<T> enumerable,
           int chunkSize)
        {
            if (chunkSize < 1)
                throw new ArgumentException("chunkSize");

            using (var enumerator = enumerable.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.GetChunk(chunkSize);
                }
            }
        }

        private static IEnumerable<T> GetChunk<T>(
            this IEnumerator<T> enumerator,
            int chunkSize)
        {
            do
            {
                yield return enumerator.Current;
            }
            while (--chunkSize > 0 && enumerator.MoveNext());
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
        {
            if (list == null)
                return true;

            return !list.Any();
        }

        /// <summary>
        /// Returns *null* if list is null or empty, otherwise returns the input list.
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        public static IEnumerable<T> ToNullIfEmpty<T>(this IEnumerable<T> list)
        {
            if (list == null)
                return null;

            if (!list.Any())
                return null;

            return list;
        }

        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> items)
        {
            if (items == null)
                return new EmptyEnumerator<T>();

            return items;
        }

        public static IEnumerable EmptyIfNull(this IEnumerable items)
        {
            if (items == null)
                return new EmptyEnumerator<object>();

            return items;
        }

        // todo: move to array extensions
        public static T[] EmptyIfNull<T>(this T[] list)
        {
            if (list == null)
                return new T[0];

            return list;
        }

        private class EmptyEnumerator<T> : IEnumerator<T>, IEnumerable<T>, IEnumerator, IEnumerable
        {
            public object Current
            {
                get { return null; }
            }

            public bool MoveNext()
            {
                return false;
            }

            public void Reset()
            {
            }

            public IEnumerator GetEnumerator()
            {
                return this;
            }

            T IEnumerator<T>.Current
            {
                get { return default(T); }
            }

            public void Dispose()
            {
            }

            IEnumerator<T> IEnumerable<T>.GetEnumerator()
            {
                return this;
            }
        }

        public static IEnumerable<T> Except<T>(this IEnumerable<T> list, T excludedElement)
        {
            return list.Except(new T[] { excludedElement });
        }

        public static int FastCount<T>(this IEnumerable<T> list)
        {
            var collection = list as ICollection;

            if (collection != null)
            {
                return collection.Count;
            }

            return list.Count();
        }

        /// <summary>
        /// Creates new List from enumerable.
        /// </summary>
        /// <param name="source">enumerable to create a List from</param>
        /// <param name="capacity">initial capacity of the list</param>
        /// <returns>List that contains elements from source.</returns>
        public static List<T> ToList<T>(this IEnumerable<T> source, int capacity)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            else
            {
                var list = new List<T>(capacity);

                list.AddRange(source);

                return list;
            }
        }

        public static ConcurrentDictionary<TKey, TSource> ToConcurrentDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return IEnumerableExtension.ToConcurrentDictionary<TSource, TKey, TSource>(source, keySelector, IdentityFunction<TSource>.Instance, (IEqualityComparer<TKey>)null);
        }

        public static ConcurrentDictionary<TKey, TSource> ToConcurrentDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return IEnumerableExtension.ToConcurrentDictionary<TSource, TKey, TSource>(source, keySelector, IdentityFunction<TSource>.Instance, comparer);
        }

        public static ConcurrentDictionary<TKey, TElement> ToConcurrentDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            return IEnumerableExtension.ToConcurrentDictionary<TSource, TKey, TElement>(source, keySelector, elementSelector, (IEqualityComparer<TKey>)null);
        }

        public static ConcurrentDictionary<TKey, TElement> ToConcurrentDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (elementSelector == null)
                throw new ArgumentNullException("elementSelector");

            comparer = comparer ?? EqualityComparer<TKey>.Default;

            ConcurrentDictionary<TKey, TElement> dictionary = new ConcurrentDictionary<TKey, TElement>(comparer);

            foreach (TSource source1 in source)
                dictionary.TryAdd(keySelector(source1), elementSelector(source1));

            return dictionary;
        }

        internal class IdentityFunction<TElement>
        {
            public static Func<TElement, TElement> Instance
            {
                get
                {
                    return (Func<TElement, TElement>)(x => x);
                }
            }

            public IdentityFunction()
            {
            }
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> method)
        {
            foreach (T item in enumerable)
            {
                method(item);
            }
        }

        public static void ForEachWithIndex<T>(this IEnumerable<T> enumerable, Action<T, int> handler)
        {
            int idx = 0;
            foreach (T item in enumerable)
                handler(item, idx++);
        }

        public static void foreachReverse<T>(this IEnumerable<T> enumerable, Action<T, int> handler)
        {
            for (int idx = enumerable.Count() - 1; idx >= 0; idx--)
                handler(enumerable.ElementAt(idx), idx);
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action, Func<bool> breakOn)
        {
            foreach (var item in enumerable)
            {
                action(item);
                if (breakOn())
                    break;
            }
        }

        public static void ForEachWithIndex<T>(this IEnumerable<T> enumerable, Action<T, int> action, Func<bool> breakOn)
        {
            int idx = 0;
            foreach (T item in enumerable)
            {
                action(item, idx++);
                if (breakOn())
                    break;
            }
        }

        public static void foreachReverse<T>(this IEnumerable<T> enumerable, Action<T, int> action, Func<bool> breakOn)
        {
            for (int idx = enumerable.Count() - 1; idx >= 0; idx--)
            {
                action(enumerable.ElementAt(idx), idx);
                if (breakOn())
                    break;
            }
        }

        /// <summary>
        /// Do the action for all enum elements
        /// </summary>
        /// <param name="source">The enum</param>
        /// <param name="action">What to do</param>
        /// <returns>Reference to the enum</returns>
        /// <remarks>NOTE: Unfortunately there is no such extension in LINQ</remarks>
        public static IEnumerable<T> Action<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T element in source)
            {
                action(element);
            }

            return source;
        }

        public static IList<T> ToGenericList<T>(this IList enumerable)
        {
            var newList = new List<T>();
            foreach (var obj in enumerable)
                newList.Add((T)obj);
            return newList;
        }

        public static IList<T> ToGenericList<T>(this IEnumerable enumerable)
        {
            var newList = new List<T>();
            foreach (var obj in enumerable)
                newList.Add((T)obj);
            return newList;
        }

        public static void Sort<T>(this ObservableCollection<T> collection, Comparison<T> comparison)
        {
            var sortableList = new List<T>(collection);
            sortableList.Sort(comparison);

            for (int i = 0; i < sortableList.Count; i++)
            {
                collection.Move(collection.IndexOf(sortableList[i]), i);
            }
        }
        //
        // Summary:
        //     Returns the last element of a sequence that satisfies a specified condition.
        //
        // Parameters:
        //   source:
        //     An System.Collections.Generic.IEnumerable`1 to return an element from.
        //
        //   predicate:
        //     A function to test each element for a condition.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     The last index of item if found in the list; otherwise, -1.        //
        public static int LastIndexOf<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            for (int idx = source.Count() - 1; idx >= 0 ; idx--)
                if (predicate.Invoke(source.ElementAt(idx)))
                    return idx;
            return -1;
        }

    }
}