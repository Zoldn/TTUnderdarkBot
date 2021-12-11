using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace TUnderdark.AI
{
    internal class TurnVariant
    {
        public Dictionary<Card, CardAction> SelectedActions { get; set; }
        public int ManaIncome { get; set; }
        public int SwordIncome { get; set; }
        public TurnVariant()
        {

        }
    }
}
