using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.PlayableOptions;

namespace UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators.Elementals
{
    internal class CrushingWaveCultistOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 10,
                isOnlyWhite: true);

            FocusHelper.Run(options, board, turn,
                CardType.CONQUEST,
                inIteraion: 10,
                focusIteration: 11,
                noneIteration: 99);

            DeployOptionHelper.Run(options, board, turn,
                inIteration: 11, outIteration: 12);
            DeployOptionHelper.Run(options, board, turn,
                inIteration: 12, outIteration: 99);

            EndCardHelper.Run(options, board, turn, 99);

            return options;
        }
    }

    internal class GarShatterkeelOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            DeployOptionHelper.Run(options, board, turn,
                inIteration: 0, outIteration: 1);
            DeployOptionHelper.Run(options, board, turn,
                inIteration: 1, outIteration: 2);
            DeployOptionHelper.Run(options, board, turn,
                inIteration: 2, outIteration: 3);

            BuyOrRecruitCardHelper.Run(options, board, turn,
                inIteration: 3,
                outIteration: 99,
                specificCardType: CardType.CONQUEST,
                costLimit: 4);

            EndCardHelper.Run(options, board, turn,
                endIteration: 99);

            return options;
        }
    }

    internal class OlhydraOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            SupplantOptionHelper.Run(options, board, turn,
               inIteration: 0,
               outIteration: 10,
               isOnlyWhite: true,
               isAnywhere: true);

            FocusHelper.Run(options, board, turn,
                CardType.CONQUEST,
                inIteraion: 10,
                focusIteration: 11,
                noneIteration: 99);

            DeployOptionHelper.Run(options, board, turn,
                inIteration: 11, outIteration: 12);
            DeployOptionHelper.Run(options, board, turn,
                inIteration: 12, outIteration: 99);


            EndCardHelper.Run(options, board, turn,
                endIteration: 99);

            return options;
        }
    }

    internal class WaterElementalMyrmidonOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            AssassinateOptionHelper.Run(options, board, turn,
               inIteration: 0,
               outIteration: 1,
               isOnlyWhite: true);

            PromoteAnotherCardPlayedThisTurnHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 99,
                CardSpecificType.WATER_ELEMENTAL_MYRMIDON
                );

            EndCardHelper.Run(options, board, turn, 99);

            /// Промоут в конце хода
            PromoteAnotherCardPlayedThisTurnHelper.RunEndTurn(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                CardSpecificType.WATER_ELEMENTAL_MYRMIDON,
                specificCardType: CardType.OBEDIENCE);

            EndCardHelper.RunEndTurn(options, board, turn, 1);

            return options;
        }
    }

    internal class WaterElementalOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            DeployOptionHelper.Run(options, board, turn,
                inIteration: 0, outIteration: 1);
            DeployOptionHelper.Run(options, board, turn,
                inIteration: 1, outIteration: 10);

            FocusHelper.Run(options, board, turn,
                CardType.CONQUEST,
                inIteraion: 10,
                focusIteration: 11,
                noneIteration: 99);

            DrawCardHelper.Run(options, board, turn, 1,
                inIteration: 11,
                outIteration: 99);

            EndCardHelper.Run(options, board, turn, 99);

            return options;
        }
    }
}
