using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUnderdark.Model.CardActions
{
    internal class PromoteAction : CardAction
    {
        protected int _promoteAnotherPlayerAtEndTurn;
        protected int _promoteFromDiscard;
        public override int PromoteAnotherPlayedCardAtEndTurn => _promoteAnotherPlayerAtEndTurn;
        public PromoteAction(int otherPlayedCard = 0, int fromDiscard = 0)
        {
            _promoteAnotherPlayerAtEndTurn = otherPlayedCard;
            _promoteFromDiscard = fromDiscard;
        }
        public override bool IsSimpleAction => true;
        public override int PromoteFromDiscard => _promoteFromDiscard;
    }
}
