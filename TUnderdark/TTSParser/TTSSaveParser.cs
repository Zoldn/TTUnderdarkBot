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

            return true;
        }

        private static void ParseLocation(Board board, JSONContainer container)
        {
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
                
            }
        }
    }
}
