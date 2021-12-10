using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace TUnderdark.TTSParser
{
    internal static class LocationPositions
    {
        public static Dictionary<LocationId, (double X1, double X2, double Z1, double Z2)>
            Positions = new Dictionary<LocationId, (double X1, double X2, double Z1, double Z2)>
            {
                { LocationId.Gauntlgrym, (27.0d, 36.0d, 17.0d, 24.0d) },
                { LocationId.Gauntlgrym2Jhachalkhyn, (24.0d, 26.0d, 20.0d, 23.0d) },
                { LocationId.Jhachalkhyn, (17.0d, 23.0d, 17.0d, 24.0d) },
                { LocationId.Jhachalkhyn2Buiyrandyn, (15.0d, 17.0d, 19.0d, 22.0d) },
                { LocationId.Buiyrandyn, (10.0d, 15.0d, 17.0d, 24.0d) },
                { LocationId.Buiyrandyn2StoneShaft, (7.0d, 9.0d, 20.0d, 22.0d) },
                { LocationId.StoneShaft, (2.0d, 7.0d, 17.0d, 24.0d) },
                { LocationId.StoneShaft2ChChitl, (-1.0d, 1.0d, 19.0d, 22.0d) },
                { LocationId.ChChitl, (-12.0d, -2.0d, 17.0d, 25.0d) },
                { LocationId.ChChitl2Kanaglym, (-11.0d, -9.0d, 14.0d, 16.0d) },
                { LocationId.Kanaglym, (-12.0d, -7.0d, 7.0d, 13.0d) },
                { LocationId.Kanaglym2Skullport, (-7.0d, -4.0d, 9.0d, 12.0d) },
                { LocationId.Skullport, (-3.0d, 3.5d, 7.0d, 14.0d) },
                { LocationId.Skullport2StoneShaft, (1.0d, 3.0d, 15.0d, 17.0d) },
                { LocationId.Skullport2Labyrinth, (4.0d, 5.0d, 9.0d, 12.0d) },
                { LocationId.Labyrinth2Skullport, (6.0d, 8.0d, 10.0d, 12.0d) },
                { LocationId.Labyrinth, (8.5d, 12.5d, 7.0d, 14.0d) },
                { LocationId.Labyrinth2Buiyrandyn, (11.0d, 12.0d, 16.0d, 17.5d) },
                { LocationId.Labyrinth2Gracklstugh, (13.0d, 14.0d, 9.0d, 11.0d) },
                { LocationId.Gracklstugh, (14.0d, 19.0d, 8.0d, 14.0d) },
                { LocationId.Gracklstugh2Jhachalkhyn, (18.0d, 20.0d, 15.0d, 17.0d) },
                { LocationId.Gracklstugh2MantolDerith, (20.0d, 21.0d, 9.0d, 10.0d) },
                { LocationId.MantolDerith, (21.0d, 26.0d, 5.0d, 12.0d) },
                { LocationId.MantolDerith2Wormwrithings, (24.0d, 26.0d, 12.0d, 15.0d) },
                { LocationId.Wormwrithings, (28.0d, 33.0d, 10.0d, 16.0d) },
                { LocationId.Wormwrithings2Gauntlgrym, (29.0d, 30.0d, 16.0d, 17.0d) },
                { LocationId.MantolDerith2Blingdenstone, (28.0d, 29.0d, 7.0d, 8.0d) },
                { LocationId.Blingdenstone, (30.0d, 35.0d, 4.0d, 10.0d) },
            };
    }
}
