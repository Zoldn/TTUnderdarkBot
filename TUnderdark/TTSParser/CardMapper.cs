using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using TUnderdark.Model.Cards.Dragons;
using TUnderdark.Model.Cards.Drow;
using TUnderdark.Model.Cards.Obedience;

namespace TUnderdark.TTSParser
{
    internal static class CardMapper
    {
        public static Dictionary<HashSet<int>, Func<Card>> CardMakers = new Dictionary<HashSet<int>, Func<Card>>() 
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

            #region Dragon

            #endregion
        };
    }
}
