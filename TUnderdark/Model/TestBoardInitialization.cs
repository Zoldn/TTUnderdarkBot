using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUnderdark.Model
{
    internal static class TestBoardInitialization
    {
        public static void CheckBoardCreation(Board board)
        {
            CheckNames(board);

            CheckLocationProperties(board);

            CheckCohesion(board);
        }

        private static void CheckCohesion(Board board)
        {
            Console.WriteLine("Checking cohesion");
            foreach (var location in board.Locations)
            {
                foreach (var neighboor in location.Neighboors)
                {
                    if (!neighboor.Neighboors.Contains(location))
                    {
                        Console.WriteLine($"Failed link {location.Name} <X==> {neighboor.Name}");
                    }
                }
            }
        }

        private static void CheckLocationProperties(Board board)
        {
            var startLocations = board.Locations.Where(l => l.IsStart).ToList();

            Console.WriteLine($"There are {startLocations.Count} start locations");
            foreach (var location in startLocations)
            {
                Console.WriteLine($"\t{location.Name}");
            }

            var bonusMPLocations = board.Locations.Where(l => l.BonusMana > 0 || l.BonusVP > 0).ToList();

            Console.WriteLine($"There are {bonusMPLocations.Count} with tokens");
            foreach (var location in bonusMPLocations)
            {
                Console.WriteLine($"\t{location.Name} {location.BonusMana} MP / {location.BonusVP} VP");
            }

            
            var sites = board.Locations.Where(l => l.IsSite).ToList();
            Console.WriteLine($"There are {sites.Count} sites");

            foreach (var site in sites)
            {
                Console.WriteLine($"\t{site.Name} {site.Size} size / {site.ControlVPs} VP");
            }
        }

        private static void CheckNames(Board board)
        {
            if (Enum.GetValues(typeof(LocationId)).Length != board.Locations.Count)
            {
                Console.WriteLine("Not for all sites created locations:");

                var ids = Enum.GetValues(typeof(LocationId)).Cast<LocationId>().ToHashSet();
                var locIds = board.Locations.Select(l => l.Id).ToHashSet();

                ids.ExceptWith(locIds);

                foreach (var id in ids)
                {
                    Console.WriteLine($"\t{id}");
                }
            }

            var duplicateIds = board.Locations
                .GroupBy(l => l.Id)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            Console.WriteLine($"There are {duplicateIds.Count} duplicate ids:");
            foreach (var id in duplicateIds)
            {
                Console.WriteLine($"\t{id}");
            }

            var duplicateNames = board.Locations
                .GroupBy(l => l.Name)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            Console.WriteLine($"There are {duplicateNames.Count} duplicate names:");
            foreach (var name in duplicateNames)
            {
                Console.WriteLine($"\t{name}");
            }

            var mismatchNames = board.Locations
                .Where(l => l.Id.ToString() != l.Name)
                .ToList();

            Console.WriteLine($"There are {mismatchNames.Count} mismatch names:");
            foreach (var location in mismatchNames)
            {
                Console.WriteLine($"\t{location.Id} != {location.Name}");
            }
        }
    }
}
