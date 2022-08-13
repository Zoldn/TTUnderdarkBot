using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUnderdark.RatingSystem.RatingUpdators
{
    public abstract class RatingUpdatorAbstract<TRatingUpdaterRecord> : IRatingUpdator
        where TRatingUpdaterRecord : IRatingUpdaterRecord
    {
        public RatingUpdatorAbstract() { }
        protected abstract Func<GamePlayerRecord, TRatingUpdaterRecord> UpdaterRecordCreator { get; }
        public List<IRatingUpdaterRecord> UpdateRatings(RatingTracker tracker, List<GamePlayerRecord> playersInGame)
        {
            List<TRatingUpdaterRecord> ratingUpdates = InitializeUpdateRecords(tracker, playersInGame);

            CalculateChanges(tracker, playersInGame, ratingUpdates);

            CheckAndFixZeroSum(ratingUpdates);

            UpdatePlayerRating(ratingUpdates);

            return ratingUpdates.Select(e => e as IRatingUpdaterRecord).ToList();
        }

        protected abstract void CalculateChanges(RatingTracker tracker, List<GamePlayerRecord> playersInGame,
             List<TRatingUpdaterRecord> ratingUpdates);

        private List<TRatingUpdaterRecord> InitializeUpdateRecords(RatingTracker tracker, List<GamePlayerRecord> playersInGame)
        {
            var ratingUpdates = playersInGame
                .Select(p => UpdaterRecordCreator(p))
                .OrderByDescending(p => p.GamePlayerRecord.ScoreVP)
                .ToList();

            foreach (var ratingUpdate in ratingUpdates)
            {
                ratingUpdate.PlayerRecord = tracker.Players[ratingUpdate.GamePlayerRecord.PlayerSteamId];
            }

            return ratingUpdates;
        }

        private static void UpdatePlayerRating(List<TRatingUpdaterRecord> ratingUpdates)
        {
            foreach (var ratingUpdate in ratingUpdates)
            {
                ratingUpdate.OldRating = ratingUpdate.PlayerRecord.Rating;
                ratingUpdate.PlayerRecord.Rating += ratingUpdate.ChangeRating;
                ratingUpdate.NewRating = ratingUpdate.PlayerRecord.Rating;
            }
        }

        private static void CheckAndFixZeroSum(List<TRatingUpdaterRecord> ratingUpdates)
        {
            int sumOfChanges = ratingUpdates.Sum(r => r.ChangeRating);

            if (sumOfChanges > 0)
            {
                foreach (var ratingUpdate in ratingUpdates)
                {
                    if (sumOfChanges == 0)
                    {
                        break;
                    }

                    ratingUpdate.ChangeRating -= 1;
                    sumOfChanges--;
                }
            }

            if (sumOfChanges < 0)
            {
                foreach (var ratingUpdate in ratingUpdates)
                {
                    if (sumOfChanges == 0)
                    {
                        break;
                    }

                    ratingUpdate.ChangeRating += 1;
                    sumOfChanges++;
                }
            }
        }
    }
}
