using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.RatingSystem
{
    public class GameRecord
    {
        public long GameRecordId { get; set; }
        public DateTime Date { get; set; }
        public List<string> HalfDecks { get; set; }
        public GameRecord()
        {
            HalfDecks = new List<string>();
        }
        public static GameRecord CreateNewGame()
        {
            var record = new GameRecord()
            {
                Date = DateTime.Now,
            };

            record.GameRecordId = record.Date.Millisecond +
                record.Date.Second * 1000 +
                record.Date.Minute * 100000 +
                record.Date.Hour * 10000000 +
                record.Date.Day * 1000000000 +
                record.Date.Month * 100000000000 +
                record.Date.Year * 10000000000000;

            return record;
        }
    }

    public class GamePlayerRecord
    {
        /// <summary>
        /// PK, FK на список игр
        /// </summary>
        public long GameRecordId { get; set; }
        /// <summary>
        /// PK, FK на игрока
        /// </summary>
        public string PlayerSteamId { get; set; }
        public string Color { get; set; }
        public int ScoreVP { get; set; }
        public int Place { get; set; }
        public int TrophyVP { get; set; }
        public int DeckVP { get; set; }
        public int PromoteVP { get; set; }
        public int TotalControlVP { get; set; }
        public int ControlVP { get; set; }
    }
}
