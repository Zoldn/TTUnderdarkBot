using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnderdarkAI.AI
{
    public class TurnMakerResult
    {
        public double InitialScore { get; set; }
        public double AfterTurnScore { get; set; }
        internal List<PlayableOption> PlayableOptions { get; set; }
        public TurnMakerResult()
        {
            PlayableOptions = new List<PlayableOption>();
        }

        public string Print(bool doPrintScoreChange = true, int verbosity = 0)
        {
            var ret = string
                .Join("\n", PlayableOptions
                    .Select(e => e.Print(verbosity, e.MonteCarloStatus))
                    .Where(e => e.Length > 0)
                    );

            if (doPrintScoreChange)
            {
                ret += $"\nScore change of this turn is {InitialScore} -> {AfterTurnScore}";
            }

            return ret;
        }
    }
}
