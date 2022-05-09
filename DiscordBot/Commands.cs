using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using TUnderdark.API;

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
