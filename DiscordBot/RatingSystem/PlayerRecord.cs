using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.RatingSystem
{
    public class PlayerRecord
    {
        public string PlayerSteamName { get; set; }
        public string PlayerSteamId { get; set; }
        public int Rating { get; set; }
        public PlayerRecord()
        {
            PlayerSteamName = string.Empty;
            PlayerSteamId = string.Empty;
            Rating = 1000;
        }
        public override string ToString()
        {
            return $"Player {PlayerSteamName} with rating = {Rating}";
        }
    }
}
