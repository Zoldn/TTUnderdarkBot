using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUnderdark.Utils
{
    public static class DictionaryExtension
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

        public static T ArgMax<T>(this IEnumerable<T> x, Func<T, double> selector)
        {
            if (!x.Any())
            {
                throw new ArgumentOutOfRangeException();
            }

            var ret = x.First();

            foreach (var item in x)
            {
                if (selector(item) > selector(ret))
                {
                    ret = item;
                }
            }

            return ret;
        }

        public static T ArgMin<T>(this IEnumerable<T> x, Func<T, double> selector)
        {
            if (!x.Any())
            {
                throw new ArgumentOutOfRangeException();
            }

            var ret = x.First();

            foreach (var item in x)
            {
                if (selector(item) < selector(ret))
                {
                    ret = item;
                }
            }

            return ret;
        }
    }
}
