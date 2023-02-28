using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.PlayableOptions;

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
}
