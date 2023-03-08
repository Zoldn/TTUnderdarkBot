using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.PlayableOptions;

namespace UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators.Aberrations
{
    internal class AmbassadorOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            PromoteAnotherCardPlayedThisTurnHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                CardSpecificType.AMBASSADOR);

            EndCardHelper.Run(options, board, turn, 1);

            /// Промоут карты в конце хода
            PromoteAnotherCardPlayedThisTurnHelper.RunEndTurn(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                CardSpecificType.AMBASSADOR);

            EndCardHelper.RunEndTurn(options, board, turn, 1);

            return options;
        }
    }

    internal class ElderBrainOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            PromoteTopDeckHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                CardSpecificType.ELDER_BRAIN);

            PlayAnotherCardHelper.Run(options, board, turn,
                CardSpecificType.ELDER_BRAIN,
                inIteration: 1,
                returnIteration: 2,
                outIteration: 99);

            EndCardHelper.Run(options, board, turn, 99);

            return options;
        }
    }

    internal class IntellectDevourerOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            ABCSelectHelper.Run(options, board, turn,
                inIteration: 0,
                (b, t) => true, outIteration1: 1,
                (b, t) => OptionUtils.IsReturnableTroops(b, t) || OptionUtils.IsReturnableEnemySpies(b, t), outIteration2: 2,
                outIteration3: 99
                );

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 99,
                (b, t) => true,
                mana: 3);

            ReturnEnemyTroopOrSpyHelper.Run(options, board, turn,
                inIteration: 2,
                returnSpyIteration: 3,
                returnTroopsIteration: 4,
                outIteration: 5);

            ReturnEnemyTroopOrSpyHelper.Run(options, board, turn,
                inIteration: 5,
                returnSpyIteration: 6,
                returnTroopsIteration: 7,
                outIteration: 99);

            EndCardHelper.Run(options, board, turn, 99);

            return options;
        }
    }

    internal class PuppeteerOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                (b, t) => true,
                mana: 2);

            PromoteAnotherCardPlayedThisTurnHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 2,
                CardSpecificType.PUPPETEER);

            EndCardHelper.Run(options, board, turn, 2);

            /// Промоут карты в конце хода
            PromoteAnotherCardPlayedThisTurnHelper.RunEndTurn(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                CardSpecificType.PUPPETEER);

            EndCardHelper.RunEndTurn(options, board, turn, 1);
            return options;
        }
    }

    internal class UlitharidOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            PlayAnotherCardHelper.Run(options, board, turn,
                initiator: CardSpecificType.ULITHARID,
                inIteration: 0, // Выбор карты
                returnIteration: 1, // Возврат на инициирующую карту
                outIteration: 2 // Возвращение карты туда, где была
                );

            DevourCardOnMarketHelper.Run(options, board, turn,
                inIteration: 2,
                outIteration: 99,
                initiator: CardSpecificType.ULITHARID
                );


            EndCardHelper.Run(options, board, turn, 99);

            return options;
        }
    }
}
