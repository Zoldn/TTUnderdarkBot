using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace UnderdarkAI.AI.RotationEstimator
{
    internal class BaseRotationEstimator
    {
        public BaseRotationEstimator() { }
        public double CalculateRotations(Board board, Turn turn, Color color,
            double promotes = 0.0d, double devoures = 0.0d, double drawers = 0.0d)
        {
            //var player = board.Players[turn.Color];
            var player = board.Players[color];
            double deck = player.Deck.Count + player.Hand.Count;
            double discard = player.Discard.Count;
            double totalCards = deck + discard;

            //var promotes = 0.0d; // Число карт - промоутеров
            //var devoures = 0.0d; // Число карт - пожирателей
            //var cardDrawers = 0.0d; // Число карт дополнительного дрова карт

            var buyingCardSpeed = 1.3d; // Скорость покупки карт

            int roundEstimate = (int)Math.Ceiling(RoundLeftEstimator(board, turn));

            double rotations = 0.0d;

            bool isFirstRotation = true;

            for (int round = 0; round < roundEstimate; round++)
            {
                var playingCardSpeed = 5.0d + drawers / totalCards; // Скорость розыгрыша карт
                //var diffCount = buyingCardSpeed - (promotes + devoures) * playingCardSpeed / deck;

                discard += playingCardSpeed + buyingCardSpeed - (promotes + devoures) * playingCardSpeed / totalCards;

                deck -= playingCardSpeed;

                if (!isFirstRotation)
                {
                    rotations += playingCardSpeed / totalCards;
                }

                if (deck <= 0)
                {
                    isFirstRotation = false;
                    
                    deck += discard;
                    discard = 0;
                }

                totalCards = discard + deck;

                if (totalCards < 8.0d)
                {
                    discard += (8.0d - totalCards);
                    totalCards = 8.0d;
                }
            }

            return rotations;
        }

        public double RoundLeftEstimator(Board board, Turn turn)
        {
            var marketProgression = (74 - board.Deck.Count) / 74.0d;

            var progression = board.Players
                .Select(kv => (40 - kv.Value.Troops) / 40.0d)
                .Append(marketProgression)
                .Max();

            Debug.Assert(progression <= 1.0d);
            Debug.Assert(progression >= 0.0d);

            var defaultEstimate = turn.CurrentRound < 15.0d ? 15.0d - turn.CurrentRound : 0.0d;

            var stateEstimate = progression > 0.1d ? turn.CurrentRound * (1.0d / progression - 1.0d) :
                defaultEstimate;

            var alpha = Math.Sqrt(progression);

            var estimateRounds = (1 - alpha) * defaultEstimate
                + alpha * stateEstimate;

            return estimateRounds;
        }
    }
}
