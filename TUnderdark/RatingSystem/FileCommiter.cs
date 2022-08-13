using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.RatingSystem.RatingUpdators;

namespace TUnderdark.RatingSystem
{
    public class FileCommiter
    {
        public IRatingUpdator RatingUpdator { get; private set; }
        public RatingTracker Source { get; private set; }
        public RatingTracker Target { get; private set; }

        public FileCommiter(IRatingUpdator ratingUpdator, RatingTracker source, RatingTracker target)
        {
            RatingUpdator = ratingUpdator;
            Source = source;
            Target = target;
        }

        public void Update() 
        {
            foreach (var (gameId, game) in Source.Games
                .OrderBy(g => g.Value.Date))
            {
                UpdateOneGame(game);
            }
        }

        private void UpdateOneGame(GameRecord game)
        {
            if (Target.Games.ContainsKey(game.GameRecordId))
            {
                Console.WriteLine($"Game {game.GameRecordId} already exists in target");
                return;
            }

            CreateOrUpdatePlayerIfNeeded(game);

            /// Регистрируем новую игру
            var gameRecord = CreateNewGameRecord(game);

            /// Добавляем записи об игроках
            var playersInGame = AddPlayerGameRecords(gameRecord, game);

            AddGameStats(playersInGame);

            /// Обновляем рейтинги
            RatingUpdator.UpdateRatings(Target, playersInGame);
        }

        private void AddGameStats(List<GamePlayerRecord> playersInGame)
        {
            foreach (var playerInGame in playersInGame)
            {
                Target.Players[playerInGame.PlayerSteamId].PlayedGames += 1;
            }

            var maxVP = playersInGame.Max(p => p.ScoreVP);

            var winners = playersInGame.Where(p => p.ScoreVP == maxVP);

            foreach (var winner in winners)
            {
                Target.Players[winner.PlayerSteamId].WonGames += 1;
            }
        }

        private List<GamePlayerRecord> AddPlayerGameRecords(GameRecord gameRecord, GameRecord game)
        {
            var gamePlayerRecords = Source
                .GamePlayers
                .Where(kv => kv.Key.Item1 == game.GameRecordId)
                .Select(kv => new GamePlayerRecord() 
                {
                    DeckVP = kv.Value.DeckVP,
                    Color = kv.Value.Color,
                    ControlVP = kv.Value.ControlVP,
                    GameRecordId = kv.Value.GameRecordId,
                    Place = kv.Value.Place,
                    PlayerSteamId = kv.Value.PlayerSteamId,
                    PromoteVP = kv.Value.PromoteVP,
                    ScoreVP = kv.Value.ScoreVP,
                    TotalControlVP = kv.Value.TotalControlVP,
                    TrophyVP = kv.Value.TrophyVP,
                })
                .ToList();

            foreach (var gamePlayerRecord in gamePlayerRecords)
            {
                Target.GamePlayers.Add((gamePlayerRecord.GameRecordId, gamePlayerRecord.PlayerSteamId), gamePlayerRecord);
            }

            return gamePlayerRecords;
        }

        private GameRecord CreateNewGameRecord(GameRecord game)
        {
            var targetGame = new GameRecord()
            {
                GameRecordId = game.GameRecordId,
                Date = game.Date,
            };

            Target.Games.Add(game.GameRecordId, targetGame);

            return targetGame;
        }

        private void CreateOrUpdatePlayerIfNeeded(GameRecord game)
        {
            var gameRecords = Source
                .GamePlayers
                .Where(kv => kv.Key.Item1 == game.GameRecordId)
                .ToList();

            foreach (var ((gameId, playerId), gamePlayerRecord) in gameRecords)
            {
                var sourcePlayerRecord = Source.Players[playerId];

                /// Если есть, то апдейтим имя
                if (Target.Players.TryGetValue(playerId, out var targetPlayerRecord))
                {
                    targetPlayerRecord.PlayerSteamName = sourcePlayerRecord.PlayerSteamName;

                    continue;
                }

                targetPlayerRecord = new PlayerRecord()
                {
                    PlayerSteamId = playerId,
                    PlayerSteamName = sourcePlayerRecord.PlayerSteamName,
                    Rating = 1000,
                };

                Target.Players.Add(playerId, targetPlayerRecord);
            }
        }
    }
}
