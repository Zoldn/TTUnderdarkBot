using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
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
        };
    }
}
