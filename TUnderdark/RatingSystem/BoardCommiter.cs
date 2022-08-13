using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using TUnderdark.RatingSystem.RatingUpdators;
using TUnderdark.TTSParser;

namespace TUnderdark.RatingSystem
{
    /// <summary>
    /// Класс, записывающий результаты последнего сохранения
    /// </summary>
    public class BoardCommiter
    {
        public IRatingUpdator RatingUpdator { get; private set; }
        public BoardCommiter(IRatingUpdator ratingUpdator)
        {
            RatingUpdator = ratingUpdator;
        }

        public List<IRatingUpdaterRecord> Update(RatingTracker tracker)
        {
            var board = BoardInitializer.Initialize(isWithChecks: false);

            string json = Program.GetJson(isLastSave: true);

            TTSSaveParser.Read(json, board);

            /// Проверяем, есть ли зарегистрированные игроки с такими steam_id
            /// если нет, то создаем
            CreateOrUpdatePlayerIfNeeded(tracker, board);

            /// Регистрируем новую игру
            var gameRecord = CreateNewGameRecord(tracker, board);

            /// Добавляем записи об игроках
            var playersInGame = AddPlayerGameRecords(tracker, gameRecord, board);

            return RatingUpdator.UpdateRatings(tracker, playersInGame);
        }

        private void CreateOrUpdatePlayerIfNeeded(RatingTracker tracker, Board board)
        {
            foreach (var (_, player) in board.Players)
            {
                if (!tracker.Players.TryGetValue(player.SteamId, out var playerRecord))
                {
                    if (player.SteamId != string.Empty)
                    {
                        tracker.Players.Add(
                            player.SteamId,
                            new PlayerRecord()
                            {
                                PlayerSteamId = player.SteamId,
                                PlayerSteamName = player.Name,
                            }
                        );
                    }
                }
                else
                {
                    if (player.Name == string.Empty)
                    {
                        continue;
                    }
                    else
                    {
                        playerRecord.PlayerSteamName = player.Name;
                    }
                }
            }
        }

        private GameRecord CreateNewGameRecord(RatingTracker tracker, Board board)
        {
            var record = GameRecord.CreateNewGame();

            /*
            var decks = board.Deck
                .Concat(board.Market)
                .Concat(board.Players.SelectMany(p => p.Value.Deck))
                .Concat(board.Players.SelectMany(p => p.Value.InnerCircle))
                .Concat(board.Players.SelectMany(p => p.Value.Hand))
                .Select(c => c.)
                .Distinct()
                .Where(e => e != Ra&& e != CardType.OBEDIENCE)
                .Select(e => e.ToString())
                .ToList();
            */

            tracker.Games.Add(record.GameRecordId, record);

            return record;
        }

        private List<GamePlayerRecord> AddPlayerGameRecords(RatingTracker tracker, GameRecord gameRecord, Board board)
        {
            var (controlVPs, totalControlVPs) = board.GetControlVPs();

            var colorScores = new Dictionary<Color, int>();

            foreach (var (color, player) in board.Players)
            {
                int score = controlVPs[color] +
                    totalControlVPs[color] +
                    player.PromoteVP +
                    player.TrophyHallVP +
                    player.DeckVP;

                colorScores.Add(color, score);
            }

            var scorePlaces = new Dictionary<int, int>();

            scorePlaces = colorScores.Select(kv => kv.Value)
                .Distinct()
                .OrderByDescending(e => e)
                .Select((e, index) => (e, index))
                .ToDictionary(p => p.e, p => p.index + 1);

            var colorPlaces = colorScores
                .ToDictionary(
                    kv => kv.Key,
                    kv => scorePlaces[kv.Value]
                    );

            var records = new List<GamePlayerRecord>();

            foreach (var (color, player) in board.Players)
            {
                if (!tracker.Players.TryGetValue(player.SteamId, out var playerRecord))
                {
                    continue;
                }

                playerRecord.PlayedGames++;

                if (colorPlaces[color] == 1)
                {
                    playerRecord.WonGames++;
                }

                var record = new GamePlayerRecord()
                {
                    Color = color.ToString(),
                    DeckVP = player.DeckVP,
                    ControlVP = controlVPs[color],
                    TotalControlVP = totalControlVPs[color],
                    PlayerSteamId = player.SteamId,
                    PromoteVP = player.PromoteVP,
                    TrophyVP = player.TrophyHallVP,
                    GameRecordId = gameRecord.GameRecordId,
                    ScoreVP = colorScores[color],
                    Place = colorPlaces[color],
                };

                records.Add(record);
            }

            foreach (var record in records)
            {
                tracker.GamePlayers.Add((gameRecord.GameRecordId, record.PlayerSteamId), record);
            }

            return records;
        }
    }
}
