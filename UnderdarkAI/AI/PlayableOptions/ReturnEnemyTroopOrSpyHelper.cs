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
        public static List<PlayableOption> ReturnEnemyTroopOrSpyHandler(Board board, Turn turn,
                int inChoiceCardStateIteration,
                int outChoiceCardStateIteration
            )
        {
            var options = new List<PlayableOption>();

            ///Выбор возвращать трупса или шпиона
            if (turn.State == SelectionState.SELECT_CARD_OPTION
                && turn.CardStateIteration == inChoiceCardStateIteration
                && turn.CardOption == CardOption.NONE_OPTION)
            {
                // На опцию А возвращаем трупсов
                if (OptionUtils.IsReturnableTroops(board, turn))
                {
                    options.Add(new CardOptionASelection() { Weight = 1.0d });
                }

                // На опцию Б возвращаем шпионов
                if (OptionUtils.IsReturnableSpies(board, turn))
                {
                    options.Add(new CardOptionBSelection() { Weight = 1.0d });
                }

                if (options.Count == 0)
                {
                    options.Add(new DoNothingOption()
                    {
                        Weight = 1.0d,
                        NextCardIteration = outChoiceCardStateIteration,
                    });
                }

                return options;
            }

            // На опцию А возвращаем трупсов
            if (turn.State == SelectionState.SELECT_CARD_OPTION
                && turn.CardStateIteration == inChoiceCardStateIteration
                && turn.CardOption == CardOption.OPTION_A)
            {
                options.AddRange(OptionUtils
                    .GetReturnTroopOptions(board, turn)
                    .Apply(p => {
                        p.NextCardIteration = outChoiceCardStateIteration;
                        p.NextCardOption = CardOption.NONE_OPTION;
                        p.NextState = SelectionState.SELECT_CARD_OPTION;
                    }));

                return options;
            }

            // На опцию B возвращаем шпионов
            if (turn.State == SelectionState.SELECT_CARD_OPTION
                && turn.CardStateIteration == 0
                && turn.CardOption == CardOption.OPTION_B)
            {
                options.AddRange(OptionUtils
                    .GetReturnEnemySpyOptions(board, turn)
                    .Apply(p => {
                        p.NextCardIteration = outChoiceCardStateIteration;
                        p.NextCardOption = CardOption.NONE_OPTION;
                        p.NextState = SelectionState.SELECT_CARD_OPTION;
                    }));

                return options;
            }

            return options;
        }
    }
}
