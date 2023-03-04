using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.PlayableOptions;

namespace UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators.Drow
{
    internal class BlackguardOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            ABCSelectHelper.Run(options, board, turn,
                inIteration: 0,
                (b, t) => OptionUtils.IsAssassinateTargets(b, t), outIteration1: 1,
                (b, t) => true, outIteration2: 2,
                outIteration3: 3
                );

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 3,
                (board, turn) => true,
                swords: 2);

            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 2,
                outIteration: 3);

            EndCardHelper.Run(options, board, turn, 3);

            return options;
        }
    }

    internal class BountyHunterOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                (board, turn) => true,
                swords: 3);

            EndCardHelper.Run(options, board, turn, 1);

            return options;
        }
    }

    internal class DeathbladeOptionGenetator : OptionGenerator
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

            EndCardHelper.Run(options, board, turn,
                endIteration: 2);

            return options;
        }
    }

    internal class DoppelgangerOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            SupplantOptionHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1);

            EndCardHelper.Run(options, board, turn,
                endIteration: 1);

            return options;
        }
    }

    internal class InquisitorOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            ABCSelectHelper.Run(options, board, turn,
                inIteration: 0,
                (b, t) => OptionUtils.IsAssassinateTargets(b, t), outIteration1: 1,
                (b, t) => true, outIteration2: 2,
                outIteration3: 3
                );

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 3,
                (board, turn) => true,
                mana: 2);

            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 2,
                outIteration: 3);

            EndCardHelper.Run(options, board, turn, 3);

            return options;
        }
    }
}
