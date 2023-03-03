using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using TUnderdark.TTSParser;
using UnderdarkAI.AI.OptionGenerators;

namespace UnderdarkAI.AI.PlayableOptions
{
    internal static class PromoteAnotherCardPlayedThisTurnHelper
    {
        internal static List<PlayableOption> Run(List<PlayableOption> options, Board board, Turn turn, 
            int inIteration,
            int outIteration,
            CardSpecificType promoter)
        {
            /// Запоминаем промоут в конце хода и завершаем карту
            if (turn.State == SelectionState.SELECT_CARD_OPTION
                && turn.CardStateIteration == inIteration)
            {
                options.Add(new EnablePromoteEndTurnOption(promoter, outIteration));
            }

            return options;
        }

        internal static List<PlayableOption> RunEndTurn(List<PlayableOption> options, Board board, Turn turn,
            int inIteration,
            int outIteration,
            CardSpecificType promoter)
        {
            ///Выбор промоута в конце хода
            if (turn.State == SelectionState.SELECT_END_TURN_CARD_OPTION
                && turn.CardStateIteration == inIteration)
            {
                options.AddRange(
                    OptionUtils.GetPromoteAnotherCardPlayedThisTurnInTheEndOptions(turn, promoter, outIteration)
                    );
            }

            return options;
        }
    }

    internal class PromoteAnotherCardOption : PlayableOption
    {
        public override int MinVerbosity => 0;
        public CardSpecificType Promoter { get; }
        public CardSpecificType Target { get; }
        public PromoteAnotherCardOption(CardSpecificType promoter, CardSpecificType target) : base()
        {
            Promoter = promoter;
            Target = target;
            NextState = SelectionState.SELECT_END_TURN_CARD_OPTION;
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
        }

        public override string GetOptionText()
        {
            return $"\tPromote {CardMapper.SpecificTypeCardMakers[Target].Name} by " +
                $"{CardMapper.SpecificTypeCardMakers[Promoter].Name}";
        }
    }

    internal class EnablePromoteEndTurnOption : PlayableOption
    {
        public override int MinVerbosity => 0;
        public CardSpecificType SpecificType { get; }
        public EnablePromoteEndTurnOption(CardSpecificType specificType, int outIteration) : base()
        {
            SpecificType = specificType;
            NextCardIteration = outIteration;
        }

        public override void ApplyOption(Board board, Turn turn)
        {
            turn.CardStates
                .Single(s => s.State == CardState.NOW_PLAYING)
                .EndTurnState = CardState.IN_HAND;
        }

        public override string GetOptionText()
        {
            return $"\tAt end of turn promote another card played this turn";
        }

    }
}
