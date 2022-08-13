using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUnderdark.RatingSystem.RatingUpdators
{
    public class RatingUpdaterRecord : IRatingUpdaterRecord
    {
        public PlayerRecord PlayerRecord { get; set; }
        public GamePlayerRecord GamePlayerRecord { get; private set; }
        public int ChangeRating { get; set; }
        public int OldRating { get; set; }
        public int NewRating { get; set; }

        //public double PartOfVPInGame { get; set; }
        //public double PartRating { get; set; }
        public RatingUpdaterRecord(GamePlayerRecord gamePlayerRecord)
        {
            GamePlayerRecord = gamePlayerRecord;
            //PartOfVPInGame = 0.0d;
            //PartRating = 0.0d;
            ChangeRating = 0;
        }
    }

    public class RatingUpdatorDefault : RatingUpdatorAbstract<RatingUpdaterRecord>
    {
        public RatingUpdatorDefault() : base() { }

        protected override Func<GamePlayerRecord, RatingUpdaterRecord> UpdaterRecordCreator => 
            p => new RatingUpdaterRecord(p);

        protected override void CalculateChanges(RatingTracker tracker, List<GamePlayerRecord> playersInGame,
             List<RatingUpdaterRecord> ratingUpdates)
        {
            int totalRating = ratingUpdates.Sum(r => r.PlayerRecord.Rating);
            int totalScore = ratingUpdates.Sum(r => r.GamePlayerRecord.ScoreVP);

            foreach (var ratingUpdate in ratingUpdates)
            {
                var partOfVPInGame = (double)ratingUpdate.GamePlayerRecord.ScoreVP / totalScore;
                var partRating = (double)ratingUpdate.PlayerRecord.Rating / totalRating;

                ratingUpdate.ChangeRating = (int)Math.Round(
                    1000.0d * (partOfVPInGame - partRating)
                    );
            }
        }
    }
}
