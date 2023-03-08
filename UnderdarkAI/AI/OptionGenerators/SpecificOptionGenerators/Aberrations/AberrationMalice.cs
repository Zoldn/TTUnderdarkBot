using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.PlayableOptions;

namespace UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators.Aberrations
{
    internal class BeholderOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1);

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 2,
                (b, t) => true,
                swords: board.Players[turn.Color].TrophyHall.Sum(kv => kv.Value) / 3);

            EndCardHelper.Run(options, board, turn,
                endIteration: 2);

            return options;
        }
    }

    internal class DeathTyrantOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            var fixedLocation = turn.LockedAssasinationLocation.HasValue ?
                    new HashSet<LocationId>() { turn.LockedAssasinationLocation.Value } :
                    new HashSet<LocationId>();

            ABCSelectHelper.Run(options, board, turn,
                inIteration: 0,
                (b, t) => OptionUtils.IsAssassinateTargets(b, t, specificLocation: board.Sites), outIteration1: 1,
                (b, t) => true, outIteration2: 50,
                outIteration3: 50);

            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 1, outIteration: 2,
                specificLocation: board.Sites,
                isLockingNextAssassination: true);

            if (turn.State == SelectionState.SELECT_CARD_OPTION
                && turn.CardStateIteration == 2)
            {
                options.Add(new KillCounterOption(outIteration: 3));
            }

            ABCSelectHelper.Run(options, board, turn,
                inIteration: 3,
                (b, t) => OptionUtils.IsAssassinateTargets(b, t, specificLocation: fixedLocation), outIteration1: 4,
                (b, t) => true, outIteration2: 50,
                outIteration3: 50);

            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 4, outIteration: 5,
                specificLocation: turn.LockedAssasinationLocation.HasValue ?
                    new HashSet<LocationId>() { turn.LockedAssasinationLocation.Value } :
                    new HashSet<LocationId>());

            if (turn.State == SelectionState.SELECT_CARD_OPTION
                && turn.CardStateIteration == 5)
            {
                options.Add(new KillCounterOption(outIteration: 6));
            }

            ABCSelectHelper.Run(options, board, turn,
                inIteration: 6,
                (b, t) => OptionUtils.IsAssassinateTargets(b, t, specificLocation: fixedLocation), outIteration1: 7,
                (b, t) => true, outIteration2: 50,
                outIteration3: 50);

            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 7, outIteration: 8,
                specificLocation: turn.LockedAssasinationLocation.HasValue ?
                    new HashSet<LocationId>() { turn.LockedAssasinationLocation.Value } :
                    new HashSet<LocationId>());

            if (turn.State == SelectionState.SELECT_CARD_OPTION
                && turn.CardStateIteration == 8)
            {
                options.Add(new KillCounterOption(outIteration: 50));
            }

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 50, outIteration: 99,
                (b, t) => true,
                mana: turn.QuaggothKills);

            EndCardHelper.Run(options, board, turn,
                endIteration: 99);

            return options;
        }
    }

    internal class GauthOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            ABCSelectHelper.Run(options, board, turn,
                inIteration: 0,
                (b, t) => true, outIteration1: 10,
                (b, t) => true, outIteration2: 20,
                outIteration3: 99);

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 10, outIteration: 99,
                (b, t) => true,
                mana: 2);

            DrawCardHelper.Run(options, board, turn,
                cardCount: 1,
                inIteration: 20,
                outIteration: 21
                );

            OpponentDiscardHelper.Run(options, board, turn,
                initiator: CardSpecificType.NOTHIC,
                specificTargets: turn.EnemyPlayers,
                inIteration: 21,
                outIteration: 99);

            EndCardHelper.Run(options, board, turn,
                endIteration: 99);

            return options;
        }
    }
    internal class MindWitnessOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 0, outIteration: 1);

            OpponentDiscardHelper.Run(options, board, turn,
                initiator: CardSpecificType.MIND_WITNESS,
                specificTargets: turn.LastKillColor.HasValue && turn.LastKillColor != Color.WHITE ? 
                    new HashSet<Color>(1) { turn.LastKillColor.Value } :
                    new HashSet<Color>(),
                inIteration: 1,
                outIteration: 99);

            EndCardHelper.Run(options, board, turn,
                endIteration: 99);

            return options;
        }
    }

    internal class SpectatorOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 0, outIteration: 99,
                (b, t) => true,
                swords: 2, mana: 1);

            EndCardHelper.Run(options, board, turn,
                endIteration: 99);

            return options;
        }
    }

}
