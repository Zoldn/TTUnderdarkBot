using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TUnderdark.Model
{
    internal class Board
    {
        public Dictionary<Color, Player> Players { get; set; }
        public List<Location> Locations { get; set; }
        public List<Card> Deck { get; set; }
        public List<Card> Market { get; set; }
        public List<Card> Devoured { get; set; }
        public int Lolths { get; set; }
        public int HouseGuards { get; set; }
        public int InsaneOutcats { get; set; }
        public Board()
        {
            Players = Enum.GetValues(typeof(Color)).Cast<Color>()
                .Where(c => c != Color.WHITE)
                .ToDictionary(
                    c => c,
                    c => new Player(c)
                );

            Locations = new();
            Deck = new();
            Market = new();
            Devoured = new();

            Lolths = 15;
            HouseGuards = 15; 
            InsaneOutcats = 30;
        }

        public void PrintResults()
        {
            Dictionary<Color, int> controlVPs = Players
                .ToDictionary(c => c.Key, c => 0);

            Dictionary<Color, int> totalControlVPs = Players
                .ToDictionary(c => c.Key, c => 0);

            foreach (var location in Locations)
            {
                if (!location.IsSite)
                {
                    continue;
                }

                var controller = location.GetControlPlayer();

                if (controller.HasValue)
                {
                    controlVPs[controller.Value] += location.ControlVPs;

                    controller = location.GetFullControl();

                    if (controller.HasValue)
                    {
                        totalControlVPs[controller.Value] += 2;
                    }
                }
            }

            Console.WriteLine("\nRESULTS:\n");

            if (false)
            {
                Console.WriteLine("PLAYER\t|Trophy\t|Deck\t|Promote|Control|Total\t|RESULT\t|");

                foreach (var (color, player) in Players)
                {
                    int result = player.TrophyHallVP + player.DeckVP + player.PromoteVP + controlVPs[color] + totalControlVPs[color];

                    Console.WriteLine($"{color}\t|{player.TrophyHallVP}\t|" +
                        $"{player.DeckVP}\t|" +
                        $"{player.PromoteVP}\t|" +
                        $"{controlVPs[color]}\t|" +
                        $"{totalControlVPs[color]}\t|" +
                        $"{result}\t|");
                }
            }
            else
            {
                foreach (var (color, player) in Players)
                {
                    int result = player.TrophyHallVP + player.DeckVP + player.PromoteVP + controlVPs[color] + totalControlVPs[color];

                    Console.WriteLine($"{color}: {player.TrophyHallVP} + " +
                        $"{player.DeckVP} + " +
                        $"{player.PromoteVP} + " +
                        $"{controlVPs[color]} + " +
                        $"{totalControlVPs[color]} = " +
                        $"{result}");
                }
            }
            



            /*
            foreach (var (color, player) in Players)
            {
                Console.WriteLine($"{color}: Trophy hall = {player.TrophyHallVP}," +
                    $"Deck = {player.DeckVP}, " +
                    $"Inner Circle = {player.PromoteVP}, " +
                    $"Controls {controlVPs[color]}, " +
                    $"Total {totalControlVPs[color]}");
            }
            */
        }

        public void PrintAllLocationWithColorUnits(Color color)
        {
            foreach (var location in Locations)
            {
                bool isPrint = (location.Troops[color] > 0) ||
                    (color != Color.WHITE && location.Spies[color]);

                if (isPrint)
                {
                    Console.WriteLine($"{location}");
                }
            }
        }
    }
}
