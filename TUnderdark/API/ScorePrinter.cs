using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using TUnderdark.TTSParser;

namespace TUnderdark.API
{
    public class ScorePrinter
    {
        public ScorePrinter() { }
        public string GetScore()
        {
            var board = BoardInitializer.Initialize(isWithChecks: false);
            
            string json = TTSLoader.GetJson(isLastSave: true);

            TTSSaveParser.Read(json, board);

            var ret = board.PrintResults();

            return ret;
        }
    }
}
