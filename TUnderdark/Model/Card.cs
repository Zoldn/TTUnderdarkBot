using System;
using System.Collections.Generic;
using System.Text;
using TUnderdark.AI;

namespace TUnderdark.Model
{
    public sealed class Card
    {
        public CardSpecificType SpecificType { get; init; }
        public Race Race { get; init; }
        public int ManaCost { get; init; }
        public int VP { get; init; }
        public int PromoteVP { get; init; }
        public CardType CardType { get; init; }
        public string Name { get; init; }
        public Card()
        {

        }

        public override string ToString()
        {
            return $"{Name}";
        }

        public Card Clone()
        {
            return new Card()
            {
                CardType = CardType,
                SpecificType = SpecificType,
                Race = Race,
                ManaCost = ManaCost,
                VP = VP,
                PromoteVP = PromoteVP,
                Name = Name,
            };
        }
    }
}
