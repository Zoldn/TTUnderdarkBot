using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using TUnderdark.API;
using TUnderdark.Model;
using TUnderdark.RatingSystem;
using TUnderdark.TTSParser;
using UnderdarkAI.AI;
using UnderdarkAI.API;
using UnderdarkAI.Context;
using UnderdarkAI.IO;

namespace DiscordBot
{
    // Keep in mind your module **must** be public and inherit ModuleBase.
    // If it isn't, it will not be discovered by AddModulesAsync!
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        // ~say hello world -> hello world
        [Command("say")]
        [Summary("Echoes a message.")]
        public Task SayAsync([Remainder][Summary("The text to echo")] string echo)
            => ReplyAsync(echo);


        // 
        [Command("lastsave")]
        [Summary("Calculate last save")]
        public Task CalculateScoreLastAsync() 
        {
            Console.WriteLine("Welcome to Underdark!");

            var context = new ModelContext();

            var excelManager = new CardExcelManager(@"..\..\..\..\TUnderdark\Resources\Cards.xlsx");

            excelManager.Load(context);

            CardMapper.ReadCardsFromContext(context);

            var scorePrinter = new ScorePrinter();

            var ret = scorePrinter.GetScore();

            return ReplyAsync(ret);
        }

        [Command("setup")]
        [Summary("Calculate last save")]
        public Task MakeSetupAsync()
        {
            var colors = new List<string>() 
            {
                "Red", "Yellow", "Green", "Blue"
            };

            colors.Shuffle();

            var selectedColor = colors.First();

            var races = new List<string>() 
            {
                "Drow :elf:",
                "Undead :skull:",
                "Dragon :dragon_face:",
                "Elemental :fire:",
                "Aberration :octopus:",
                "Demon :japanese_ogre:",
            };

            races.Shuffle();
            var selectedRaces = $"{races[0]} and {races[1]}";

            return ReplyAsync($"First player is {selectedColor}\n" +
                $"Your half-decks are {selectedRaces}");
        }


        [Command("top")]
        [Summary("Get 10 top player ratings")]
        public Task GetTopRating()
        {
            var ratingTracker = new RatingTracker();

            ratingTracker.ReadData();
            var ret = ratingTracker.GetTopRatings(20);

            return ReplyAsync(ret);
        }

        [Command("commitlast")]
        [Summary("Commit results of last game to ratings")]
        public Task CommitGame()
        {
            var context = new ModelContext();

            var excelManager = new CardExcelManager(@"..\..\..\..\TUnderdark\Resources\Cards.xlsx");

            excelManager.Load(context);

            CardMapper.ReadCardsFromContext(context);

            var ratingTracker = new RatingTracker();

            var ret = ratingTracker.CommitGame();

            return ReplyAsync(ret);
        }

        private static readonly List<string> quotes = new()
        {
            "\"Я последний буду\" - Олег, 19 февраля 2023, 14:40",
        };

        [Command("quote")]
        [Summary("Quotes")]
        public Task GetQuote()
        {
            var random = new Random();

            var index = random.Next(quotes.Count);

            return ReplyAsync(quotes[index]);
        }

        [Command("run")]
        [Summary("Run solving turn for selected color")]
        public Task MakeTurn([Remainder][Summary("The text to echo")] string args)
        {
            var entryPoint = new AIEntryPoint();

            return ReplyAsync(entryPoint.RunTurn(args));
        }

        [Command("changelog")]
        [Summary("Change log for bot")]
        public Task GetChangeLog()
        {
            string dot = "\n\t・";

            string version_0_3 = $"Версия бота Underdark 0.3:" +
                $"{dot}Добавлен штраф за контроль локаций в начале игры, " +
                $"без которого боты пытались рано занимать обычные локации в ущерб городам;" +
                $"{dot}Добавлены настройки выбора лучшего хода (лучший в среднем/лучший из найденных);" +
                $"{dot}Карты, получающие бонус за выставления шпионов в локации с определенными условиями (Баньши, Лич и т.п.) " +
                $"теперь глубже просчитывают локации, удовлетворяющие этим условиям;" +
                $"{dot}По просьбе @Kai добавлен опциональный агрессивный режим бота, в котором они играют не за первое место, " +
                $"а для победы над игроками-людьми в случае, если ботов больше одного;" +
                $"{dot}Исправлена ошибка, из-за которой при оценке выгоды от полного контроля города в будущем бот считал, " +
                $"что в нем всегда находится все 3 вражеских шпиона.";

            return ReplyAsync(version_0_3);
        }
    }

    public static class ShuffleExtensions
    {
        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
