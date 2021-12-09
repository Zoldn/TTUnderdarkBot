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
                var controller = location.GetControlPlayer();

                if (controller.HasValue)
                {
                    controlVPs[controller.Value] += location.ControlVPs;
                }

                controller = location.GetFullControl();

                if (controller.HasValue)
                {
                    controlVPs[controller.Value] += location.TotalControlVPs;
                }
            }

            foreach (var (color, player) in Players)
            {
                Console.WriteLine($"{color}: Trophy hall = {player.TrophyHallVP}," +
                    $"Deck = {player.DeckVP}, " +
                    $"Inner Circle = {player.PromoteVP}, " +
                    $"Controls {controlVPs[color]}, " +
                    $"Total {totalControlVPs[color]}");
            }
        }
    }
}
