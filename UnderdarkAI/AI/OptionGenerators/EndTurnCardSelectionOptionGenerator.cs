using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using TUnderdark.TTSParser;

namespace UnderdarkAI.AI.OptionGenerators
{
    /// <summary>
    /// Генерирует опции конца хода
    /// </summary>
    internal class EndTurnCardSelectionOptionGenerator : OptionGenerator
    {
        public override SelectionState State => SelectionState.SELECT_CARD_END_TURN;

        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var cardsToPlay = turn.CardStates
                .Where(s => s.EndTurnState == CardState.IN_HAND)
                .Select(s => s.SpecificType)
                .Distinct()
                .ToList();

            if (cardsToPlay.Count == 0)
            {
                return new List<PlayableOption>(1) { new NoToPlayInTheEndOption() { Weight = 1.0d } };
            }

            var ret = new List<PlayableOption>();

            foreach (var cardSpecificType in cardsToPlay)
            {
                ret.Add(new PlayCardInTheEndOption(cardSpecificType) { Weight = 1.0d });
            }

            return ret;
        }
    }

    internal class PlayCardInTheEndOption : PlayableOption
    {
        public CardSpecificType SpecificType { get; }
        public PlayCardInTheEndOption(CardSpecificType specificType)
        {
            SpecificType = specificType;
        }

        public override int MinVerbosity => 10;

        public override void ApplyOption(Board board, Turn turn)
        {
            turn.CardStates
                .First(s => s.SpecificType == SpecificType && s.EndTurnState == CardState.IN_HAND)
                .EndTurnState = CardState.NOW_PLAYING;
        }

        public override void UpdateTurnState(Turn turn)
        {
            turn.State = SelectionState.SELECT_END_TURN_CARD_OPTION;
        }

        public override string GetOptionText()
        {
            return $"Playing end of turn effect of {CardMapper.SpecificTypeCardMakers[SpecificType].Name}";
        }
    }

    internal class NoToPlayInTheEndOption : PlayableOption
    {
        public override int MinVerbosity => 10;

        public override void ApplyOption(Board board, Turn turn)
        {
            
        }

        public override void UpdateTurnState(Turn turn)
        {
            turn.State = SelectionState.FINISH_SELECTION;
        }

        public override string GetOptionText()
        {
            return $"Nothing more to play in the end of turn";
        }
    }
}
