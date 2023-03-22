using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using TUnderdark.TTSParser;
using UnderdarkAI.AI.RotationEstimator;
using UnderdarkAI.Context;
using TUnderdark.Utils;

namespace UnderdarkAI.AI.TargetFunctions
{
    internal class FutureScoreTargetFunction : ITargetFunction
    {
        private ModelContext context;
        public FutureScoreTargetFunction(ModelContext context) 
        {
            this.context = context;
        }
        public double Evaluate(Board board, Turn turn)
        {
            //double rotations = turn.RotationsLeft;
            var estimator = new BaseRotationEstimator();

            var basicScoreFunction = new VPScoreTargetFunction();

            var currentResults = basicScoreFunction.GetScores(board, turn)
                .ToDictionary(kv => kv.Color, kv => kv.Score);

            Dictionary<Color, double> futureScore = DeckFutureScoreEstimate(board, turn, estimator);

            foreach (var (color, value) in futureScore)
            {
                currentResults[color] += value;
            }

            //Dictionary<Color, double> townPresentScore = TownPresentScoreEstimate(board, turn, estimator);

            //foreach (var (color, value) in townPresentScore)
            //{
            //    currentResults[color] += value;
            //}

            Dictionary<Color, double> townFutureScore = TownFutureScoreEstimate(board, turn, estimator);

            foreach (var (color, value) in townFutureScore)
            {
                currentResults[color] += value;
            }

            return VPScoreTargetFunction.GetDifferenceWithClosestOpponent(currentResults, board, turn);
        }

        private Dictionary<Color, double> DeckFutureScoreEstimate(Board board, Turn turn, BaseRotationEstimator estimator)
        {
            Dictionary<Color, double> futureScore = board.Players
                .ToDictionary(
                    kv => kv.Key,
                    kv => 0.0d
                    );

            /// Оценка будущего value карт
            foreach (var color in turn.AllPlayers)
            {
                var player = board.Players[color];

                var totalCards = player.Hand
                    .Concat(player.Deck)
                    .Concat(player.Discard)
                    .ToList();

                var promotes = totalCards.Sum(e => context.CardsStatsDict[e.SpecificType].PromoteSpeed);

                var devoures = totalCards.Sum(e => context.CardsStatsDict[e.SpecificType].DevourSpeed);

                var drawers = totalCards.Sum(e => context.CardsStatsDict[e.SpecificType].DrawSpeed);

                var totalCardValueForRotation = totalCards
                    .Sum(c => context.CardsStatsDict[c.SpecificType].BaseValuePerTurn);

                var rotations = estimator.CalculateRotations(board, turn, color,
                    promotes: promotes, devoures: devoures, drawers: drawers);

                var futureValueForCards = rotations * totalCardValueForRotation;

                futureScore[color] += futureValueForCards;
            }

            return futureScore;
        }

        private static Dictionary<Color, double> TownPresentScoreEstimate(Board board, Turn turn, BaseRotationEstimator estimator)
        {
            ///Оценка будущего вэлью контроля городов от маны и бонусных VP
            var townFutureScore = board.Players
                .ToDictionary(
                    kv => kv.Key,
                    kv => 0.0d
                    );

            var turnLeft = estimator.RoundLeftEstimator(board, turn);

            foreach (var location in board.Locations)
            {
                var controlPlayer = location.GetControlPlayer();

                if (controlPlayer != Color.WHITE
                    && controlPlayer.HasValue)
                {
                    townFutureScore[controlPlayer.Value] += 0.4d * turnLeft;
                }

                var totalControlPlayer = location.GetFullControl();

                if (totalControlPlayer != Color.WHITE
                    && totalControlPlayer.HasValue)
                {
                    townFutureScore[totalControlPlayer.Value] += location.BonusVP * turnLeft;
                }
            }

            return townFutureScore;
        }

        private class LocationRoundDistance
        {
            public Location Location { get; }
            public double RoundDistance { get; set; }
            public double PreformedDistance { get; private set; }
            public bool MinCurrentOrNew(double newValue)
            {
                if (PreformedDistance > newValue)
                {
                    PreformedDistance = newValue;
                    return true;
                }

                return false;
            }
            public LocationRoundDistance(Location location)
            {
                Location = location;

                Reset();
            }

            internal void Reset()
            {
                RoundDistance = double.PositiveInfinity;
                PreformedDistance = double.PositiveInfinity;
            }
        }

        private class LocationCityFutureImpact 
        {
            public Location Location { get; }
            public double RoundsToControl { get; set; }
            public double RoundsToFullControl { get; set; }
            public bool IsControlUsed { get; set; }
            public bool IsFullControlUsed { get; set; }
            public LocationCityFutureImpact(Location location)
            {
                Location = location;
                Reset();
            }

            public double GetClosestImpact(double turnLeft)
            {
                if (!IsControlUsed)
                {
                    if (RoundsToControl < turnLeft)
                    {
                        return (turnLeft - RoundsToControl) * 0.4d;
                    }
                    else
                    {
                        return 0.0d;
                    }
                }

                if (!IsFullControlUsed)
                {
                    if ((RoundsToFullControl - RoundsToControl) < turnLeft)
                    {
                        return (turnLeft - (RoundsToFullControl - RoundsToControl)) * Location.BonusVP;
                    }
                    else
                    {
                        return 0.0d;
                    }
                }

                return 0.0d;
            }

            public void Reset()
            {
                RoundsToControl = double.PositiveInfinity;
                RoundsToFullControl = double.PositiveInfinity;
                IsControlUsed = false;
                IsFullControlUsed = false;
            }
        }

        private Dictionary<Color, double> TownFutureScoreEstimate(Board board, Turn turn, BaseRotationEstimator estimator)
        {
            ///Оценка будущего вэлью контроля городов от маны и бонусных VP
            var townFutureScore = board.Players
                .ToDictionary(
                    kv => kv.Key,
                    kv => 0.0d
                    );

            var turnLeft = estimator.RoundLeftEstimator(board, turn);

            var locationDistances = board
                .Locations
                .ToDictionary(
                    l => l,
                    l => new LocationRoundDistance(l)
                );

            foreach (var color in turn.AllPlayers)
            {
                var player = board.Players[color];

                if (color == Color.YELLOW)
                {
                    int y = 1;
                }

                var totalCards = player.Hand
                    .Concat(player.Deck)
                    .Concat(player.Discard)
                    .ToList();

                double handSize = 5.0d;

                var whiteDisplacementsPerRound = totalCards
                    .Average(c => context.CardsStatsDict[c.SpecificType].WhiteDisplacement)
                    * handSize;
                var colorDisplacementsPerRound = totalCards
                    .Average(c => context.CardsStatsDict[c.SpecificType].ColorDisplacement)
                    * handSize;
                var deploymentsPerRound = totalCards
                    .Average(c => context.CardsStatsDict[c.SpecificType].DeploySpeed)
                    * handSize;
                var spiesPerRound = totalCards.Average(
                    c => CardMapper.SpecificTypeCardMakers[c.SpecificType].CardType == CardType.GUILE ? 1 : 0
                    ) * handSize;
                var returnEnemySpiesPerRound = totalCards
                    .Average(c => context.CardsStatsDict[c.SpecificType].ReturnEnemySpy)
                    * handSize;

                var roundsToWD = whiteDisplacementsPerRound > 0.0d ? 1 / whiteDisplacementsPerRound : double.PositiveInfinity;
                var roundsToCD = colorDisplacementsPerRound > 0.0d ? 1 / colorDisplacementsPerRound : double.PositiveInfinity;
                var roundsToDP = deploymentsPerRound > 0.0d ? 1 / deploymentsPerRound : double.PositiveInfinity;
                var roundsToSP = spiesPerRound > 0.0d ? 1 / spiesPerRound : double.PositiveInfinity;
                var roundsToRS = returnEnemySpiesPerRound > 0.0d ? 1 / returnEnemySpiesPerRound : double.PositiveInfinity;

                foreach (var (location, distanceInfo) in locationDistances)
                {
                    if (location.IsPresence(color))
                    {
                        distanceInfo.RoundDistance = 0.0d;
                    }
                }

                /// Инициализируем нулями точки, где есть присутствие
                foreach (var (location, distanceInfo) in locationDistances)
                {
                    if (location.IsPresence(color))
                    {
                        distanceInfo.RoundDistance = 0.0d;
                    }
                }

                int posInfCountBefore = locationDistances.Count(d => double.IsPositiveInfinity(d.Value.RoundDistance));

                /// Ищем число ходов, чтобы добраться (получить присутствие) во всех точек на карте
                while (true)
                {
                    /// Идем по непроверенным точкам
                    foreach (var (location, distanceInfo) in locationDistances)
                    {
                        //if (!double.IsPositiveInfinity(distanceInfo.RoundDistance))
                        //{
                        //    continue;
                        //}

                        if (location.Id == LocationId.Phaerlin)
                        {
                            int y = 1;
                        }

                        locationDistances[location].MinCurrentOrNew(roundsToSP);

                        foreach (var neighboor in location.Neighboors)
                        {
                            if (neighboor.Id == LocationId.Phaerlin2Everfire)
                            {
                                int z = 1;
                            }

                            if (double.IsPositiveInfinity(locationDistances[neighboor].RoundDistance))
                            {
                                continue;
                            }

                            if (neighboor.FreeSpaces > 0)
                            {
                                locationDistances[location].MinCurrentOrNew(
                                    locationDistances[neighboor].RoundDistance + roundsToDP
                                    );
                            }
                            else
                            {
                                if (neighboor.Troops[Color.WHITE] > 0)
                                {
                                    locationDistances[location].MinCurrentOrNew(
                                        locationDistances[neighboor].RoundDistance + Math.Max(roundsToDP, roundsToWD)
                                        );
                                }

                                if (neighboor.Troops.Any(kv => kv.Key != color && kv.Key != Color.WHITE && kv.Value > 0))
                                {
                                    locationDistances[location].MinCurrentOrNew(
                                        locationDistances[neighboor].RoundDistance + Math.Max(roundsToDP, roundsToCD)
                                        );
                                }
                            }
                        }
                    }

                    foreach (var (location, distanceInfo) in locationDistances)
                    {
                        //if (double.IsPositiveInfinity(distanceInfo.RoundDistance)
                        //    && !double.IsPositiveInfinity(distanceInfo.PreformedDistance)
                        //    )
                        //{
                        //    distanceInfo.RoundDistance = distanceInfo.PreformedDistance;
                        //}
                        if (distanceInfo.RoundDistance > distanceInfo.PreformedDistance)
                        {
                            distanceInfo.RoundDistance = distanceInfo.PreformedDistance;
                        }
                    }

                    int posInfCountAfter = locationDistances.Count(d => double.IsPositiveInfinity(d.Value.RoundDistance));

                    if (posInfCountAfter == posInfCountBefore)
                    {
                        break;
                    }
                    else
                    {
                        posInfCountBefore = posInfCountAfter;
                    }
                }

                /// Идем по городам
                var cityDistancesForFull = new Dictionary<Location, LocationCityFutureImpact>();

                foreach (var (location, distanceInfo) in locationDistances)
                {
                    if (location.BonusVP == 0)
                    {
                        continue;
                    }

                    cityDistancesForFull.Add(location, new LocationCityFutureImpact(location));
                }

                foreach (var (location, distanceInfo) in cityDistancesForFull)
                {
                    int whiteToDispForFull = location.Troops[Color.WHITE];
                    int colorToDispForFull = location.Troops
                        .Where(kv => kv.Key != color && kv.Key != Color.WHITE)
                        .Sum(kv => kv.Value);
                    int spiesToDispForFull = location.Spies
                        .Count(kv => kv.Key != color);
                    int troopsDeployForFull = location.Size - location.Troops[color];

                    var times = new List<double>(4)
                    {
                        whiteToDispForFull * roundsToWD,
                        colorToDispForFull * roundsToCD,
                        spiesToDispForFull * roundsToRS,
                        troopsDeployForFull * roundsToDP,
                    };

                    var roundToGetFullControl = locationDistances[location].RoundDistance + times.Max();

                    distanceInfo.RoundsToFullControl = roundToGetFullControl;
                }

                foreach (var (location, distanceInfo) in cityDistancesForFull)
                {
                    int halfSize = (int)Math.Ceiling((location.Size + 0.1d) / 2.0d);

                    int whiteToDispForControl = 0;
                    int colorToDispForControl = 0;

                    if (location.FreeSpaces + location.Troops[color] < halfSize)
                    {
                        int toDisplace = halfSize - (location.FreeSpaces + location.Troops[color]);

                        whiteToDispForControl = Math.Min(location.Troops[Color.WHITE], toDisplace);
                        colorToDispForControl = Math.Min(location.Troops
                            .Where(kv => kv.Key != color && kv.Key != Color.WHITE)
                            .Sum(kv => kv.Value), toDisplace);
                    }

                    int troopsDeployForControl = halfSize > location.Troops[color] ? halfSize - location.Troops[color] : 0;

                    var times = new List<double>(4)
                    {
                        whiteToDispForControl * roundsToWD,
                        colorToDispForControl * roundsToCD,
                        troopsDeployForControl * roundsToDP,
                    };

                    var roundToGetFullControl = locationDistances[location].RoundDistance + times.Max();

                    distanceInfo.RoundsToControl = roundToGetFullControl;
                }

                double currentTurnLeft = turnLeft;

                while (currentTurnLeft > 0)
                {
                    var locationFIs = cityDistancesForFull
                        .Select(kv => kv.Value)
                        .Where(kv => !kv.IsControlUsed || !kv.IsFullControlUsed)
                        .ToList();

                    if (locationFIs.Count == 0)
                    {
                        break;
                    }

                    var locationFI = locationFIs.ArgMax(c => c.GetClosestImpact(currentTurnLeft));

                    townFutureScore[color] += locationFI.GetClosestImpact(currentTurnLeft);

                    if (!locationFI.IsControlUsed)
                    {
                        locationFI.IsControlUsed = true;
                        currentTurnLeft -= locationFI.RoundsToControl;
                    }
                    else
                    {
                        locationFI.IsFullControlUsed = true;
                        currentTurnLeft -= (locationFI.RoundsToFullControl - locationFI.RoundsToControl);
                    }
                }

                foreach (var (_, info) in locationDistances)
                {
                    info.Reset();
                }
            }

            return townFutureScore;
        }
    }
}