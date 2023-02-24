
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
        public double Evaluate(Board board, Turn turn)
        {
            var (controlVPs, totalControlVPs) = board.GetControlVPs();

            var results = new List<(Color Color, int Score)>(4);

            var currentPlayerScore = 0;

            foreach (var (color, player) in board.Players)
            {
                int result = player.TrophyHallVP + player.DeckVP + player.PromoteVP 
                    + controlVPs[color] + totalControlVPs[color];

                results.Add((color, result));

                if (color == turn.Color)
                {
                    currentPlayerScore = result;
                }
            }

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
    }
}
