using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.OptionGenerators;
using UnderdarkAI.Utils;

namespace UnderdarkAI.AI.PlayableOptions
{
    internal static class ReturnEnemyTroopOrSpyHelper
    {
        public static List<PlayableOption> Run(List<PlayableOption> options, Board board, Turn turn,
                int inIteration,
                int returnSpyIteration,
                int returnTroopsIteration,
                int outIteration
            )
        {
            ///Выбор возвращать трупса или шпиона
            ABCSelectHelper.Run(options, board, turn,
                inIteration,
                (board, turn) => OptionUtils.IsReturnableTroops(board, turn), returnTroopsIteration,
                (board, turn) => OptionUtils.IsReturnableEnemySpies(board, turn), returnSpyIteration,
                outIteration
                );

            // На опцию А возвращаем трупсов
            if (turn.State == SelectionState.SELECT_CARD_OPTION
                && turn.CardStateIteration == returnTroopsIteration
                )
            {
                options.AddRange(OptionUtils
                    .GetReturnTroopOptions(board, turn, outIteration));

                return options;
            }

            // На опцию B возвращаем шпионов
            if (turn.State == SelectionState.SELECT_CARD_OPTION
                && turn.CardStateIteration == returnSpyIteration
                )
            {
                options.AddRange(OptionUtils
                    .GetReturnEnemySpyOptions(board, turn, outIteration));

                return options;
            }

            return options;
        }
    }

    internal static class ReturnEnemySpyHelper
    {
        public static List<PlayableOption> Run(List<PlayableOption> options, Board board, Turn turn,
                int inIteration,
                int outIteration
            )
        {
            // На опцию B возвращаем шпионов
            if (turn.State == SelectionState.SELECT_CARD_OPTION
                && turn.CardStateIteration == inIteration
                )
            {
                options.AddRange(OptionUtils
                    .GetReturnEnemySpyOptions(board, turn, outIteration));

                return options;
            }

            return options;
        }
    }
}
