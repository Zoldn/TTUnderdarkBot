﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using TUnderdark.TTSParser;
using UnderdarkAI.AI.PlayableOptions;

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
                        options.Add(new ResourceGainOption(mana: 2) { Weight = 1.0d });
                        break;
                    case CardOption.OPTION_B:
                        options.Add(new EnablePromoteEndTurnOption(CardSpecificType.ADVOCATE) { Weight = 1.0d });
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

            if (turn.State == SelectionState.SELECT_CARD_OPTION)
            {
                if (turn.CardStateIteration == 0)
                {
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
                        options.Add(new ResourceGainOption(mana: 0)
                        {
                            Weight = 1.0d,
                            NextCardIteration = 1,
                            NextState = SelectionState.SELECT_CARD_OPTION,
                        });
                    }
                }
                else if (turn.CardStateIteration == 1)
                {
                    options.Add(new EnablePromoteEndTurnOption(CardSpecificType.DROW_NEGOTIATOR) { Weight = 1.0d });
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

    //internal class DrowNegotiatorPlayableOption : PlayableOption
    //{
    //    public override int MinVerbosity => 0;

    //    public override void ApplyOption(Board board, Turn turn)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override string GetOptionText()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override void UpdateTurnState(Turn turn)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
