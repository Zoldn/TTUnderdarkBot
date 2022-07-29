using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using TUnderdark.TTSParser;
using TUnderdark.Utils;

namespace TUnderdark.RatingSystem
{
    public class RatingTracker
    {
        private class RatingUpdaterRecord
        {
            public PlayerRecord PlayerRecord { get; set; }
            public GamePlayerRecord GamePlayerRecord { get; private set; }
            public int Score => GamePlayerRecord.ScoreVP;
            public double PartOfVPInGame { get; set; }
            public double PartRating { get; set; }
            public int ChangeRating { get; set; }
            public int OldRating { get; set; }
            public int NewRating { get; internal set; }

            public RatingUpdaterRecord(GamePlayerRecord gamePlayerRecord)
            {
                GamePlayerRecord = gamePlayerRecord;
                PartOfVPInGame = 0.0d;
                PartRating = 0.0d;
                ChangeRating = 0;
            }
        }

        public static readonly string RATING_DATA_FILE = "rating.json";
        /// <summary>
        /// Список игроков с ключом SteamId
        /// </summary>
        public Dictionary<string, PlayerRecord> Players { get; private set; }

        

        /// <summary>
        /// Список игр с ключом Id игры
        /// </summary>
        public Dictionary<long, GameRecord> Games { get; private set; }
        /// <summary>
        /// Записи о играх каждого игрока с ключом Id игры, SteamId игрока
        /// </summary>
        public Dictionary<(long, string), GamePlayerRecord> GamePlayers { get; private set; }
        public RatingTracker()
        {
            Players = new Dictionary<string, PlayerRecord>();
            Games = new Dictionary<long, GameRecord>();
            GamePlayers = new Dictionary<(long, string), GamePlayerRecord>();
        }

        public bool ReadData()
        {
            string json = string.Empty;

            if (!File.Exists(RATING_DATA_FILE))
            {
                CleanData();
            }

            try
            {
                json = File.ReadAllText(RATING_DATA_FILE);
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to read rating data from file");
            }

            var container = JsonConvert.DeserializeObject<JSONRatingDataContainer>(json);

            if (container == null)
            {
                Console.WriteLine("Failed to parse data from file");
                return false;
            }

            Players = container
                .Players
                .ToDictionary(r => r.PlayerSteamId);

            Games = container
                .Games
                .ToDictionary(r => r.GameRecordId);

            GamePlayers = container
                .GamePlayerRecords
                .ToDictionary(r => (r.GameRecordId, r.PlayerSteamId));

            Console.WriteLine("Successfull load data from json");

            return true;
        }

        public void CleanData()
        {
            var container = new JSONRatingDataContainer();

            var json = JsonConvert.SerializeObject(container);

            try
            {
                File.WriteAllText(RATING_DATA_FILE, json);
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to clean data from file");
            }
        }

        public bool WriteData()
        {
            var container = new JSONRatingDataContainer()
            {
                Players = Players.Values.ToList(),
                Games = Games.Values.ToList(),
                GamePlayerRecords = GamePlayers.Values.ToList(),
            };

            bool isSuccess = true;

            var json = JsonConvert.SerializeObject(container);

            if (json is null)
            {
                Console.WriteLine("Failed to serialize rating data");
                return false;
            }

            try
            {
                File.WriteAllText(RATING_DATA_FILE, json);
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to write data from file");
                isSuccess = false;
            }

            return isSuccess;
        }

        public string GetTopRatings(int ntop = 10)
        {
            string ret = "`\n";

            var players = Players
                .OrderByDescending(p => p.Value.Rating)
                .Take(ntop)
                .Select(p => p.Value)
                .ToList();

            int longestName = players.Count > 0 ? players.Max(e => e.PlayerSteamName.Length) :
                8;

            ret += $"Top {ntop} players:\n";

            ret += $"{"Name".AddSpacesToSize(longestName + 3)}" +
                    $"{"Rating".AddSpacesToSize(8)}" +
                    $"{"Games".AddSpacesToSize(8)}" +
                    $"{"Wins".AddSpacesToSize(8)}\n";

            ret += $"{"Roukert".AddSpacesToSize(longestName + 3)}" +
                    $"{"1000000".AddSpacesToSize(8)}" +
                    $"{"+Inf".AddSpacesToSize(8)}" +
                    $"{"+Inf".AddSpacesToSize(8)}   left undefeated, eternal top 1\n";

            foreach (var player in players)
            {
                string playerStr = $"{player.PlayerSteamName.AddSpacesToSize(longestName + 3)}" +
                    $"{player.Rating.AddSpacesToSize(8)}" +
                    $"{player.PlayedGames.AddSpacesToSize(8)}" + 
                    $"{player.WonGames.AddSpacesToSize(8)}\n";

                ret += playerStr;
            }

            ret += "`";

            Console.WriteLine(ret);

            return ret;
        }

        public string CommitGame()
        {
            ReadData();

            var board = BoardInitializer.Initialize(isWithChecks: false);

            string json = Program.GetJson(isLastSave: true);

            TTSSaveParser.Read(json, board);

            /// Проверяем, есть ли зарегистрированные игроки с такими steam_id
            /// если нет, то создаем
            CreateOrUpdatePlayerIfNeeded(board);

            /// Регистрируем новую игру
            var gameRecord = CreateNewGameRecord(board);

            /// Добавляем записи об игроках
            var playersInGame = AddPlayerGameRecords(gameRecord, board);

            /// Апдейтим рейтинги
            var ratingUpdates = UpdateRatings(playersInGame);

            /// Возвращаем список игроков с изменениями рейтинга
            string changeString = GetChangeRatingString(ratingUpdates);

            WriteData();

            //board.PrintResults();
            return changeString;
        }

        private string GetChangeRatingString(List<RatingUpdaterRecord> ratingUpdates)
        {
            string ret = "`\n";

            ret += $"Current game rating changes:\n";

            int maxNameLength = ratingUpdates.Max(r => r.PlayerRecord.PlayerSteamName.Length);

            ret += $"{"Name".AddSpacesToSize(maxNameLength + 5)}" +
                $"Old Rating -> " +
                $"New Rating   " +
                $"Change\n";

            foreach (var ratingUpdate in ratingUpdates
                .OrderByDescending(r => r.ChangeRating))
            {
                ret += $"{ratingUpdate.PlayerRecord.PlayerSteamName.AddSpacesToSize(maxNameLength + 5)}" +
                    $"{ratingUpdate.OldRating.AddSpacesToSize(10)} -> " +
                    $"{ratingUpdate.NewRating.AddSpacesToSize(10)}   " +
                    $"{ratingUpdate.ChangeRating.AddSignIfNeeded()}\n";
            }

            Console.WriteLine(ret);

            ret += "`";

            return ret;
        }

        private List<RatingUpdaterRecord> UpdateRatings(List<GamePlayerRecord> playersInGame)
        {
            var ratingUpdates = playersInGame
                .Select(p => new RatingUpdaterRecord(p))
                .ToList();

            foreach (var ratingUpdate in ratingUpdates)
            {
                ratingUpdate.PlayerRecord = Players[ratingUpdate.GamePlayerRecord.PlayerSteamId];
            }

            int totalRating = ratingUpdates.Sum(r => r.PlayerRecord.Rating);
            int totalScore = ratingUpdates.Sum(r => r.GamePlayerRecord.ScoreVP);

            if (totalRating == 0 || totalScore == 0)
            {
                return new List<RatingUpdaterRecord>();
            }

            foreach (var ratingUpdate in ratingUpdates)
            {
                ratingUpdate.PartOfVPInGame = (double)ratingUpdate.GamePlayerRecord.ScoreVP / totalScore;
                ratingUpdate.PartRating = (double)ratingUpdate.PlayerRecord.Rating / totalRating;

                ratingUpdate.ChangeRating = (int)Math.Round(
                    1000.0d * (ratingUpdate.PartOfVPInGame - ratingUpdate.PartRating)
                    );
            }

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

            foreach (var ratingUpdate in ratingUpdates)
            {
                ratingUpdate.OldRating = ratingUpdate.PlayerRecord.Rating;
                ratingUpdate.PlayerRecord.Rating += ratingUpdate.ChangeRating;
                ratingUpdate.NewRating = ratingUpdate.PlayerRecord.Rating;
            }

            return ratingUpdates;
        }

        private List<GamePlayerRecord> AddPlayerGameRecords(GameRecord gameRecord, Board board)
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
                if (!Players.TryGetValue(player.SteamId, out var playerRecord))
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
                GamePlayers.Add((gameRecord.GameRecordId, record.PlayerSteamId), record);
            }

            return records;
        }

        private GameRecord CreateNewGameRecord(Board board)
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

            Games.Add(record.GameRecordId, record);

            return record;
        }

        private void CreateOrUpdatePlayerIfNeeded(Board board)
        {
            foreach (var (_, player) in board.Players)
            {
                if (!Players.TryGetValue(player.SteamId, out var playerRecord))
                {
                    if (player.SteamId != string.Empty)
                    {
                        Players.Add(
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
    }
}
