using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnderdarkAI.Utils
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> Apply<T>(this IEnumerable<T> x, Action<T> action)
        {
            foreach (var item in x)
            {
                action(item);
            }

            return x;
        }
    }
}
