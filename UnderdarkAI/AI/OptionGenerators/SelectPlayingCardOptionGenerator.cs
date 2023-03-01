﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators;

namespace UnderdarkAI.AI.OptionGenerators
{
    /// <summary>
    /// Выбирает опцию на карте
    /// </summary>
    internal class SelectPlayingCardOptionGenerator : OptionGenerator
    {
        public Dictionary<CardSpecificType, OptionGenerator> SpecificCardSelectors { get; private set; }
        public override SelectionState State => SelectionState.SELECT_CARD_OPTION;

        public SelectPlayingCardOptionGenerator()
        {
            SpecificCardSelectors = new()
            {
                { CardSpecificType.NOBLE, new ResourceGainOptionSelector(mana: 1) },
                { CardSpecificType.SOLDIER, new ResourceGainOptionSelector(swords: 1) },
                { CardSpecificType.LOLTH, new ResourceGainOptionSelector(mana: 2) },
                { CardSpecificType.HOUSEGUARD, new ResourceGainOptionSelector(swords: 2) },
#region Drow
                { CardSpecificType.ADVOCATE, new AdvocateOptionGenerator() },
                { CardSpecificType.DROW_NEGOTIATOR, new DrowNegotiatorOptionGenerator() },
                { CardSpecificType.CHOSEN_OF_LOLTH, new ChosenOfLolthOptionGenetator() },
#endregion
            };
        }
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            if (turn.ActiveCard is null)
            {
                throw new NullReferenceException();
            }

            if (SpecificCardSelectors.TryGetValue(turn.ActiveCard.Value, out var generator))
            {
                return generator.GeneratePlayableOptions(board, turn);
            }
            else
            {
                throw new NullReferenceException();
            }
        }
    }
}
