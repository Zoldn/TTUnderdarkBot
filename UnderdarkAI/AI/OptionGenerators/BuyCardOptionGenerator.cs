using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using TUnderdark.TTSParser;

namespace UnderdarkAI.AI.OptionGenerators
{
    internal class BuyCardOptionGenerator : OptionGenerator
    {
        public override SelectionState State => SelectionState.BUY_CARD_BY_MANA;

        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var ret = new List<PlayableOption>();

            
            AddHouseguardOption(board, turn, ret);
            AddLolthBuyOption(board, turn, ret);
            AddMarketOptions(board, turn, ret);

            return ret;
        }

        private static void AddHouseguardOption(Board board, Turn turn, List<PlayableOption> ret)
        {
            if (board.HouseGuards <= 0 || turn.Mana < 3)
            {
                return;
            }

            var card = CardMapper.SpecificTypeCardMakers[CardSpecificType.HOUSEGUARD];

            ret.Add(new BuyingOption(card.SpecificType)
            {
                Weight = GetWeight(card),
            }
            );
        }

        private static void AddLolthBuyOption(Board board, Turn turn, List<PlayableOption> ret)
        {
            if (board.Lolths <= 0 || turn.Mana < 2)
            {
                return;
            }

            var card = CardMapper.SpecificTypeCardMakers[CardSpecificType.LOLTH];

            ret.Add(new BuyingOption(card.SpecificType)
            {
                Weight = GetWeight(card)
            }
            );
        }

        private static void AddMarketOptions(Board board, Turn turn, List<PlayableOption> ret)
        {
            var cardsOnMarket = board.Market
                .Where(c => c.ManaCost <= turn.Mana)
                .ToList();

            foreach (var card in cardsOnMarket)
            {
                ret.Add(new BuyingOption(card.SpecificType)
                {
                    Weight = GetWeight(card),
                }
                );
            }
        }

        private static double GetWeight(Card card)
        {
            return 1.0d + (double)card.VP / card.ManaCost;
        }
    }

    internal class BuyingOption : PlayableOption
    {
        public CardSpecificType SpecificType { get; }
        public CardSpecificType? NewSpecificType { get; private set; }
        public override int MinVerbosity => 0;
        public BuyingOption(CardSpecificType specificType)
        {
            SpecificType = specificType;
        }

        public override SelectionState GetNextState()
        {
            return SelectionState.CARD_OR_FREE_ACTION;
        }

        public override void ApplyOption(Board board, Turn turn)
        {
            turn.Mana -= CardMapper.SpecificTypeCardMakers[SpecificType].ManaCost;

            Card? boughtCard = null;
            
            if (CardMapper.SpecificTypeCardMakers[SpecificType].CardType != CardType.OBEDIENCE)
            {
                boughtCard = board.Market.First(c => c.SpecificType == SpecificType);

                board.Market.Remove(boughtCard);

                if (board.Deck.Count > 0)
                {
                    var newCard = board.Deck.First();
                    board.Market.Add(newCard);
                    board.Deck.Remove(newCard);

                    NewSpecificType = newCard.SpecificType;
                }
            }
            else
            {
                if (SpecificType == CardSpecificType.LOLTH)
                {
                    board.Lolths--;
                }
                if (SpecificType == CardSpecificType.HOUSEGUARD)
                {
                    board.HouseGuards--;
                }

                boughtCard = CardMapper.SpecificTypeCardMakers[SpecificType].Clone();
            }

            board.Players[turn.Color].Discard.Add(boughtCard);
        }

        public override string GetOptionText()
        {
            if (NewSpecificType.HasValue)
            {
                return $"Buying " +
                    $"{CardMapper.SpecificTypeCardMakers[SpecificType].Name}, " +
                    $"new card on market is " +
                    $"{CardMapper.SpecificTypeCardMakers[NewSpecificType.Value].Name}";
            }
            else
            {
                return $"Buying {CardMapper.SpecificTypeCardMakers[SpecificType].Name}";
            }
        }
    }
}
