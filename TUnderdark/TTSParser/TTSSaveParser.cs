using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace TUnderdark.TTSParser
{
    internal class TTSSaveParser
    {
        public static bool Read(string json, Board board)
        {
            Console.WriteLine("Parsing JSON save file...");

            var container = JsonConvert.DeserializeObject<JSONContainer>(json);

            if (container == null)
            {
                return false;
            }

            if (container.GameMode != "Tyrants of the Underdark")
            {
                Console.WriteLine($"This save is not for Underdark but for {container.GameMode}");
                return false;
            }

            ParseLocation(board, container);

            CheckControlSumOfUnits(board, container);

            ParseTrophyHall(board, container);

            Console.WriteLine("Save has been loaded into virtual board");

            return true;
        }

        private static void CheckControlSumOfUnits(Board board, JSONContainer container)
        {
            Console.WriteLine("Checking control sums...");

            Dictionary<Color, int> totalTroops = ColorUtils
                .GetAllColorList()
                .ToDictionary(c => c, c => 0);

            Dictionary<Color, int> totalSpies = ColorUtils
                .GetPlayerColorList()
                .ToDictionary(c => c, c => 0);

            var objectsInLocation = container.ObjectStates
                .Where(o => o.IsPositionIn(-14.0, 37.0, -26.0, 26.0))
                .ToList();

            foreach (var objectInLocation in objectsInLocation)
            {
                switch (objectInLocation.Nickname)
                {
                    case "Unaligned Troops":
                        totalTroops[Color.WHITE] += 1;
                        break;
                    case "Mizzrym Troops":
                        totalTroops[Color.GREEN] += 1;
                        break;
                    case "Barrison Del'Armgo Troops":
                        totalTroops[Color.RED] += 1;
                        break;
                    case "Xorlarrin Troops":
                        totalTroops[Color.BLUE] += 1;
                        break;
                    case "Baenre Troops":
                        totalTroops[Color.YELLOW] += 1;
                        break;
                    case "Baenre Spy":
                        totalSpies[Color.YELLOW] += 1;
                        break;
                    case "Barrison Del'Armgo Spy":
                        totalSpies[Color.RED] += 1;
                        break;
                    case "Xorlarrin Spy":
                        totalSpies[Color.BLUE] += 1;
                        break;
                    case "Mizzrym Spy":
                        totalSpies[Color.GREEN] += 1;
                        break;
                    default:
                        break;
                }
            }

            var localTroops = board
                .Locations
                .SelectMany(l => l.Troops)
                .GroupBy(kv => kv.Key)
                .ToDictionary(g => g.Key, g => g.Sum(kv => kv.Value));

            var localSpies = board
                .Locations
                .SelectMany(l => l.Spies)
                .GroupBy(kv => kv.Key)
                .ToDictionary(g => g.Key, g => g.Sum(kv => kv.Value ? 1 : 0));

            foreach (var (color, count) in totalTroops)
            {
                if (count != localTroops[color])
                {
                    Console.WriteLine($"\tMismatch troops {color} count: " +
                        $"global = {count}, local = {localTroops[color]}");
                }
            }

            foreach (var (color, count) in totalSpies)
            {
                if (count != localSpies[color])
                {
                    Console.WriteLine($"\tMismatch troops {color} count: " +
                        $"global = {count}, local = {localSpies[color]}");
                }
            }

            Console.WriteLine("Control sums have been checked");
        }

        private static void ParseLocation(Board board, JSONContainer container)
        {
            Console.WriteLine("Parsing locations...");

            foreach (var location in board.Locations)
            {
                var coords = LocationPositions.Positions[location.Id];

                var objectsInLocation = container.ObjectStates
                    .Where(o => o.IsPositionIn(coords.X1, coords.X2, coords.Z1, coords.Z2))
                    .ToList();

                foreach (var objectInLocation in objectsInLocation)
                {
                    switch (objectInLocation.Nickname)
                    {
                        case "Unaligned Troops":
                            location.Troops[Color.WHITE] += 1;
                            break;
                        case "Mizzrym Troops":
                            location.Troops[Color.GREEN] += 1;
                            break;
                        case "Barrison Del'Armgo Troops":
                            location.Troops[Color.RED] += 1;
                            break;
                        case "Xorlarrin Troops":
                            location.Troops[Color.BLUE] += 1;
                            break;
                        case "Baenre Troops":
                            location.Troops[Color.YELLOW] += 1;
                            break;
                        case "Baenre Spy":
                            location.Spies[Color.YELLOW] = true;
                            break;
                        case "Barrison Del'Armgo Spy":
                            location.Spies[Color.RED] = true;
                            break;
                        case "Xorlarrin Spy":
                            location.Spies[Color.BLUE] = true;
                            break;
                        case "Mizzrym Spy":
                            location.Spies[Color.GREEN] = true;
                            break;
                        default:
                            break;
                    }
                }

                if (location.TotalTroops > location.Size)
                {
                    throw new ArgumentOutOfRangeException($"Too many troops in {location.Name}");
                }
            }

            Console.WriteLine("Locations have parsed");
        }

        private static void ParseTrophyHall(Board board, JSONContainer container)
        {
            Console.WriteLine("Parsing player zones...");

            foreach (var (color, player) in board.Players)
            {
                var coords = PlayerZonePositions.Zones[color];

                var objectsInLocation = container.ObjectStates
                    .Where(o => o.IsPositionIn(coords.X1, coords.X2, coords.Z1, coords.Z2))
                    .ToList();

                foreach (var objectInLocation in objectsInLocation)
                {
                    switch (objectInLocation.Nickname)
                    {
                        case "Unaligned Troops":
                            player.TrophyHall[Color.WHITE] += 1;
                            break;
                        case "Mizzrym Troops":
                            player.TrophyHall[Color.GREEN] += 1;
                            break;
                        case "Barrison Del'Armgo Troops":
                            player.TrophyHall[Color.RED] += 1;
                            break;
                        case "Xorlarrin Troops":
                            player.TrophyHall[Color.BLUE] += 1;
                            break;
                        case "Baenre Troops":
                            player.TrophyHall[Color.YELLOW] += 1;
                            break;
                        case "1 VP":
                            player.VPTokens += 1;
                            break;
                        case "5 VP":
                            player.VPTokens += 5;
                            break;
                        case "First Player marker":
                            player.IsFirstPlayer = true;
                            break;
                        default:
                            break;
                    }
                }

                player.TrophyHall[color] = 0;
            }

            Console.WriteLine("Player zones have parsed");
        }
    }
}
