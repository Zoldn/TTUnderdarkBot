
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace UnderdarkAI.AI.TargetFunctions
{
    internal class VPScoreTargetFunction : ITargetFunction
    {
        public VPScoreTargetFunction() { }

        public List<(Color Color, double Score)> GetScores(Board board, Turn turn)
        {
            var (controlVPs, totalControlVPs) = board.GetControlVPs();

            var results = new List<(Color Color, double Score)>(4);

            foreach (var (color, player) in board.Players)
            {
                int result = player.TrophyHallVP + player.DeckVP + player.PromoteVP
                    + controlVPs[color] + totalControlVPs[color];

                results.Add((color, result));
            }

            return results;
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
