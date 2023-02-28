using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace UnderdarkAI.AI.PlayableOptions
{
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
