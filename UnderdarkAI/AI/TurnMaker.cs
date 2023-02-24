using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.Selectors;
using UnderdarkAI.AI.TargetFunctions;
using UnderdarkAI.MetricUtils;
using UnderdarkAI.Utils;

namespace UnderdarkAI.AI
{
    internal sealed class TurnMaker
    {
        private Random random;
        /// <summary>
        /// Исходное состояние игры, не менять
        /// </summary>
        public Board InitialBoard { get; }
        /// <summary>
        /// Цвет игрока, за которого надо сделать ход
        /// </summary>
        public Color Color { get; }
        /// <summary>
        /// Количество рестартов с начального решения
        /// </summary>
        public int RestartLimit { get; set; }

        public Dictionary<SelectionState, IEffectSelector> StateSelectors { get; private set; }
        public ITargetFunction TargetFunction { get; private set; }

        public TurnMaker(Board board, Color color, int? seed = null)
        {
            InitialBoard = board;
            Color = color;
            RestartLimit = 1;

            if (seed.HasValue)
            {
                random = new Random(seed.Value);
            }
            else
            {
                random = new Random();
            }

            StateSelectors = new Dictionary<SelectionState, IEffectSelector>()
            {
                { SelectionState.CARD_OR_FREE_ACTION, new CardOrFreeActionSelection() },
                { SelectionState.SELECT_END_TURN, new EndTurnSelection() },
                { SelectionState.SELECT_CARD, new CardToPlaySelection() },
                { SelectionState.SELECT_CARD_OPTION, new CardOptionSelection() },
                { SelectionState.SELECT_FREE_ACTION, new FreeActionSelection() },
            };

            TargetFunction = new VPScoreTargetFunction();
        }

        public void MakeTurn()
        {
            for (int iteration = 0; iteration < RestartLimit; iteration++)
            {
                CopyRoot(out var board, out var turn);

                Selection(board, turn);
            }
        }

        private void CopyRoot(out Board board, out Turn turn)
        {
            /// Клонируем исходное состояние
            board = InitialBoard.Clone();

            /// Текущий игрок, за которого надо сделать ход
            var currentPlayer = board.Players[Color];

            turn = InitializeNewTurn(board, currentPlayer);
        }

        private void Selection(Board board, Turn turn)
        {
            Console.WriteLine();

            /// Текущий игрок, за которого надо сделать ход
            var currentPlayer = board.Players[Color];

            /// Шаффлим руку, чтобы рандомизировать порядок розыгрыша карт
            currentPlayer.Hand.Shuffle(random);

            /// Шаффлим колоды рынка и игрока, для устранения ошибки заглядывания в будущее
            currentPlayer.Deck.Shuffle(random);
            board.Deck.Shuffle(random);

            while (turn.State != SelectionState.FINISH_SELECTION)
            {
                if (StateSelectors.TryGetValue(turn.State, out var selector))
                {
                    var effectVariations = selector.GenerateOptions(board, turn);

                    var selectedEffectVariation = RandomSelector.SelectRandomWithWeights(effectVariations, random);

                    turn.SelectionSequence.AddRange(selectedEffectVariation);

                    foreach (var effect in selectedEffectVariation)
                    {
                        effect.ApplyEffect(board, turn);
                        effect.PrintEffect();
                    }
                }
                else
                {
                    break;
                }
            }

            ControlMetrics.GetVPForSiteControlMarkersInTheEnd(board, turn);

            var score = TargetFunction.Evaluate(board, turn);
        }

        private Turn InitializeNewTurn(Board board, Player currentPlayer)
        {
            var turn = new Turn(Color);

            foreach (var card in currentPlayer.Hand)
            {
                turn.CardStates.Add(card, CardState.IN_HAND);
            }

            foreach (var location in board.Locations)
            {
                turn.LocationStates.Add(location, new LocationState(location, Color));
            }

            ControlMetrics.GetStartManaFromSites(board, currentPlayer, turn);

            DistanceCalculator.CalculatePresenceAndDistances(board, turn);

            //turn.DebugPrintDistances();

            return turn;
        }
    }
}
