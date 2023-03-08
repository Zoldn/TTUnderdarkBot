using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using TUnderdark.TTSParser;
using UnderdarkAI.AI.PlayableOptions;

namespace UnderdarkAI.AI.OptionGenerators
{
    internal static class OptionUtils
    {
        internal static List<PlayableOption> GetMoveFromOptions(List<PlayableOption> options, 
            Board board, Turn turn, int nextIteration, int outIteration)
        {
            foreach (var (location, locationState) in turn.LocationStates)
            {
                if (!locationState.HasPresence)
                {
                    continue;
                }

                foreach (var (color, count) in location.Troops)
                {
                    if (color == turn.Color || count == 0)
                    {
                        continue;
                    }

                    options.Add(new MoveFromOption(location.Id, color) { NextCardIteration = nextIteration});
                }
            }

            if (options.Count == 0)
            {
                options.Add(new DoNothingOption(outIteration));
            }

            return options;
        }

        internal static List<PlayableOption> GetMoveToOptions(List<PlayableOption> options, Board board, Turn turn, 
            int outChoiceCardStateIteration)
        {
            foreach (var (location, _) in turn.LocationStates)
            {
                if (location.Id == turn.LocationMoveFrom || location.FreeSpaces == 0)
                {
                    continue;
                }

                options.Add(new MoveToOption(location.Id)
                {
                    NextCardIteration = outChoiceCardStateIteration,
                });
            }

            return options;
        }

        /// <summary>
        /// Опции для промоута другой карты в конце хода
        /// </summary>
        /// <param name="turn"></param>
        /// <param name="promoter"></param>
        /// <returns></returns>
        internal static List<PlayableOption> GetPromoteAnotherCardPlayedThisTurnInTheEndOptions(
            List<PlayableOption> options, Board board, Turn turn, 
            CardSpecificType promoter, int outIteration, bool canBeSkipped = false, 
            Race? specificRaceOnly = null, CardType? specificCardType = null)
        {
            var ret = turn.CardStates
                .Where(s => !s.IsPromotedInTheEnd
                    && s.State == CardState.PLAYED
                    && s.EndTurnState != CardState.NOW_PLAYING
                    && !s.IsElderBrainTarget
                    && !s.IsUlitharidTarget
                    && (!specificRaceOnly.HasValue
                        || specificRaceOnly.Value == CardMapper.SpecificTypeCardMakers[s.SpecificType].Race)
                    && (!specificCardType.HasValue
                        || specificCardType.Value == CardMapper.SpecificTypeCardMakers[s.SpecificType].CardType)
                )
                .Select(s => s.SpecificType)
                .Distinct()
                .Select(s => new PromotePlayedCardOption(promoter, s, outIteration))
                .ToList();

            turn.WeightGenerator.FillPromoteOptions(board, turn, ret);

            if (ret.Count == 0 || canBeSkipped)
            {
                options.Add(new DoNothingEndTurnOption(outIteration));
            }

            options.AddRange(ret);

            return options;
        }

        internal static List<PlayableOption> GetReturnEnemySpyOptions(Board board, Turn turn, 
            int outIteration, bool isBaseAction = false)
        {
            var ret = new List<ReturnEnemySpyOption>();

            foreach (var (location, locationState) in turn.LocationStates)
            {
                if (!locationState.HasPresence)
                {
                    continue;
                }

                foreach (var (color, isSpy) in location.Spies)
                {
                    if (color == turn.Color || !isSpy)
                    {
                        continue;
                    }

                    ret.Add(new ReturnEnemySpyOption(location.Id, color, outIteration, isBaseAction: isBaseAction) { Weight = 1.0d });
                }
            }

            turn.WeightGenerator.FillReturnEnemySpyOptions(board, turn, ret);

            return ret.Select(o => o as PlayableOption).ToList();
        }

        internal static List<PlayableOption> GetReturnTroopOptions(Board board, Turn turn, int outIteration)
        {
            var options = new List<PlayableOption>();

            foreach (var (location, locationState) in turn.LocationStates)
            {
                if (!locationState.HasPresence)
                {
                    continue;
                }

                foreach (var (color, count) in location.Troops)
                {
                    if (color == turn.Color || color == Color.WHITE || count == 0)
                    {
                        continue;
                    }

                    options.Add(new ReturnTroopOption(location.Id, color, outIteration));
                }
            }

            return options;
        }

        internal static bool IsMoveAvailable(Board board, Turn turn)
        {
            bool isEmptyLocation = false;
            bool isMovableTargets = false;

            foreach (var (location, locationState) in turn.LocationStates)
            {
                if (locationState.HasPresence && location.Troops.Any(kv => kv.Key != turn.Color && kv.Value > 0))
                {
                    isMovableTargets = true;
                }

                if (location.FreeSpaces > 0)
                {
                    isEmptyLocation = true;
                }

                if (isEmptyLocation && isMovableTargets)
                {
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// Есть ли опции для возвращения шпионов
        /// </summary>
        /// <param name="board"></param>
        /// <param name="turn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static bool IsReturnableEnemySpies(Board board, Turn turn)
        {
            return turn.LocationStates
                .Any(l => l.Value.HasPresence && l.Key.Spies.Any(kv => kv.Value && kv.Key != turn.Color));
        }

        /// <summary>
        /// Опции для возвращения трупсов
        /// </summary>
        /// <param name="board"></param>
        /// <param name="turn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static bool IsReturnableTroops(Board board, Turn turn)
        {
            foreach (var (location, locationState) in turn.LocationStates)
            {
                if (!locationState.HasPresence)
                {
                    continue;
                }

                foreach (var (color, count) in location.Troops)
                {
                    if (color == turn.Color || color == Color.WHITE || count == 0)
                    {
                        continue;
                    }

                    return true;
                }
            }

            return false;
        }

        internal static bool IsPlacedSpyContainsEnemyPlayerTroop(Board board, Turn turn)
        {
            Debug.Assert(turn.PlacedSpies.Count <= 1);

            if (turn.PlacedSpies.Count == 0)
            {
                return false;
            }

            var locationId = turn.PlacedSpies.Single();

            foreach (var (color, count) in board.LocationIds[locationId].Troops)
            {
                if (color == Color.WHITE || color == turn.Color || count == 0)
                {
                    continue;
                }

                return true;
            }

            return false;
        }

        internal static bool IsReturnableOwnSpies(Board board, Turn turn)
        {
            foreach (var location in board.Locations)
            {
                if (location.Spies[turn.Color])
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsSupplantTargets(Board board, Turn turn,
            bool isAnywhere = false,
            bool isOnlyWhite = false,
            LocationId? specificLocation = null
            )
        {
            Debug.Assert(!isAnywhere || !specificLocation.HasValue);

            foreach (var (location, locationState) in turn.LocationStates)
            {
                if (!locationState.HasPresence && !isAnywhere)
                {
                    continue;
                }

                if (specificLocation.HasValue && location.Id != specificLocation.Value)
                {
                    continue;
                }

                foreach (var (color, count) in location.Troops)
                {
                    if (count == 0 || color == turn.Color)
                    {
                        continue;
                    }

                    if (color != Color.WHITE && isOnlyWhite)
                    {
                        continue;
                    }

                    return true;
                }
            }

            return false;
        }

        internal static int GetTotalWhiteTroopsOnBoard(Board board, Turn _)
        {
            return board.Locations.Sum(l => l.Troops[Color.WHITE]);
        }

        public static bool IsAssassinateTargets(Board board, Turn turn,
            bool isOnlyWhite = false,
            HashSet<LocationId>? specificLocation = null)
        {
            foreach (var (location, locationState) in turn.LocationStates)
            {
                if (!locationState.HasPresence)
                {
                    continue;
                }

                if (specificLocation != null && !specificLocation.Contains(location.Id))
                {
                    continue;
                }

                foreach (var (color, count) in location.Troops)
                {
                    if (count == 0 || color == turn.Color)
                    {
                        continue;
                    }

                    if (color != Color.WHITE && isOnlyWhite)
                    {
                        continue;
                    }

                    return true;
                }
            }

            return false;
        }

        internal static bool IsPlacedSpyContainsEnemySpy(Board board, Turn turn)
        {
            Debug.Assert(turn.PlacedSpies.Count <= 1);

            if (turn.PlacedSpies.Count == 0)
            {
                return false;
            }

            var locationId = turn.PlacedSpies.Single();

            foreach (var (color, count) in board.LocationIds[locationId].Spies)
            {
                if (color == turn.Color || !count)
                {
                    continue;
                }

                return true;
            }

            return false;
        }

        internal static bool IsDeployAvailable(Board board, Turn turn, bool isAnywhere)
        {
            foreach (var (location, locationState) in turn.LocationStates)
            {
                if (!isAnywhere && !locationState.HasPresence)
                {
                    continue;
                }

                if (location.FreeSpaces == 0)
                {
                    continue;
                }

                return true;
            }

            return false;
        }
    }
}
