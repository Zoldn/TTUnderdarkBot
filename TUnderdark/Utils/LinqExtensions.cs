using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUnderdark.Utils
{
    internal static class DictionaryExtension
    {
        public static void Deconstruct<T1, T2>(this KeyValuePair<T1, T2> tuple, out T1 key, out T2 value)
        {
            key = tuple.Key;
            value = tuple.Value;
        }

        public static void Deconstruct<T1, T2>(this IGrouping<T1, T2> grouping,
            out T1 key,
            out IEnumerable<T2> values)
        {
            key = grouping.Key;
            values = grouping;
        }
    }
}
