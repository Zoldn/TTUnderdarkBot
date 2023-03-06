using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using TUnderdark.TTSParser;

namespace UnderdarkAI.AI.PlayableOptions
{
    internal static class InsaneOutcastDiscardHelper
    {
        public static List<PlayableOption> Run(List<PlayableOption> options, Board board, Turn turn,
            int inIteration, int outIteration)
        {
            if (turn.State == SelectionState.SELECT_CARD_OPTION
                && turn.CardStateIteration == inIteration)
            {
                var cardsInHand = turn.CardStates
                    .Where(s => s.State == CardState.IN_HAND)
                    .Select(s => s.SpecificType)
                    .Distinct()
                    .ToList();

                foreach (var card in cardsInHand)
                {
                    options.Add(new InsaneOutcastDiscard(card, outIteration));
                }
            }

            return options;
        }
    }

    internal class InsaneOutcastDiscard : PlayableOption
    {
        public CardSpecificType DiscardedCard { get; }
        public InsaneOutcastDiscard(CardSpecificType discardedCard, int outIteration) : base()
        {
            DiscardedCard = discardedCard;
            NextCardIteration = outIteration;
        }

        public override int MinVerbosity => 0;

        public override void ApplyOption(Board board, Turn turn)
        {
            var discardedCardState = turn.CardStates.First(s => 
                s.SpecificType == DiscardedCard && s.State == CardState.IN_HAND);

            discardedCardState.State = CardState.DISCARDED;

            var insaneOutcastState = turn.CardStates.Single(s => s.State == CardState.NOW_PLAYING);

            insaneOutcastState.CardLocation = CardLocation.INSANE_OUTCASTS;

            var insaneOutCast = board.Players[turn.Color].Hand
                .First(c => c.SpecificType == CardSpecificType.INSANE_OUTCAST);

            board.Players[turn.Color].Hand.Remove(insaneOutCast);
            board.InsaneOutcasts++;
        }

        public override string GetOptionText()
        {
            return $"\tReturn in supply by discarding " +
                $"{CardMapper.SpecificTypeCardMakers[DiscardedCard].Name} from hand";
        }
    }
}
