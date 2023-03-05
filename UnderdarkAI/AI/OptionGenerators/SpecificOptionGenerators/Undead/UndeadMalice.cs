using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.PlayableOptions;

namespace UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators.Undead
{
    internal class CarrionCrawlerOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                (board, turn) => true,
                swords: 3);

            DevourCardOnMarketHelper.Run(options, board, turn,
                inIteration: 1, outIteration: 2,
                replacer: CardSpecificType.CARRION_CRAWLER);

            EndCardHelper.Run(options, board, turn, 2);

            return options;
        }
    }

    internal class DeathKnightOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            SupplantOptionHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1);

            GetOptionalVPHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 2,
                (b, t) => b.Players[t.Color].TrophyHall.Where(kv => kv.Key != Color.WHITE).Sum(kv => kv.Value)
                );

            EndCardHelper.Run(options, board, turn,
                endIteration: 2);

            return options;
        }
    }

    internal class FleshGolemOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                (b, t) => true, 
                swords: 2);

            ABCSelectHelper.Run(options, board, turn,
                inIteration: 1,
                (b, t) => OptionUtils.IsAssassinateTargets(b, t) , outIteration1: 2, // Съесть себя и убить
                (b, t) => true, outIteration2: 99, // ничего не делать
                outIteration3: 99
                );

            DevourSelfHelper.Run(options, board, turn,
                inIteration: 2,
                outIteration: 3);

            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 3,
                outIteration: 99);

            EndCardHelper.Run(options, board, turn,
                endIteration: 99);

            return options;
        }
    }

    internal class RevenantOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1);
            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 2);

            PromoteSelfHelper.Run(options, board, turn,
                CardSpecificType.REVENANT,
                (b, t) => b.Players[t.Color].TrophyHall.Sum(kv => kv.Value) >= 8,
                inIteration: 2,
                outIteration: 99
                );

            EndCardHelper.Run(options, board, turn,
                endIteration: 99);

            return options;
        }
    }

    internal class WightOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            ABCSelectHelper.Run(options, board, turn,
                inIteration: 0,
                (b, t) => t.CardStates.Any(s => s.State == CardState.IN_HAND), outIteration1: 1,
                (b, t) => true, outIteration2: 20,
                outIteration3: 99);

            DevourCardFromHandHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 2,
                exitIteration: 99,
                CardSpecificType.WIGHT);

            SupplantOptionHelper.Run(options, board, turn,
                inIteration: 2,
                outIteration: 99);

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 20,
                outIteration: 99,
                (b, t) => true,
                swords: 2);

            EndCardHelper.Run(options, board, turn,
                endIteration: 99);

            return options;
        }
    }
}
