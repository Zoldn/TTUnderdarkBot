using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using TUnderdark.TTSParser;

namespace UnderdarkAI.AI.PlayableOptions
{
    internal static class DevourCardOnMarketHelper
    {
        public static List<PlayableOption> Run(List<PlayableOption> options, Board board, Turn turn,
            int inIteration,
            int outIteration)
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
                    options.Add(new DevourCardOnMarketOption(card, outIteration));
                }
            }

            return options;
        }
    }

    internal class DevourCardOnMarketOption : PlayableOption
    {
        public CardSpecificType TargetCard { get; }
        public CardSpecificType? NewSpecificType { get; private set; }
        public override int MinVerbosity => 0;
        public DevourCardOnMarketOption(CardSpecificType target, int outIteration)
        {
            TargetCard = target;
            NextCardIteration = outIteration;
        }

        public override void ApplyOption(Board board, Turn turn)
        {
            var card = board.Market
                .First(c => c.SpecificType == TargetCard);

            board.Market.Remove(card);

            board.Devoured.Add(card);

            if (board.Deck.Count > 0)
            {
                var newCard = board.Deck.First();
                board.Market.Add(newCard);
                board.Deck.Remove(newCard);

                NewSpecificType = newCard.SpecificType;
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
}
