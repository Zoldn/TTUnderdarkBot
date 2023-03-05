using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using TUnderdark.TTSParser;
using UnderdarkAI.AI.PlayableOptions;

namespace UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators.Undead
{
    internal class CultistOfMyrkulOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            ABCSelectHelper.Run(options, board, turn,
                inIteration: 0,
                (b, t) => true, outIteration1: 1, // Получить ману
                (b, t) => true, outIteration2: 2, // Сожрать эту карту и промоуты
                outIteration3: 99
                );

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 99,
                (board, turn) => true,
                mana: 2);

            DevourSelfHelper.Run(options, board, turn,
                inIteration: 2,
                outIteration: 3);

            PromoteAnotherCardPlayedThisTurnHelper.Run(options, board, turn,
                inIteration: 3,
                outIteration: 99,
                CardSpecificType.CULTIST_OF_MYRKUL);

            EndCardHelper.Run(options, board, turn, 99);

            /// Промоут до двух карт в конце хода
            PromoteAnotherCardPlayedThisTurnHelper.RunEndTurn(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                CardSpecificType.CULTIST_OF_MYRKUL,
                canBeSkipped: true);

            PromoteAnotherCardPlayedThisTurnHelper.RunEndTurn(options, board, turn,
                inIteration: 1,
                outIteration: 2,
                CardSpecificType.CULTIST_OF_MYRKUL,
                canBeSkipped: true);

            EndCardHelper.RunEndTurn(options, board, turn, 2);

            return options;
        }
    }

    internal class HighPriestOfMyrkulOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            ReturnEnemyTroopOrSpyHelper.Run(options, board, turn,
                inIteration: 0,
                returnSpyIteration: 1,
                returnTroopsIteration: 2,
                outIteration: 3);

            PromoteAnotherCardPlayedThisTurnHelper.Run(options, board, turn,
                inIteration: 3,
                outIteration: 99,
                CardSpecificType.HIGH_PRIEST_OF_MYRKUL);

            EndCardHelper.Run(options, board, turn, 99);

            /// Промоут до двух карт в конце хода
            ABCSelectHelper.RunEndTurn(options, board, turn,
                inIteration: 0,
                (b, t) => 
                {
                    return t.CardStates
                        .Any(c => CardMapper.SpecificTypeCardMakers[c.SpecificType].Race == Race.UNDEAD
                            && c.State == CardState.PLAYED
                        );
                }, outIteration1: 1, // Промоутнуть сыгранную undead карту
                (b, t) => true, outIteration2: 99, // Выход
                outIteration3: 3);

            PromoteAnotherCardPlayedThisTurnHelper.RunEndTurn(options, board, turn,
                inIteration: 1,
                outIteration: 0,
                CardSpecificType.HIGH_PRIEST_OF_MYRKUL,
                specificRaceOnly: Race.UNDEAD);

            EndCardHelper.RunEndTurn(options, board, turn, 99);

            return options;
        }
    }

    internal class NecromancerOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            ABCSelectHelper.Run(options, board, turn,
                inIteration: 0,
                (b, t) => true, outIteration1: 1, // Получить ману
                (b, t) => true, outIteration2: 2, // Промоуты
                outIteration3: 99
                );

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 99,
                (board, turn) => true,
                mana: 3);

            ABCSelectHelper.Run(options, board, turn,
                inIteration: 2,
                (b, t) => true, outIteration1: 3, // Промоут себя
                (b, t) => b.Players[t.Color].Discard.Count > 0, outIteration2: 4, // Промоут сброса
                (b, t) => t.CardStates.Any(s => s.State == CardState.IN_HAND && !s.IsPromotedInTheEnd), 
                outIteration3: 5, // Промоут из руки
                outIteration4: 99
                );

            PromoteSelfHelper.Run(options, board, turn,
                CardSpecificType.NECROMANCER,
                inIteration: 3,
                outIteration: 99);

            PromoteFromDiscardHelper.Run(options, board, turn,
                CardSpecificType.NECROMANCER,
                inIteration: 4,
                outIteration: 99);

            PromoteFromHandHelper.Run(options, board, turn,
                CardSpecificType.NECROMANCER,
                inIteration: 5,
                outIteration: 99);

            EndCardHelper.Run(options, board, turn, 99);

            return options;
        }
    }

    internal class VampireSpawnOptionGenerator : OptionGenerator
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

    internal class VampireOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            ABCSelectHelper.Run(options, board, turn,
                inIteration: 0,
                (b, t) => OptionUtils.IsSupplantTargets(b, t), outIteration1: 1,
                (b, t) => b.Players[t.Color].Discard.Count > 0, outIteration2: 2,
                outIteration3: 99);

            SupplantOptionHelper.Run(options, board, turn,
                inIteration: 1,
                outIteration: 99);

            PromoteFromDiscardHelper.Run(options, board, turn,
                CardSpecificType.VAMPIRE,
                inIteration: 2,
                outIteration: 3);

            GetOptionalVPHelper.Run(options, board, turn,
                inIteration: 3,
                outIteration: 99,
                (b, t) => b.Players[t.Color].InnerCircle.Count / 3
                );

            EndCardHelper.Run(options, board, turn, 99);

            return options;
        }
    }
}
