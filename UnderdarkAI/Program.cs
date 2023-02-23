using TUnderdark.Model;
using TUnderdark.TTSParser;
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

            board.PrintResults();

            board.Clone().PrintResults();

            Console.ReadLine();
        }
    }
}