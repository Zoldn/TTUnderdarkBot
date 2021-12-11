using System;
using System.IO;
using System.Linq;
using TUnderdark.Model;
using TUnderdark.TTSParser;

namespace TUnderdark
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Underdark!");

            var board = BoardInitializer.Initialize(isWithChecks: false);

            string json = GetJson(isLastSave: false);

            TTSSaveParser.Read(json, board);

            board.PrintResults();

            Console.ReadLine();
        }

        private static string GetJson(bool isLastSave = true)
        {
            if (isLastSave)
            {
                string targetDirectory = @"C:\Users\User\Documents\My Games\Tabletop Simulator\Saves\";

                return GetNewestSaveFileInFolder(targetDirectory);
            }
            else
            {
                string targerSave = @"C:\Users\User\Documents\My Games\Tabletop Simulator\Saves\TS_Save_17.json";

                return GetSaveFile(targerSave);
            }
        }

        private static string GetSaveFile(string filePath)
        {
            return File.ReadAllText(filePath);
        }

        private static string GetNewestSaveFileInFolder(string folder)
        {
            string[] fileEntries = Directory.GetFiles(folder);

            var newestFilePath = fileEntries
                .Where(f => f.Contains("TS") && f.EndsWith(".json"))
                .ToDictionary(f => f, f => File.GetLastWriteTime(f))
                .OrderByDescending(kv => kv.Value)
                .Select(kv => kv.Key)
                .FirstOrDefault();

            //foreach (var item in fileEntries)
            //{
            //    Console.WriteLine($"{item} => {File.GetLastWriteTime(item)}");
            //}

            Console.WriteLine($"Newest save file is {newestFilePath}");

            string json = File.ReadAllText(newestFilePath);

            return json;
        }
    }
}
