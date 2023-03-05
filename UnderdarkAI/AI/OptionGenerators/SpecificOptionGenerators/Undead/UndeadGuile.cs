using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.PlayableOptions;

namespace UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators.Undead
{
    internal class BansheeOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            PlaceSpyHelper.Run(options, board, turn,
                inIteration: 0,
                returnIteration: 1,
                placeIteration: 2,
                outIteration: 3);

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 3,
                outIteration: 4,
                (b, t) => OptionUtils.IsPlacedSpyContainsEnemySpy(b, t),
                swords: 3
                );

            EndCardHelper.Run(options, board, turn,
                endIteration: 4);

            return options;
        }
    }

    internal class ConjurerOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            PlaceOrReturnSpyHelper.Run(options, board, turn,
                inIteration: 0,
                outPlaceSpyIteration: 1,
                returnSpyIteration: 4,
                outReturnSpyIteration: 5);

            PlaceSpyHelper.Run(options, board, turn,
                inIteration: 1,
                returnIteration: 2,
                placeIteration: 3,
                outIteration: 99);

            BuyOrRecruitCardHelper.Run(options, board, turn, 
                inIteration: 5, 
                outIteration: 6, 
                costLimit: 3);

            BuyOrRecruitCardHelper.Run(options, board, turn,
                inIteration: 6,
                outIteration: 99,
                costLimit: 3);

            EndCardHelper.Run(options, board, turn,
                endIteration: 99);

            return options;
        }
    }
}
