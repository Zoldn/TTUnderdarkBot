using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUnderdark.Model.CardActions
{
    internal class DrawCardAction : CardAction
    {
        protected int _drawCard;
        public override int DrawCard => _drawCard;
        public override bool IsSimpleAction => false;
        public DrawCardAction(int drawCards = 1)
        {
            _drawCard = drawCards;
        }
    }
}
