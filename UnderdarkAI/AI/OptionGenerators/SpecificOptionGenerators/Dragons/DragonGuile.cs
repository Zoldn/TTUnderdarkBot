using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.PlayableOptions;

namespace UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators.Dragons
{
    internal class EnchanterOfThayOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            PlaceOrReturnSpyHelper.Run(options, board, turn,
                inIteration: 0,
                outPlaceSpyIteration: 1,
                returnSpyIteration: 4,
                outReturnSpyIteration: 5);

            PlaceSpyHelper.Run(options, board, turn,
                inIteration: 1,
                returnIteration: 2,
                placeIteration: 3,
                outIteration: 5);

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 4,
                outIteration: 5,
                (b, t) => true,
                swords: 4
                );

            EndCardHelper.Run(options, board, turn,
                endIteration: 5);

            return options;
        }
    }

    internal class WatcherOfThayOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            PlaceOrReturnSpyHelper.Run(options, board, turn,
                inIteration: 0,
                outPlaceSpyIteration: 1,
                returnSpyIteration: 4,
                outReturnSpyIteration: 5);

            PlaceSpyHelper.Run(options, board, turn,
                inIteration: 1,
                returnIteration: 2,
                placeIteration: 3,
                outIteration: 5);

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 4,
                outIteration: 5,
                (b, t) => true,
                mana: 3
                );

            EndCardHelper.Run(options, board, turn,
                endIteration: 5);

            return options;
        }
    }

    internal class GreenDragonOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            PlaceOrReturnSpyHelper.Run(options, board, turn,
                inIteration: 0,
                outPlaceSpyIteration: 1,
                returnSpyIteration: 5,
                outReturnSpyIteration: 6);

            PlaceSpyHelper.Run(options, board, turn,
                inIteration: 1,
                returnIteration: 2,
                placeIteration: 3,
                outIteration: 4);

            SupplantOptionHelper.Run(options, board, turn,
                inIteration: 4,
                outIteration: 8,
                specificLocation: turn.PlacedSpies.ToHashSet());

            SupplantOptionHelper.Run(options, board, turn,
                inIteration: 6,
                outIteration: 7,
                specificLocation: turn.ReturnedSpies.ToHashSet());

            GetOptionalVPHelper.Run(options, board, turn,
                inIteration: 7,
                outIteration: 8,
                (b, t) => 
                {
                    int ret = 0;

                    foreach (var location in b.Locations)
                    {
                        if (location.BonusVP == 0)
                        {
                            continue;
                        }

                        if (location.GetControlPlayer() == t.Color)
                        {
                            ret += 1;
                        }
                    }

                    return ret;
                });

            EndCardHelper.Run(options, board, turn,
                endIteration: 8);

            return options;
        }
    }

    internal class GreenWyrmlingOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            PlaceSpyHelper.Run(options, board, turn,
                inIteration: 0,
                returnIteration: 1,
                placeIteration: 2,
                outIteration: 3);

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 3,
                outIteration: 4,
                (b, t) => OptionUtils.IsPlacedSpyContainsEnemyPlayerTroop(b, t),
                mana: 2
                );

            EndCardHelper.Run(options, board, turn,
                endIteration: 4);

            return options;
        }
    }

    internal class RathModarOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            PlaceSpyHelper.Run(options, board, turn,
                inIteration: 0,
                returnIteration: 1,
                placeIteration: 2,
                outIteration: 3);

            DrawCardHelper.Run(options, board, turn, cardCount: 2,
                inIteration: 3,
                outIteration: 4);

            EndCardHelper.Run(options, board, turn,
                endIteration: 4);

            return options;
        }
    }
}
