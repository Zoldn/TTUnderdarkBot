using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TUnderdark.Model
{
    internal enum Color
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

    internal enum CardType
    {
        OBEDIENCE,
        AMBITION,
        CONQUEST,
        MALICE,
        GUILE,
        INSANE,
    }

    internal enum Race
    {
        DROW, 
        DOPPELGANGER,
        DRAGON,
        KOBOLD,
        HUMAN,
        DWARF,
        HALFDRAGON,
        TROGLODYTE,
    };
}
