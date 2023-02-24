using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI;

namespace UnderdarkAI.AI.Selectors
{
    internal class CardOrFreeActionSelection : IEffectSelector
    {

        #region Statics 
        /// <summary>
        /// Можно ли купить что-то на рынке
        /// </summary>
        /// <param name="board"></param>
        /// <param name="turn"></param>
        /// <returns></returns>
        public static bool IsAvailableBuy(Board board, Turn turn)
        {
            bool buyFromMarketAvailable = board.Market.Count > 0
                && board.Market.Min(c => c.ManaCost) <= turn.Mana;

            bool buyLolthAvailable = board.Lolths > 0 && turn.Mana >= 2;
            bool buyHouseguardAvailable = board.HouseGuards > 0 && turn.Mana >= 3;

            return buyFromMarketAvailable || buyLolthAvailable || buyHouseguardAvailable;
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

            if (board.Players[turn.Color].Troops == 0)
            {
                return true;
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

        #endregion

        public CardOrFreeActionSelection() { }

        public Dictionary<List<IAtomicEffect>, double> GenerateOptions(Board board, Turn turn)
        {
            var ret = new Dictionary<List<IAtomicEffect>, double>();

            if (IsFreeAction(board, turn))
            {
                ret.Add(new List<IAtomicEffect>() { new ToFreeActionSelectionMode() }, 50.0d);
            }

            if (IsCardInHands(board, turn))
            {
                ret.Add(new List<IAtomicEffect>() { new ToCardToPlaySelectionMode() }, 50.0d);
            }

            if (!ret.Any())
            {
                ret.Add(new List<IAtomicEffect>() { new ToEndTurnSwitch() }, 100.0d);
            }

            return ret;
        }
    }

    internal class ToFreeActionSelectionMode : IAtomicEffect
    {
        public bool IsNewInfoRecieved => false;

        public Card? Card => null;

        public double Value { get; set; }

        public void ApplyEffect(Board board, Turn turn)
        {
            turn.State = SelectionState.SELECT_FREE_ACTION;
        }

        public void PrintEffect()
        {
            //Console.WriteLine($"Change selection to free selection");
        }
    }

    internal class ToCardToPlaySelectionMode : IAtomicEffect
    {
        public bool IsNewInfoRecieved => false;

        public Card? Card => null;

        public double Value { get; set; }

        public void ApplyEffect(Board board, Turn turn)
        {
            turn.State = SelectionState.SELECT_CARD;
        }

        public void PrintEffect()
        {
            //Console.WriteLine($"Select card to play from hand");
        }
    }

    internal class ToEndTurnSwitch : IAtomicEffect
    {
        public bool IsNewInfoRecieved => false;

        public Card? Card => null;

        public double Value { get; set; }

        public void ApplyEffect(Board board, Turn turn)
        {
            turn.State = SelectionState.SELECT_END_TURN;
        }

        public void PrintEffect()
        {
            Console.WriteLine($"Ending turn");
        }
    }
}
