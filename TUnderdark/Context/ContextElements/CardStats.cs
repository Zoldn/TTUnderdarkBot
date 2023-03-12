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
