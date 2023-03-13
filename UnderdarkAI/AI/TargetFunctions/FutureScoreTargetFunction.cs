using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.RotationEstimator;
using UnderdarkAI.Context;

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

            foreach (var (color, value) in futureScore)
            {
                currentResults[color] += value;
            }

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

            foreach (var (color, value) in townFutureScore)
            {
                currentResults[color] += value;
            }


            return VPScoreTargetFunction.GetDifferenceWithClosestOpponent(currentResults, board, turn);
        }
    }
}
