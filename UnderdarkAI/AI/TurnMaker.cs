﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.OptionGenerators;
using UnderdarkAI.AI.TargetFunctions;
using UnderdarkAI.MetricUtils;
using UnderdarkAI.Utils;

namespace UnderdarkAI.AI
{
    internal sealed class TurnMaker
    {
        private readonly Random random;
        /// <summary>
        /// Исходное состояние игры, не менять
        /// </summary>
        public Board InitialBoard { get; }
        /// <summary>
        /// Состояние игры с частично выполненным ходом
        /// </summary>
        public Board FixedBoard { get; private set; }
        /// <summary>
        /// Частично зафиксированный ход
        /// </summary>
        public Turn FixedTurn { get; private set; }
        /// <summary>
        /// Цвет игрока, за которого надо сделать ход
        /// </summary>
        public Color Color { get; }
        /// <summary>
        /// Количество рестартов с начального решения
        /// </summary>
        public int RestartLimit { get; set; }
        public Dictionary<SelectionState, OptionGenerator> StateSelectors { get; private set; }
        public ITargetFunction TargetFunction { get; private set; }

        public TurnMaker(Board board, Color color, int? seed = null)
        {
            InitialBoard = board;
            Color = color;
            RestartLimit = 1;

            FixedBoard = InitialBoard.Clone();
            FixedTurn = InitializeNewTurn(FixedBoard);

            if (seed.HasValue)
            {
                random = new Random(seed.Value);
            }
            else
            {
                random = new Random();
            }

            StateSelectors = new Dictionary<SelectionState, OptionGenerator>()
            {
                { SelectionState.CARD_OR_FREE_ACTION, new PlayCardOrBaseActionOptionGenerator() },
                //{ SelectionState.SELECT_END_TURN, new EndTurnSelection() },
                { SelectionState.SELECT_CARD, new SelectCardToPlayOptionGenerator() },
                { SelectionState.SELECT_CARD_OPTION, new SelectPlayingCardOptionGenerator() },
                { SelectionState.SELECT_BASE_ACTION, new BaseActionOptionGenerator() },
                { SelectionState.BUY_CARD_BY_MANA, new BuyCardOptionGenerator() },
                { SelectionState.DEPLOY_BY_SWORD, new DeployBySwordOptionGenerator() },
                { SelectionState.ASSASSINATE_BY_SWORD, new AssasinateBySwordOptionGenerator() },
                { SelectionState.RETURN_ENEMY_SPY_BY_SWORD, new ReturnSpyBySwordOptionGenerator() },
                { SelectionState.SELECT_CARD_END_TURN, new EndTurnCardSelectionOptionGenerator() },
                { SelectionState.SELECT_END_TURN_CARD_OPTION, new SelectPlayingCardOptionGenerator() },
            };

            TargetFunction = new VPScoreTargetFunction();
        }

        public void MakeTurn()
        {
            Console.WriteLine();

            while (FixedTurn.State != SelectionState.FINISH_SELECTION)
            {
                if (StateSelectors.TryGetValue(FixedTurn.State, out var selector))
                {
                    var options = selector.GeneratePlayableOptions(FixedBoard, FixedTurn);

                    PlayableOption? selectedOption = null;

                    if (options.Count == 0)
                    {
                        break;
                    }

                    if (options.Count == 1)
                    {
                        selectedOption = options[0];
                    }

                    if (options.Count > 1)
                    {
                        selectedOption = RunMonteCarloSelection(options);
                    }

                    if (selectedOption is null)
                    {
                        throw new NullReferenceException();
                    }

                    selectedOption.ApplyOption(FixedBoard, FixedTurn);
                    selectedOption.Print(0);

                    FixedTurn.State = selectedOption.GetNextState();
                }
                else
                {
                    break;
                }
            }

            ControlMetrics.GetVPForSiteControlMarkersInTheEnd(FixedBoard, FixedTurn);

            var score = TargetFunction.Evaluate(FixedBoard, FixedTurn);

            Console.WriteLine($"Final score is {score}");
        }

        /// <summary>
        /// Делает прогоны, начиная с указанных опций, и возвращает ту, которая дает наилучший средний результат
        /// по ансамблю запусков
        /// </summary>
        /// <param name="firstOptions"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        private PlayableOption? RunMonteCarloSelection(List<PlayableOption> firstOptions)
        {
            var scoresOfFirstChoiseIndexes = firstOptions
                .ToDictionary(
                    o => o, 
                    o => new List<double>()
                    );

            var restartCount = Math.Max(firstOptions.Count, RestartLimit);

            for (int iteration = 0; iteration < restartCount; iteration++)
            {
                //Console.WriteLine($"\nNew Iteration {iteration}\n");

                CopyRoot(out var board, out var turn);

                /// Шаффлим руку, чтобы рандомизировать порядок розыгрыша карт
                board.Players[Color].Hand.Shuffle(random);

                /// Шаффлим колоды рынка и игрока, для устранения ошибки заглядывания в будущее
                board.Players[Color].Deck.Shuffle(random);
                board.Deck.Shuffle(random);

                PlayableOption? selectedFirstSelectionOption = null;
                bool isFirstSelection = true;

                while (turn.State != SelectionState.FINISH_SELECTION)
                {
                    if (StateSelectors.TryGetValue(turn.State, out var selector))
                    {
                        List<PlayableOption> options; // selector.GeneratePlayableOptions(board, turn);
                        PlayableOption selectedOption;

                        if (isFirstSelection)
                        {
                            options = firstOptions;

                            /// Гарантирует, что каждая опция будет использована хотя бы один раз,
                            /// чтобы потом при усреднении результатов не было проблем с пустыми вариантами
                            if (iteration < options.Count)
                            {
                                selectedOption = options[iteration];
                            }
                            else
                            {
                                selectedOption = RandomSelector.SelectRandomWithWeights(options, o => o.Weight, random);
                            }

                            selectedFirstSelectionOption = selectedOption;

                            isFirstSelection = false;
                        }
                        else
                        {
                            options = selector.GeneratePlayableOptions(board, turn);
                            selectedOption = RandomSelector.SelectRandomWithWeights(options, o => o.Weight, random);
                        }

                        if (options.Count == 0)
                        {
                            break;
                        }

                        selectedOption.ApplyOption(board, turn);
                        //selectedOption.Print(100);

                        turn.State = selectedOption.GetNextState();
                    }
                    else
                    {
                        break;
                    }
                }

                ControlMetrics.GetVPForSiteControlMarkersInTheEnd(board, turn);

                var score = TargetFunction.Evaluate(board, turn);

                if (selectedFirstSelectionOption is null)
                {
                    throw new NullReferenceException();
                }

                scoresOfFirstChoiseIndexes[selectedFirstSelectionOption].Add(score);
            }

            if (FixedTurn.State == SelectionState.BUY_CARD_BY_MANA)
            {
                int y = 1;
            }

            var aggedResults = scoresOfFirstChoiseIndexes
                .ToDictionary(
                    g => g.Key,
                    g => g.Value.Average()
                );

            var bestOption = aggedResults
                .OrderByDescending(g => g.Value)
                .First()
                .Key;

            ///TODO

            return bestOption;
        }

        private void CopyRoot(out Board board, out Turn turn)
        {
            /// Клонируем исходное состояние
            board = FixedBoard.Clone();

            turn = FixedTurn.Clone(board);
        }

        private Turn InitializeNewTurn(Board board)
        {
            var turn = new Turn(Color);

            foreach (var card in board.Players[Color].Hand)
            {
                turn.CardStates.Add(new TurnCardState(card.SpecificType));
            }

            foreach (var location in board.Locations)
            {
                turn.LocationStates.Add(location, new LocationState(location, Color));
            }

            ControlMetrics.GetStartManaFromSites(board, board.Players[Color], turn);

            DistanceCalculator.CalculatePresenceAndDistances(board, turn);

            //turn.DebugPrintDistances();

            return turn;
        }
    }
}
