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
        ERROR = 0,
        NOBLE,
        SOLDIER,
        LOLTH,

        HOUSEGUARD,
        BOUNTY_HUNTER,
        DROW_NEGOTIATOR,
        INFILTRATOR,
        DOPPELGANGER,
        ADVOCATE,
        SPY_MASTER,
        CHOSEN_OF_LOLTH,
        UNDERDARK_RANGER,
        ADVANCE_SCOUT,
        MASTER_OF_MELEE_MAGTHERE,
        INFORMATION_BROCKER,
        MERCENARY_SQUAD,
        SPELL_SPINNER,
        BLACKGUARD,
        WEAPONMASTER,
        DEATHBLADE,
        COUNCIL_MEMBER,
        INQUISITOR,
        MASTER_OF_SORCERE,
        MATRON_MOTHER,

        WHITE_WYRMLING,
        KOBOLD,
        GREEN_WYRMLING,
        DRAGON_CULTIST,
        ENCHANTER_OF_THAY,
        RED_WYRMLING,
        WATCHER_OF_THAY,
        BLUE_WYRMLING,
        DRAGONCLAW,
        BLACK_WYRMLING,
        WYRMSPEAKER,
        CULT_FANATIC,
        CLERIC_OF_LAOGZED,
        WHITE_DRAGON,
        BLACK_DRAGON,
        BLUE_DRAGON,
        GREEN_DRAGON,
        RATH_MODAR,
        SEVERIN_SILRAJIN,
        RED_DRAGON,
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
