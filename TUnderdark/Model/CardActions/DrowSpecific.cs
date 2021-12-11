using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUnderdark.Model.CardActions
{
    internal class DrowNegotiatorAction : PromoteAction
    {
        public override int Mana => ActivePlayer.InnerCircle.Count >= 4 ? 3 : 0;
        public override int PromoteAnotherPlayedCardAtEndTurn => 1;
    }

    internal class InfiltratorAction : CardAction
    {
        /// <summary>
        /// Если есть другие игроки
        /// </summary>
        public override int Swords => SpyLocation.IsOtherPlayerTroops(ActivePlayer) ? 1 : 0;
        public override List<Location> PreferrableLocationsToPlaceSpy =>
            Board.Locations.Where(l => l.IsOtherPlayerTroops(ActivePlayer)).ToList();
        public override int PlaceSpy => 1;
        public override bool IsSimpleAction => false;
    }

    internal class ChosenOfLolthReturnSpy : ReturnEnemySpyAction
    {
        public override int PromoteAnotherPlayedCardAtEndTurn => 1;
    }

    internal class ChosenOfLolthReturnTroop : StaticWarAction
    {
        public override int PromoteAnotherPlayedCardAtEndTurn => 1;
        public override int ReturnTroop => 1;
    }

    internal class SpellSpinnerReturnSpy : ReturnOwnSpyAction
    {
        public override bool IsSimpleAction => false;
        public override int SupplantInSpyLocation => 1;
        public override List<Location> PreferrableLocationsToRemoveSpy =>
            Board.Locations.Where(l => l.Spies[ActivePlayer.Color] && l.IsOtherTroops(ActivePlayer)).ToList();
    }

    internal class CouncilMemberAction : StaticWarAction
    {
        public override int PromoteAnotherPlayedCardAtEndTurn => 1;
        public override int MoveTroop => 2;
        public override bool IsSimpleAction => false;
    }

    internal class MatronMotherAction : PromoteAction
    {
        public override int PromoteFromDiscard => 1;
        public override bool IsMoveDeckToDiscard => true;
        public override bool IsSimpleAction => false;
    }
}
