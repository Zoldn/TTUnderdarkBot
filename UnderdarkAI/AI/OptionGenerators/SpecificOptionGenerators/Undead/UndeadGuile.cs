using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.PlayableOptions;

namespace UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators.Undead
{
    internal class BansheeOptionGenetator : OptionGenerator
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
                (b, t) => OptionUtils.IsPlacedSpyContainsEnemySpy(b, t),
                swords: 3
                );

            EndCardHelper.Run(options, board, turn,
                endIteration: 4);

            return options;
        }
    }

    internal class ConjurerOptionGenetator : OptionGenerator
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
                outIteration: 99);

            BuyOrRecruitCardHelper.Run(options, board, turn, 
                inIteration: 5, 
                outIteration: 6, 
                costLimit: 3);

            BuyOrRecruitCardHelper.Run(options, board, turn,
                inIteration: 6,
                outIteration: 99,
                costLimit: 3);

            EndCardHelper.Run(options, board, turn,
                endIteration: 99);

            return options;
        }
    }

    internal class GhostOptionGenetator : OptionGenerator
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
                outIteration: 99);

            EnableBuyFromDevouredHelper.Run(options, board, turn,
                inIteration: 5, outIteration: 99);

            EndCardHelper.Run(options, board, turn,
                endIteration: 99);

            return options;
        }
    }

    internal class LichOptionGenetator : OptionGenerator
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
          

            TakeTroopFromEnemyTrophyAndDeploy.Run(options, board, turn,
                inIteration: 3,
                outIteration: 4,
                exitIteration: 99,
                filterPlayerColors: playerColors
                );

            DeployOptionHelper.Run(options, board, turn,
                inIteration: 4,
                outIteration: 5,
                isFromTrophy: true,
                isAnywhere: false);

            TakeTroopFromEnemyTrophyAndDeploy.Run(options, board, turn,
                inIteration: 5,
                outIteration: 6,
                exitIteration: 99,
                filterPlayerColors: playerColors
                );

            DeployOptionHelper.Run(options, board, turn,
                inIteration: 6,
                outIteration: 99,
                isFromTrophy: true,
                isAnywhere: false);

            EndCardHelper.Run(options, board, turn,
                endIteration: 99);

            return options;
        }
    }

    internal class WraithOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            PlaceSpyHelper.Run(options, board, turn,
                inIteration: 0,
                returnIteration: 1,
                placeIteration: 2,
                outIteration: 3);

            ABCSelectHelper.Run(options, board, turn,
                inIteration: 3,
                (b, t) => 
                {
                    var location = b.LocationIds[t.PlacedSpies.Single()];
                    return location.Troops.Any(kv => kv.Key != t.Color && kv.Value > 0);
                }, outIteration1: 4, // Съесть себя и убить в этой локации
                (b, t) => true, outIteration2: 99, // ничего не делать
                outIteration3: 99
                );

            DevourSelfHelper.Run(options, board, turn,
                inIteration: 4,
                outIteration: 5);

            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 5,
                outIteration: 99,
                specificLocation: turn.PlacedSpies.ToHashSet());

            EndCardHelper.Run(options, board, turn,
                endIteration: 99);

            return options;
        }
    }
}
