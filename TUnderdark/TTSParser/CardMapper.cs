using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using TUnderdark.Model.Cards.Aberrations;
using TUnderdark.Model.Cards.Demons;
using TUnderdark.Model.Cards.Dragons;
using TUnderdark.Model.Cards.Drow;
using TUnderdark.Model.Cards.Elementals;
using TUnderdark.Model.Cards.Obedience;
using TUnderdark.Model.Cards.Undeads;

namespace TUnderdark.TTSParser
{
    internal static class CardMapper
    {
        public static Dictionary<HashSet<int>, Func<Card>> RawCardMakers = new Dictionary<HashSet<int>, Func<Card>>() 
        {
            { new HashSet<int>() { 1042 },              () => new Noble() }, 
            { new HashSet<int>() { 1044 },              () => new Soldier() }, 
            { new HashSet<int>() { 243 },               () => new PriestessOfLolth() }, 
            { new HashSet<int>() { 240 },               () => new Houseguard() },

            #region Drow

            { new HashSet<int>() { 211, 212 },          () => new BountyHunter() }, 
            { new HashSet<int>() { 219, 220 },          () => new DrowNegotiator() }, 
            { new HashSet<int>() { 221, 222 },          () => new Infiltrator() }, 
            { new HashSet<int>() { 217, 218 },          () => new Doppelganger() }, 
            { new HashSet<int>() { 203, 204, 205, 206 },() => new Advocate() }, 
            { new HashSet<int>() { 235, 236 },          () => new SpyMaster() }, 
            { new HashSet<int>() { 213, 214 },          () => new ChosenOfLolth() }, 
            { new HashSet<int>() { 237, 238 },          () => new UnderdarkRanger() }, 
            { new HashSet<int>() { 200, 201, 202 },     () => new AdvancedScout() }, 
            { new HashSet<int>() { 226, 227 },          () => new MasterOfMeleeMagthere() }, 
            { new HashSet<int>() { 223, 224 },          () => new InformationBrocker() }, 
            { new HashSet<int>() { 230, 231 },          () => new MercenarySquad() }, 
            { new HashSet<int>() { 232, 233, 234 },     () => new SpellSpinner() }, 
            { new HashSet<int>() { 207, 208, 209, 210 },() => new Blackguard() }, 
            { new HashSet<int>() { 239 },               () => new WeaponMaster() }, 
            { new HashSet<int>() { 216 },               () => new Deathblade() }, 
            { new HashSet<int>() { 215 },               () => new CouncilMember() }, 
            { new HashSet<int>() { 225 },               () => new Inquisitor() }, 
            { new HashSet<int>() { 228 },               () => new MasterOfSorcere() }, 
            { new HashSet<int>() { 229 },               () => new MatronMother() },

            #endregion

            #region Dragon

            { new HashSet<int>() { 11634, 11635, 11636 },       () => new WhiteWyrmling() },
            { new HashSet<int>() { 11622, 11623, 11624 },       () => new Kobold() },
            { new HashSet<int>() { 11620, 11621 },              () => new GreenWyrmling() },
            { new HashSet<int>() { 11610, 11611, 11612, 11613 },() => new DragonCultist() },
            { new HashSet<int>() { 11616, 11617, 11618 },       () => new EnchanterOfThay() },
            { new HashSet<int>() { 11627, 11628 },              () => new RedWyrmling() },
            { new HashSet<int>() { 11630, 11631, 11632 },       () => new WatcherOfThay() },
            { new HashSet<int>() { 11604, 11605 },              () => new BlueWyrmling() },
            { new HashSet<int>() { 11614, 11615 },              () => new Dragonclaw() },
            { new HashSet<int>() { 11601, 11602 },              () => new BlackWyrmling() },
            { new HashSet<int>() { 11637, 11638, 11639,  },     () => new Wyrmspeaker() },
            { new HashSet<int>() { 11608, 11609  },             () => new CultFanatic() },
            { new HashSet<int>() { 11606, 11607  },             () => new ClericOfLaogzed() },
            { new HashSet<int>() { 11633 },                     () => new WhiteDragon() },
            { new HashSet<int>() { 11600 },                     () => new BlackDragon() },
            { new HashSet<int>() { 11603 },                     () => new BlueDragon() },
            { new HashSet<int>() { 11619 },                     () => new GreenDragon() },
            { new HashSet<int>() { 11625 },                     () => new RathModar() },
            { new HashSet<int>() { 11629 },                     () => new SeverinSilrajin() },
            { new HashSet<int>() { 11626 },                     () => new RedDragon() },

            #endregion

            #region Demon

            { new HashSet<int>() { 27407, 27408, 27409 },       () => new Ghoul() },
            { new HashSet<int>() { 27417, 27418, 27419 },       () => new Jackalwere() },
            { new HashSet<int>() { 27402, 27403 },              () => new Derro() },
            { new HashSet<int>() { 27420, 27421 },              () => new Marilith() },
            { new HashSet<int>() { 27430, 27431 },              () => new Nafleshnee() },
            { new HashSet<int>() { 27428, 27429 },              () => new MiconydSovereign() },
            { new HashSet<int>() { 27404, 27405, 27406 },       () => new Ettin() },
            { new HashSet<int>() { 27436, 27437 },              () => new Succubus() },
            { new HashSet<int>() { 27415, 27416 },              () => new Hezrou() },
            { new HashSet<int>() { 27422, 27423, 27424 },       () => new MindFlayer() },
            { new HashSet<int>() { 27410, 27411, 27412 },       () => new GibberingMouther() },
            { new HashSet<int>() { 27425, 27426, 27427 },       () => new MyconidAdult() },
            { new HashSet<int>() { 27432, 27433, 27434 },       () => new NightHag() },
            { new HashSet<int>() { 27435 },                     () => new Orcus() },
            { new HashSet<int>() { 27439 },                     () => new Zuggtmoy() },
            { new HashSet<int>() { 27400 },                     () => new Balor() },
            { new HashSet<int>() { 27438 },                     () => new Vrock() },
            { new HashSet<int>() { 27414 },                     () => new Grazzt() },
            { new HashSet<int>() { 27413 },                     () => new Glabrezu() },
            { new HashSet<int>() { 27401 },                     () => new Demongorgon() },

            { new HashSet<int>() { 241 },                       () => new InsaneOutcast() },

            #endregion

            #region Undeads

            { new HashSet<int>() { 11731 },                     () => new OgreZombie() },
            { new HashSet<int>() { 11722 },                     () => new Conjurer() },
            { new HashSet<int>() { 11724 },                     () => new Wight() },
            { new HashSet<int>() { 11719 },                     () => new Banshee() },
            { new HashSet<int>() { 11727 },                     () => new CultistOfMyrkul() },
            { new HashSet<int>() { 11735 },                     () => new MinotuarSkeleton() },
            { new HashSet<int>() { 11730 },                     () => new Wraith() },
            { new HashSet<int>() { 11737 },                     () => new CarrionCrawler() },
            { new HashSet<int>() { 11729 },                     () => new SkeletalHorde() },
            { new HashSet<int>() { 11726 },                     () => new RavenousZombie() },
            { new HashSet<int>() { 11720 },                     () => new VampireSpawn() },
            { new HashSet<int>() { 11736 },                     () => new FleshGolem() },
            { new HashSet<int>() { 11723 },                     () => new Ghost() },
            { new HashSet<int>() { 11734 },                     () => new Necromancer() },
            { new HashSet<int>() { 11728 },                     () => new Vampire() },
            { new HashSet<int>() { 11725 },                     () => new MummyLord() },
            { new HashSet<int>() { 11721 },                     () => new DeathKnight() },
            { new HashSet<int>() { 11738 },                     () => new Revenant() },
            { new HashSet<int>() { 11733 },                     () => new HighPriestOfMyrkul() },
            { new HashSet<int>() { 11732 },                     () => new Lich() },

            #endregion
            #region Aberrations

            { new HashSet<int>() { 11817 },                     () => new Gauth() },
            { new HashSet<int>() { 11807 },                     () => new BrainwashedSlave() },
            { new HashSet<int>() { 11818 },                     () => new Spectator() },
            { new HashSet<int>() { 11812 },                     () => new Grimlock() },
            { new HashSet<int>() { 11816 },                     () => new MindWitness() },
            { new HashSet<int>() { 11814 },                     () => new CraniumRats() },
            { new HashSet<int>() { 11803 },                     () => new IntellectDevourer() },
            { new HashSet<int>() { 11808 },                     () => new Nothic() },
            { new HashSet<int>() { 11809 },                     () => new Cloaker() },
            { new HashSet<int>() { 11804 },                     () => new Ambassador() },
            { new HashSet<int>() { 11806 },                     () => new Chuul() },
            { new HashSet<int>() { 11811 },                     () => new Quaggoth() },
            { new HashSet<int>() { 11939 },                     () => new UmberHulk() },
            { new HashSet<int>() { 11802 },                     () => new Puppeteer() },
            { new HashSet<int>() { 11805 },                     () => new Aboleth() },
            { new HashSet<int>() { 11813 },                     () => new DeathTyrant() },
            { new HashSet<int>() { 11810 },                     () => new Neogi() },
            { new HashSet<int>() { 11801 },                     () => new Ulitharid() },
            { new HashSet<int>() { 11815 },                     () => new Beholder() },
            { new HashSet<int>() { 11800 },                     () => new ElderBrain() },
            #endregion

            #region Elementals

            { new HashSet<int>() { 27501, 27502, 27503, 27504 },() => new AirElemental() },
            { new HashSet<int>() { 27511, 27512, 27513,  },     () => new CrushingWaveCultist() },
            { new HashSet<int>() { 27521, 27522, 27523,  },     () => new FireElemental() },
            { new HashSet<int>() { 27518, 27519, 27520,  },     () => new EternalFlameCultist() },
            { new HashSet<int>() { 27534, 27535, 27536,  },     () => new WaterElemental() },
            { new HashSet<int>() { 27507, 27508, 27509, 27510 },() => new BlackEarthCultist() },
            { new HashSet<int>() { 27527, 27528 },              () => new HowlingHatredCultist() },
            { new HashSet<int>() { 27537, 27538 },              () => new WaterElementalMyrmidon() },
            { new HashSet<int>() { 27526 },                     () => new GarShatterkeel() },
            { new HashSet<int>() { 27500 },                     () => new AerisiKalinoth() },
            { new HashSet<int>() { 27530 },                     () => new MarlosUrnrayle() },
            { new HashSet<int>() { 27533 },                     () => new Vanifer() },
            { new HashSet<int>() { 27525, 27524 },              () => new FireElementalMyrmidon() },
            { new HashSet<int>() { 27514, 27515 },              () => new EarthElemental() },
            { new HashSet<int>() { 27505, 27506 },              () => new AirElementalMyrmidon() },
            { new HashSet<int>() { 27516, 27517 },              () => new EarthElementalMyrmidon() },
            { new HashSet<int>() { 27531 },                     () => new Ogremoch() },
            { new HashSet<int>() { 27529 },                     () => new Imix() },
            { new HashSet<int>() { 27539 },                     () => new YanCBin() },
            { new HashSet<int>() { 27532 },                     () => new Olhydra() },

            #endregion
        };

        public static Dictionary<int, Func<Card>> CardMakers => RawCardMakers
            .SelectMany(kv => kv.Key, (kv, cardId) => (CardId: cardId, Creator: kv.Value))
            .ToDictionary(
                kv => kv.CardId,
                kv => kv.Creator
            );
    }
}
