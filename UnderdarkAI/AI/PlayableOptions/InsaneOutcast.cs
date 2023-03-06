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

            var discardedCard = board.Players[turn.Color].Hand
                .First(c => c.SpecificType == DiscardedCard);

            board.Players[turn.Color].Hand.Remove(discardedCard);
            board.Players[turn.Color].Discard.Add(discardedCard);

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

    internal class RecruitInsaneOutcastOption : PlayableOption
    {
        public HashSet<Color> TargetPlayers { get; }
        public RecruitInsaneOutcastOption(HashSet<Color> targetPlayers, int outIteration) : base()
        {
            TargetPlayers = targetPlayers;
            NextCardIteration = outIteration;
        }

        public override int MinVerbosity => 0;

        public override void ApplyOption(Board board, Turn turn)
        {
            foreach (var color in TargetPlayers)
            {
                if (board.InsaneOutcasts > 0)
                {
                    var card = CardMapper.SpecificTypeCardMakers[CardSpecificType.INSANE_OUTCAST].Clone();

                    board.Players[color].Discard.Add(card);

                    board.InsaneOutcasts--;
                }
            }
        }

        public override string GetOptionText()
        {
            var players = string.Join(", ", TargetPlayers);

            return TargetPlayers.Any() ? $"\t{players} player(s) recruiting Insane Outcast(s)" :
                $"\tNone players recruiting Insane Outcast(s)";
        }
    }

    internal static class RecruitInsaneOutcastHelper
    {
        public static void Run(List<PlayableOption> options, Board board, Turn turn,
            int inIteration, int outIteration,
            HashSet<Color> specificPlayers,
            bool isToAll = false)
        {
            if (turn.State == SelectionState.SELECT_CARD_OPTION
                && turn.CardStateIteration == inIteration)
            {
                if (isToAll)
                {
                    options.Add(new RecruitInsaneOutcastOption(specificPlayers, outIteration));
                    
                }
                else
                {
                    foreach (var color in specificPlayers)
                    {
                        options.Add(new RecruitInsaneOutcastOption(new HashSet<Color>(1) { color }, outIteration));
                    }
                }

                if (options.Count == 0)
                {
                    options.Add(new DoNothingOption(outIteration));
                }
            }
        }
    }
}
