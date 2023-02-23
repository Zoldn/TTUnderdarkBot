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

            Console.WriteLine("Reading current save");

            var board = BoardInitializer.Initialize(isWithChecks: false);

            string json = TTSLoader.GetJson(isLastSave: false, saveName: @"TS_Save_70.json");

            TTSSaveParser.Read(json, board);

            var turnMaker = new TurnMaker(board, Color.YELLOW) 
            {
                RestartLimit = 100,
            };

            turnMaker.MakeTurn();

            //board.PrintResults();

            //board.Clone().PrintResults();

            Console.ReadLine();
        }
    }
}