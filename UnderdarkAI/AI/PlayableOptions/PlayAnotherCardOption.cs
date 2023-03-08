using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using TUnderdark.TTSParser;

namespace UnderdarkAI.AI.PlayableOptions
{
    internal static class PlayAnotherCardHelper
    {
        public static void Run(List<PlayableOption> options, Board board, Turn turn,
            CardSpecificType initiator,
            int inIteration, 
            int returnIteration,
            int outIteration)
        {
            if (turn.State == SelectionState.SELECT_CARD_OPTION
                && turn.CardStateIteration == inIteration)
            {
                var initiatorCard = turn.CardStates
                    .Single(s => s.State == CardState.NOW_PLAYING);

                if (initiator == CardSpecificType.ELDER_BRAIN)
                {
                    var innerCircleCards = board.Players[turn.Color].InnerCircle
                        .Select(e => e.SpecificType)
                        .Distinct()
                        .ToList();

                    foreach (var innerCircleCard in innerCircleCards)
                    {
                        options.Add(new PlayAnotherCardOption(innerCircleCard, CardLocation.INNER_CIRCLE,
                            initiatorCard.SpecificType, initiatorCard.CardLocation, returnIteration));
                    }

                    if (options.Count == 0)
                    {
                        options.Add(new DoNothingOption(returnIteration));
                    }
                }
                else if (initiator == CardSpecificType.ULITHARID)
                {
                    var buyOptions = new List<PlayableOption>();

                    BuyOrRecruitCardHelper.Run(buyOptions, board, turn, 0, 0, costLimit: 4);

                    var buyCardOptions = buyOptions
                        .Where(o => o is BuyingOption)
                        .Select(o => o as BuyingOption)
                        .ToList();

                    foreach (var buycard in buyCardOptions)
                    {
                        options.Add(new PlayAnotherCardOption(buycard.SpecificType, buycard.CardLocation,
                            initiatorCard.SpecificType, initiatorCard.CardLocation, returnIteration));
                    }

                    if (options.Count == 0)
                    {
                        options.Add(new DoNothingOption(returnIteration));
                    }
                }
            }

            if (turn.State == SelectionState.SELECT_CARD_OPTION
                && turn.CardStateIteration == returnIteration)
            {
                if (initiator == CardSpecificType.ELDER_BRAIN)
                {
                    options.Add(new ElderBrainCardOption(outIteration));
                }

                if (initiator == CardSpecificType.ULITHARID)
                {
                    options.Add(new UlitaridReturnCardOption(outIteration));
                }
            }
        }
    }

    internal class HoldCardStackElement
    {
        public CardSpecificType CardSpecificType { get; }
        public CardLocation CardLocation { get; }
        public int CardStateIteration { get; }
        public HoldCardStackElement(CardSpecificType cardSpecificType, CardLocation cardLocation, int cardStateIteration)
        {
            CardSpecificType = cardSpecificType;
            CardLocation = cardLocation;
            CardStateIteration = cardStateIteration;
        }
        public HoldCardStackElement Clone()
        {
            return new HoldCardStackElement(CardSpecificType, CardLocation, CardStateIteration);
        }
    }

    /// <summary>
    /// Elder brain and Ulitharid
    /// </summary>
    internal class PlayAnotherCardOption : PlayableOption
    {
        public CardSpecificType Target { get; }
        public CardLocation TargetLocation { get; }
        public CardSpecificType Initiator { get; }
        public CardLocation InitiatorLocation { get; }
        public override int MinVerbosity => 0;
        public int SavedOutIteration { get; }
        
        public PlayAnotherCardOption(CardSpecificType target, CardLocation targetLocation, 
            CardSpecificType initiator, CardLocation initiatorLocation, int outIteration) 
            : base()
        {
            Target = target;
            TargetLocation = targetLocation;
            Initiator = initiator;
            InitiatorLocation = initiatorLocation;
            NextCardIteration = outIteration;
            SavedOutIteration = outIteration;
        }

        public override void ApplyOption(Board board, Turn turn)
        {
            // Ставим на холд текущую карту-инициатор
            var initiatorState = turn.CardStates.Single(s => s.State == CardState.NOW_PLAYING);

            initiatorState.State = CardState.HOLD;

            turn.HoldedCardStack.Push(new HoldCardStackElement(Initiator, InitiatorLocation, SavedOutIteration));

            // Перемещаем выбранную карту в руку, чтобы она нормально отработала
            var targetCard = turn.GetCard(board, Target, TargetLocation);

            //turn.RemoveCardFromLocation(board, targetCard, TargetLocation);
            //turn.MoveCardToLocation(board, targetCard, CardLocation.IN_HAND);
            turn.MoveCard(board, targetCard, from: TargetLocation, to: CardLocation.IN_HAND);
            //board.Players[turn.Color].Hand.Add(targetCard);

            Debug.Assert(TargetLocation != CardLocation.IN_HAND);

            var targetState = new TurnCardState(Target, state: CardState.NOW_PLAYING)
            {
                CardLocation = CardLocation.IN_HAND,
                IsUlitharidTarget = TargetLocation == CardLocation.MARKET || TargetLocation == CardLocation.DEVOURED,
                IsElderBrainTarget = TargetLocation == CardLocation.INNER_CIRCLE,
                PrevLocation = TargetLocation,
            };

            turn.CardStates.Add(targetState);
            
            // Переключаем состояние для игры Следующей карты
            turn.CardStateIteration = 0;
            NextCardIteration = 0;
            NextState = SelectionState.SELECT_CARD_OPTION;
            targetState.State = CardState.NOW_PLAYING;
        }

        public override string GetOptionText()
        {
            return $"\t{CardMapper.SpecificTypeCardMakers[Initiator]} playing " +
                $"{CardMapper.SpecificTypeCardMakers[Target]} in {TargetLocation}";
        }
    }

    internal class UlitaridReturnCardOption : PlayableOption
    {
        public override int MinVerbosity => 0;
        public UlitaridReturnCardOption(int outIteration) : base()
        {
            NextCardIteration = outIteration;
        }
        public override void ApplyOption(Board board, Turn turn)
        {
            var targetCard = turn.CardStates.Single(s => s.IsUlitharidTarget);

            var card = turn.GetCard(board, targetCard.SpecificType, targetCard.CardLocation);

            if (targetCard.CardLocation != targetCard.PrevLocation)
            {
                turn.RemoveCardFromLocation(board, card, targetCard.CardLocation);

                turn.MoveCardToLocation(board, card, targetCard.PrevLocation.Value);

                //board.Players[turn.Color].InnerCircle.Add(card);
            }
        }

        public override string GetOptionText()
        {
            return $"\tCard played by Ulitarid return to market";
        }
    }

    internal class ElderBrainCardOption : PlayableOption
    {
        public CardSpecificType? NewCardOnMarket { get; private set; }
        public override int MinVerbosity => 0;
        public ElderBrainCardOption(int outIteration) : base()
        {
            NextCardIteration = outIteration;
            NewCardOnMarket = null;
        }
        public override void ApplyOption(Board board, Turn turn)
        {
            var targetCard = turn.CardStates.Single(s => s.IsElderBrainTarget);

            var card = turn.GetCard(board, targetCard.SpecificType, targetCard.CardLocation);

            if (targetCard.CardLocation != targetCard.PrevLocation)
            {
                turn.RemoveCardFromLocation(board, card, targetCard.CardLocation);

                turn.MoveCardToLocation(board, card, targetCard.PrevLocation.Value);

                if (targetCard.CardLocation == CardLocation.MARKET)
                {
                    NewCardOnMarket = turn.DrawNewCardOnMarket(board);
                }
            }
        }

        public override string GetOptionText()
        {
            var suffix = NewCardOnMarket.HasValue ? $", new card on market " +
                $"{CardMapper.SpecificTypeCardMakers[NewCardOnMarket.Value].Name}" : "";

            return $"\tCard played by Elder brain return to inner circle{suffix}";
        }
    }
}
