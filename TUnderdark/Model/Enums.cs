using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TUnderdark.Model
{
    public enum Color
    {
        WHITE,
        RED,
        YELLOW,
        GREEN,
        BLUE,
    }

    internal static class ColorUtils
    {
        public static List<Color> GetAllColorList()
        {
            return Enum.GetValues(typeof(Color))
                .Cast<Color>()
                .ToList();
        }

        public static List<Color> GetPlayerColorList()
        {
            return Enum.GetValues(typeof(Color))
                .Cast<Color>()
                .Where(c => c != Color.WHITE)
                .ToList();
        }
    }

    public enum CardSpecificType
    {
        NOBLE = 0,
        SOLDIER,
        LOLTH,
        HOUSEGUARD,
    }

    public enum CardType
    {
        OBEDIENCE,
        AMBITION,
        CONQUEST,
        MALICE,
        GUILE,
        INSANE,
    }

    public enum Race
    {
        DROW, 
        DOPPELGANGER,
        DRAGON,
        KOBOLD,
        HUMAN,
        DWARF,
        HALFDRAGON,
        TROGLODYTE,
        UNDEAD,
        SHAPECHANGER,
        DERRO,
        FIEND,
        MICONYD,
        GIANT,
        ILLITHID,
        ABERRATION,
        MONSTROSITY,
        CONSTRUCT,
        GRIMLOCK,
        BEAST,
        QUAGGOTH,
        ELEMENTAL,
        ELF,
        MEDUSA,
        TIEFLING,
        ELEMENTALPRINCE,
    };
}
