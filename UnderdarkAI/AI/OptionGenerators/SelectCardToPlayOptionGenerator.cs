using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using TUnderdark.TTSParser;

namespace UnderdarkAI.AI.OptionGenerators
{
    internal class SelectCardToPlayOptionGenerator : OptionGenerator
    {
        public override SelectionState State => SelectionState.SELECT_CARD;

        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var ret = new List<PlayableOption>();

            var cardsToPlay = turn.CardStates
                .Where(s => s.State == CardState.IN_HAND)
                .Select(s => s.SpecificType)
                .Distinct()
                .ToList();

            foreach (var card in cardsToPlay)
            {
                ret.Add(new SelectCardToPlayGameOption(card)
                { 
                    Weight = 10.0d,
                });
            }

            return ret;
        }
    }

    internal class SelectCardToPlayGameOption : PlayableOption
    {
        public CardSpecificType SpecificType { get; }
        public SelectCardToPlayGameOption(CardSpecificType cardSpecificType)
        {
            SpecificType = cardSpecificType;
        }
        public override int MinVerbosity => 0;
        public override void ApplyOption(Board board, Turn turn)
        {
            turn.CardStates
                .First(s => s.SpecificType == SpecificType 
                    && s.State == CardState.IN_HAND)
                .State = CardState.NOW_PLAYING;
        }

        public override SelectionState GetNextState()
        {
            return SelectionState.SELECT_CARD_OPTION;
        }

        public override string GetOptionText()
        {
            return $"Playing card {CardMapper.SpecificTypeCardMakers[SpecificType].Name}";
        }
    }
}
