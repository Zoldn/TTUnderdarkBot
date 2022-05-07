using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using TUnderdark.Model.Cards.Obedience;

namespace TUnderdark.AI
{
    internal enum BuyType
    {
        BUY,
        FREE,
        DEVOUR,
    }
    internal class BuyStep
    {
        public Card Card { get; set; }
        public BuyType BuyType { get; set; }
        public int Mana => BuyType == BuyType.BUY ? Card.ManaCost : 0;
    }
    internal class BuyOption
    {
        public List<BuyStep> Steps { get; set; }
    }

    internal static class CardExtenstions
    {
        public static double BuyPriority(this Card card)
        {
            return 0.0d;
        }
    }

    internal class ManaManager
    {
        public static double GoodCardThreshold = 1.0d;

        public static void CalculateManaOptions(int mana, Board board, Player activePlayer)
        {

        }
    }
}
