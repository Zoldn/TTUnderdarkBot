using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using TUnderdark.TTSParser;

namespace UnderdarkAI.AI.PlayableOptions
{
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
}
