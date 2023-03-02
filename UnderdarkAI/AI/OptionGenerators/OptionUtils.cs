using System;
using System.Collections.Generic;
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
        internal static List<PlayableOption> GetMoveFromOptions(Board board, Turn turn)
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
                    if (color == turn.Color || count == 0)
                    {
                        continue;
                    }

                    options.Add(new MoveFromOption(location.Id, color));
                }
            }

            return options;
        }

        internal static List<PlayableOption> GetMoveToOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            foreach (var (location, _) in turn.LocationStates)
            {
                if (location.Id == turn.LocationMoveFrom || location.FreeSpaces == 0)
                {
                    continue;
                }

                options.Add(new MoveToOption(location.Id));
            }

            return options;
        }

        /// <summary>
        /// Опции для промоута другой карты в конце хода
        /// </summary>
        /// <param name="turn"></param>
        /// <param name="promoter"></param>
        /// <returns></returns>
        internal static List<PlayableOption> GetPromoteAnotherCardPlayedThisTurnInTheEndOptions(Turn turn,
            CardSpecificType promoter)
        {
            var options = turn.CardStates
                    .Where(s => !s.IsPromotedInTheEnd
                        && s.State == CardState.PLAYED
                        && s.EndTurnState != CardState.NOW_PLAYING
                    )
                    .Select(s => s.SpecificType)
                    .Distinct()
                    .Select(s => new PromoteAnotherCardOption(promoter, s) 
                    { Weight = 1.0d +
                        CardMapper.SpecificTypeCardMakers[s].PromoteVP -
                        CardMapper.SpecificTypeCardMakers[s].VP
                    })
                    .ToList();

            return new List<PlayableOption>(options);
        }

        internal static List<PlayableOption> GetReturnEnemySpyOptions(Board board, Turn turn, bool isBaseAction = false)
        {
            var ret = new List<PlayableOption>();

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

                    ret.Add(new ReturnSpyOption(location.Id, color, isBaseAction: isBaseAction) { Weight = 1.0d });
                }
            }

            return ret;
        }

        internal static List<PlayableOption> GetReturnTroopOptions(Board board, Turn turn)
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

                    options.Add(new ReturnTroopOption(location.Id, color));
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
        internal static bool IsReturnableSpies(Board board, Turn turn)
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
    }
}
