using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.PlayableOptions;

namespace UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators.Undead
{
    internal class MinotaurSkeletonOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            ABCSelectHelper.Run(options, board, turn,
                inIteration: 0,
                (b, t) => true, outIteration1: 1,
                (b, t) => OptionUtils.IsAssassinateTargets(b, t, isOnlyWhite: true, b.Sites), outIteration2: 4,
                outIteration3: 99);

            DeployOptionHelper.Run(options, board, turn,
                inIteration: 1, outIteration: 2);
            DeployOptionHelper.Run(options, board, turn,
                inIteration: 2, outIteration: 3);
            DeployOptionHelper.Run(options, board, turn,
                inIteration: 3, outIteration: 99);

            DevourSelfHelper.Run(options, board, turn,
                inIteration: 4, outIteration: 5);

            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 5, outIteration: 6,
                specificLocation: board.Sites,
                isOnlyWhite: true,
                isLockingNextAssassination: true);

            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 6, outIteration: 7,
                specificLocation: turn.LockedAssasinationLocation.HasValue ? 
                    new HashSet<LocationId>() { turn.LockedAssasinationLocation.Value } :
                    new HashSet<LocationId>(),
                isOnlyWhite: true);

            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 7, outIteration: 99,
                specificLocation: turn.LockedAssasinationLocation.HasValue ?
                    new HashSet<LocationId>() { turn.LockedAssasinationLocation.Value } :
                    new HashSet<LocationId>(), 
                isOnlyWhite: true);

            EndCardHelper.Run(options, board, turn,
                endIteration: 99);

            return options;
        }
    }

    internal class MummyLordOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            ABCSelectHelper.Run(options, board, turn,
                inIteration: 0,
                (b, t) => b.Players.Any(kv => kv.Key != t.Color && kv.Value.TrophyHall[Color.WHITE] > 0), 
                outIteration1: 1, // забрать и выставить
                (b, t) => OptionUtils.IsAssassinateTargets(b, t, isOnlyWhite: true), 
                outIteration2: 3, // убить
                outIteration3: 99);

            TakeTroopFromEnemyTrophyAndDeploy.Run(options, board, turn,
                inIteration: 1,
                outIteration: 2,
                exitIteration: 99,
                isAnywhere: true,
                isOnlyWhite: true
                );

            DeployOptionHelper.Run(options, board, turn,
                inIteration: 2,
                outIteration: 10,
                isFromTrophy: true,
                isAnywhere: true);

            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 3, outIteration: 10,
                isOnlyWhite: true);

            ABCSelectHelper.Run(options, board, turn,
                inIteration: 10,
                (b, t) => b.Players.Any(kv => kv.Key != t.Color && kv.Value.TrophyHall[Color.WHITE] > 0),
                outIteration1: 11, // забрать и выставить
                (b, t) => OptionUtils.IsAssassinateTargets(b, t, isOnlyWhite: true),
                outIteration2: 13, // убить
                outIteration3: 99);

            TakeTroopFromEnemyTrophyAndDeploy.Run(options, board, turn,
                inIteration: 11,
                outIteration: 12,
                exitIteration: 99,
                isAnywhere: true,
                isOnlyWhite: true
                );

            DeployOptionHelper.Run(options, board, turn,
                inIteration: 12,
                outIteration: 99,
                isFromTrophy: true,
                isAnywhere: true);

            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 13, outIteration: 99,
                isOnlyWhite: true);

            EndCardHelper.Run(options, board, turn,
                endIteration: 99);

            return options;
        }
    }

    internal class OgreZombieOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            SupplantOptionHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                isOnlyWhite: true,
                isAnywhere: true);

            EndCardHelper.Run(options, board, turn,
                endIteration: 1);

            return options;
        }
    }

    internal class RavenousZombieOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                isOnlyWhite: true);

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 2,
                (b, t) => true, 
                swords: 1);

            EndCardHelper.Run(options, board, turn,
                endIteration: 2);

            return options;
        }
    }

    internal class SkeletalHordeOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            DeployOptionHelper.Run(options, board, turn,
                inIteration: 0, outIteration: 1);
            DeployOptionHelper.Run(options, board, turn,
                inIteration: 1, outIteration: 2);

            ABCSelectHelper.Run(options, board, turn,
                inIteration: 2,
                (b, t) => true, outIteration1: 3,
                (b, t) => true, outIteration2: 99,
                outIteration3: 99);

            DevourSelfHelper.Run(options, board, turn,
                inIteration: 3, outIteration: 4);

            DeployOptionHelper.Run(options, board, turn,
                inIteration: 4, outIteration: 5);
            DeployOptionHelper.Run(options, board, turn,
                inIteration: 5, outIteration: 6);
            DeployOptionHelper.Run(options, board, turn,
                inIteration: 6, outIteration: 99);

            EndCardHelper.Run(options, board, turn,
                endIteration: 99);

            return options;
        }
    }
}
