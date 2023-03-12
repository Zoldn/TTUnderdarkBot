using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using TUnderdark.RatingSystem;

namespace TUnderdark.TTSParser
{
    public static class CardMapper
    {
        private sealed class JSONContainer
        {
            public List<Card> Cards { get; set; }
        }

        public static bool TryMakeNewFromId(int cardId, out Card card)
        {
            if (CardMakers.TryGetValue(cardId, out var cardSpecificType))
            {
                card = CardMakers[cardId].Clone();
                return true;
            }
            else
            {
                card = null;
                return false;
            }
        }

        public static void ReadCards()
        {
            var cards = new List<Card>();

            var json = File.ReadAllText(@"..\..\..\..\TUnderdark\Resources\Cards.json");

            var container = JsonConvert.DeserializeObject<JSONContainer>(json);

            SpecificTypeCardMakers = container
                .Cards
                .ToDictionary(c => c.SpecificType);

            CardMakers = TTSIdCardMapper
                .SelectMany(kv => kv.Key, (kv, cardId) => (CardId: cardId, Creator: kv.Value))
                .ToDictionary(
                    kv => kv.CardId,
                    kv => SpecificTypeCardMakers[kv.Creator]
                );
        }
        public static Dictionary<int, Card> CardMakers { get; private set; }

        public static Dictionary<CardSpecificType, Card> SpecificTypeCardMakers =
            new Dictionary<CardSpecificType, Card>();

        public static Dictionary<HashSet<int>, CardSpecificType> TTSIdCardMapper = 
            new Dictionary<HashSet<int>, CardSpecificType>() 
        {
            { new HashSet<int>() { 1042 },              CardSpecificType.NOBLE }, 
            { new HashSet<int>() { 1044 },              CardSpecificType.SOLDIER }, 
            { new HashSet<int>() { 243 },               CardSpecificType.LOLTH }, 
            { new HashSet<int>() { 240 },               CardSpecificType.HOUSEGUARD },

            
            #region Drow

            { new HashSet<int>() { 211, 212 },          CardSpecificType.BOUNTY_HUNTER }, 
            { new HashSet<int>() { 219, 220 },          CardSpecificType.DROW_NEGOTIATOR }, 
            { new HashSet<int>() { 221, 222 },          CardSpecificType.INFILTRATOR }, 
            { new HashSet<int>() { 217, 218 },          CardSpecificType.DOPPELGANGER }, 
            { new HashSet<int>() { 203, 204, 205, 206 },CardSpecificType.ADVOCATE }, 
            { new HashSet<int>() { 235, 236 },          CardSpecificType.SPY_MASTER }, 
            { new HashSet<int>() { 213, 214 },          CardSpecificType.CHOSEN_OF_LOLTH }, 
            { new HashSet<int>() { 237, 238 },          CardSpecificType.UNDERDARK_RANGER }, 
            { new HashSet<int>() { 200, 201, 202 },     CardSpecificType.ADVANCE_SCOUT }, 
            { new HashSet<int>() { 226, 227 },          CardSpecificType.MASTER_OF_MELEE_MAGTHERE }, 
            { new HashSet<int>() { 223, 224 },          CardSpecificType.INFORMATION_BROCKER }, 
            { new HashSet<int>() { 230, 231 },          CardSpecificType.MERCENARY_SQUAD }, 
            { new HashSet<int>() { 232, 233, 234 },     CardSpecificType.SPELL_SPINNER }, 
            { new HashSet<int>() { 207, 208, 209, 210 },CardSpecificType.BLACKGUARD }, 
            { new HashSet<int>() { 239 },               CardSpecificType.WEAPONMASTER }, 
            { new HashSet<int>() { 216 },               CardSpecificType.DEATHBLADE }, 
            { new HashSet<int>() { 215 },               CardSpecificType.COUNCIL_MEMBER }, 
            { new HashSet<int>() { 225 },               CardSpecificType.INQUISITOR }, 
            { new HashSet<int>() { 228 },               CardSpecificType.MASTER_OF_SORCERE }, 
            { new HashSet<int>() { 229 },               CardSpecificType.MATRON_MOTHER },

            #endregion
            
            #region Dragon

            { new HashSet<int>() { 11634, 11635, 11636 },        CardSpecificType.WHITE_WYRMLING },
            { new HashSet<int>() { 11622, 11623, 11624 },        CardSpecificType.KOBOLD },
            { new HashSet<int>() { 11620, 11621 },               CardSpecificType.GREEN_WYRMLING },
            { new HashSet<int>() { 11610, 11611, 11612, 11613 }, CardSpecificType.DRAGON_CULTIST },
            { new HashSet<int>() { 11616, 11617, 11618 },        CardSpecificType.ENCHANTER_OF_THAY },
            { new HashSet<int>() { 11627, 11628 },               CardSpecificType.RED_WYRMLING },
            { new HashSet<int>() { 11630, 11631, 11632 },        CardSpecificType.WATCHER_OF_THAY },
            { new HashSet<int>() { 11604, 11605 },               CardSpecificType.BLUE_WYRMLING },
            { new HashSet<int>() { 11614, 11615 },               CardSpecificType.DRAGONCLAW },
            { new HashSet<int>() { 11601, 11602 },               CardSpecificType.BLACK_WYRMLING },
            { new HashSet<int>() { 11637, 11638, 11639,  },      CardSpecificType.WYRMSPEAKER },
            { new HashSet<int>() { 11608, 11609  },              CardSpecificType.CULT_FANATIC },
            { new HashSet<int>() { 11606, 11607  },              CardSpecificType.CLERIC_OF_LAOGZED },
            { new HashSet<int>() { 11633 },                      CardSpecificType.WHITE_DRAGON },
            { new HashSet<int>() { 11600 },                      CardSpecificType.BLACK_DRAGON },
            { new HashSet<int>() { 11603 },                      CardSpecificType.BLUE_DRAGON },
            { new HashSet<int>() { 11619 },                      CardSpecificType.GREEN_DRAGON },
            { new HashSet<int>() { 11625 },                      CardSpecificType.RATH_MODAR },
            { new HashSet<int>() { 11629 },                      CardSpecificType.SEVERIN_SILRAJIN },
            { new HashSet<int>() { 11626 },                      CardSpecificType.RED_DRAGON },

            #endregion
            
            #region Demon

            { new HashSet<int>() { 27407, 27408, 27409 },       CardSpecificType.GHOUL },
            { new HashSet<int>() { 27417, 27418, 27419 },       CardSpecificType.JACKALWERE },
            { new HashSet<int>() { 27402, 27403 },              CardSpecificType.DERRO },
            { new HashSet<int>() { 27420, 27421 },              CardSpecificType.MARILITH },
            { new HashSet<int>() { 27430, 27431 },              CardSpecificType.NAFLESHNEE },
            { new HashSet<int>() { 27428, 27429 },              CardSpecificType.MICONYD_SOVEREIGN },
            { new HashSet<int>() { 27404, 27405, 27406 },       CardSpecificType.ETTIN },
            { new HashSet<int>() { 27436, 27437 },              CardSpecificType.SUCCUBUS },
            { new HashSet<int>() { 27415, 27416 },              CardSpecificType.HEZROU },
            { new HashSet<int>() { 27422, 27423, 27424 },       CardSpecificType.MIND_FLAYER },
            { new HashSet<int>() { 27410, 27411, 27412 },       CardSpecificType.GIBBERING_MOUTHER },
            { new HashSet<int>() { 27425, 27426, 27427 },       CardSpecificType.MYCONID_ADULT },
            { new HashSet<int>() { 27432, 27433, 27434 },       CardSpecificType.NIGHT_HAG },
            { new HashSet<int>() { 27435 },                     CardSpecificType.ORCUS },
            { new HashSet<int>() { 27439 },                     CardSpecificType.ZUGGTMOY },
            { new HashSet<int>() { 27400 },                     CardSpecificType.BALOR },
            { new HashSet<int>() { 27438 },                     CardSpecificType.VROCK },
            { new HashSet<int>() { 27414 },                     CardSpecificType.GRAZZT },
            { new HashSet<int>() { 27413 },                     CardSpecificType.GLABREZU },
            { new HashSet<int>() { 27401 },                     CardSpecificType.DEMOGORGON },
                                                                
            { new HashSet<int>() { 241 },                       CardSpecificType.INSANE_OUTCAST },
                                                                
            #endregion                                         
                                                               
            #region Undeads                                     
                                                                
            { new HashSet<int>() { 11731 },                     CardSpecificType.OGRE_ZOMBIE },
            { new HashSet<int>() { 11722 },                     CardSpecificType.CONJURER },
            { new HashSet<int>() { 11724 },                     CardSpecificType.WIGHT },
            { new HashSet<int>() { 11719 },                     CardSpecificType.BANSHEE },
            { new HashSet<int>() { 11727 },                     CardSpecificType.CULTIST_OF_MYRKUL },
            { new HashSet<int>() { 11735 },                     CardSpecificType.MINOTUAR_SKELETON },
            { new HashSet<int>() { 11730 },                     CardSpecificType.WRAITH },
            { new HashSet<int>() { 11737 },                     CardSpecificType.CARRION_CRAWLER },
            { new HashSet<int>() { 11729 },                     CardSpecificType.SKELETAL_HORDE },
            { new HashSet<int>() { 11726 },                     CardSpecificType.RAVENOUS_ZOMBIE },
            { new HashSet<int>() { 11720 },                     CardSpecificType.VAMPIRE_SPAWN },
            { new HashSet<int>() { 11736 },                     CardSpecificType.FLESH_GOLEM },
            { new HashSet<int>() { 11723 },                     CardSpecificType.GHOST },
            { new HashSet<int>() { 11734 },                     CardSpecificType.NECROMANCER },
            { new HashSet<int>() { 11728 },                     CardSpecificType.VAMPIRE },
            { new HashSet<int>() { 11725 },                     CardSpecificType.MUMMY_LORD },
            { new HashSet<int>() { 11721 },                     CardSpecificType.DEATH_KNIGHT },
            { new HashSet<int>() { 11738 },                     CardSpecificType.REVENANT },
            { new HashSet<int>() { 11733 },                     CardSpecificType.HIGH_PRIEST_OF_MYRKUL },
            { new HashSet<int>() { 11732 },                     CardSpecificType.LICH },
                                                                
            #endregion                                         
            #region Aberrations                                 
                                                                
            { new HashSet<int>() { 11817 },                     CardSpecificType.GAUTH },
            { new HashSet<int>() { 11807 },                     CardSpecificType.BRAINWASHED_SLAVE },
            { new HashSet<int>() { 11818 },                     CardSpecificType.SPECTATOR },
            { new HashSet<int>() { 11812 },                     CardSpecificType.GRIMLOCK },
            { new HashSet<int>() { 11816 },                     CardSpecificType.MIND_WITNESS },
            { new HashSet<int>() { 11814 },                     CardSpecificType.CRANIUM_RATS },
            { new HashSet<int>() { 11803 },                     CardSpecificType.INTELLECT_DEVOURER },
            { new HashSet<int>() { 11808 },                     CardSpecificType.NOTHIC },
            { new HashSet<int>() { 11809 },                     CardSpecificType.CLOAKER },
            { new HashSet<int>() { 11804 },                     CardSpecificType.AMBASSADOR },
            { new HashSet<int>() { 11806 },                     CardSpecificType.CHUUL },
            { new HashSet<int>() { 11811 },                     CardSpecificType.QUAGGOTH },
            { new HashSet<int>() { 11939 },                     CardSpecificType.UMBER_HULK },
            { new HashSet<int>() { 11802 },                     CardSpecificType.PUPPETEER },
            { new HashSet<int>() { 11805 },                     CardSpecificType.ABOLETH },
            { new HashSet<int>() { 11813 },                     CardSpecificType.DEATH_TYRANT },
            { new HashSet<int>() { 11810 },                     CardSpecificType.NEOGI },
            { new HashSet<int>() { 11801 },                     CardSpecificType.ULITHARID },
            { new HashSet<int>() { 11815 },                     CardSpecificType.BEHOLDER },
            { new HashSet<int>() { 11800 },                     CardSpecificType.ELDER_BRAIN },
            #endregion                                          
                                                               
            #region Elementals                             
                                                               
            { new HashSet<int>() { 27501, 27502, 27503, 27504 },CardSpecificType.AIR_ELEMENTAL },
            { new HashSet<int>() { 27511, 27512, 27513,  },     CardSpecificType.CRUSHING_WAVE_CULTIST },
            { new HashSet<int>() { 27521, 27522, 27523,  },     CardSpecificType.FIRE_ELEMENTAL },
            { new HashSet<int>() { 27518, 27519, 27520,  },     CardSpecificType.ETERNAL_FLAME_CULTIST },
            { new HashSet<int>() { 27534, 27535, 27536,  },     CardSpecificType.WATER_ELEMENTAL },
            { new HashSet<int>() { 27507, 27508, 27509, 27510 },CardSpecificType.BLACK_EARTH_CULTIST },
            { new HashSet<int>() { 27527, 27528 },              CardSpecificType.HOWLING_HATRED_CULTIST },
            { new HashSet<int>() { 27537, 27538 },              CardSpecificType.WATER_ELEMENTAL_MYRMIDON },
            { new HashSet<int>() { 27526 },                     CardSpecificType.GAR_SHATTERKEEL },
            { new HashSet<int>() { 27500 },                     CardSpecificType.AERISI_KALINOTH },
            { new HashSet<int>() { 27530 },                     CardSpecificType.MARLOS_URNRAYLE },
            { new HashSet<int>() { 27533 },                     CardSpecificType.VANIFER },
            { new HashSet<int>() { 27525, 27524 },              CardSpecificType.FIRE_ELEMENTAL_MYRMIDON },
            { new HashSet<int>() { 27514, 27515 },              CardSpecificType.EARTH_ELEMENTAL },
            { new HashSet<int>() { 27505, 27506 },              CardSpecificType.AIR_ELEMENTAL_MYRMIDON },
            { new HashSet<int>() { 27516, 27517 },              CardSpecificType.EARTH_ELEMENTAL_MYRMIDON },
            { new HashSet<int>() { 27529 },                     CardSpecificType.IMIX },
            { new HashSet<int>() { 27539 },                     CardSpecificType.YANCBIN },
            { new HashSet<int>() { 27532 },                     CardSpecificType.OLHYDRA },
            { new HashSet<int>() { 27531 },                     CardSpecificType.OGREMOCH },
            
            #endregion
        };

        
        //public static Dictionary<int, Card> CardMakers => TTSIdCardMapper
        //    .SelectMany(kv => kv.Key, (kv, cardId) => (CardId: cardId, Creator: kv.Value))
        //    .ToDictionary(
        //        kv => kv.CardId,
        //        kv => SpecificTypeCardMakers[kv.Creator]
        //    );
    }
}
