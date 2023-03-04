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

            board.Players[Color.YELLOW].Hand.Add(
                CardMapper.SpecificTypeCardMakers[CardSpecificType.CULT_FANATIC]
                );

            //board.Players[Color.YELLOW].Hand.Add(
            //    CardMapper.SpecificTypeCardMakers[CardSpecificType.SPY_MASTER]
            //    );

            //board.LocationIds[LocationId.SSZuraassnee].Spies[Color.YELLOW] = true;

            //board.Players[Color.YELLOW].Spies = 4;
            //board.Players[Color.YELLOW].InnerCircle.Add(CardMapper.SpecificTypeCardMakers[CardSpecificType.NOBLE]);
            //board.Players[Color.YELLOW].InnerCircle.Add(CardMapper.SpecificTypeCardMakers[CardSpecificType.NOBLE]);
            //board.Players[Color.YELLOW].InnerCircle.Add(CardMapper.SpecificTypeCardMakers[CardSpecificType.NOBLE]);
            board.LocationIds[LocationId.ChedNasad].Spies[Color.RED] = true;
            //board.LocationIds[LocationId.ChedNasad].Troops[Color.RED] = 1;

            var turnMaker = new TurnMaker(board, Color.YELLOW) 
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