using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.RotationEstimator;

namespace UnderdarkAI.AI.TargetFunctions
{

    internal sealed class CachedScoreValue
    {
        public Dictionary<Color, double> Scores { get; set; }
        public int Count { get; set; }
        /// <summary>
        /// Значения оставшейся части целевой функции 
        /// Ключ - округленное до двух знаков значение полной целевой функции
        /// Значение - количество заходов в это значение
        /// </summary>
        public Dictionary<double, int> OtherPartValues { get; set; }
        public CachedScoreValue()
        {
            Scores = new();
            OtherPartValues = new();
            Count = 0;
        }
    }

    internal interface IMapScoreCalculator 
    {
        public Dictionary<Color, double> CalculateMapScore(Board board, Turn turn);
    }

    internal interface IPlayerZoneScoreCalculator
    {
        public Dictionary<Color, double> CalculatePlayerZoneScore(Board board, Turn turn);
    }


    internal class CashedTargetFunction : ITargetFunction
    {
        public Dictionary<MapState, CachedScoreValue> CachedMapStateInfo { get; private set; }
        //public Dictionary<PlayerState, CachedScoreValue> CachedPlayerStateInfo { get; private set; }

        public BaseRotationEstimator? RotationEstimator { get; init; }
        public AgainstHumanStrategy AgainstHumanStrategy { get; init; }

        public IMapScoreCalculator MapScoreCalculator { get; private set; }
        public IPlayerZoneScoreCalculator PlayerZoneCalculator { get; private set; }

        public CashedTargetFunction(IMapScoreCalculator mapScorer,
            IPlayerZoneScoreCalculator playerScorer)
        {
            RotationEstimator = null;
            AgainstHumanStrategy = AgainstHumanStrategy.DEFAULT;

            MapScoreCalculator = mapScorer;
            PlayerZoneCalculator = playerScorer;

            CachedMapStateInfo = new();
        }

        public double Evaluate(Board board, Turn turn)
        {
            var initialScore = turn.AllPlayers
                .ToDictionary(
                    c => c,
                    c => 0.0d
                );

            var mapResults = GetScoreForMap(board, turn);

            AddToThis(initialScore, mapResults.Scores);

            var playerResults = GetScoreForPlayerZone(board, turn);

            AddToThis(initialScore, playerResults);

            var diff = GetDifferenceWithClosestOpponent(initialScore, board, turn);

            mapResults.Count++;

            var roundedDiff = Math.Round(diff, 2);

            if (mapResults.OtherPartValues.ContainsKey(roundedDiff))
            {
                mapResults.OtherPartValues[roundedDiff] += 1;
            }
            else
            {
                mapResults.OtherPartValues[roundedDiff] = 1;
            }

            return diff;
        }

        public CachedScoreValue GetScoreForMap(Board board, Turn turn)
        {
            if (CachedMapStateInfo.TryGetValue(board.MapState, out var val))
            {
                return val;
            }
            else
            {
                var score = CalculateNewMapScore(board, turn);

                var info = new CachedScoreValue()
                {
                    Scores = score,
                };

                CachedMapStateInfo.Add(board.MapState, info);

                return info;
            }
        }

        public Dictionary<Color, double> CalculateNewMapScore(Board board, Turn turn)
        {
            return MapScoreCalculator.CalculateMapScore(board, turn);
        }

        public Dictionary<Color, double> GetScoreForPlayerZone(Board board, Turn turn)
        {
            return PlayerZoneCalculator.CalculatePlayerZoneScore(board, turn);
        }

        public static Dictionary<Color, double> AddToThis(Dictionary<Color, double> target,
            Dictionary<Color, double> addable)
        {
            foreach (var (color, value) in addable)
            {
                target[color] += value;
            }

            return target;
        }

        public double GetDifferenceWithClosestOpponent(Dictionary<Color, double> results,
            Board board, Turn turn)
        {
            var currentPlayerScore = results.Single(p => p.Key == turn.Color).Value;

            if (AgainstHumanStrategy == AgainstHumanStrategy.AGGRESSIVE
                && !board.Players[turn.Color].IsHuman
                && board.Players.Any(e => e.Value.IsHuman))
            {
                results = results
                    .Where(r => board.Players[r.Key].IsHuman
                        || r.Key == turn.Color
                    )
                    .ToDictionary(kv => kv.Key, kv => kv.Value);
            }

            var lresults = results
                .Select(kv => (Color: kv.Key, Score: kv.Value))
                .OrderByDescending(p => p.Score)
                .ToList();

            if (lresults[0].Color == turn.Color)
            {
                return currentPlayerScore - lresults[1].Score;
            }
            else
            {
                return currentPlayerScore - lresults[0].Score;
            }
        }
    }
}
