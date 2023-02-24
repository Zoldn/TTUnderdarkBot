using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI;

namespace UnderdarkAI.AI.Selectors
{
    internal static class CardOrFreeActionSelection
    {
        /// <summary>
        /// Можно ли купить что-то на рынке
        /// </summary>
        /// <param name="board"></param>
        /// <param name="turn"></param>
        /// <returns></returns>
        public static bool IsAvailableBuy(Board board, Turn turn)
        {
            if (board.Market.Count == 0)
            {
                return false;
            }

            return board.Market.Min(c => c.ManaCost) <= turn.Mana;
        }
        /// <summary>
        /// Можно ли поставить юниты
        /// </summary>
        /// <returns></returns>
        public static bool IsPlaceActionAvailable(Board board, Turn turn)
        {
            if (turn.Swords < 1)
            {
                return false;
            }

            var possibleDeploys = turn.LocationStates
                .Where(l => l.Value.HasPresence && l.Key.FreeSpaces > 0)
                .ToList();

            return possibleDeploys.Any();
        }

        /// <summary>
        /// Можно ли вернуть шпиона
        /// </summary>
        /// <param name="board"></param>
        /// <param name="turn"></param>
        /// <returns></returns>
        public static bool IsReturnEnemySpyBySwords(Board board, Turn turn)
        {
            if (turn.Swords < 3)
            {
                return false;
            }

            var possibleSpiesRemoval = turn.LocationStates
                .Where(l => l.Value.HasPresence && l.Key.Spies.Any(kv => kv.Value && kv.Key != turn.Color))
                .ToList();

            return possibleSpiesRemoval.Any();
        }

        /// <summary>
        /// Можно ли убить за мечи
        /// </summary>
        /// <param name="board"></param>
        /// <param name="turn"></param>
        /// <returns></returns>
        public static bool IsAssassinateBySwords(Board board, Turn turn)
        {
            if (turn.Swords < 3)
            {
                return false;
            }

            var possibleAssasinates = turn.LocationStates
                .Where(l => l.Value.HasPresence && l.Key.Troops.Any(kv => kv.Value > 0 && kv.Key != turn.Color))
                .ToList();

            return possibleAssasinates.Any();
        }

        public static bool IsFreeAction(Board board, Turn turn)
        {
            return IsAvailableBuy(board, turn)
                || IsPlaceActionAvailable(board, turn)
                || IsReturnEnemySpyBySwords(board, turn)
                || IsAssassinateBySwords(board, turn);
        }

        public static bool IsCardInHands(Board board, Turn turn)
        {
            return turn.CardStates.Any(kv => kv.Value == CardState.IN_HAND);
        }
    }
}
