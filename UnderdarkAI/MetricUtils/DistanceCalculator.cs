using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI;

namespace UnderdarkAI.MetricUtils
{
    internal static class DistanceCalculator
    {
        public static void CalculatePresenceAndDistances(Board board, Turn turn)
        {
            InitialPresence(board, turn);

            CalculateDistances(board, turn);
        }

        private static void InitialPresence(Board board, Turn turn)
        {
            foreach (var location in board.Locations)
            {
                /// Если есть трупс, то присутствие в этой локе и всех соседях
                if (location.Troops[turn.Color] > 0)
                {
                    turn.LocationStates[location].HasPresence = true;

                    foreach (var neighboor in location.Neighboors)
                    {
                        turn.LocationStates[neighboor].HasPresence = true;
                    }
                }

                /// Если шпион, то только в этой локе
                if (location.Spies[turn.Color])
                {
                    turn.LocationStates[location].HasPresence = true;
                }

                /// Вычисляем, можно ли распространяться отсюда
                turn.LocationStates[location].IsPropagatable = location.FreeSpaces > 0 ||
                    location.Troops[turn.Color] > 0;
            }
        }

        private static void CalculateDistances(Board board, Turn turn)
        {
            /// Стратовая позиция (0, если есть Presence
            /// 1, в соседях города, если шпион в нем с пустым слотом)
            foreach (var location in board.Locations)
            {
                if (turn.LocationStates[location].HasPresence)
                {
                    turn.LocationStates[location].Distance = 0;

                    turn.LocationStates[location].IsPropagatable = location.FreeSpaces > 0 ||
                        location.Troops[turn.Color] > 0;

                    if (location.Spies[turn.Color] && 
                        location.Troops[turn.Color] == 0 &&
                        location.FreeSpaces > 0)
                    {
                        foreach (var neighboor in location.Neighboors)
                        {
                            if (turn.LocationStates[neighboor].Distance == LocationState.NOT_ANALYSED)
                            {
                                turn.LocationStates[neighboor].Distance = 1;
                            }
                        }
                    }
                }
            }

            int currentDistance = 0;

            while (true)
            {
                var checkingLocations = board.Locations
                    .Where(l => turn.LocationStates[l].Distance == currentDistance
                        && turn.LocationStates[l].IsPropagatable
                    )
                    .ToList();

                var newLocations = new HashSet<Location>();

                foreach (var location in checkingLocations)
                {
                    foreach (var neighbour in location.Neighboors)
                    {
                        if (turn.LocationStates[neighbour].Distance == LocationState.NOT_ANALYSED)
                        {
                            turn.LocationStates[neighbour].Distance = currentDistance + 1;
                            newLocations.Add(neighbour);
                        }
                    }
                }

                if (!newLocations.Any())
                {
                    break;
                }

                ++currentDistance;
            }

            foreach (var location in board.Locations)
            {
                if (turn.LocationStates[location].Distance == LocationState.NOT_ANALYSED)
                {
                    turn.LocationStates[location].Distance = LocationState.UNREACHABLE;
                }
            }
        }
    }
}
