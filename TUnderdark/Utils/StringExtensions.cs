using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUnderdark.Utils
{
    public static class StringExtensions
    {
        public static string AddSpacesToSize(this string s, int length)
        {
            int spaceCount = length - s.Length;
            if (spaceCount > 0)
            {
                var spaces = new string(' ', spaceCount);
                return s + spaces;
            }
            return s;
        }

        public static string AddSpacesToSize(this int s, int length)
        {
            return AddSpacesToSize(s.ToString(), length);
        }

        public static string AddSignIfNeeded(this int s)
        {
            if (s > 0)
            {
                return "+" + s.ToString();
            }

            if (s == 0)
            {
                return " " + s.ToString();
            }

            return s.ToString();
        }
    }
}
