using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators.Aberrations;
using UnderdarkAI.AI.PlayableOptions;
using UnderdarkAI.Utils;

namespace UnderdarkAI.AI.OptionGenerators
{
    internal class SelectDiscardCardOptionGenerator : OptionGenerator
    {
        public bool IsEndTurn { get; }
        public SelectDiscardCardOptionGenerator(bool isEndTurn = false)
        {
            IsEndTurn = isEndTurn;
        }
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var firstInQueue = turn.DiscardCardQueue.Peek();

            var options = new List<PlayableOption>();

            ChooseDiscardHelper.Run(options, board, turn, firstInQueue.CardSpecificType, firstInQueue.TargetPlayerColor,
                IsEndTurn);

            return options;
        }
    }

    internal class OnDiscardCardOptionGenerator : OptionGenerator
    {
        public bool IsEndTurn { get; }
        public Dictionary<CardSpecificType, OptionGenerator> CardOptionGenerators { get; private set; }
        public OnDiscardCardOptionGenerator(bool isEndTurn = false)
        {
            IsEndTurn = isEndTurn;
            CardOptionGenerators = new Dictionary<CardSpecificType, OptionGenerator>() 
            {
                { CardSpecificType.AMBASSADOR, new AmbassadorOptionGenerator() },
                { CardSpecificType.GRIMLOCK, new GrimlockOptionGenerator() },
                { CardSpecificType.UMBER_HULK, new UmberhulkOptionGenerator() },
            };
        }
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            var firstInQueue = turn.DiscardCardQueue.Peek();

            if (!turn.CurrentDiscardingCard.HasValue)
            {
                throw new NullReferenceException();
            }

            if (CardOptionGenerators.TryGetValue(turn.CurrentDiscardingCard.Value, out var generator))
            {
                options.AddRange(generator.GeneratePlayableOptions(board, turn));
            }
            else
            {
                options.Add(new NoEffectDiscardOption(firstInQueue.TargetPlayerColor, turn.CurrentDiscardingCard.Value) 
                {
                    NextState = IsEndTurn ? SelectionState.SELECT_CARD_END_TURN
                        : SelectionState.CARD_OR_FREE_ACTION,
                });
            }

            return options;
        }
    }
}
