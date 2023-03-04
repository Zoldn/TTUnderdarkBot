using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.PlayableOptions;

namespace UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators.Drow
{
    internal class AdvanceScoutOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            SupplantOptionHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                isOnlyWhite: true
                );

            EndCardHelper.Run(options, board, turn,
                endIteration: 1);

            return options;
        }
    }

    internal class MercenarySquadOptionGenetator : OptionGenerator
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

            EndCardHelper.Run(options, board, turn,
                endIteration: 3);

            return options;
        }
    }

    internal class UnderdarkRangerOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                isOnlyWhite: true);
            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 2,
                isOnlyWhite: true);

            EndCardHelper.Run(options, board, turn,
                endIteration: 2);

            return options;
        }
    }

    internal class MasterOfMeleeMagthereOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            ABCSelectHelper.Run(options, board, turn,
                inIteration: 0,
                (b, t) => OptionUtils.GetTotalWhiteTroopsOnBoard(b, t) > 0, outIteration1: 1,
                (b, t) => true, outIteration2: 2,
                outIteration3: 6
                );

            SupplantOptionHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 6,
                isOnlyWhite: true,
                isAnywhere: true);

            DeployOptionHelper.Run(options, board, turn,
                inIteration: 2, outIteration: 3);
            DeployOptionHelper.Run(options, board, turn,
                inIteration: 3, outIteration: 4);
            DeployOptionHelper.Run(options, board, turn,
                inIteration: 4, outIteration: 5);
            DeployOptionHelper.Run(options, board, turn,
                inIteration: 5, outIteration: 6);

            EndCardHelper.Run(options, board, turn,
                endIteration: 6);

            return options;
        }
    }

    internal class WeaponMasterOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();
            #region 1st run
            ABCSelectHelper.Run(options, board, turn,
                inIteration: 0,
                (b, t) => OptionUtils.IsAssassinateTargets(b, t), outIteration1: 1,
                (b, t) => true, outIteration2: 2,
                outIteration3: 9
                );

            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 3,
                isOnlyWhite: true);

            DeployOptionHelper.Run(options, board, turn,
                inIteration: 2, outIteration: 3);
            #endregion
            #region 2nd run
            ABCSelectHelper.Run(options, board, turn,
                inIteration: 3,
                (b, t) => OptionUtils.IsAssassinateTargets(b, t), outIteration1: 4,
                (b, t) => true, outIteration2: 5,
                outIteration3: 9
                );

            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 4,
                outIteration: 6,
                isOnlyWhite: true);

            DeployOptionHelper.Run(options, board, turn,
                inIteration: 5, outIteration: 6);
            #endregion
            #region 3rd run
            ABCSelectHelper.Run(options, board, turn,
                inIteration: 6,
                (b, t) => OptionUtils.IsAssassinateTargets(b, t), outIteration1: 7,
                (b, t) => true, outIteration2: 8,
                outIteration3: 9
                );

            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 7,
                outIteration: 9,
                isOnlyWhite: true);

            DeployOptionHelper.Run(options, board, turn,
                inIteration: 8, outIteration: 9);
            #endregion

            EndCardHelper.Run(options, board, turn,
                endIteration: 9);

            return options;
        }
    }
}
