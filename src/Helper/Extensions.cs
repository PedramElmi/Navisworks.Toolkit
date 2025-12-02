using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Community.Navisworks.Toolkit.Helper
{
    public static class Extensions
    {
        internal static void Insert<TKey>(this IDictionary<TKey, object> dictionary, TKey key, object value)
        {
            if(dictionary.ContainsKey(key))
            {
                if(dictionary[key] is IList item)
                {
                    item.Add(value);
                }
                else
                {
                    var firstValue = dictionary[key];
                    dictionary[key] = new List<object>()
                    {
                        firstValue,
                        value,
                    };
                }
            }
            else
            {
                dictionary.Add(key, value);
            }
        }

        internal static IEnumerable<T> IntersectAll<T>(this IEnumerable<IEnumerable<T>> listsOfLists)
        {
            if(listsOfLists == null)
                throw new ArgumentNullException(nameof(listsOfLists));

            using(var en = listsOfLists.GetEnumerator())
            {
                if(!en.MoveNext())
                    return Enumerable.Empty<T>(); // no lists -> empty intersection

                // start with the first list (guard against null)
                IEnumerable<T> acc = en.Current ?? Enumerable.Empty<T>();

                while(en.MoveNext())
                {
                    var next = en.Current ?? Enumerable.Empty<T>();
                    acc = acc.Intersect(next);
                }

                return acc;
            }
        }

        internal static IEnumerable<T> UnionAll<T>(this IEnumerable<IEnumerable<T>> listsOfLists)
        {
            if(listsOfLists == null)
                throw new ArgumentNullException(nameof(listsOfLists));

            using(var en = listsOfLists.GetEnumerator())
            {
                if(!en.MoveNext())
                    return Enumerable.Empty<T>(); // no lists

                IEnumerable<T> acc = en.Current ?? Enumerable.Empty<T>();

                while(en.MoveNext())
                {
                    var next = en.Current ?? Enumerable.Empty<T>();
                    acc = acc.Union(next); // lazy union
                }

                return acc;
            }
        }

    }
}