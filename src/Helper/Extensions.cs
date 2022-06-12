using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PedramElmi.Navisworks.Toolkit.Helper
{
    internal static class Extensions
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
    }
}
