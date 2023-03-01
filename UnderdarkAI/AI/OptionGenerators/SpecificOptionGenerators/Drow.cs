using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using TUnderdark.TTSParser;
using UnderdarkAI.AI.PlayableOptions;
using UnderdarkAI.Utils;

namespace UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators
{
    internal class AdvocateOptionGenerator : OptionGenerator
    {
        public override SelectionState State => SelectionState.SELECT_CARD_OPTION;

        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            if (turn.State == SelectionState.SELECT_CARD_OPTION)
            {
                switch (turn.CardOption)
                {
                    case CardOption.NONE_OPTION:
                        options.Add(new CardOptionASelection() { Weight = 1.0d });
                        options.Add(new CardOptionBSelection() { Weight = 1.0d });
                        break;
                    case CardOption.OPTION_A:
                        options.Add(new ResourceGainOption(mana: 2) 
                        { 
                            Weight = 1.0d ,
                            WillMakeCurrentCardPlayed = true
                        });
                        break;
                    case CardOption.OPTION_B:
                        options.Add(new EnablePromoteEndTurnOption(CardSpecificType.ADVOCATE) 
                        { 
                            Weight = 1.0d ,
                            WillMakeCurrentCardPlayed = true
                        });
                        break;
                }
            }

            if (turn.State == SelectionState.SELECT_END_TURN_CARD_OPTION)
            {
                options.AddRange(
                    OptionUtils.GetPromoteAnotherCardPlayedThisTurnInTheEndOptions(turn, CardSpecificType.ADVOCATE)
                    );
            }
            
            return options;
        }
    }

    internal class DrowNegotiatorOptionGenerator : OptionGenerator
    {
        public override SelectionState State => SelectionState.SELECT_CARD_OPTION;

        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            switch (turn.State)
            {
                case SelectionState.SELECT_CARD_OPTION:
                    switch (turn.CardStateIteration)
                    {
                        case 0:
                            if (board.Players[turn.Color].InnerCircle.Count >= 3)
                            {
                                options.Add(new ResourceGainOption(mana: 3) { 
                                    Weight = 1.0d,
                                    NextCardIteration = 1,
                                    NextState = SelectionState.SELECT_CARD_OPTION,
                                });
                            }
                            else
                            {
                                options.Add(new DoNothingOption()
                                {
                                    Weight = 1.0d,
                                    NextCardIteration = 1,
                                    NextState = SelectionState.SELECT_CARD_OPTION,
                                });
                            }
                            break;
                        case 1:
                            options.Add(new EnablePromoteEndTurnOption(CardSpecificType.DROW_NEGOTIATOR) 
                            { 
                                Weight = 1.0d,
                                WillMakeCurrentCardPlayed = true,
                            });
                            break;
                        default:
                            throw new IndexOutOfRangeException();
                    }
                    break;
                case SelectionState.SELECT_END_TURN_CARD_OPTION:
                    options.AddRange(
                        OptionUtils.GetPromoteAnotherCardPlayedThisTurnInTheEndOptions(turn, CardSpecificType.DROW_NEGOTIATOR)
                        );
                    break;
                default:
                    throw new IndexOutOfRangeException();
            }

            return options;
        }
    }

    internal class ChosenOfLolthOptionGenetator : OptionGenerator
    {
        public override SelectionState State => throw new NotImplementedException();

        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            ///Выбор промоута в конце хода
            if (turn.State == SelectionState.SELECT_END_TURN_CARD_OPTION)
            {
                options.AddRange(
                    OptionUtils.GetPromoteAnotherCardPlayedThisTurnInTheEndOptions(turn, CardSpecificType.CHOSEN_OF_LOLTH)
                    );

                return options;
            }

            /// Вернуть шпиона или трупс
            options.AddRange(
                ReturnEnemyTroopOrSpyHelper.ReturnEnemyTroopOrSpyHandler(board, turn, 
                    inChoiceCardStateIteration: 0, 
                    outChoiceCardStateIteration: 1)
                );

            /// Запоминаем промоут в конце хода и завершаем карту
            if (turn.State == SelectionState.SELECT_CARD_OPTION
                && turn.CardStateIteration == 1)
            {
                options.Add(new EnablePromoteEndTurnOption(CardSpecificType.CHOSEN_OF_LOLTH)
                {
                    Weight = 1.0d,
                    WillMakeCurrentCardPlayed = true,
                });

                return options;
            }

            return options;
        }
    }

    internal class CouncilMemberOptionGenetator : OptionGenerator
    {
        public override SelectionState State => throw new NotImplementedException();

        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            throw new NotImplementedException();
        }
    }
}
