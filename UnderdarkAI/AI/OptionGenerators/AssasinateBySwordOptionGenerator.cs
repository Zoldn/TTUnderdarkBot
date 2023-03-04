using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.PlayableOptions;

namespace UnderdarkAI.AI.OptionGenerators
{
    internal class AssasinateBySwordOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            if (turn.Swords < 3)
            {
                throw new ArgumentOutOfRangeException();
            }

            var ret = new List<PlayableOption>();

            foreach (var (location, locationState) in turn.LocationStates)
            {
                if (!locationState.HasPresence)
                {
                    continue;
                }

                foreach (var (color, troop) in location.Troops)
                {
                    if (color == turn.Color || troop == 0)
                    {
                        continue;
                    }

                    ret.Add(new AssassinateOption(location.Id, color, outIteration: 0, isBaseAction: true));
                }
            }

            return ret;
        }
    }
}
