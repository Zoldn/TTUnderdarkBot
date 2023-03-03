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
        public static List<PlayableOption> Run(List<PlayableOption> options, 
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
                return options;
            }

            if (optionAAvailable(board, turn))
            {
                options.Add(new CardOptionASelection() { Weight = 1.0d, NextCardIteration = outIteration1 });
            }

            if (optionBAvailable(board, turn))
            {
                options.Add(new CardOptionBSelection() { Weight = 1.0d, NextCardIteration = outIteration2 });
            }

            if (options.Count == 0)
            {
                options.Add(new DoNothingOption() { Weight = 1.0d, NextCardIteration = outIteration3 });
            }

            return options;
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
}
