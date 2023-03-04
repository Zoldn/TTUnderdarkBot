using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.PlayableOptions;

namespace UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators.Drow
{
    internal class AdvanceScoutOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            SupplantOptionHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                isOnlyWhite: true
                );

            EndCardHelper.Run(options, board, turn,
                endIteration: 1);

            return options;
        }
    }

    internal class MercenarySquadOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            DeployOptionHelper.Run(options, board, turn,
                inIteration: 0, outIteration: 1);
            DeployOptionHelper.Run(options, board, turn,
                inIteration: 1, outIteration: 2);
            DeployOptionHelper.Run(options, board, turn,
                inIteration: 2, outIteration: 3);

            EndCardHelper.Run(options, board, turn,
                endIteration: 3);

            return options;
        }
    }

    internal class UnderdarkRangerOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                isOnlyWhite: true);
            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 2,
                isOnlyWhite: true);

            EndCardHelper.Run(options, board, turn,
                endIteration: 2);

            return options;
        }
    }
}
