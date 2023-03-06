using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.PlayableOptions;

namespace UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators.Demons
{
    internal class HezrouOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            MoveTroopHelper.Run(options, board, turn,
                inIteration: 0,
                targetIteration: 1,
                outIteration: 2);

            PromoteTopDeckHelper
                .Run(options, board, turn,
                    inIteration: 2, 
                    outIteration: 99, 
                    CardSpecificType.HEZROU);

            EndCardHelper.Run(options, board, turn, 99);

            return options;
        }
    }

    internal class MyconidAdultOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                (board, turn) => true,
                mana: 2);

            RecruitInsaneOutcastHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 99,
                specificPlayers: turn.EnemyPlayers);

            EndCardHelper.Run(options, board, turn, 99);

            return options;
        }
    }

    internal class MyconidSovereignOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            RecruitInsaneOutcastHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                specificPlayers: turn.EnemyPlayers);

            PromoteAnotherCardPlayedThisTurnHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 99,
                CardSpecificType.MICONYD_SOVEREIGN);

            EndCardHelper.Run(options, board, turn, 99);

            /// Промоут карты в конце хода
            PromoteAnotherCardPlayedThisTurnHelper.RunEndTurn(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                CardSpecificType.MICONYD_SOVEREIGN);
            EndCardHelper.RunEndTurn(options, board, turn, 1);

            return options;
        }
    }

    internal class NaflshneeOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                (board, turn) => true,
                mana: 3);

            PromoteTopDeckHelper
                .Run(options, board, turn,
                    inIteration: 1,
                    outIteration: 99,
                    CardSpecificType.NAFLESHNEE);

            EndCardHelper.Run(options, board, turn, 99);

            return options;
        }
    }

    internal class ZuggtmoyOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            DevourCardInInnerCircleHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                exitIteration: 99,
                devourer: CardSpecificType.ZUGGTMOY
                );

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 2,
                (board, turn) => true,
                mana: 3);

            PromoteAnotherCardPlayedThisTurnHelper.Run(options, board, turn,
                inIteration: 2,
                outIteration: 99,
                CardSpecificType.ZUGGTMOY);

            EndCardHelper.Run(options, board, turn, 99);

            /// Промоут карты в конце хода
            PromoteAnotherCardPlayedThisTurnHelper.RunEndTurn(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                CardSpecificType.ZUGGTMOY,
                canBeSkipped: true);

            PromoteAnotherCardPlayedThisTurnHelper.RunEndTurn(options, board, turn,
                inIteration: 1,
                outIteration: 2,
                CardSpecificType.ZUGGTMOY,
                canBeSkipped: true);

            EndCardHelper.RunEndTurn(options, board, turn, 2);


            return options;
        }
    }
}
