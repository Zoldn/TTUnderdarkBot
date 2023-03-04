using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.PlayableOptions;

namespace UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators.Dragons
{
    internal class BlackDragonOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            SupplantOptionHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                isOnlyWhite: true,
                isAnywhere: true
                );

            GetOptionalVPHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 2,
                (b, t) => b.Players[t.Color].TrophyHall[Color.WHITE] / 3
                );

            EndCardHelper.Run(options, board, turn,
                endIteration: 2);

            return options;
        }
    }

    internal class BlackWyrmlingOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                (b, t) => true,
                mana: 1);

            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 2,
                isOnlyWhite: true);

            EndCardHelper.Run(options, board, turn,
                endIteration: 2);

            return options;
        }
    }

    internal class KoboldOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();
            #region 1st run
            ABCSelectHelper.Run(options, board, turn,
                inIteration: 0,
                (b, t) => OptionUtils.IsAssassinateTargets(b, t), outIteration1: 1,
                (b, t) => true, outIteration2: 2,
                outIteration3: 3
                );

            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 3,
                isOnlyWhite: true);

            DeployOptionHelper.Run(options, board, turn,
                inIteration: 2, outIteration: 3);
            #endregion
           
            EndCardHelper.Run(options, board, turn,
                endIteration: 3);

            return options;
        }
    }

    internal class WhiteDragonOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            DeployOptionHelper.Run(options, board, turn,
                inIteration: 0, outIteration: 1);
            DeployOptionHelper.Run(options, board, turn,
                inIteration: 1, outIteration: 2);
            DeployOptionHelper.Run(options, board, turn,
                inIteration: 2, outIteration: 3);

            GetOptionalVPHelper.Run(options, board, turn,
                inIteration: 3,
                outIteration: 4,
                (b, t) => 
                {
                    int ret = 0;

                    foreach (var location in b.Locations)
                    {
                        if (location.GetControlPlayer() == t.Color)
                        {
                            ret++;
                        }
                    }

                    return ret / 2;
                }
                );

            EndCardHelper.Run(options, board, turn,
                endIteration: 4);

            return options;
        }
    }

    internal class WhiteWyrmlingOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            DeployOptionHelper.Run(options, board, turn,
                inIteration: 0, outIteration: 1);
            DeployOptionHelper.Run(options, board, turn,
                inIteration: 1, outIteration: 2);

            DevourCardOnMarketHelper.Run(options, board, turn,
                inIteration: 2, outIteration: 3);

            EndCardHelper.Run(options, board, turn,
                endIteration: 3);

            return options;
        }
    }
}
