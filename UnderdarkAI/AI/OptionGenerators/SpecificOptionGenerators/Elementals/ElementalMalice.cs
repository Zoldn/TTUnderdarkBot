using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.PlayableOptions;

namespace UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators.Elementals
{
    internal class EternalFlameCultistOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            AssassinateOptionHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 10);

            FocusHelper.Run(options, board, turn,
                CardType.MALICE,
                inIteraion: 10,
                focusIteration: 11,
                noneIteration: 99);

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 11,
                outIteration: 99,
                (board, turn) => true,
                swords: 2);

            EndCardHelper.Run(options, board, turn, 99);

            return options;
        }
    }

    internal class FireElementalMyrmidonOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                (board, turn) => true,
                swords: 2);

            PromoteAnotherCardPlayedThisTurnHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 99,
                CardSpecificType.FIRE_ELEMENTAL_MYRMIDON
                );

            EndCardHelper.Run(options, board, turn, 99);

            /// Промоут в конце хода
            PromoteAnotherCardPlayedThisTurnHelper.RunEndTurn(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                CardSpecificType.FIRE_ELEMENTAL_MYRMIDON,
                specificCardType: CardType.OBEDIENCE);

            EndCardHelper.RunEndTurn(options, board, turn, 1);

            return options;
        }
    }

    internal class FireElementalOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            ABCSelectHelper.Run(options, board, turn,
                inIteration: 0,
                (b, t) => true, outIteration1: 1,
                (b, t) => true, outIteration2: 2,
                outIteration3: 10
                );

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 10,
                (board, turn) => true,
                swords: 2);

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 2,
                outIteration: 10,
                (board, turn) => true,
                mana: 2);

            FocusHelper.Run(options, board, turn,
                CardType.MALICE,
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

    internal class ImyxOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 10,
                (board, turn) => true,
                swords: 4);

            FocusHelper.Run(options, board, turn,
                CardType.MALICE,
                inIteraion: 10,
                focusIteration: 11,
                noneIteration: 99);

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 11,
                outIteration: 99,
                (board, turn) => true,
                swords: 2);

            EndCardHelper.Run(options, board, turn, 99);

            return options;
        }
    }

    internal class VanyferOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            AssassinateOptionHelper.Run(options, board, turn,
               inIteration: 0,
               outIteration: 1);

            BuyOrRecruitCardHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 99,
                specificCardType: CardType.MALICE,
                costLimit: 4);

            EndCardHelper.Run(options, board, turn,
                endIteration: 99);

            return options;
        }
    }
}
