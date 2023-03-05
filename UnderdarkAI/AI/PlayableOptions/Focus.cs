using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace UnderdarkAI.AI.PlayableOptions
{
    internal class FocusHelper
    {
        internal static void Run(List<PlayableOption> options, Board board, Turn turn, 
            CardType cardType,
            int inIteraion, 
            int focusIteration, 
            int noneIteration)
        {

            ABCSelectHelper.Run(options, board, turn, inIteraion,
                (b, t) => t.IsFocus(cardType), focusIteration,
                (b, t) => !t.IsFocus(cardType), noneIteration,
                noneIteration);
        }

        internal static void RunEndTurn(List<PlayableOption> options, Board board, Turn turn,
            CardType cardType,
            int inIteration,
            int focusIteration,
            int noneIteration)
        {

            ABCSelectHelper.RunEndTurn(options, board, turn, inIteration,
                (b, t) => t.IsFocus(cardType), focusIteration,
                (b, t) => !t.IsFocus(cardType), noneIteration,
                noneIteration);
        }
    }
}
