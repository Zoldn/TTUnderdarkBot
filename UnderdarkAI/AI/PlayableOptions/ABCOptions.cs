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
        public CardOptionASelection() : base() 
        {
            NextState = SelectionState.SELECT_CARD_OPTION;
            NextCardOption = CardOption.OPTION_A;
        }
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
        public CardOptionBSelection() : base()
        {
            NextState = SelectionState.SELECT_CARD_OPTION;
            NextCardOption = CardOption.OPTION_B;
        }

        public override void ApplyOption(Board board, Turn turn)
        {

        }

        public override string GetOptionText()
        {
            return $"Option B selected";
        }
    }
}
