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

        OGRE_ZOMBIE,
        CONJURER,
        WIGHT,
        BANSHEE,
        CULTIST_OF_MYRKUL,
        MINOTUAR_SKELETON,
        WRAITH,
        CARRION_CRAWLER,
        SKELETAL_HORDE,
        RAVENOUS_ZOMBIE,
        VAMPIRE_SPAWN,
        FLESH_GOLEM,
        GHOST,
        NECROMANCER,
        VAMPIRE,
        MUMMY_LORD,
        DEATH_KNIGHT,
        REVENANT,
        HIGH_PRIEST_OF_MYRKUL,
        LICH,

        AIR_ELEMENTAL,
        CRUSHING_WAVE_CULTIST,
        FIRE_ELEMENTAL,
        ETERNAL_FLAME_CULTIST,
        WATER_ELEMENTAL,
        BLACK_EARTH_CULTIST,
        HOWLING_HATRED_CULTIST,
        WATER_ELEMENTAL_MYRMIDON,
        GAR_SHATTERKEEL,
        AERISI_KALINOTH,
        MARLOS_URNRAYLE,
        VANIFER,
        FIRE_ELEMENTAL_MYRMIDON,
        EARTH_ELEMENTAL,
        AIR_ELEMENTAL_MYRMIDON,
        EARTH_ELEMENTAL_MYRMIDON,
        OGREMOCH,
        IMIX,
        YANCBIN,
        OLHYDRA,

        GHOUL,
        JACKALWERE,
        DERRO,
        MARILITH,
        NAFLESHNEE,
        MICONYD_SOVEREIGN,
        ETTIN,
        SUCCUBUS,
        HEZROU,
        MIND_FLAYER,
        GIBBERING_MOUTHER,
        MYCONID_ADULT,
        NIGHT_HAG,
        ORCUS,
        ZUGGTMOY,
        BALOR,
        VROCK,
        GRAZZT,
        GLABREZU,
        DEMONGORGON,
        INSANE_OUTCAST,

        GAUTH,
        BRAINWASHED_SLAVE,
        SPECTATOR,
        GRIMLOCK,
        MIND_WITNESS,
        CRANIUM_RATS,
        INTELLECT_DEVOURER,
        NOTHIC,
        CLOAKER,
        AMBASSADOR,
        CHUUL,
        QUAGGOTH,
        UMBER_HULK,
        PUPPETEER,
        ABOLETH,
        DEATH_TYRANT,
        NEOGI,
        ULITHARID,
        BEHOLDER,
        ELDER_BRAIN,
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
