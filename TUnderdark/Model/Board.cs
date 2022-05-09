using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TUnderdark.Output;

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

        public string PrintResults(bool isMonoSpaceFormat = false)
        {
            var ret = string.Empty;

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

            if (isMonoSpaceFormat)
            {
                ret += "PLAYER\t|Trophy\t|Deck\t|Promote|Control|Total\t|RESULT\t|\n";
                Console.WriteLine("PLAYER\t|Trophy\t|Deck\t|Promote|Control|Total\t|RESULT\t|");

                foreach (var (color, player) in Players)
                {
                    int result = player.TrophyHallVP + player.DeckVP + player.PromoteVP + controlVPs[color] + totalControlVPs[color];

                    ret += $"{player.Name}\t|{player.TrophyHallVP}\t|" +
                        $"{player.DeckVP}\t|" +
                        $"{player.PromoteVP}\t|" +
                        $"{controlVPs[color]}\t|" +
                        $"{totalControlVPs[color]}\t|" +
                        $"{result}\t|\n";

                    Console.WriteLine($"{player.Name}\t|{player.TrophyHallVP}\t|" +
                        $"{player.DeckVP}\t|" +
                        $"{player.PromoteVP}\t|" +
                        $"{controlVPs[color]}\t|" +
                        $"{totalControlVPs[color]}\t|" +
                        $"{result}\t|");
                }
            }
            else
            {
                int largestNameSize = Math.Max(6, Players.Max(e => e.Value.Name.Length));

                ret += "`Results are:\n";

                //Console.WriteLine($"{AddSpacesToSize("Player", largestNameSize)}|Trophy\t|Deck\t|Promote|Control|Total\t|RESULT\t|");
                Console.WriteLine($"RESULT\tTrophy\tDeck\tPromote\tControl\tTotal\tPlayer");
                //ret += $"`RESULT = Trophy + Deck + Promote + Control + TotalControl\n";
                ret += $"{AddSpacesToSize("Name", largestNameSize + 3)}" +
                    $"{AddSpacesToSize("Score", 8)}" +
                    $"{AddSpacesToSize("Trophy", 8)}" +
                    $"{AddSpacesToSize("Deck", 8)}" +
                    $"{AddSpacesToSize("Promote", 8)}" +
                    $"{AddSpacesToSize("Control", 8)}" +
                    $"{AddSpacesToSize("TotalControl", 12)}\n";

                foreach (var (color, player) in Players
                    .OrderByDescending(p => p.Value.TrophyHallVP + p.Value.DeckVP + p.Value.PromoteVP + controlVPs[p.Key] + totalControlVPs[p.Key])
                    .ToDictionary(kv => kv.Key, kv => kv.Value))
                {
                    int result = player.TrophyHallVP + player.DeckVP + player.PromoteVP + controlVPs[color] + totalControlVPs[color];

                    /*
                    Console.WriteLine($"{player.Name}: {player.TrophyHallVP} + " +
                        $"{player.DeckVP} + " +
                        $"{player.PromoteVP} + " +
                        $"{controlVPs[color]} + " +
                        $"{totalControlVPs[color]} = " +
                        $"{result}");
                    */

                    Console.WriteLine(
                        $"{result} = " +
                        $"{player.TrophyHallVP} + \t" +
                        $"{player.DeckVP} + \t" +
                        $"{player.PromoteVP} + \t" +
                        $"{controlVPs[color]} + \t" +
                        $"{totalControlVPs[color]} \t  " +
                        $"\t{player.Name}");

                    /*
                    ret += $"{result} = " +
                        $"\t{player.TrophyHallVP} + " +
                        $"\t{player.DeckVP} + " +
                        $"\t{player.PromoteVP} + " +
                        $"\t{controlVPs[color]} + " +
                        $"\t{totalControlVPs[color]}" +
                        $"\t{player.Name}\n";
                    */

                    ret += $"{AddSpacesToSize(player.Name, largestNameSize + 3)}" +
                        $"{AddSpacesToSize(result, 8)}" +
                        $"{AddSpacesToSize(player.TrophyHallVP, 8)}" +
                        $"{AddSpacesToSize(player.DeckVP, 8)}" +
                        $"{AddSpacesToSize(player.PromoteVP, 8)}" +
                        $"{AddSpacesToSize(controlVPs[color], 8)}" +
                        $"{AddSpacesToSize(totalControlVPs[color], 12)}\n";
                }

                ret += "`";
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

            return ret;
        }

        private string AddSpacesToSize(string s, int length)
        {
            int spaceCount = length - s.Length;
            if (spaceCount > 0)
            {
                var spaces = new string(' ', spaceCount);
                return s + spaces;
            }
            return s;
        }
        private string AddSpacesToSize(int s, int length)
        {
            return AddSpacesToSize(s.ToString(), length);
        }

        internal List<ResultRecord> GetResults(int turn, Dictionary<Color, string> playerNames)
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

            DateTime timeStamp = DateTime.Now;

            var results = new List<ResultRecord>();

            foreach (var (color, player) in Players)
            {
                results.Add(new ResultRecord()
                {
                    Color = color,
                    Name = playerNames[color],
                    TimeStamp = timeStamp,
                    Turn = turn,
                    Statictic = ResultRecordStatictic.CONTROL_VP,
                    Value = controlVPs[color],
                });

                results.Add(new ResultRecord()
                {
                    Color = color,
                    Name = playerNames[color],
                    TimeStamp = timeStamp,
                    Turn = turn,
                    Statictic = ResultRecordStatictic.TOTAL_CONTROL_VP,
                    Value = totalControlVPs[color],
                });

                results.Add(new ResultRecord()
                {
                    Color = color,
                    Name = playerNames[color],
                    TimeStamp = timeStamp,
                    Turn = turn,
                    Statictic = ResultRecordStatictic.TROPHY_HALL_VP,
                    Value = player.TrophyHallVP,
                });

                results.Add(new ResultRecord()
                {
                    Color = color,
                    Name = playerNames[color],
                    TimeStamp = timeStamp,
                    Turn = turn,
                    Statictic = ResultRecordStatictic.DECK_VP,
                    Value = player.DeckVP,
                });

                results.Add(new ResultRecord()
                {
                    Color = color,
                    Name = playerNames[color],
                    TimeStamp = timeStamp,
                    Turn = turn,
                    Statictic = ResultRecordStatictic.INNER_CIRCLE_VP,
                    Value = player.PromoteVP,
                });

                results.Add(new ResultRecord()
                {
                    Color = color,
                    Name = playerNames[color],
                    TimeStamp = timeStamp,
                    Turn = turn,
                    Statictic = ResultRecordStatictic.TOTAL_VP,
                    Value = player.PromoteVP + player.DeckVP + player.TrophyHallVP + controlVPs[color] + totalControlVPs[color],
                });

                results.Add(new ResultRecord()
                {
                    Color = color,
                    Name = playerNames[color],
                    TimeStamp = timeStamp,
                    Turn = turn,
                    Statictic = ResultRecordStatictic.INSANE_OUTCASTS,
                    Value = player.Deck.Where(c => c.CardType == CardType.INSANE).Count() +
                        player.Hand.Where(c => c.CardType == CardType.INSANE).Count() +
                        player.Discard.Where(c => c.CardType == CardType.INSANE).Count(),
                });

                results.Add(new ResultRecord()
                {
                    Color = color,
                    Name = playerNames[color],
                    TimeStamp = timeStamp,
                    Turn = turn,
                    Statictic = ResultRecordStatictic.DECK_COUNT,
                    Value = player.Deck.Count() +
                        player.Hand.Count() +
                        player.Discard.Count(),
                });

                results.Add(new ResultRecord()
                {
                    Color = color,
                    Name = playerNames[color],
                    TimeStamp = timeStamp,
                    Turn = turn,
                    Statictic = ResultRecordStatictic.TOTAL_MP_COST,
                    Value = player.Deck.Sum(c => c.ManaCost) +
                        player.Hand.Sum(c => c.ManaCost) +
                        player.Discard.Sum(c => c.ManaCost) +
                        player.InnerCircle.Sum(c => c.ManaCost),
                });
            }

            return results;
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
