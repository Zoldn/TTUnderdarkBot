using System;
using System.Collections.Generic;
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
            int inIteration, int outIteration,
            CardLocation cardLocation)
        {
            if (turn.State == SelectionState.SELECT_CARD_OPTION
                && turn.CardStateIteration == inIteration)
            {
                var initiatorCard = turn.CardStates
                    .Single(s => s.State == CardState.NOW_PLAYING);

                if (cardLocation == CardLocation.INNER_CIRCLE)
                {
                    var innerCircleCards = board.Players[turn.Color].InnerCircle
                        .Select(e => e.SpecificType)
                        .Distinct()
                        .ToList();

                    foreach (var innerCircleCard in innerCircleCards)
                    {
                        options.Add(new PlayAnotherCardOption(innerCircleCard, CardLocation.INNER_CIRCLE,
                            initiatorCard.SpecificType, initiatorCard.CardLocation, outIteration));
                    }

                    if (options.Count == 0)
                    {
                        options.Add(new DoNothingOption(outIteration));
                    }
                }
                else if (cardLocation == CardLocation.MARKET)
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
                            initiatorCard.SpecificType, initiatorCard.CardLocation, outIteration));
                    }

                    if (options.Count == 0)
                    {
                        options.Add(new DoNothingOption(outIteration));
                    }
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
            var initiatorState = turn.CardStates.Single(s => s.State == CardState.NOW_PLAYING);

            initiatorState.State = CardState.HOLD;

            var targetState = new TurnCardState(Target, state: CardState.NOW_PLAYING) 
            {
                CardLocation = TargetLocation,
            };

            turn.CardStates.Add(targetState);

            turn.HoldedCardStack.Push(new HoldCardStackElement(Initiator, InitiatorLocation, SavedOutIteration));

            turn.CardStateIteration = 0;

            NextCardIteration = 0;
            NextState = SelectionState.SELECT_CARD_OPTION;

            if (TargetLocation == CardLocation.MARKET || TargetLocation == CardLocation.DEVOURED)
            {
                turn.UlitaridPlayedCard = Target;
                turn.UlitaridPlayedLocation = TargetLocation;
            }
        }

        public override string GetOptionText()
        {
            return $"\t{CardMapper.SpecificTypeCardMakers[Initiator]} playing " +
                $"{CardMapper.SpecificTypeCardMakers[Target]} in {TargetLocation}";
        }
    }

    internal class UlitaridDisableOption : PlayableOption
    {
        public override int MinVerbosity => 10;
        public UlitaridDisableOption(int outIteration) : base()
        {
            NextCardIteration = outIteration;
        }
        public override void ApplyOption(Board board, Turn turn)
        {
            turn.UlitaridPlayedCard = null;
            turn.UlitaridPlayedLocation = null;
        }

        public override string GetOptionText()
        {
            return $"Disable Ulitarid";
        }
    }
}
