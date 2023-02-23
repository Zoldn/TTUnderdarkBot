using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Config;

namespace TUnderdark.TTSParser
{
    public static class TTSLoader
    {
        public static string GetJson(bool isLastSave = true, string saveName = @"TS_Save_70.json")
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
                string targerSave = config.PathToSaveFiles + saveName;

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
