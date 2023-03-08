using TUnderdark.Model;
using TUnderdark.TTSParser;
using UnderdarkAI.AI;
using UnderdarkAI.Utils;

namespace UnderdarkAI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CardMapper.ReadCards();

            //ArgumentParser.Parse("-color=y -turn=1 -iters=50", out var parseResultInfo, out var parsedArgs);

            Console.WriteLine("Reading current save");

            var board = BoardInitializer.Initialize(isWithChecks: false);

            string json = TTSLoader.GetJson(isLastSave: false, saveName: @"TS_Save_70.json");

            TTSSaveParser.Read(json, board);

            //TestRandom();

            board.Players[Color.YELLOW].Hand.Remove(
                board.Players[Color.YELLOW].Hand.First(d => d.SpecificType == CardSpecificType.NOBLE)
                );

            board.Players[Color.YELLOW].Hand.Add(
                CardMapper.SpecificTypeCardMakers[CardSpecificType.QUAGGOTH]
                );

            //board.Market.Remove(board.Market.First());
            //board.Market.Remove(board.Market.First());

            //board.Market.Add(CardMapper.SpecificTypeCardMakers[CardSpecificType.CARRION_CRAWLER].Clone());
            //board.Market.Add(CardMapper.SpecificTypeCardMakers[CardSpecificType.AMBASSADOR].Clone());

            //board.Players[Color.YELLOW].Hand.Add(
            //    CardMapper.SpecificTypeCardMakers[CardSpecificType.VANIFER]
            //    );

            // board.Devoured.Add(CardMapper.SpecificTypeCardMakers[CardSpecificType.WATCHER_OF_THAY]);

            //board.Players[Color.YELLOW].Hand.Add(
            //    CardMapper.SpecificTypeCardMakers[CardSpecificType.SPY_MASTER]
            //    );

            board.LocationIds[LocationId.SSZuraassnee].Spies[Color.YELLOW] = true;
            //board.LocationIds[LocationId.StoneShaft].Spies[Color.YELLOW] = true;
            //board.LocationIds[LocationId.Araumycos].Spies[Color.YELLOW] = true;
            //board.LocationIds[LocationId.Bridge].Spies[Color.YELLOW] = true;

            board.Players[Color.YELLOW].Spies = 4;
            //board.Players[Color.YELLOW].InnerCircle.Add(CardMapper.SpecificTypeCardMakers[CardSpecificType.NOBLE]);
            //board.Players[Color.YELLOW].InnerCircle.Add(CardMapper.SpecificTypeCardMakers[CardSpecificType.NOBLE]);
            //board.Players[Color.YELLOW].InnerCircle.Add(CardMapper.SpecificTypeCardMakers[CardSpecificType.NOBLE]);
            board.LocationIds[LocationId.ChedNasad].Spies[Color.RED] = true;
            //board.LocationIds[LocationId.Bridge].Troops[Color.RED] = 1;
            board.Players[Color.RED].Spies = 4;
            board.Players[Color.RED].TrophyHall[Color.YELLOW] = 2;
            board.LocationIds[LocationId.Chaulssin].Troops[Color.BLUE] = 1;
            //board.Players[Color.YELLOW].TrophyHall[Color.GREEN] = 8;
            //board.LocationIds[LocationId.ChedNasad].Troops[Color.RED] = 1;

            var turnMaker = new TurnMaker(board, Color.YELLOW, seed: 8984314) 
            {
                RestartLimit = 100,
            };

            var resultTurn = turnMaker.MakeTurn();

            Console.WriteLine(resultTurn.Print());

            //board.PrintResults();

            //board.Clone().PrintResults();

            Console.ReadLine();
        }

        private static void TestRandom()
        {
            Random random = new Random((int)(DateTime.Now.Ticks % 1000000));

            List<double> rvalues = Enumerable.Range(0, 10000)
                .Select(e => random.NextDouble())
                .ToList();

            var t = rvalues
                .GroupBy(e => (int)Math.Floor(10 * e))
                .ToDictionary(g => g.Key, g => g.Count());
        }
    }
}