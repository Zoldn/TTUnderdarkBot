using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnderdarkAI.Utils
{
    public static class HashcodeHelper
    {
        public static int EmptyCollectionPrimeNumber = 19;

        public static int Of<T>(T item) => item?.GetHashCode() ?? 0;

        public static int OfEach<T>(IEnumerable<T> items) =>
            items == null ? 0 : GetHashCode(items, 0);

        public static int And<T>(int hash, T item) =>
            CombineHashCodes(hash, Of(item));

        public static int AndEach<T>(int hash, IEnumerable<T> items)
        {
            if (items == null)
            {
                return hash;
            }

            return GetHashCode(items, hash);
        }

        private static int CombineHashCodes(int h1, int h2)
        {
            unchecked
            {
                // Code copied from System.Tuple a good way to combine hashes.
                return ((h1 << 5) + h1) ^ h2;
            }
        }

        private static int GetHashCode<T>(T item) => item?.GetHashCode() ?? 0;

        private static int GetHashCode<T>(IEnumerable<T> items, int startHashCode)
        {
            var temp = startHashCode;

            var enumerator = items.GetEnumerator();
            if (enumerator.MoveNext())
            {
                temp = CombineHashCodes(temp, GetHashCode(enumerator.Current));

                while (enumerator.MoveNext())
                {
                    temp = CombineHashCodes(temp, GetHashCode(enumerator.Current));
                }
            }
            else
            {
                temp = CombineHashCodes(temp, EmptyCollectionPrimeNumber);
            }

            return temp;
        }
    }
}
