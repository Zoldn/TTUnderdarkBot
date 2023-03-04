using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using TUnderdark.TTSParser;
using UnderdarkAI.AI.PlayableOptions;
using UnderdarkAI.Utils;

namespace UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators.Drow
{
    internal class AdvocateOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            ABCSelectHelper.Run(options, board, turn,
                inIteration: 0,
                (b, t) => true, outIteration1: 1,
                (b, t) => true, outIteration2: 2,
                outIteration3: 3
                );

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 3,
                (board, turn) => true,
                mana: 2);

            PromoteAnotherCardPlayedThisTurnHelper.Run(options, board, turn,
                inIteration: 2,
                outIteration: 3,
                CardSpecificType.ADVOCATE);

            EndCardHelper.Run(options, board, turn, 3);

            /// Промоут карты в конце хода
            PromoteAnotherCardPlayedThisTurnHelper.RunEndTurn(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                CardSpecificType.ADVOCATE);
            EndCardHelper.RunEndTurn(options, board, turn, 1);

            return options;
        }
    }

    internal class DrowNegotiatorOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                (board, turn) => board.Players[turn.Color].InnerCircle.Count >= 3,
                mana: 3);

            PromoteAnotherCardPlayedThisTurnHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 2,
                CardSpecificType.DROW_NEGOTIATOR);

            EndCardHelper.Run(options, board, turn, 2);

            /// Промоут карты в конце хода
            PromoteAnotherCardPlayedThisTurnHelper.RunEndTurn(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                CardSpecificType.DROW_NEGOTIATOR);
            EndCardHelper.RunEndTurn(options, board, turn, 1);

            return options;
        }
    }

    internal class ChosenOfLolthOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            /// Вернуть шпиона или трупс
            ReturnEnemyTroopOrSpyHelper.Run(options, board, turn,
                inIteration: 0,
                returnSpyIteration: 1,
                returnTroopsIteration: 2,
                outIteration: 3);
            /// Промоут карты в конце хода
            PromoteAnotherCardPlayedThisTurnHelper.Run(options, board, turn,
                inIteration: 3,
                outIteration: 4,
                CardSpecificType.CHOSEN_OF_LOLTH);
            EndCardHelper.Run(options, board, turn,
                endIteration: 4);

            /// Промоут карты в конце хода
            PromoteAnotherCardPlayedThisTurnHelper.RunEndTurn(options, board, turn,
                0, 1, CardSpecificType.CHOSEN_OF_LOLTH);

            EndCardHelper.RunEndTurn(options, board, turn, 1);

            return options;
        }
    }

    internal class CouncilMemberOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            MoveTroopHelper.Run(options, board, turn,
                inIteration: 0,
                targetIteration: 1,
                outIteration: 2);
            MoveTroopHelper.Run(options, board, turn,
                inIteration: 2,
                targetIteration: 3,
                outIteration: 4);
            PromoteAnotherCardPlayedThisTurnHelper.Run(options, board, turn, 4, 5, CardSpecificType.COUNCIL_MEMBER);
            EndCardHelper.Run(options, board, turn, 5);

            PromoteAnotherCardPlayedThisTurnHelper.RunEndTurn(options, board, turn, 0, 1, CardSpecificType.COUNCIL_MEMBER);
            EndCardHelper.RunEndTurn(options, board, turn, 1);

            return options;
        }
    }
    internal class MatronMotherOptionGenetator : OptionGenerator
    {
        internal class MoveDeckToDiscard : PlayableOption
        {
            public MoveDeckToDiscard(int outIteration) : base()
            {
                NextCardIteration = outIteration;
            }

            public override int MinVerbosity => 0;

            public override void ApplyOption(Board board, Turn turn)
            {
                var player = board.Players[turn.Color];
                player.Discard.AddRange(player.Deck);
                player.Deck.Clear();
            }

            public override string GetOptionText()
            {
                return $"\tMove deck to discard";
            }
        }

        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            if (turn.State == SelectionState.SELECT_CARD_OPTION
                && turn.CardStateIteration == 0)
            {
                options.Add(new MoveDeckToDiscard(1));
            }

            PromoteFromDiscardHelper.Run(options, board, turn, CardSpecificType.MATRON_MOTHER, 1, 2);

            EndCardHelper.Run(options, board, turn, 2);

            return options;
        }
    }
}
