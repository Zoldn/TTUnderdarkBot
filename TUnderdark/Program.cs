using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TUnderdark.Config;
using TUnderdark.Interaction;
using TUnderdark.Model;
using TUnderdark.RatingSystem;
using TUnderdark.RatingSystem.RatingUpdators;
using TUnderdark.TTSParser;

namespace TUnderdark
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Underdark!");

            var board = BoardInitializer.Initialize(isWithChecks: false);

            string json = GetJson(isLastSave: true);

            TTSSaveParser.Read(json, board);

            board.PrintResults();

            Console.ReadLine();


            //RecalcRatings();

            /*
            var ratingTracker = new RatingTracker();

            ratingTracker.CleanData();
            ratingTracker.WriteData();
            ratingTracker.ReadData();
            ratingTracker.CommitGame();
            ratingTracker.GetTopRatings();

            Console.WriteLine("Welcome to Underdark!");

            var board = BoardInitializer.Initialize(isWithChecks: false);

            string json = GetJson(isLastSave: true);

            TTSSaveParser.Read(json, board);

            board.PrintResults();
            */
        }

        private static void RecalcRatings()
        {
            var sourceTracker = new RatingTracker(
                            @"D:\Projects\TUnderdark\TUnderdark\DiscordBot\bin\Debug\net5.0\rating.json");
            var targetTracker = new RatingTracker(
                @"D:\Projects\TUnderdark\TUnderdark\DiscordBot\bin\Debug\net5.0\new_rating.json");

            sourceTracker.ReadData();

            targetTracker.CleanData();
            targetTracker.WriteData();

            var fileCommiter = new FileCommiter(new RatingUpdatorNew(), sourceTracker, targetTracker);

            fileCommiter.Update();

            targetTracker.WriteData();

            Console.WriteLine(targetTracker.GetTopRatings());
        }

        private static void RunTracker()
        {
            //var saveFile = GetJson(false);

            var playerNames = new Dictionary<Color, string>()
            {
                { Color.RED, "Epic" },
                { Color.YELLOW, "Nyamka" },
                { Color.GREEN, "msnk" },
                { Color.BLUE, "Раскрученная преданностъ" },
            };

            Console.WriteLine("\nTracker has started!\n");

            var saveFile = @"C:\Users\User\Documents\My Games\Tabletop Simulator\Saves\TS_Save_23.json";

            var partyTracker = new PartyTracker(saveFile, playerNames);

            partyTracker.Start();
        }

        public static string GetJson(bool isLastSave = true)
        {
            var config = Configuration.LoadFromFile();

            //var pathToUserFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            if (isLastSave)
            {
                //string targetDirectory = @"C:\Users\User\Documents\My Games\Tabletop Simulator\Saves\";
                //string targetDirectory = pathToUserFolder + @"\Documents\My Games\Tabletop Simulator\Saves\";
                string targetDirectory = config.PathToSaveFiles;
                return GetNewestSaveFileInFolder(targetDirectory);
            }
            else
            {
                //string targerSave = @"C:\Users\User\Documents\My Games\Tabletop Simulator\Saves\TS_Save_28.json";
                //string targerSave = pathToUserFolder + @"\Documents\My Games\Tabletop Simulator\Saves\TS_Save_45.json";
                string targerSave = config.PathToSaveFiles + @"TS_Save_45.json";

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
