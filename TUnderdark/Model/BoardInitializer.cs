using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUnderdark.Model
{
    internal enum LocationId
    {
        Gauntlgrym,
        Gauntlgrym2Jhachalkhyn,
        Jhachalkhyn,
        Jhachalkhyn2Buiyrandyn,
        Buiyrandyn,
        Buiyrandyn2StoneShaft,
        StoneShaft,
        StoneShaft2ChChitl,
        ChChitl,
        ChChitl2Kanaglym,
        Kanaglym,
        Kanaglym2Skullport,
        Skullport,
        Skullport2StoneShaft,
        Skullport2Labyrinth,
        Labyrinth2Skullport,
        Labyrinth,
        Labyrinth2Buiyrandyn,
        Labyrinth2Gracklstugh,
        Gracklstugh,
        Gracklstugh2Jhachalkhyn,
        Gracklstugh2MantolDerith,
        MantolDerith,
        MantolDerith2Wormwrithings,
        Wormwrithings,
        Wormwrithings2Gauntlgrym,
        MantolDerith2Blingdenstone,
        Blingdenstone,
    }

    internal static class BoardInitializer
    {
        public static void Initialize(Board board)
        {
            board.Locations.Add(new Location(LocationId.Gauntlgrym)
            {
                BonusMana = 1,
                ControlVPs = 2,
                HasControlMarker = true,
                IsSite = true,
                Name = "Gauntlgrym",
                Size = 3,
            }
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Gauntlgrym2Jhachalkhyn, "Gauntlgrym-to-Jhachalkhyn")
                );

            board.Locations.Add(new Location(LocationId.Buiyrandyn)
            {
                ControlVPs = 4,
                IsSite = true,
                Name = "Jhachalkhyn",
                Size = 4,
            }
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Jhachalkhyn2Buiyrandyn, "Jhachalkhyn-to-Buiyrandyn")
                );

            board.Locations.Add(new Location(LocationId.Buiyrandyn)
            {
                ControlVPs = 3,
                IsSite = true,
                Name = "Buiyrandyn",
                Size = 3,
                IsStart = true,
            }
                );
        }
    }
}
