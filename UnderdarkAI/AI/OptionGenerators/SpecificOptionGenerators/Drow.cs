using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using TUnderdark.TTSParser;

namespace UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators
{
    internal class AdvocateOptionGenerator : OptionGenerator
    {
        public override SelectionState State => SelectionState.SELECT_CARD_OPTION;

        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            if (turn.State == SelectionState.SELECT_CARD_OPTION)
            {
                switch (turn.CardOption)
                {
                    case CardOption.NONE_OPTION:
                        options.Add(new CardOptionASelection() { Weight = 1.0d });
                        options.Add(new CardOptionBSelection() { Weight = 1.0d });
                        break;
                    case CardOption.OPTION_A:
                        options.Add(new ResourceGainOption(mana: 2) { Weight = 1.0d });
                        break;
                    case CardOption.OPTION_B:
                        options.Add(new EnablePromoteEndTurnOption(CardSpecificType.ADVOCATE) { Weight = 1.0d });
                        break;
                }
            }

            if (turn.State == SelectionState.SELECT_END_TURN_CARD_OPTION)
            {
                var ret = turn.CardStates
                    .Where(s => !s.IsPromotedInTheEnd
                        && s.State == CardState.PLAYED
                        && s.EndTurnState != CardState.NOW_PLAYING
                    )
                    .Select(s => s.SpecificType)
                    .Distinct()
                    .Select(s => new PromoteAnotherCardOption(CardSpecificType.ADVOCATE, s) { Weight = 1.0d })
                    .ToList();

                options.AddRange(ret);
            }
            
            return options;
        }
    }

    internal class PromoteAnotherCardOption : PlayableOption
    {
        public override int MinVerbosity => 0;
        public CardSpecificType Promoter { get; }
        public CardSpecificType Target { get; }
        public PromoteAnotherCardOption(CardSpecificType promoter, CardSpecificType target)
        {
            Promoter = promoter;
            Target = target;
        }

        public override void UpdateTurnState(Turn turn)
        {
            turn.State = SelectionState.SELECT_CARD_END_TURN;
        }

        public override void ApplyOption(Board board, Turn turn)
        {
            var target = turn.CardStates
                .First(s => !s.IsPromotedInTheEnd
                    && s.State == CardState.PLAYED
                    && s.EndTurnState != CardState.NOW_PLAYING
                    && s.SpecificType == Target
                );

            target.IsPromotedInTheEnd = true;

            turn.CardStates.Single(s => s.EndTurnState == CardState.NOW_PLAYING).EndTurnState = CardState.PLAYED;
        }

        public override string GetOptionText()
        {
            return $"Promote {CardMapper.SpecificTypeCardMakers[Target].Name} by " +
                $"{CardMapper.SpecificTypeCardMakers[Promoter].Name}";
        }
    }

    internal class EnablePromoteEndTurnOption : PlayableOption
    {
        public override int MinVerbosity => 0;
        public CardSpecificType SpecificType { get; }
        public EnablePromoteEndTurnOption(CardSpecificType specificType)
        {
            SpecificType = specificType;
        }

        public override void ApplyOption(Board board, Turn turn)
        {
            turn.CardStates
                .Single(s => s.State == CardState.NOW_PLAYING)
                .EndTurnState = CardState.IN_HAND;

            turn.CardStates.Single(s => s.State == CardState.NOW_PLAYING).State = CardState.PLAYED;
        }

        public override string GetOptionText()
        {
            return $"\tAt end of turn promote another card played this turn";
        }

        public override void UpdateTurnState(Turn turn)
        {
            turn.State = SelectionState.CARD_OR_FREE_ACTION;
        }
    }

    internal class CardOptionASelection : PlayableOption
    {
        public override int MinVerbosity => 10;

        public override void ApplyOption(Board board, Turn turn)
        {
            turn.CardOption = CardOption.OPTION_A;
        }

        public override string GetOptionText()
        {
            return $"Option A selected";
        }

        public override void UpdateTurnState(Turn turn)
        {
            turn.State = SelectionState.SELECT_CARD_OPTION;
        }
    }

    internal class CardOptionBSelection : PlayableOption
    {
        public override int MinVerbosity => 10;

        public override void ApplyOption(Board board, Turn turn)
        {
            turn.CardOption = CardOption.OPTION_B;
        }

        public override string GetOptionText()
        {
            return $"Option B selected";
        }

        public override void UpdateTurnState(Turn turn)
        {
            turn.State = SelectionState.SELECT_CARD_OPTION;
        }
    }
}
