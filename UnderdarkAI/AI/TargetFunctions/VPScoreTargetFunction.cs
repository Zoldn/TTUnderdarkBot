
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.RotationEstimator;

namespace UnderdarkAI.AI.TargetFunctions
{
    internal class VPScoreTargetFunction : ITargetFunction
    {
        private BaseRotationEstimator? rotationEstimator { get; }
        public VPScoreTargetFunction() 
        {
            rotationEstimator = null;
        }

        public VPScoreTargetFunction(BaseRotationEstimator estimator)
        {
            rotationEstimator = estimator;
        }

        public List<(Color Color, double Score)> GetScores(Board board, Turn turn)
        {
            var (controlVPs, totalControlVPs) = board.GetControlVPs();

            var results = new List<(Color Color, double Score)>(4);
            
            var controlPenaltyCoef = PenaltyCoefForControl(board, turn);

            foreach (var (color, player) in board.Players)
            {
                double result = player.TrophyHallVP 
                    + player.DeckVP + player.PromoteVP
                    + controlPenaltyCoef * controlVPs[color] 
                    + controlPenaltyCoef * totalControlVPs[color];

                results.Add((color, result));
            }

            return results;
        }

        private double PenaltyCoefForControl(Board board, Turn turn)
        {
            if (rotationEstimator is null)
            {
                return 1.0d;
            }

            var turnLeft = rotationEstimator.RoundLeftEstimator(board, turn);

            if (turnLeft < 2.0d)
            {
                return 1.0d;
            }

            if (turnLeft < 10.0d)
            {
                return (turnLeft - 10.0d) / (2.0d - 10.0d) * 0.9d + 0.1d;
            }

            return 0.1d;
        }

        public static double GetDifferenceWithClosestOpponent(Dictionary<Color, double> results,
            Board board, Turn turn)
        {
            return GetDifferenceWithClosestOpponent(results.Select(kv => (kv.Key, kv.Value)).ToList(), board, turn);
        }

        public static double GetDifferenceWithClosestOpponent(List<(Color Color, double Score)> results,
            Board board, Turn turn)
        {
            var currentPlayerScore = results.Single(p => p.Color == turn.Color).Score;

            results = results
                .OrderByDescending(p => p.Score)
                .ToList();

            if (results[0].Color == turn.Color)
            {
                return currentPlayerScore - results[1].Score;
            }
            else
            {
                return currentPlayerScore - results[0].Score;
            }
        }

        public double Evaluate(Board board, Turn turn)
        {
            var results = GetScores(board, turn);

            return GetDifferenceWithClosestOpponent(results, board, turn);
        }
    }
}
