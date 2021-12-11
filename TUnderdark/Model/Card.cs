using System;
using System.Collections.Generic;
using System.Text;
using TUnderdark.AI;

namespace TUnderdark.Model
{
    internal abstract class Card
    {
        public abstract Race Race { get; }
        public abstract int ManaCost { get; }
        public abstract int VP { get; }
        public abstract int PromoteVP { get; }
        public abstract CardType CardType { get; }
        public abstract string Name { get; }
        public abstract string ShortName { get; }
        public virtual bool IsPurchasable => true;
        public List<CardAction> Actions { get; }
        public Card()
        {
            Actions = new();
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
