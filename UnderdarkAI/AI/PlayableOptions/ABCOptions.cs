using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace UnderdarkAI.AI.PlayableOptions
{
    internal static class ABCSelectHelper
    {
        public static void Run(List<PlayableOption> options, 
            Board board, Turn turn,
            int inIteration,
            Func<Board, Turn, bool> optionAAvailable,
            int outIteration1,
            Func<Board, Turn, bool> optionBAvailable,
            int outIteration2,
            int outIteration3)
        {
            if (turn.CardStateIteration != inIteration || turn.State != SelectionState.SELECT_CARD_OPTION)
            {
                return;
            }

            if (optionAAvailable(board, turn))
            {
                options.Add(new CardOptionASelection() { NextCardIteration = outIteration1 });
            }

            if (optionBAvailable(board, turn))
            {
                options.Add(new CardOptionBSelection() {NextCardIteration = outIteration2 });
            }

            if (options.Count == 0)
            {
                options.Add(new DoNothingOption(outIteration3));
            }
        }

        public static void Run(List<PlayableOption> options,
            Board board, Turn turn,
            int inIteration,
            Func<Board, Turn, bool> optionAAvailable, int outIteration1,
            Func<Board, Turn, bool> optionBAvailable, int outIteration2,
            Func<Board, Turn, bool> optionСAvailable, int outIteration3,
            int outIteration4)
        {
            if (turn.CardStateIteration != inIteration || turn.State != SelectionState.SELECT_CARD_OPTION)
            {
                return;
            }

            if (optionAAvailable(board, turn))
            {
                options.Add(new CardOptionASelection() { NextCardIteration = outIteration1 });
            }

            if (optionBAvailable(board, turn))
            {
                options.Add(new CardOptionBSelection() { NextCardIteration = outIteration2 });
            }

            if (optionСAvailable(board, turn))
            {
                options.Add(new CardOptionCSelection() { NextCardIteration = outIteration3 });
            }

            if (options.Count == 0)
            {
                options.Add(new DoNothingOption(outIteration4));
            }
        }

        internal static void RunEndTurn(List<PlayableOption> options, Board board, Turn turn, 
            int inIteration, 
            Func<Board, Turn, bool> optionAAvailable, int outIteration1, 
            Func<Board, Turn, bool> optionBAvailable, int outIteration2, 
            int outIteration3)
        {
            if (turn.CardStateIteration != inIteration || turn.State != SelectionState.SELECT_END_TURN_CARD_OPTION)
            {
                return;
            }

            if (optionAAvailable(board, turn))
            {
                options.Add(new CardOptionASelection() { 
                    NextCardIteration = outIteration1,
                    NextState = SelectionState.SELECT_END_TURN_CARD_OPTION,
                });
            }

            if (optionBAvailable(board, turn))
            {
                options.Add(new CardOptionBSelection() 
                { 
                    NextCardIteration = outIteration2,
                    NextState = SelectionState.SELECT_END_TURN_CARD_OPTION,
                });
            }

            if (options.Count == 0)
            {
                options.Add(new DoNothingOption(outIteration3) 
                {
                    NextState = SelectionState.SELECT_END_TURN_CARD_OPTION,
                });
            }

            return;
        }
    }
    internal class CardOptionASelection : PlayableOption
    {
        public CardOptionASelection() : base() { }
        public override int MinVerbosity => 10;

        public override void ApplyOption(Board board, Turn turn)
        {

        }

        public override string GetOptionText()
        {
            return $"Option A selected";
        }
    }

    internal class CardOptionBSelection : PlayableOption
    {
        public override int MinVerbosity => 10;
        public CardOptionBSelection() : base() { }

        public override void ApplyOption(Board board, Turn turn)
        {

        }

        public override string GetOptionText()
        {
            return $"Option B selected";
        }
    }

    internal class CardOptionCSelection : PlayableOption
    {
        public override int MinVerbosity => 10;
        public CardOptionCSelection() : base() { }

        public override void ApplyOption(Board board, Turn turn)
        {

        }

        public override string GetOptionText()
        {
            return $"Option C selected";
        }
    }
}
