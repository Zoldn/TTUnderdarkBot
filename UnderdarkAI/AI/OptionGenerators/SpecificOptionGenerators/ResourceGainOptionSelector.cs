using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators
{
    internal class ResourceGainOptionSelector : OptionGenerator
    {
        public int Mana { get; }
        public int Swords { get; }

        public override SelectionState State => throw new NotImplementedException();

        public ResourceGainOptionSelector(int mana = 0, int swords = 0)
        {
            Mana = mana;
            Swords = swords;
        }

        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            return new List<PlayableOption>(1) { new ResourceGainOption(Mana, Swords) { Weight = 1.0d } };
        }
    }

    internal class ResourceGainOption : PlayableOption
    {
        public int Mana { get; }
        public int Swords { get; }
        public override int MinVerbosity => 0;
        public ResourceGainOption(int mana = 0, int swords = 0)
        {
            Mana = mana;
            Swords = swords;
        }
        public override void ApplyOption(Board board, Turn turn)
        {
            turn.Mana += Mana;
            turn.Swords += Swords;

            turn.CardStates.Single(s => s.State == CardState.NOW_PLAYING).State = CardState.PLAYED;
        }

        public override void UpdateTurnState(Turn turn)
        {
            turn.State = SelectionState.CARD_OR_FREE_ACTION;
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
