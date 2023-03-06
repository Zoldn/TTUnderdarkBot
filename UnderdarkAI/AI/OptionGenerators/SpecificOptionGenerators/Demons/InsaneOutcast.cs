using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.PlayableOptions;

namespace UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators.Demons
{
    internal class InsaneOutcastOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            ABCSelectHelper.Run(options, board, turn,
                inIteration: 0,
                (b, t) => t.CardStates.Any(s => s.State == CardState.IN_HAND), outIteration1: 1,
                (b, t) => true, outIteration2: 99,
                outIteration3: 99);

            InsaneOutcastDiscardHelper.Run(options, board, turn,
                inIteration: 1, outIteration: 99);

            EndCardHelper.Run(options, board, turn, 99);

            return options;
        }
    }
}
