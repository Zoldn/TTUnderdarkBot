using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace UnderdarkAI.AI.OptionGenerators
{
    internal class PlayCardOrBaseActionOptionGenerator : OptionGenerator
    {
        public override SelectionState State => SelectionState.CARD_OR_FREE_ACTION;

        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var ret = new List<PlayableOption>(2);

            if (IsFreeAction(board, turn))
            {
                ret.Add(new SwitchToBaseActionSelectionOption() { Weight = 50.0d });
            }

            if (IsCardInHands(board, turn))
            {
                ret.Add(new SwitchToCardSelectionOption() { Weight = 50.0d });
            }

            if (!ret.Any())
            {
                ret.Add(new SwitchToEndTurnSelectionOption() { Weight = 100.0d });
            }

            return ret;
        }

        #region Statics 
        /// <summary>
        /// Можно ли купить что-то на рынке
        /// </summary>
        /// <param name="board"></param>
        /// <param name="turn"></param>
        /// <returns></returns>
        public static bool IsAvailableBuy(Board board, Turn turn)
        {
            if (!turn.IsBuyingEnabled)
            {
                return false;
            }

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
            return turn.CardStates.Any(s => s.State == CardState.IN_HAND);
        }

        #endregion
    }

    internal class SwitchToCardSelectionOption : PlayableOption
    {
        public SwitchToCardSelectionOption() : base()
        {
            NextState = SelectionState.SELECT_CARD;
        }
        public override void ApplyOption(Board board, Turn turn)
        {

        }

        public override int MinVerbosity => 10;


        public override string GetOptionText()
        {
            return $"Switch to card to play selection";
        }
    }

    internal class SwitchToBaseActionSelectionOption : PlayableOption
    {
        public SwitchToBaseActionSelectionOption() : base()
        {
            NextState = SelectionState.SELECT_BASE_ACTION;
        }
        public override void ApplyOption(Board board, Turn turn)
        {

        }

        public override int MinVerbosity => 10;

        public override string GetOptionText()
        {
            return $"Switch to free action selection";
        }
    }

    internal class SwitchToEndTurnSelectionOption : PlayableOption
    {
        public SwitchToEndTurnSelectionOption() : base()
        {
            NextState = SelectionState.SELECT_CARD_END_TURN;
        }
        public override void ApplyOption(Board board, Turn turn)
        {

        }

        public override int MinVerbosity => 0;


        public override string GetOptionText()
        {
            return $"In the end of turn action(s)";
        }
    }
}
