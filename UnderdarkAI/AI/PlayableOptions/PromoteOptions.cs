﻿using Discord;
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
    internal interface IPromoteCardOption : IPlayableOption
    {
        public CardSpecificType PromoteTarget { get; }
    }

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
                //options.AddRange(
                OptionUtils.GetPromoteAnotherCardPlayedThisTurnInTheEndOptions(options, board, turn, promoter,
                    outIteration);
                   // );
            }

            return options;
        }
    }

    internal class PromotePlayedCardOption : PlayableOption, IPromoteCardOption
    {
        public override int MinVerbosity => 0;
        public CardSpecificType Promoter { get; }
        public CardSpecificType PromoteTarget { get; }
        public PromotePlayedCardOption(CardSpecificType promoter, CardSpecificType target, int outIteration) : base()
        {
            Promoter = promoter;
            PromoteTarget = target;
            NextState = SelectionState.SELECT_END_TURN_CARD_OPTION;
            NextCardIteration = outIteration;
        }


        public override void ApplyOption(Board board, Turn turn)
        {
            var target = turn.CardStates
                .First(s => !s.IsPromotedInTheEnd
                    && s.State == CardState.PLAYED
                    && s.EndTurnState != CardState.NOW_PLAYING
                    && s.SpecificType == PromoteTarget
                );

            target.IsPromotedInTheEnd = true;
        }

        public override string GetOptionText()
        {
            return $"\tPromote {CardMapper.SpecificTypeCardMakers[PromoteTarget].Name} by " +
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

    internal class PromoteCardFromDiscard : PlayableOption, IPromoteCardOption
    {
        public CardSpecificType PromoteTarget { get; set; }
        public CardSpecificType Promoter { get; set; }
        public PromoteCardFromDiscard(CardSpecificType target, CardSpecificType promoter, int outIteration) : base()
        {
            NextCardIteration = outIteration;
            PromoteTarget = target;
            Promoter = promoter;
        }

        public override int MinVerbosity => 0;

        public override void ApplyOption(Board board, Turn turn)
        {
            var player = board.Players[turn.Color];
            var card = player.Discard.First(c => c.SpecificType == PromoteTarget);
            player.Discard.Remove(card);
            player.InnerCircle.Add(card);
        }

        public override string GetOptionText()
        {
            return $"\tPromote {CardMapper.SpecificTypeCardMakers[PromoteTarget]} " +
                $"from discard by {CardMapper.SpecificTypeCardMakers[Promoter]}";
        }
    }

    internal static class PromoteFromDiscardHelper
    {
        public static List<PlayableOption> Run(List<PlayableOption> options, Board board, Turn turn,
            CardSpecificType promoter,
            int inIteration, int outIteration)
        {
            if (turn.State == SelectionState.SELECT_CARD_OPTION
                && turn.CardStateIteration == inIteration)
            {
                var targets = board.Players[turn.Color]
                    .Discard
                    .Select(c => c.SpecificType)
                    .Distinct()
                    .ToList();

                var ret = new List<PromoteCardFromDiscard>();

                foreach (var target in targets)
                {
                    ret.Add(new PromoteCardFromDiscard(target, promoter, outIteration));
                }

                turn.WeightGenerator.FillPromoteOptions(board, turn, ret);

                if (ret.Count == 0)
                {
                    options.Add(new DoNothingOption(outIteration));
                }
                else
                {
                    options.AddRange(ret);
                }
            }

            return options;
        } 
    }
}
