using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.PlayableOptions;

namespace UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators.Elementals
{
    internal class BlackEarthCultistOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            PromoteAnotherCardPlayedThisTurnHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                CardSpecificType.BLACK_EARTH_CULTIST);

            FocusHelper.Run(options, board, turn,
                CardType.AMBITION,
                inIteraion: 1,
                focusIteration: 2, 
                noneIteration: 99);

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 2,
                outIteration: 99,
                (board, turn) => true,
                mana: 2);

            EndCardHelper.Run(options, board, turn, 99);

            /// Промоут до двух карт в конце хода
            PromoteAnotherCardPlayedThisTurnHelper.RunEndTurn(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                CardSpecificType.BLACK_EARTH_CULTIST,
                canBeSkipped: true);

            EndCardHelper.RunEndTurn(options, board, turn, 1);

            return options;
        }
    }

    internal class EarthElementalMyrmidonOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            PromoteAnotherCardPlayedThisTurnHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                CardSpecificType.EARTH_ELEMENTAL_MYRMIDON);

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 99,
                (board, turn) => true,
                mana: 2);

            EndCardHelper.Run(options, board, turn, 99);

            /// Промоут до двух карт в конце хода
            PromoteAnotherCardPlayedThisTurnHelper.RunEndTurn(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                CardSpecificType.EARTH_ELEMENTAL_MYRMIDON,
                canBeSkipped: true);

            EndCardHelper.RunEndTurn(options, board, turn, 1);

            return options;
        }
    }

    internal class EarthElementalOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                (board, turn) => true,
                mana: 1);

            ReturnEnemyTroopOrSpyHelper.Run(options, board, turn,
                inIteration: 1,
                returnSpyIteration: 2,
                returnTroopsIteration: 3,
                outIteration: 4
                );

            FocusHelper.Run(options, board, turn,
                CardType.AMBITION,
                inIteraion: 4,
                focusIteration: 5,
                noneIteration: 99);

            DrawCardHelper.Run(options, board, turn, 1,
                inIteration: 5, outIteration: 99);

            EndCardHelper.Run(options, board, turn, 99);

            return options;
        }
    }

    internal class MarlosUrnrayleOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                (board, turn) => true,
                mana: 1);

            PromoteAnotherCardPlayedThisTurnHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 2,
                CardSpecificType.MARLOS_URNRAYLE);

            BuyOrRecruitCardHelper.Run(options, board, turn,
                inIteration: 2,
                outIteration: 99,
                specificCardType: CardType.AMBITION,
                costLimit: 4);

            EndCardHelper.Run(options, board, turn, 99);

            /// Промоут до двух карт в конце хода
            PromoteAnotherCardPlayedThisTurnHelper.RunEndTurn(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                CardSpecificType.MARLOS_URNRAYLE,
                canBeSkipped: true);

            EndCardHelper.RunEndTurn(options, board, turn, 1);

            return options;
        }
    }

    internal class OgremochOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            PromoteAnotherCardPlayedThisTurnHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                CardSpecificType.EARTH_ELEMENTAL_MYRMIDON);

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 99,
                (board, turn) => true,
                mana: 2);

            EndCardHelper.Run(options, board, turn, 99);

            /// Промоут до двух карт в конце хода
            PromoteAnotherCardPlayedThisTurnHelper.RunEndTurn(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                CardSpecificType.OGREMOCH);

            FocusHelper.RunEndTurn(options, board, turn,
                CardType.AMBITION,
                inIteration: 1,
                focusIteration: 2, 
                noneIteration: 99);

            PromoteAnotherCardPlayedThisTurnHelper.RunEndTurn(options, board, turn,
                inIteration: 2,
                outIteration: 99,
                CardSpecificType.OGREMOCH,
                canBeSkipped: true);

            EndCardHelper.RunEndTurn(options, board, turn, 99);

            return options;
        }
    }
}
