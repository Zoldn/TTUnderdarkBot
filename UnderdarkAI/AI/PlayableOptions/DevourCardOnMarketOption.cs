using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using TUnderdark.TTSParser;

namespace UnderdarkAI.AI.PlayableOptions
{
    internal static class DevourCardFromHandHelper
    {
        public static List<PlayableOption> Run(List<PlayableOption> options, Board board, Turn turn,
            int inIteration,
            int outIteration,
            int exitIteration,
            CardSpecificType devourer)
        {
            if (turn.State == SelectionState.SELECT_CARD_OPTION
                && turn.CardStateIteration == inIteration)
            {
                var cards = turn.CardStates
                    .Where(e => e.State == CardState.IN_HAND)
                    .Select(e => e.SpecificType)
                    .Distinct()
                    .ToList();

                foreach (var card in cards)
                {
                    options.Add(new DevourCardInHandOption(card, devourer, outIteration));
                }

                if (options.Count == 0)
                {
                    options.Add(new DoNothingOption(exitIteration));
                }
            }

            return options;
        }
    }

    internal static class DevourCardOnMarketHelper
    {
        public static List<PlayableOption> Run(List<PlayableOption> options, Board board, Turn turn,
            int inIteration,
            int outIteration,
            CardSpecificType? replacer = null)
        {
            if (turn.State == SelectionState.SELECT_CARD_OPTION
                && turn.CardStateIteration == inIteration)
            {
                var cards = board.Market
                    .Select(e => e.SpecificType)
                    .Distinct()
                    .ToList();

                foreach (var card in cards)
                {
                    options.Add(new DevourCardOnMarketOption(card, outIteration, replacer));
                }
            }

            return options;
        }
    }

    internal class DevourCardOnMarketOption : PlayableOption
    {
        public CardSpecificType TargetCard { get; }
        public CardSpecificType? NewSpecificType { get; private set; }
        public CardSpecificType? Replacer { get; private set; }
        public override int MinVerbosity => 0;
        public DevourCardOnMarketOption(CardSpecificType target, int outIteration, CardSpecificType? replacer)
        {
            TargetCard = target;
            NextCardIteration = outIteration;
            Replacer = replacer;
        }

        public override void ApplyOption(Board board, Turn turn)
        {
            var card = board.Market
                .First(c => c.SpecificType == TargetCard);

            board.Market.Remove(card);

            board.Devoured.Add(card);

            if (Replacer.HasValue)
            {
                var cardState = turn.CardStates
                    .Single(s => s.State == CardState.NOW_PLAYING);

                cardState.CardLocation = CardLocation.MARKET;

                var newCard = board.Players[turn.Color].Hand.First(s => s.SpecificType == Replacer);
                board.Market.Add(newCard);
                board.Players[turn.Color].Hand.Remove(newCard);

                NewSpecificType = newCard.SpecificType;
            }
            else
            {
                if (board.Deck.Count > 0)
                {
                    var newCard = board.Deck.First();
                    board.Market.Add(newCard);
                    board.Deck.Remove(newCard);

                    NewSpecificType = newCard.SpecificType;
                }
            }
        }

        public override string GetOptionText()
        {
            if (NewSpecificType.HasValue)
            {
                return $"\tDevouring " +
                    $"{CardMapper.SpecificTypeCardMakers[TargetCard].Name}, " +
                    $"new card on market is " +
                    $"{CardMapper.SpecificTypeCardMakers[NewSpecificType.Value].Name}";
            }
            else
            {
                return $"\tDevouring {CardMapper.SpecificTypeCardMakers[TargetCard].Name}";
            }
        }
    }

    internal static class DevourSelfHelper
    {
        public static List<PlayableOption> Run(List<PlayableOption> options, Board board, Turn turn,
            int inIteration,
            int outIteration)
        {
            if (turn.State == SelectionState.SELECT_CARD_OPTION
                && turn.CardStateIteration == inIteration)
            {
                options.Add(new DevourSelfOption(turn.ActiveCard.Value, outIteration));
            }

            return options;
        }
    }

    internal class DevourSelfOption : PlayableOption
    {
        public CardSpecificType TargetCard { get; }
        public override int MinVerbosity => 0;
        public DevourSelfOption(CardSpecificType target, int outIteration)
        {
            TargetCard = target;
            NextCardIteration = outIteration;
        }

        public override void ApplyOption(Board board, Turn turn)
        {
            var card = board.Players[turn.Color].Hand
                .First(c => c.SpecificType == TargetCard);

            board.Players[turn.Color].Hand.Remove(card);

            var cardState = turn
                .CardStates
                .Single(s => s.State == CardState.NOW_PLAYING);

            //cardState.State = CardState.DEVOURED;
            cardState.CardLocation = CardLocation.DEVOURED;

            board.Devoured.Add(card);
        }

        public override string GetOptionText()
        {
            return $"\tDevouring this card";
        }
    }

    internal class DevourCardInHandOption : PlayableOption
    {
        public CardSpecificType TargetCard { get; }
        public CardSpecificType Devourer { get; }
        public override int MinVerbosity => 0;
        public DevourCardInHandOption(CardSpecificType target, CardSpecificType devourer, int outIteration)
        {
            TargetCard = target;
            NextCardIteration = outIteration;
            Devourer = devourer;
        }

        public override void ApplyOption(Board board, Turn turn)
        {
            var card = board.Players[turn.Color].Hand
                .First(c => c.SpecificType == TargetCard);

            board.Players[turn.Color].Hand.Remove(card);

            var cardState = turn
                .CardStates
                .First(s => s.State == CardState.IN_HAND && s.SpecificType == TargetCard);

            cardState.State = CardState.DEVOURED;
            cardState.CardLocation = CardLocation.DEVOURED;

            if (card.SpecificType == CardSpecificType.INSANE_OUTCAST)
            {
                board.InsaneOutcats++;
            }
            else
            {
                board.Devoured.Add(card);
            }
        }

        public override string GetOptionText()
        {
            return $"\tDevouring card " +
                $"{CardMapper.SpecificTypeCardMakers[TargetCard].Name} from hand";
        }
    }
}
