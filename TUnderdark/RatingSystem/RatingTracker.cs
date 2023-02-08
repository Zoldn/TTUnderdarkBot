using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using TUnderdark.RatingSystem.RatingUpdators;
using TUnderdark.TTSParser;
using TUnderdark.Utils;

namespace TUnderdark.RatingSystem
{
    

    public class RatingTracker
    {
        public const string DEFAULT_RATING_FILE = "rating.json";
        public string RatingDataFile { get; protected set; }
        /// <summary>
        /// Список игроков с ключом SteamId
        /// </summary>
        public Dictionary<string, PlayerRecord> Players { get; private set; }

        /// <summary>
        /// Список игр с ключом Id игры
        /// </summary>
        public Dictionary<long, GameRecord> Games { get; private set; }
        /// <summary>
        /// Записи о играх каждого игрока с ключом Id игры, SteamId игрока
        /// </summary>
        public Dictionary<(long, string), GamePlayerRecord> GamePlayers { get; private set; }
        public RatingTracker(string inputFile = DEFAULT_RATING_FILE)
        {
            Players = new Dictionary<string, PlayerRecord>();
            Games = new Dictionary<long, GameRecord>();
            GamePlayers = new Dictionary<(long, string), GamePlayerRecord>();

            RatingDataFile = inputFile;
        }

        public bool ReadData()
        {
            string json = string.Empty;

            if (!File.Exists(RatingDataFile))
            {
                CleanData();
            }

            try
            {
                json = File.ReadAllText(RatingDataFile);
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to read rating data from file");
            }

            var container = JsonConvert.DeserializeObject<JSONRatingDataContainer>(json);

            if (container == null)
            {
                Console.WriteLine("Failed to parse data from file");
                return false;
            }

            Players = container
                .Players
                .ToDictionary(r => r.PlayerSteamId);

            Games = container
                .Games
                .ToDictionary(r => r.GameRecordId);

            GamePlayers = container
                .GamePlayerRecords
                .ToDictionary(r => (r.GameRecordId, r.PlayerSteamId));

            Console.WriteLine("Successfull load data from json");

            return true;
        }

        public void CleanData()
        {
            var container = new JSONRatingDataContainer();

            var json = JsonConvert.SerializeObject(container);

            try
            {
                File.WriteAllText(RatingDataFile, json);
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to clean data from file");
            }
        }

        public bool WriteData()
        {
            var container = new JSONRatingDataContainer()
            {
                Players = Players.Values.ToList(),
                Games = Games.Values.ToList(),
                GamePlayerRecords = GamePlayers.Values.ToList(),
            };

            bool isSuccess = true;

            var json = JsonConvert.SerializeObject(container);

            if (json is null)
            {
                Console.WriteLine("Failed to serialize rating data");
                return false;
            }

            try
            {
                File.WriteAllText(RatingDataFile, json);
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to write data from file");
                isSuccess = false;
            }

            return isSuccess;
        }

        public string GetTopRatings(int ntop = 10)
        {
            string ret = "`\n";

            var players = Players
                .OrderByDescending(p => p.Value.Rating)
                .Take(ntop)
                .Select(p => p.Value)
                .ToList();

            int longestName = players.Count > 0 ? players.Max(e => e.PlayerSteamName.Length) :
                8;

            ret += $"Top {ntop} players:\n";

            ret += $"{"Name".AddSpacesToSize(longestName + 3)}" +
                    $"{"Rating".AddSpacesToSize(8)}" +
                    $"{"Games".AddSpacesToSize(8)}" +
                    $"{"Wins".AddSpacesToSize(8)}\n";

            ret += $"{"Roukert".AddSpacesToSize(longestName + 3)}" +
                    $"{"1000000".AddSpacesToSize(8)}" +
                    $"{"+Inf".AddSpacesToSize(8)}" +
                    $"{"+Inf".AddSpacesToSize(8)}   left undefeated, eternal top 1\n";

            foreach (var player in players)
            {
                string playerStr = $"{player.PlayerSteamName.AddSpacesToSize(longestName + 3)}" +
                    $"{player.Rating.AddSpacesToSize(8)}" +
                    $"{player.PlayedGames.AddSpacesToSize(8)}" + 
                    $"{player.WonGames.AddSpacesToSize(8)}\n";

                ret += playerStr;
            }

            ret += "`";

            Console.WriteLine(ret);

            return ret;
        }

        public string CommitGame()
        {
            ReadData();

            var boardCommiter = new BoardCommiter(new RatingUpdatorNew());

            var ratingUpdates = boardCommiter.Update(this);

            /// Возвращаем список игроков с изменениями рейтинга
            string changeString = GetChangeRatingString(ratingUpdates);

            WriteData();

            //board.PrintResults();
            return changeString;
        }

        public string GetChangeRatingString(List<IRatingUpdaterRecord> ratingUpdates)
        {
            string ret = "`\n";

            ret += $"Current game rating changes:\n";

            int maxNameLength = ratingUpdates.Max(r => r.PlayerRecord.PlayerSteamName.Length);

            ret += $"{"Name".AddSpacesToSize(maxNameLength + 5)}" +
                $"Old Rating -> " +
                $"New Rating   " +
                $"Change\n";

            foreach (var ratingUpdate in ratingUpdates
                .OrderByDescending(r => r.ChangeRating))
            {
                ret += $"{ratingUpdate.PlayerRecord.PlayerSteamName.AddSpacesToSize(maxNameLength + 5)}" +
                    $"{ratingUpdate.OldRating.AddSpacesToSize(10)} -> " +
                    $"{ratingUpdate.NewRating.AddSpacesToSize(10)}   " +
                    $"{ratingUpdate.ChangeRating.AddSignIfNeeded()}\n";
            }

            Console.WriteLine(ret);

            ret += "`";

            return ret;
        }
    }
}
