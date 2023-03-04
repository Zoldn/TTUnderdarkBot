using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.PlayableOptions;

namespace UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators.Dragons
{
    internal class DragonCultistOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            ABCSelectHelper.Run(options, board, turn,
                inIteration: 0,
                (b, t) => true, outIteration1: 1,
                (b, t) => true, outIteration2: 2,
                outIteration3: 3
                );

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 3,
                (board, turn) => true,
                swords: 2);

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 2,
                outIteration: 3,
                (board, turn) => true,
                mana: 2);

            EndCardHelper.Run(options, board, turn, 3);

            return options;
        }
    }

    internal class DragonclawOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1);

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 2,
                (b, t) => b.Players[t.Color].TrophyHall.Where(kv => kv.Key != Color.WHITE).Sum(kv => kv.Value) >= 5,
                swords: 2);

            EndCardHelper.Run(options, board, turn,
                endIteration: 2);

            return options;
        }
    }

    internal class RedDragonOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            SupplantOptionHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1);

            ReturnEnemySpyHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 2
                );

            GetOptionalVPHelper.Run(options, board, turn,
                inIteration: 2,
                outIteration: 3,
                (b, t) =>
                {
                    int ret = 0;

                    foreach (var location in b.Locations)
                    {
                        if (location.GetFullControl() == t.Color)
                        {
                            ret++;
                        }
                    }

                    return ret;
                }
                );

            EndCardHelper.Run(options, board, turn,
                endIteration: 3);

            return options;
        }
    }

    internal class RedWyrmlingOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                (board, turn) => true,
                swords: 2, mana: 2);

            EndCardHelper.Run(options, board, turn, 1);

            return options;
        }
    }

    internal class SeverinSilrajinOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                (board, turn) => true,
                swords: 5);

            EndCardHelper.Run(options, board, turn, 1);

            return options;
        }
    }
}
