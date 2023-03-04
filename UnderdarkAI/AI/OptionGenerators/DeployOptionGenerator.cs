using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.PlayableOptions;

namespace UnderdarkAI.AI.OptionGenerators
{
    internal class DeployOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var ret = new List<PlayableOption>();

            if (turn.Swords < 1)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (board.Players[turn.Color].Troops == 0)
            {
                ret.Add(new DeployWithOutTroopsOption(isBaseAction: true, outIteration: 0) { Weight = 1.0d });
            }
            else
            {
                foreach (var (location, locationState) in turn.LocationStates)
                {
                    if (!locationState.HasPresence || location.FreeSpaces == 0)
                    {
                        continue;
                    }

                    ret.Add(new DeployOption(location.Id, isBaseAction: true, outIteration: 0) { Weight = 1.0d });
                }
            }

            return ret;
        }
    }
}
