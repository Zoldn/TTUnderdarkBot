using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using TUnderdark.TTSParser;
using UnderdarkAI.AI.CardEffects;

namespace UnderdarkAI.AI.Selectors
{
    internal class CardOptionSelection : IEffectSelector
    {
        public Dictionary<CardSpecificType, ICardEffectSelector> SpecificCardSelectors { get; private set; }
        public CardOptionSelection() 
        {
            var cardEffectSelectors = new List<ICardEffectSelector>()
            {
                new ResourceGainSelection(CardSpecificType.NOBLE, mana: 1),
                new ResourceGainSelection(CardSpecificType.SOLDIER, swords: 1),
                new ResourceGainSelection(CardSpecificType.LOLTH, mana: 2),
                new ResourceGainSelection(CardSpecificType.HOUSEGUARD, swords: 2),
            };

            SpecificCardSelectors = cardEffectSelectors
                .ToDictionary(e => e.Card.SpecificType);
        }

        public Dictionary<List<IAtomicEffect>, double> GenerateOptions(Board board, Turn turn)
        {
            if (turn.ActiveCard is null)
            {
                throw new NullReferenceException();
            }

            if (SpecificCardSelectors.TryGetValue(turn.ActiveCard.SpecificType, out var selector))
            {
                return selector.GenerateOptions(board, turn);
            }
            else
            {
                Console.WriteLine($"I don't know how to play this card yet: {turn.ActiveCard}");
                return new Dictionary<List<IAtomicEffect>, double>()
                {
                    { new List<IAtomicEffect>(1) { new DiscardCurrentEffect() }, 1.0d }
                };
            }
        }
    }

    internal class ResourceGainSelection : IEffectSelector, ICardEffectSelector
    {
        public Card Card { get; }
        public int Mana { get; set; }
        public int Swords { get; set; }
        public ResourceGainSelection(CardSpecificType cardSpecificType, int mana = 0, int swords = 0)
        {
            Card = CardMapper.SpecificTypeCardMakers[cardSpecificType].Clone();
            Mana = mana;
            Swords = swords;
        }
        public Dictionary<List<IAtomicEffect>, double> GenerateOptions(Board board, Turn turn)
        {
            return new Dictionary<List<IAtomicEffect>, double>()
            {
                {
                    new List<IAtomicEffect>(1)
                    {
                        new ResourceGainEffect(Card.SpecificType, Mana, Swords)
                    }, 
                    1.0d 
                },
            };
        }
    }
}
