using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.PlayableOptions;

namespace UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators.Elementals
{
    internal class AerisiKalinothOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                (board, turn) => true,
                swords: 1);

            PlaceSpyHelper.Run(options, board, turn,
                inIteration: 1,
                returnIteration: 2,
                placeIteration: 3,
                outIteration: 4);

            BuyOrRecruitCardHelper.Run(options, board, turn,
                inIteration: 4,
                outIteration: 99,
                specificCardType: CardType.GUILE,
                costLimit: 4);

            EndCardHelper.Run(options, board, turn, 99);

            return options;
        }
    }

    internal class AirElementalMyrmidonOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            PlaceSpyHelper.Run(options, board, turn,
                inIteration: 0,
                returnIteration: 1,
                placeIteration: 2,
                outIteration: 3);

            PromoteAnotherCardPlayedThisTurnHelper.Run(options, board, turn,
                inIteration: 3,
                outIteration: 99,
                CardSpecificType.AIR_ELEMENTAL_MYRMIDON
                );

            EndCardHelper.Run(options, board, turn, 99);

            /// Промоут в конце хода
            PromoteAnotherCardPlayedThisTurnHelper.RunEndTurn(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                CardSpecificType.AIR_ELEMENTAL_MYRMIDON,
                specificCardType: CardType.OBEDIENCE);

            EndCardHelper.RunEndTurn(options, board, turn, 1);

            return options;
        }
    }

    internal class AirElementalOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            PlaceOrReturnSpyHelper.Run(options, board, turn,
                inIteration: 0,
                outPlaceSpyIteration: 10,
                returnSpyIteration: 1,
                outReturnSpyIteration: 2);

            DeployOptionHelper.Run(options, board, turn,
                inIteration: 2, outIteration: 3);
            DeployOptionHelper.Run(options, board, turn,
                inIteration: 3, outIteration: 4);
            DeployOptionHelper.Run(options, board, turn,
                inIteration: 4, outIteration: 10);

            FocusHelper.Run(options, board, turn,
                CardType.GUILE,
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

    internal class HowlingHatredCultistOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            PlaceOrReturnSpyHelper.Run(options, board, turn,
                inIteration: 0,
                outPlaceSpyIteration: 10,
                returnSpyIteration: 1,
                outReturnSpyIteration: 2);

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 2,
                outIteration: 10,
                (b, t) => true,
                mana: 3);

            FocusHelper.Run(options, board, turn,
                CardType.GUILE,
                inIteraion: 10,
                focusIteration: 11,
                noneIteration: 99);

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 11,
                outIteration: 99,
                (b, t) => true,
                swords: 1);

            EndCardHelper.Run(options, board, turn, 99);

            return options;
        }
    }

    internal class YinCBinOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            PlaceSpyHelper.Run(options, board, turn,
                inIteration: 0,
                returnIteration: 1,
                placeIteration: 2,
                outIteration: 3);

            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 3,
                outIteration: 4,
                specificLocation: turn.PlacedSpies.ToHashSet());

            FocusHelper.Run(options, board, turn,
                CardType.GUILE,
                inIteraion: 4,
                focusIteration: 5,
                noneIteration: 99);

            PlaceSpyHelper.Run(options, board, turn,
                inIteration: 5,
                returnIteration: 6,
                placeIteration: 7,
                outIteration: 99);

            EndCardHelper.Run(options, board, turn, 99);

            return options;
        }
    }
}
