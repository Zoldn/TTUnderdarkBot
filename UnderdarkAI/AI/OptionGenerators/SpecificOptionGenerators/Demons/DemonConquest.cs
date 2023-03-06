using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.PlayableOptions;

namespace UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators.Demons
{
    internal class BalorOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            DevourCardFromHandHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                exitIteration: 99,
                CardSpecificType.BALOR);

            SupplantOptionHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 2,
                isAnywhere: true,
                isOnlyWhite: true);

            DeployOptionHelper.Run(options, board, turn,
                inIteration: 2,
                outIteration: 99);

            EndCardHelper.Run(options, board, turn, 99);

            return options;
        }
    }

    internal class DemogorgonOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            DevourCardFromHandHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                exitIteration: 99,
                CardSpecificType.DEMOGORGON);

            SupplantOptionHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 2,
                isOnlyWhite: true);

            SupplantOptionHelper.Run(options, board, turn,
                inIteration: 2,
                outIteration: 3,
                isOnlyWhite: true);

            RecruitInsaneOutcastHelper.Run(options, board, turn,
                inIteration: 3,
                outIteration: 4,
                specificPlayers: turn.EnemyPlayers,
                isToAll: true);

            RecruitInsaneOutcastHelper.Run(options, board, turn,
                inIteration: 4,
                outIteration: 99,
                specificPlayers: turn.EnemyPlayers,
                isToAll: true);


            EndCardHelper.Run(options, board, turn, 99);

            return options;
        }
    }

    internal class DerroOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            SupplantOptionHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                isOnlyWhite: true,
                isAnywhere: true);

            RecruitInsaneOutcastHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 99,
                specificPlayers: turn.ThisPlayer);

            EndCardHelper.Run(options, board, turn,
                endIteration: 99);

            return options;
        }
    }

    internal class EttinOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();
            #region 1st run
            ABCSelectHelper.Run(options, board, turn,
                inIteration: 0,
                (b, t) => OptionUtils.IsAssassinateTargets(b, t), outIteration1: 10,
                (b, t) => true, outIteration2: 20,
                outIteration3: 99
                );

            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 10,
                outIteration: 11,
                isOnlyWhite: true);

            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 11,
                outIteration: 99,
                isOnlyWhite: true);

            DeployOptionHelper.Run(options, board, turn,
                inIteration: 20, outIteration: 21);
            DeployOptionHelper.Run(options, board, turn,
                inIteration: 21, outIteration: 22);
            DeployOptionHelper.Run(options, board, turn,
                inIteration: 22, outIteration: 99);
            #endregion

            EndCardHelper.Run(options, board, turn,
                endIteration: 99);

            return options;
        }
    }

    internal class GibberingMoutherOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            DeployOptionHelper.Run(options, board, turn,
                inIteration: 0, outIteration: 1);
            DeployOptionHelper.Run(options, board, turn,
                inIteration: 1, outIteration: 2);

            RecruitInsaneOutcastHelper.Run(options, board, turn,
                inIteration: 2, 
                outIteration: 99,
                specificPlayers: turn.AdjacentPlayersToDeploy);

            EndCardHelper.Run(options, board, turn,
                endIteration: 99);

            return options;
        }
    }
}
