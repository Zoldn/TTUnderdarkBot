using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using TUnderdark.TTSParser;
using UnderdarkAI.AI.PlayableOptions;

namespace UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators.Demons
{
    internal class GrazztOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            ABCSelectHelper.Run(options, board, turn,
                inIteration: 0,
                (b, t) => true, outIteration1: 1,
                (b, t) => b.Players[t.Color].Spies < 5, outIteration2: 10,
                outIteration3: 99);

            PlaceSpyHelper.Run(options, board, turn,
                inIteration: 1,
                returnIteration: 2,
                placeIteration: 3,
                outIteration: 4);

            PlaceSpyHelper.Run(options, board, turn,
                inIteration: 4,
                returnIteration: 5,
                placeIteration: 6,
                outIteration: 99);

            /// Промоут до двух карт в конце хода
            ABCSelectHelper.Run(options, board, turn,
                inIteration: 10,
                (b, t) => b.Players[t.Color].Spies < 5, outIteration1: 11, // Промоутнуть сыгранную undead карту
                (b, t) => true, outIteration2: 99, // Выход
                outIteration3: 99);

            ReturnOwnSpyHelper.Run(options, board, turn,
                inIteration: 11,
                outIteration: 12);

            SupplantOptionHelper.Run(options, board, turn,
                inIteration: 12,
                outIteration: 10,
                isAnywhere: true,
                specificLocation: 
                    turn.ReturnedSpies.Count > 0 ? 
                        new HashSet<LocationId>(1) { turn.ReturnedSpies[^1] } : 
                        null);

            EndCardHelper.Run(options, board, turn,
                endIteration: 99);

            return options;
        }
    }

    internal class JackalWereOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            PlaceOrReturnSpyHelper.Run(options, board, turn,
                inIteration: 0,
                outPlaceSpyIteration: 10,
                returnSpyIteration: 20,
                outReturnSpyIteration: 21);

            PlaceSpyHelper.Run(options, board, turn,
                inIteration: 10,
                returnIteration: 11,
                placeIteration: 12,
                outIteration: 99);

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 21,
                outIteration: 99,
                (b, t) => true,
                swords: 2,
                mana: 2
                );

            EndCardHelper.Run(options, board, turn,
                endIteration: 99);

            return options;
        }
    }

    internal class NightHagOptionGenetator : OptionGenerator
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
                outIteration: 6);

            DrawCardHelper.Run(options, board, turn, cardCount: 2,
                inIteration: 5,
                outIteration: 6);

            EndCardHelper.Run(options, board, turn,
                endIteration: 6);

            return options;
        }
    }

    internal class SuccubusOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            DevourCardFromHandHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                exitIteration: 99,
                CardSpecificType.SUCCUBUS);

            PlaceSpyHelper.Run(options, board, turn,
                inIteration: 1,
                returnIteration: 2,
                placeIteration: 3,
                outIteration: 4);

            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 4,
                outIteration: 99,
                specificLocation: turn.PlacedSpies.ToHashSet());

            EndCardHelper.Run(options, board, turn, 99);

            return options;
        }
    }

    internal class VrockOptionGenerator : OptionGenerator
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
                outIteration: 5);

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 4,
                outIteration: 5,
                (b, t) => true,
                swords: 5
                );

            EndCardHelper.Run(options, board, turn,
                endIteration: 5);

            return options;
        }
    }
}
