using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace UnderdarkAI.AI.OptionGenerators
{
    internal class ReturnSpyBySwordOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            if (turn.Swords < 3)
            {
                throw new ArgumentOutOfRangeException();
            }

            return OptionUtils.GetReturnEnemySpyOptions(board, turn, isBaseAction: true);
        }
    }
}
