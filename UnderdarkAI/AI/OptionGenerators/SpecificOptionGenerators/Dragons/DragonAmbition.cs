using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.PlayableOptions;

namespace UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators.Dragons
{
    internal class BlueDragonOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            PromoteAnotherCardPlayedThisTurnHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                CardSpecificType.BLUE_DRAGON);

            EndCardHelper.Run(options, board, turn, 1);

            /// Промоут карты в конце хода
            PromoteAnotherCardPlayedThisTurnHelper.RunEndTurn(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                CardSpecificType.BLUE_DRAGON,
                canBeSkipped: true);

            PromoteAnotherCardPlayedThisTurnHelper.RunEndTurn(options, board, turn,
                inIteration: 1,
                outIteration: 2,
                CardSpecificType.BLUE_DRAGON,
                canBeSkipped: true);

            /// Получить VP
            GetOptionalVPHelper.RunEndTurn(options, board, turn,
                inIteration: 2,
                outIteration: 3,
                (b, t) => (b.Players[t.Color].InnerCircle.Count + t.CardStates.Count(s => s.IsPromotedInTheEnd)) / 3 );

            EndCardHelper.RunEndTurn(options, board, turn, 3);

            return options;
        }
    }

    internal class BlueWyrmlingOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            // Получить 3 маны
            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                (board, turn) => true,
                mana: 3);

            /// Вернуть шпиона или трупс
            ReturnEnemyTroopOrSpyHelper.Run(options, board, turn,
                inIteration: 1,
                returnSpyIteration: 2,
                returnTroopsIteration: 3,
                outIteration: 4);

            EndCardHelper.Run(options, board, turn,
                endIteration: 4);

            return options;
        }
    }

    internal class ClericOfLaogzedOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            MoveTroopHelper.Run(options, board, turn,
                inIteration: 0,
                targetIteration: 1,
                outIteration: 2);

            PromoteAnotherCardPlayedThisTurnHelper.Run(options, board, turn,
                inIteration: 2,
                outIteration: 3,
                CardSpecificType.CLERIC_OF_LAOGZED);

            EndCardHelper.Run(options, board, turn, 3);

            /// Промоут карты в конце хода
            PromoteAnotherCardPlayedThisTurnHelper.RunEndTurn(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                CardSpecificType.CLERIC_OF_LAOGZED);

            EndCardHelper.RunEndTurn(options, board, turn, 1);

            return options;
        }
    }

    internal class WyrmSpeakerOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            // Получить 1 ману
            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                (board, turn) => true,
                mana: 1);

            PromoteAnotherCardPlayedThisTurnHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 2,
                CardSpecificType.WYRMSPEAKER);

            EndCardHelper.Run(options, board, turn, 2);

            /// Промоут карты в конце хода
            PromoteAnotherCardPlayedThisTurnHelper.RunEndTurn(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                CardSpecificType.WYRMSPEAKER);

            EndCardHelper.RunEndTurn(options, board, turn, 1);

            return options;
        }
    }

    internal class CultFanaticOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            // Получить 2 маны
            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                (board, turn) => true,
                mana: 2);

            DevourCardOnMarketHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 2);

            //PromoteAnotherCardPlayedThisTurnHelper.Run(options, board, turn,
            //    inIteration: 1,
            //    outIteration: 2,
            //    CardSpecificType.WYRMSPEAKER);

            EndCardHelper.Run(options, board, turn, 2);

            return options;
        }
    }
}
