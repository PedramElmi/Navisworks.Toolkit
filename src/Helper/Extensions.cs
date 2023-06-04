using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PedramElmi.Navisworks.Toolkit.Helper
{
    public static class Extensions
    {
        internal static IEnumerable<T> IntersectAll<T>(this IEnumerable<IEnumerable<T>> listsOfLists)
        {
            return listsOfLists
                .Skip(1)
                .Aggregate(
                listsOfLists.First(),
                (h, e) => { h.Intersect(e); return h; }
                );
        }

        internal static void Insert<TKey>(this IDictionary<TKey, object> dictionary, TKey key, object value)
        {
            if (dictionary.ContainsKey(key))
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
    }
}
