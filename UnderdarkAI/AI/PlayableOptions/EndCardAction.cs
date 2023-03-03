using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace UnderdarkAI.AI.PlayableOptions
{
    internal static class EndCardHelper
    {
        public static List<PlayableOption> Run(List<PlayableOption> options, Board board, Turn turn, int endIteration)
        {
            if (turn.State == SelectionState.SELECT_CARD_OPTION && turn.CardStateIteration == endIteration)
            {
                options.Add(new EndCardAction() 
                {
                    NextCardIteration = 0,
                    NextState = SelectionState.CARD_OR_FREE_ACTION,
                });
            }

            return options;
        }

        public static List<PlayableOption> RunEndTurn(List<PlayableOption> options, Board board, Turn turn, int endIteration)
        {
            if (turn.State == SelectionState.SELECT_END_TURN_CARD_OPTION 
                && turn.CardStateIteration == endIteration)
            {
                options.Add(new EndCardInEndTurnAction() 
                { 
                    Weight = 1.0d 
                });
            }

            return options;
        }
    }

    internal class EndCardAction : PlayableOption
    {
        public override int MinVerbosity => 10;
        public EndCardAction() : base() 
        {
            NextCardIteration = 0;
            NextState = SelectionState.CARD_OR_FREE_ACTION;
        }

        public override void ApplyOption(Board board, Turn turn)
        {
            turn.MakeCurrentCardPlayed();
        }

        public override string GetOptionText()
        {
            return $"\tCurrent card has played";
        }
    }

    internal class EndCardInEndTurnAction : PlayableOption
    {
        public override int MinVerbosity => 10;
        public EndCardInEndTurnAction() : base() 
        {
            NextCardIteration = 0;
            NextState = SelectionState.SELECT_CARD_END_TURN;
        }

        public override void ApplyOption(Board board, Turn turn)
        {
            turn.MakeCurrentCardPlayedEndTurn();
        }

        public override string GetOptionText()
        {
            return $"\tCurrent card has played in the end";
        }
    }
}
