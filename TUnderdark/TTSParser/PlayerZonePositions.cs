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
            Positions = new Dictionary<Color, (double X1, double X2, double Z1, double Z2)>() 
            {
                { Color.RED, (-3.0d, 50.0d, -55.0d, -27.0d) },
                { Color.YELLOW, (-80.0d, -3.0d, -55.0d, -27.0d) },
                { Color.GREEN, (-3.0d, 50.0d, 27.0d, 55.0d) },
                { Color.BLUE, (-80.0d, -3.0d, 27.0d, 55.0d) },
            };
    }
}
