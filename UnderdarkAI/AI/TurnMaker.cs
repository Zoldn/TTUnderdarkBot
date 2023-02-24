using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
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
        }

        public void MakeTurn()
        {
            for (int iteration = 0; iteration < RestartLimit; iteration++)
            {
                Selection();
            }
        }

        private void Selection()
        {
            /// Клонируем исходное состояние
            var board = InitialBoard.Clone();

            /// Текущий игрок, за которого надо сделать ход
            var currentPlayer = board.Players[Color];

            Turn turn = InitializeNewTurn(board, currentPlayer);

            /// Шаффлим руку, чтобы рандомизировать порядок розыгрыша карт
            currentPlayer.Hand.Shuffle(random);

            /// Шаффлим колоды рынка и игрока, для устранения ошибки заглядывания в будущее
            currentPlayer.Deck.Shuffle(random);
            board.Deck.Shuffle(random);

            int currentHandIndex = 0;

            /// Пока не закончились карты последовательно их разыгрываем
            /// Заранее не знаем, сколько их будет, так как есть Draw a card
            while (currentHandIndex < currentPlayer.Hand.Count)
            {
                var card = currentPlayer.Hand[currentHandIndex];



                ++currentHandIndex;
            }
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
