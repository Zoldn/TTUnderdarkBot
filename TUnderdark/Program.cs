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

            string targetDirectory = @"C:\Users\User\Documents\My Games\Tabletop Simulator\Saves\";

            string[] fileEntries = Directory.GetFiles(targetDirectory);

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

            var board = new Board();

            BoardInitializer.Initialize(board);

            TTSSaveParser.Read(json, board);

            Console.ReadLine();
        }
    }
}
