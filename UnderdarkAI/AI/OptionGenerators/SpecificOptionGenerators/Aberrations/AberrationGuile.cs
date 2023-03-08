using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.PlayableOptions;

namespace UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators.Aberrations
{
    internal class AbolethOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            ABCSelectHelper.Run(options, board, turn,
                inIteration: 0,
                (b, t) => true, outIteration1: 10,
                (b, t) => b.Players[t.Color].Spies < 5, outIteration2: 20,
                outIteration3: 99);

            PlaceSpyHelper.Run(options, board, turn,
                inIteration: 10,
                returnIteration: 11,
                placeIteration: 12,
                outIteration: 13);

            PlaceSpyHelper.Run(options, board, turn,
                inIteration: 13,
                returnIteration: 14,
                placeIteration: 15,
                outIteration: 99);

            DrawCardHelper.Run(options, board, turn,
                cardCount: 5 - board.Players[turn.Color].Spies,
                inIteration: 20,
                outIteration: 99
                );

            EndCardHelper.Run(options, board, turn,
                endIteration: 99);

            return options;
        }
    }

    internal class BrainwashedSlaveOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            PlaceOrReturnSpyHelper.Run(options, board, turn,
                inIteration: 0,
                outPlaceSpyIteration: 10,
                returnSpyIteration: 20,
                outReturnSpyIteration: 21);

            PlaceSpyHelper.Run(options, board, turn,
                inIteration: 10,
                returnIteration: 11,
                placeIteration: 12,
                outIteration: 99);

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 21,
                outIteration: 99,
                (b, t) => true,
                swords: 2,
                mana: 2
                );

            EndCardHelper.Run(options, board, turn,
                endIteration: 99);

            return options;
        }
    }

    internal class ChuulOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            PlaceSpyHelper.Run(options, board, turn,
                inIteration: 0,
                returnIteration: 1,
                placeIteration: 2,
                outIteration: 3);

            var playerColors = new HashSet<Color>();

            if (turn.PlacedSpies.Count == 1)
            {
                var locationId = turn.PlacedSpies.Single();
                var location = board.LocationIds[locationId];
                playerColors = location.Troops
                    .Where(kv => kv.Key != turn.Color && kv.Key != Color.WHITE && kv.Value > 0)
                    .Select(kv => kv.Key)
                    .ToHashSet();
            }
            else if (turn.PlacedSpies.Count > 1)
            {
                throw new ArgumentOutOfRangeException();
            }

            OpponentDiscardHelper.Run(options, board, turn,
                initiator: CardSpecificType.CHUUL,
                specificTargets: playerColors,
                inIteration: 3,
                outIteration: 99,
                isToAll: true);

            EndCardHelper.Run(options, board, turn,
                endIteration: 99);

            return options;
        }
    }

    internal class CloakerOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            PlaceOrReturnSpyHelper.Run(options, board, turn,
                inIteration: 0,
                outPlaceSpyIteration: 10,
                returnSpyIteration: 20,
                outReturnSpyIteration: 21);

            PlaceSpyHelper.Run(options, board, turn,
                inIteration: 10,
                returnIteration: 11,
                placeIteration: 12,
                outIteration: 99);

            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 21,
                outIteration: 99,
                specificLocation: turn.ReturnedSpies.ToHashSet(),
                isAnyWhere: true
                );

            EndCardHelper.Run(options, board, turn,
                endIteration: 99);

            return options;
        }
    }

    internal class NothicOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            PlaceOrReturnSpyHelper.Run(options, board, turn,
                inIteration: 0,
                outPlaceSpyIteration: 10,
                returnSpyIteration: 20,
                outReturnSpyIteration: 21);

            PlaceSpyHelper.Run(options, board, turn,
                inIteration: 10,
                returnIteration: 11,
                placeIteration: 12,
                outIteration: 99);

            DrawCardHelper.Run(options, board, turn,
                cardCount: 1,
                inIteration: 21,
                outIteration: 22
                );

            OpponentDiscardHelper.Run(options, board, turn,
                initiator: CardSpecificType.NOTHIC,
                specificTargets: turn.EnemyPlayers,
                inIteration: 22,
                outIteration: 99,
                isToAll: true);

            EndCardHelper.Run(options, board, turn,
                endIteration: 99);

            return options;
        }
    }
}
