using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUnderdark.RatingSystem.RatingUpdators
{
    public interface IRatingUpdator
    {
        public List<IRatingUpdaterRecord> UpdateRatings(RatingTracker tracker, List<GamePlayerRecord> gamePlayerRecords);
    }
}
