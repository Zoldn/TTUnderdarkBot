using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.PlayableOptions;

namespace UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators.Demons
{
    internal class GhoulOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                (b, t) => true,
                swords: 2);

            RecruitInsaneOutcastHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 99,
                specificPlayers: turn.EnemyPlayers,
                isToAll: true);

            EndCardHelper.Run(options, board, turn, 99);

            return options;
        }
    }

    internal class GlabrezuOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            DevourCardFromHandHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                exitIteration: 99,
                CardSpecificType.GLABREZU);

            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 2);

            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 2,
                outIteration: 99);

            EndCardHelper.Run(options, board, turn, 99);

            return options;
        }
    }

    internal class MarilithOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            DevourCardFromHandHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                exitIteration: 99,
                CardSpecificType.MARILITH);

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 99,
                (b, t) => true,
                swords: 5);

            EndCardHelper.Run(options, board, turn, 99);

            return options;
        }
    }

    internal class MindFlayerOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            DevourCardFromHandHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                exitIteration: 99,
                CardSpecificType.MIND_FLAYER);

            ABCSelectHelper.Run(options, board, turn,
                inIteration: 1,
                (b, t) => OptionUtils.IsAssassinateTargets(b, t), outIteration1: 10,
                (b, t) => true, outIteration2: 20,
                outIteration3: 99);

            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 10,
                outIteration: 99);

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 20,
                outIteration: 99,
                (b, t) => true,
                mana: 3);

            EndCardHelper.Run(options, board, turn, 99);

            return options;
        }
    }

    internal class OrcusOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            DevourCardFromHandHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                exitIteration: 99,
                CardSpecificType.ORCUS);

            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 2);

            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 2,
                outIteration: 3);

            TakeTroopFromEnemyTrophyAndDeploy.Run(options, board, turn,
                inIteration: 3,
                outIteration: 4,
                exitIteration: 99,
                isAnywhere: true);

            DeployOptionHelper.Run(options, board, turn,
                inIteration: 4,
                outIteration: 5,
                isFromTrophy: true,
                isAnywhere: true);

            TakeTroopFromEnemyTrophyAndDeploy.Run(options, board, turn,
                inIteration: 5,
                outIteration: 6,
                exitIteration: 99,
                isAnywhere: true);

            DeployOptionHelper.Run(options, board, turn,
                inIteration: 6,
                outIteration: 99,
                isFromTrophy: true,
                isAnywhere: true);

            EndCardHelper.Run(options, board, turn, 99);

            return options;
        }
    }
}
