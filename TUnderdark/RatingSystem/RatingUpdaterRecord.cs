using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUnderdark.RatingSystem
{
    public interface IRatingUpdaterRecord
    {
        public PlayerRecord PlayerRecord { get; set; }
        public GamePlayerRecord GamePlayerRecord { get; }
        public int ChangeRating { get; set; }
        public int OldRating { get; set; }
        public int NewRating { get; set; }
    }
}
