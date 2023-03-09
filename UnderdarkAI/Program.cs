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

            //string json = TTSLoader.GetJson(isLastSave: false, saveName: @"TS_Save_70.json");
            string json = TTSLoader.GetJson(isLastSave: true);

            TTSSaveParser.Read(json, board);

            //TestRandom();

            //board.Players[Color.YELLOW].Hand.Remove(
            //    board.Players[Color.YELLOW].Hand.First(d => d.SpecificType == CardSpecificType.NOBLE)
            //    );

            //board.Players[Color.YELLOW].Hand.Add(
            //    CardMapper.SpecificTypeCardMakers[CardSpecificType.NEOGI]
            //    );

            var turnMaker = new TurnMaker(board, Color.GREEN) //, seed: 8984314
            {
                RestartLimit = 400,
            };

            //turnMaker.AddForcedDiscardForCurrentPlayer(sourcePlayer: Color.RED, sourceCard: CardSpecificType.ERROR);

            var resultTurn = turnMaker.MakeTurn();

            Console.WriteLine(resultTurn.Print());

            //board.PrintResults();

            Console.ReadLine();
        }
    }
}