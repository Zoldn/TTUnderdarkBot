using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.PlayableOptions;

namespace UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators.Drow
{
    internal class SpyMasterOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            PlaceSpyHelper.Run(options, board, turn, 
                inIteration: 0, 
                returnIteration: 1, 
                placeIteration: 2, 
                outIteration: 3);

            EndCardHelper.Run(options, board, turn,
                endIteration: 3);

            return options;
        }
    }

    internal class InfiltratorOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            PlaceSpyHelper.Run(options, board, turn,
                inIteration: 0,
                returnIteration: 1,
                placeIteration: 2,
                outIteration: 3);

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 3,
                outIteration: 4,
                (b, t) => OptionUtils.IsPlacedSpyContainsEnemyPlayerTroop(b, t),
                swords: 1
                );

            EndCardHelper.Run(options, board, turn,
                endIteration: 4);

            return options;
        }
    }

    internal class MasterOfSorcereOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            PlaceOrReturnSpyHelper.Run(options, board, turn,
                inIteration: 0,
                outPlaceSpyIteration: 1,
                returnSpyIteration: 7,
                outReturnSpyIteration: 8);

            PlaceSpyHelper.Run(options, board, turn,
                inIteration: 1,
                returnIteration: 2,
                placeIteration: 3,
                outIteration: 4);

            PlaceSpyHelper.Run(options, board, turn,
                inIteration: 4,
                returnIteration: 5,
                placeIteration: 6,
                outIteration: 9);

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 8,
                outIteration: 9,
                (b, t) => true,
                swords: 4
                );

            EndCardHelper.Run(options, board, turn,
                endIteration: 9);

            return options;
        }
    }
}
