using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace UnderdarkAI.Context.ContextElements
{
    public class CardStats
    {
        public CardSpecificType CardSpecificType { get; set; }
        public CardType CardType { get; set; }
        public Race Race { get; set; }
        public string Name { get; set; }
        public int ManaCost { get; set; }
        public int VP { get; set; }
        public int PromoteVP { get; set; }

        public double BaseValuePerTurn { get; set; }
        public double WhiteDisplacement { get; set; }
        public double ColorDisplacement { get; set; }
        public double PromoteSpeed { get; set; }
        public double DevourSpeed { get; set; }
        public double DrawSpeed { get; set; }

        public CardStats()
        {
            Name = "";
        }
        public override string ToString()
        {
            return $"Card {Name} ({VP}/{PromoteVP})";
        }
    }
}
