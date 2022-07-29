using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.RatingSystem
{
    public class RatingTracker
    {
        public static readonly string RATING_DATA_FILE = "rating.json";
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
        public RatingTracker()
        {

        }

        public bool ReadData()
        {
            string json = string.Empty;
            try
            {
                json = File.ReadAllText(RATING_DATA_FILE);
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

        public bool WriteDate()
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
                File.WriteAllText(RATING_DATA_FILE, json);
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to write data from file");
                isSuccess = false;
            }

            return isSuccess;
        }
    }
}
