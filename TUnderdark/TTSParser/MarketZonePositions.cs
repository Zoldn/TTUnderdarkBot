using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUnderdark.TTSParser
{
    internal static class MarketZonePositions
    {
        public static (double X1, double X2, double Z1, double Z2) MarketZone =>
            (-24.6, -15.5d, -21.0d, 21.0d);
        public static (double X1, double X2, double Z1, double Z2) Common =>
            (-34.0d, -24.5, -21.0d, 0.0d);
        public static (double X1, double X2, double Z1, double Z2) Deck =>
            (-34.0d, -24.5, 13.5d, 21.0d);
        public static (double X1, double X2, double Z1, double Z2) Devoured =>
            (-34.0d, -24.5, 6.0d, 13.4d);
    }
}
