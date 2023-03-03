using Discord;
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
        public ResourceGainOptionSelector(int mana = 0, int swords = 0)
        {
            Mana = mana;
            Swords = swords;
        }

        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                (board, turn) => true,
                mana: Mana, swords: Swords);

            EndCardHelper.Run(options, board, turn, 1);

            return options;
        }
    }
}
