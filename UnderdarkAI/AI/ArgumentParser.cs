using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace UnderdarkAI.AI
{
    internal class TurnMakerArguments
    {
        public Color? Color { get; internal set; }
        /// <summary>
        /// Цвет игрока, который заставляет скинуть карту. Если указано не null, 
        /// то возвращает только карту, которую надо скинуть
        /// </summary>
        public Color? DiscardFromColor { get; internal set; }
        public int? TurnNumber { get; internal set; }
        public int Iterations { get; internal set; }
        public TurnMakerArguments()
        {
            Iterations = 100;
            Color = null;
            TurnNumber = null;
            DiscardFromColor = null;
        }
    }
    internal static class ArgumentParser
    {
        public static HashSet<string> ArgStrings = new HashSet<string>() 
        {
            "color",
            "turn",
            "iters",
            "discard",
        };
        public static Dictionary<string, Color> ColorStrings = new()
        {
            { "r", Color.RED },
            { "y", Color.YELLOW },
            { "g", Color.GREEN },
            { "b", Color.BLUE },
        };

        public static bool Parse(string args, out string output, out TurnMakerArguments? parsedArgs)
        {
            output = "";
            parsedArgs = new TurnMakerArguments();

            var parts = args
                .Split("-")
                .ToList();

            var keyValuePairs = new Dictionary<string, string>();

            foreach (var part in parts)
            {
                var keyValue = part
                    .Split("=")
                    .Select(x => x.Trim())
                    .ToList();

                if (keyValue.Count != 2)
                {
                    continue;
                }

                if (!ArgStrings.Contains(keyValue[0]))
                {
                    output = $"Unknown argument {keyValue[0]}";
                    parsedArgs = null;
                    return false;
                }

                keyValuePairs.Add(keyValue[0], keyValue[1]);
            }

            if (!keyValuePairs.TryGetValue("color", out var colorParam))
            {
                output = $"color parameter have to be specified";
                parsedArgs = null;
                return false;
            }
            else
            {
                if (!ColorStrings.TryGetValue(colorParam.ToLower(), out var colorValue))
                {
                    output = $"Unknown color parameter {colorParam.ToLower()}";
                    parsedArgs = null;
                    return false;
                }
                else
                {
                    parsedArgs.Color = colorValue;
                }
            }

            if (keyValuePairs.TryGetValue("discard", out var discardParam))
            {
                if (!ColorStrings.TryGetValue(discardParam.ToLower(), out var colorValue))
                {
                    output = $"Unknown discard parameter {colorParam.ToLower()}";
                    parsedArgs = null;
                    return false;
                }
                else
                {
                    parsedArgs.DiscardFromColor = colorValue;
                }
            }

            if (!keyValuePairs.TryGetValue("turn", out var turnParam))
            {
                output = $"turn parameter have to be specified";
                parsedArgs = null;
                return false;
            }
            else
            {
                if (!int.TryParse(turnParam, out var turn) || turn <= 0)
                {
                    output = $"Turn have to be natural number";
                    parsedArgs = null;
                    return false;
                }
                else
                {
                    parsedArgs.TurnNumber = turn;
                }
            }

            if (keyValuePairs.TryGetValue("iters", out var iterParam))
            {
                if (!int.TryParse(iterParam, out var iter) || iter <= 0)
                {
                    output = $"Iterations have to be natural number";
                    parsedArgs = null;
                    return false;
                }
                else
                {
                    parsedArgs.Iterations = iter;
                }
            }

            return true;
        }
    }
}
