using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUnderdark.RatingSystem.RatingUpdators
{
    public class RatingUpdatorNew : RatingUpdatorAbstract<RatingUpdaterRecord>
    {
        protected static readonly IReadOnlyDictionary<int, int> PlacingBonusRating = new Dictionary<int, int>()
        {
            { 1, +3 },
            { 2, +1 },
            { 3, -1 },
            { 4, -3 },
        };

        public RatingUpdatorNew() : base() { }

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
                    200.0d * (partOfVPInGame - partRating)
                    );
            }

            /// Дополнительные очки за позиции

            var orderedScores = playersInGame
                .OrderByDescending(p => p.ScoreVP)
                .Select((p, index) => (Record: p, Position: index + 1))
                .GroupBy(p => p.Record.ScoreVP)
                .ToDictionary(
                    g => g.Key, 
                    g => (int)Math.Round(g.Average(pair => PlacingBonusRating[pair.Position]))
                    );

            foreach (var ratingUpdate in ratingUpdates)
            {
                ratingUpdate.ChangeRating += orderedScores[ratingUpdate.GamePlayerRecord.ScoreVP];
            }
        }
    }
}
