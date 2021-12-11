using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace TUnderdark.TTSParser
{
    internal static class PlayerZonePositions
    {
        public static Dictionary<Color, (double X1, double X2, double Z1, double Z2)>
            Zones = new Dictionary<Color, (double X1, double X2, double Z1, double Z2)>() 
            {
                { Color.RED, (-3.0d, 50.0d, -55.0d, -27.0d) },
                { Color.YELLOW, (-80.0d, -3.0d, -55.0d, -27.0d) },
                { Color.GREEN, (-3.0d, 50.0d, 27.0d, 55.0d) },
                { Color.BLUE, (-80.0d, -3.0d, 27.0d, 55.0d) },
            };
        
        public static Dictionary<Color, (double X1, double X2, double Z1, double Z2)>
            Decks = new Dictionary<Color, (double X1, double X2, double Z1, double Z2)>()
            {
                { Color.RED, (16.0d, 23.0d, -39.0d, -29.0d) },
                { Color.YELLOW, (-34.0d, -27.0d, -39.0d, -29.0d) },
                { Color.GREEN, (29.0d, 36.0d, 29.0d, 39.0d) },
                { Color.BLUE, (-19.0d, -12.0d, 29.0d, 39.0d) },
            };

        public static Dictionary<Color, (double X1, double X2, double Z1, double Z2)>
            Discard = new Dictionary<Color, (double X1, double X2, double Z1, double Z2)>()
            {
                { Color.RED, (29.0d, 36.0d, -39.0d, -29.0d) },
                { Color.YELLOW, (-21.0d, -14.0d, -39.0d, -29.0d) },
                { Color.GREEN, (16.0d, 23.0d, 29.0d, 39.0d) },
                { Color.BLUE, (-32.0d, -25.0d, 29.0d, 39.0d) },
            };

        public static Dictionary<Color, (double X1, double X2, double Z1, double Z2)>
            InnerCircle = new Dictionary<Color, (double X1, double X2, double Z1, double Z2)>()
            {
                { Color.RED, (38.0d, 45.0d, -39.0d, -29.0d) },
                { Color.YELLOW, (-11.0d, -4.0d, -39.0d, -29.0d) },
                { Color.GREEN, (6.0d, 14.0d, 29.0d, 39.0d) },
                { Color.BLUE, (-41.0d, -34.0d, 29.0d, 39.0d) },
            };

        public static Dictionary<Color, (double X1, double X2, double Z1, double Z2)>
            Hands = new Dictionary<Color, (double X1, double X2, double Z1, double Z2)>()
            {
                { Color.RED, (8.0d, 42.0d, -100.0d, -47.0d) },
                { Color.YELLOW, (-42.0d, -8.0d, -100.0d, -47.0d) },
                { Color.GREEN, (8.0d, 42.0d, 47.0d, 100.0d) },
                { Color.BLUE, (-42.0d, -8.0d, 47.0d, 100.0d) },
            };
    }
}
