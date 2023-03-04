using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators;
using UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators.Dragons;
using UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators.Drow;
using UnderdarkAI.AI.PlayableOptions;

namespace UnderdarkAI.AI.OptionGenerators
{
    /// <summary>
    /// Выбирает опцию на карте
    /// </summary>
    internal class SelectPlayingCardOptionGenerator : OptionGenerator
    {
        public Dictionary<CardSpecificType, OptionGenerator> SpecificCardSelectors { get; private set; }
        public SelectPlayingCardOptionGenerator()
        {
            SpecificCardSelectors = new()
            {
                { CardSpecificType.NOBLE, new ResourceGainOptionSelector(mana: 1) },
                { CardSpecificType.SOLDIER, new ResourceGainOptionSelector(swords: 1) },
                { CardSpecificType.LOLTH, new ResourceGainOptionSelector(mana: 2) },
                { CardSpecificType.HOUSEGUARD, new ResourceGainOptionSelector(swords: 2) },
                #region Drow
                { CardSpecificType.ADVOCATE, new AdvocateOptionGenerator() },
                { CardSpecificType.DROW_NEGOTIATOR, new DrowNegotiatorOptionGenerator() },
                { CardSpecificType.CHOSEN_OF_LOLTH, new ChosenOfLolthOptionGenetator() },
                { CardSpecificType.COUNCIL_MEMBER, new CouncilMemberOptionGenetator() },
                { CardSpecificType.MATRON_MOTHER, new MatronMotherOptionGenetator() },
                { CardSpecificType.SPY_MASTER, new SpyMasterOptionGenetator() },
                { CardSpecificType.INFILTRATOR, new InfiltratorOptionGenetator() },
                { CardSpecificType.MASTER_OF_SORCERE, new MasterOfSorcereOptionGenetator() },
                { CardSpecificType.INFORMATION_BROCKER, new InformationBrockerOptionGenetator() },
                { CardSpecificType.SPELL_SPINNER, new SpellSpinnerOptionGenetator() },
                { CardSpecificType.ADVANCE_SCOUT, new AdvanceScoutOptionGenetator() },
                { CardSpecificType.MERCENARY_SQUAD, new MercenarySquadOptionGenetator() },
                { CardSpecificType.UNDERDARK_RANGER, new UnderdarkRangerOptionGenetator() },
                { CardSpecificType.MASTER_OF_MELEE_MAGTHERE, new MasterOfMeleeMagthereOptionGenerator() },
                { CardSpecificType.WEAPONMASTER, new WeaponMasterOptionGenerator() },
                { CardSpecificType.BLACKGUARD, new BlackguardOptionGenerator() },
                { CardSpecificType.BOUNTY_HUNTER, new BountyHunterOptionGenerator() },
                { CardSpecificType.DEATHBLADE, new DeathbladeOptionGenetator() },
                { CardSpecificType.DOPPELGANGER, new DoppelgangerOptionGenetator() },
                { CardSpecificType.INQUISITOR, new InquisitorOptionGenerator() },
                #endregion
                #region Dragons
                { CardSpecificType.BLUE_DRAGON, new BlueDragonOptionGenerator() },
                { CardSpecificType.BLUE_WYRMLING, new BlueWyrmlingOptionGenerator() },
                { CardSpecificType.CLERIC_OF_LAOGZED, new ClericOfLaogzedOptionGenerator() },
                { CardSpecificType.WYRMSPEAKER, new WyrmSpeakerOptionGenerator() },
                { CardSpecificType.CULT_FANATIC, new CultFanaticOptionGenerator() },
                #endregion
            };
        }
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            if (turn.ActiveCard is null)
            {
                throw new NullReferenceException();
            }

            if (SpecificCardSelectors.TryGetValue(turn.ActiveCard.Value, out var generator))
            {
                return generator.GeneratePlayableOptions(board, turn);
            }
            else
            {
                throw new NullReferenceException();
            }
        }
    }
}
