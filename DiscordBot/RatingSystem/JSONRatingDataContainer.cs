using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.RatingSystem
{
    public class JSONRatingDataContainer
    {
        public List<PlayerRecord> Players { get; set; }
        public List<GameRecord> Games { get; set; }
        public List<GamePlayerRecord> GamePlayerRecords { get; set; }
        public JSONRatingDataContainer()
        {

        }
    }
}
