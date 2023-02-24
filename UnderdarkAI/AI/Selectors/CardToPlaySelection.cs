using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace UnderdarkAI.AI.Selectors
{
    internal class CardToPlaySelection : IEffectSelector
    {
        public CardToPlaySelection() { }
        public Dictionary<List<IAtomicEffect>, double> GenerateOptions(Board board, Turn turn)
        {
            return turn.CardStates
                .Where(kv => kv.Value == CardState.IN_HAND)
                .ToDictionary(
                    c => new List<IAtomicEffect>(1) { new SelectedCardToPlay(c.Key) },
                    c => 10.0d
                );
        }
    }

    internal class SelectedCardToPlay : IAtomicEffect
    {
        public bool IsNewInfoRecieved => false;

        public Card? Card => SelectedCard;
        public Card SelectedCard { get; }

        public double Value { get; set; }

        public SelectedCardToPlay(Card card)
        {
            SelectedCard = card;
        }

        public void ApplyEffect(Board board, Turn turn)
        {
            turn.CardStates[SelectedCard] = CardState.NOW_PLAYING;
            turn.State = SelectionState.SELECT_CARD_OPTION;
        }

        public void PrintEffect()
        {
            Console.WriteLine($"Now playing card {SelectedCard.Name}");
        }
    }
}
