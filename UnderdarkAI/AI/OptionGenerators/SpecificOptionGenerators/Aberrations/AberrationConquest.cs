using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.PlayableOptions;

namespace UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators.Aberrations
{
    internal class CraniumRatsOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            DeployOptionHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1);

            DeployOptionHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 2);

            OpponentDiscardHelper.Run(options, board, turn,
                initiator: CardSpecificType.CRANIUM_RATS,
                specificTargets: turn.EnemyPlayers,
                inIteration: 2,
                outIteration: 99,
                isToAll: false);

            EndCardHelper.Run(options, board, turn,
                endIteration: 99);

            return options;
        }
    }

    internal class GrimlockOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            DeployOptionHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 99);

            EndCardHelper.Run(options, board, turn,
                endIteration: 99);

            /// On discard
            if (turn.State == SelectionState.ON_DISCARD_CARD)
            {
                var firstInQueue = turn.DiscardCardQueue.Peek();

                if (!turn.CurrentDiscardingCard.HasValue)
                {
                    throw new NullReferenceException();
                }

                options.Add(new DrawCardOnDiscardOption(firstInQueue.TargetPlayerColor, 
                    turn.CurrentDiscardingCard.Value, cards: 2)
                {
                    NextState = SelectionState.CARD_OR_FREE_ACTION,
                }); // сбросить себя и дровнуть 2 карты
            }

            return options;
        }
    }

    internal class NeogiOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            DeployOptionHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1);
            DeployOptionHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 2);
            DeployOptionHelper.Run(options, board, turn,
                inIteration: 2,
                outIteration: 3);
            DeployOptionHelper.Run(options, board, turn,
                inIteration: 3,
                outIteration: 4);

            if (turn.State == SelectionState.SELECT_CARD_OPTION
                && turn.CardStateIteration == 4)
            {
                options.Add(new NeogiActivation(outIteration: 99));
            }

            EndCardHelper.Run(options, board, turn,
                endIteration: 99);

            OpponentDiscardHelper.RunEndTurn(options, board, turn,
                initiator: CardSpecificType.NEOGI,
                specificTargets: turn.EnemyPlayers,
                inIteration: 0,
                outIteration: 99,
                isToAll: true,
                cardLimit: 0);

            EndCardHelper.RunEndTurn(options, board, turn,
                endIteration: 99);

            return options;
        }
    }

    internal class UmberhulkOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            DeployOptionHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1);
            DeployOptionHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 2);
            DeployOptionHelper.Run(options, board, turn,
                inIteration: 2,
                outIteration: 99);

            EndCardHelper.Run(options, board, turn,
                endIteration: 99);

            /// On discard
            if (turn.State == SelectionState.ON_DISCARD_CARD)
            {
                var firstInQueue = turn.DiscardCardQueue.Peek();

                if (!turn.CurrentDiscardingCard.HasValue)
                {
                    throw new NullReferenceException();
                }

                options.Add(new ReverseDiscardOption(firstInQueue.TargetPlayerColor, firstInQueue.SourcePlayerColor,
                    turn.CurrentDiscardingCard.Value)
                {
                    NextState = SelectionState.CARD_OR_FREE_ACTION,
                }); // сбросить себя и заставить сбросить в ответ
            }

            return options;
        }
    }

    internal class QuaggothOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            if (turn.State == SelectionState.SELECT_CARD_OPTION
                && turn.CardStateIteration == 0)
            {
                options.Add(new QuaggothSetupOption(outIteration: 1));
            }

            ABCSelectHelper.Run(options, board, turn,
                inIteration: 1,
                (b, t) => OptionUtils.IsAssassinateTargets(b, t, isOnlyWhite: true) 
                    && t.QuaggothKills < t.MaxQuaggothKills, outIteration1: 2,
                (b, t) => false, outIteration2: 99,
                outIteration3: 99);

            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 2,
                outIteration: 3,
                isOnlyWhite: true);

            if (turn.State == SelectionState.SELECT_CARD_OPTION
                && turn.CardStateIteration == 3)
            {
                options.Add(new KillCounterOption(outIteration: 1));
            }

            EndCardHelper.Run(options, board, turn,
                endIteration: 99);

            return options;
        }
    }
}
