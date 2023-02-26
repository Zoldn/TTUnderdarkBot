using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace UnderdarkAI.AI
{
    public class TurnMakerResult
    {
        public double InitialScore { get; set; }
        public double AfterTurnScore { get; set; }
        internal List<PlayableOption> PlayableOptions { get; set; }
        public Color ActivePlayerColor { get; set; }
        public TurnMakerResult(Color color)
        {
            PlayableOptions = new List<PlayableOption>();
            ActivePlayerColor = color;
        }

        public string Print(bool doPrintScoreChange = true, int verbosity = 0)
        {
            string ret = $"Recommended turn for {ActivePlayerColor} is:\n\n";

            ret += string
                .Join("\n", PlayableOptions
                    .Select(e => e.Print(verbosity, e.MonteCarloStatus))
                    .Where(e => e.Length > 0)
                    );

            if (doPrintScoreChange)
            {
                ret += $"\n\nScore change of this turn is {InitialScore} -> {AfterTurnScore}";
            }

            return ret;
        }
    }
}
