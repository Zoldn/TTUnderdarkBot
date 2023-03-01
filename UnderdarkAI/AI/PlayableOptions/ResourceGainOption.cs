using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace UnderdarkAI.AI.PlayableOptions
{
    internal class ResourceGainOption : PlayableOption
    {
        public int Mana { get; }
        public int Swords { get; }
        public override int MinVerbosity => 0;
        public bool MakeCurrentCardPlayed { get; set; }
        public ResourceGainOption(int mana = 0, int swords = 0) : base()
        {
            Mana = mana;
            Swords = swords;
            NextState = SelectionState.CARD_OR_FREE_ACTION;
        }
        public override void ApplyOption(Board board, Turn turn)
        {
            turn.Mana += Mana;
            turn.Swords += Swords;
        }


        public override string GetOptionText()
        {
            if (Mana > 0 && Swords == 0)
            {
                return $"\tGain {Mana} mana";
            }
            if (Mana == 0 && Swords > 0)
            {
                return $"\tGain {Swords} sword(s)";
            }

            return $"\tGain {Mana} mana and {Swords} sword(s)";
        }
    }
}
