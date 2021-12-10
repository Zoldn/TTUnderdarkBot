using System;
using System.Collections.Generic;
using System.Text;

namespace TUnderdark.Model
{
    internal class CardAction
    {

    }

    internal class Card
    {
        public int ManaCost { get; set; }
        public int VP { get; set; }
        public int PromoteVP { get; set; }
        public CardType CardType { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public List<CardAction> Actions { get; set; }
        public Card()
        {
            Actions = new();
        }
    }
}
