using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using TUnderdark.TTSParser;

namespace UnderdarkAI.AI.PlayableOptions
{
    internal static class BuyOrRecruitCardHelper
    {
        public static void Run(List<PlayableOption> options, Board board, Turn turn,
            int inIteration, 
            int outIteration,
            bool isBaseAction = false, CardType? specificCardType = null, int? costLimit = null)
        {
            if ((!isBaseAction 
                    || turn.State != SelectionState.BUY_CARD_BY_MANA)
                && (isBaseAction 
                    || turn.State != SelectionState.SELECT_CARD_OPTION 
                    || turn.CardStateIteration != inIteration))
            {
                return;
            }

            var boptions = new List<BuyingOption>();

            AddHouseguardBuyOption(board, turn, boptions, isBaseAction, outIteration, costLimit, specificCardType);
            AddLolthBuyOption(board, turn, boptions, isBaseAction, outIteration, costLimit, specificCardType);
            AddMarketOptions(board, turn, boptions, isBaseAction, outIteration, costLimit, specificCardType);
            AddDevouredOptions(board, turn, boptions, isBaseAction, outIteration, costLimit, specificCardType);

            turn.WeightGenerator.FillBuyOptions(board, turn, boptions);

            if (boptions.Count > 0)
            {
                options.AddRange(boptions);
            }
            else
            {
                options.Add(new DoNothingOption(outIteration));
            }

            AddDisableBuyOption(board, turn, options, isBaseAction);

            return;
        }

        private static void AddDevouredOptions(Board board, Turn turn, List<BuyingOption> options, bool isBaseAction, int outIteration, int? costLimit, CardType? specificCardType)
        {
            if (!turn.IsBuyTopDevouredEnabled)
            {
                return;
            }

            if (board.Devoured.Count == 0)
            {
                return;
            }

            var card = board.Devoured[0];

            if (card.SpecificType == CardSpecificType.NOBLE
                || card.SpecificType == CardSpecificType.SOLDIER)
            {
                return;
            }

            if (isBaseAction && turn.Mana < card.ManaCost)
            {
                return;
            }

            if (costLimit.HasValue && costLimit < card.ManaCost)
            {
                return;
            }

            if (specificCardType.HasValue && specificCardType != card.CardType)
            {
                return;
            }

            options.Add(new BuyingOption(card.SpecificType, CardLocation.DEVOURED, outIteration,
                isBaseAction: isBaseAction));
        }

        private static void AddDisableBuyOption(Board board, Turn turn, List<PlayableOption> ret, 
            bool isBaseAction)
        {
            if (isBaseAction)
            {
                ret.Add(new DisableBuyOption() { Weight = 0.5d });
            }
        }

        private static void AddHouseguardBuyOption(Board board, Turn turn, List<BuyingOption> ret,
            bool isBaseAction, int outIteration, int? costLimit, CardType? specificCardType)
        {
            if (board.HouseGuards <= 0)
            {
                return;
            }

            if (isBaseAction && turn.Mana < 3)
            {
                return;
            }

            if (costLimit.HasValue && costLimit < 3)
            {
                return;
            }

            if (specificCardType.HasValue && specificCardType != CardType.OBEDIENCE)
            {
                return;
            }

            var card = CardMapper.SpecificTypeCardMakers[CardSpecificType.HOUSEGUARD];

            ret.Add(new BuyingOption(card.SpecificType, CardLocation.MARKET, outIteration, 
                isBaseAction: isBaseAction));
        }

        private static void AddLolthBuyOption(Board board, Turn turn, List<BuyingOption> ret,
            bool isBaseAction, int outIteration, int? costLimit, CardType? specificCardType)
        {
            if (board.Lolths <= 0)
            {
                return;
            }

            if (isBaseAction && turn.Mana < 2)
            {
                return;
            }

            if (costLimit.HasValue && costLimit < 2)
            {
                return;
            }

            if (specificCardType.HasValue && specificCardType != CardType.OBEDIENCE)
            {
                return;
            }

            var card = CardMapper.SpecificTypeCardMakers[CardSpecificType.LOLTH];

            ret.Add(new BuyingOption(card.SpecificType, CardLocation.MARKET, outIteration, 
                isBaseAction: isBaseAction));
        }

        private static void AddMarketOptions(Board board, Turn turn, List<BuyingOption> ret,
            bool isBaseAction, int outIteration, int? costLimit, CardType? specificCardType)
        {
            foreach (var card in board.Market)
            {
                if (isBaseAction && turn.Mana < card.ManaCost)
                {
                    continue;
                }

                if (costLimit.HasValue && costLimit < card.ManaCost)
                {
                    continue;
                }

                if (specificCardType.HasValue && specificCardType != card.CardType)
                {
                    continue;
                }

                ret.Add(new BuyingOption(card.SpecificType, CardLocation.MARKET, outIteration, 
                    isBaseAction: isBaseAction));
            }
        }
    }

    internal class BuyingOption : PlayableOption
    {
        public CardSpecificType SpecificType { get; }
        public CardLocation CardLocation { get; }
        public CardSpecificType? NewSpecificType { get; private set; }
        public override int MinVerbosity => 0;
        public bool IsBaseAction { get; }
        public BuyingOption(CardSpecificType specificType, CardLocation cardLocatoin, int outIteration,
            bool isBaseAction) : base()
        {
            SpecificType = specificType;

            IsBaseAction = isBaseAction;
            CardLocation = cardLocatoin;
            NextState = isBaseAction ? SelectionState.CARD_OR_FREE_ACTION : SelectionState.SELECT_CARD_OPTION;
            NextCardIteration = outIteration;
        }

        public override void ApplyOption(Board board, Turn turn)
        {
            if (IsBaseAction)
            {
                turn.Mana -= CardMapper.SpecificTypeCardMakers[SpecificType].ManaCost;
            }

            Card? boughtCard = null;

            if (CardLocation == CardLocation.MARKET)
            {
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
            }
            else if (CardLocation == CardLocation.DEVOURED)
            {
                boughtCard = board.Devoured.First(c => c.SpecificType == SpecificType);
                board.Devoured.Remove(boughtCard);
            }

            board.Players[turn.Color].Discard.Add(boughtCard);
        }

        public override string GetOptionText()
        {
            string prefix = IsBaseAction ? "Buying" : "\tRecruiting";
            string suffix = NewSpecificType.HasValue ? $", new card is " +
                    $"{CardMapper.SpecificTypeCardMakers[NewSpecificType.Value].Name}" : "";

            return $"{prefix} " +
                $"{CardMapper.SpecificTypeCardMakers[SpecificType].Name}" +
                $"{suffix}";
        }
    }

    internal class DisableBuyOption : PlayableOption
    {
        public override int MinVerbosity => 10;
        public DisableBuyOption() : base()
        {
            NextState = SelectionState.CARD_OR_FREE_ACTION;
        }

        public override void ApplyOption(Board board, Turn turn)
        {
            turn.IsBuyingEnabled = false;
        }

        public override string GetOptionText()
        {
            return $"Disable buying";
        }
    }

    internal static class EnableBuyFromDevouredHelper
    {
        public static List<PlayableOption> Run(List<PlayableOption> options, Board board, Turn turn,
            int inIteration, int outIteration)
        {
            if (turn.State == SelectionState.SELECT_CARD_OPTION
                && turn.CardStateIteration == inIteration)
            {
                options.Add(new EnableBuyFromDevouredOption(outIteration));
            }

            return options;
        }
    }

    internal class EnableBuyFromDevouredOption : PlayableOption
    {
        public override int MinVerbosity => 0;
        public EnableBuyFromDevouredOption(int outIteration) : base()
        {
            NextCardIteration = outIteration;
        }

        public override void ApplyOption(Board board, Turn turn)
        {
            turn.IsBuyTopDevouredEnabled = true;
        }

        public override string GetOptionText()
        {
            return $"\tUntil end of turn enable buying top card on devoured deck";
        }
    }
}
