using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUnderdark.Model.CardActions
{
    internal class WhiteWyrmlingAction : StaticWarAction
    {
        public override int Deploy => 2;
        public override int DevoureMarket => 1;
    }

    internal class GreenWyrmlingAction : CardAction
    {
        /// <summary>
        /// Если есть другие игроки
        /// </summary>
        public override int Mana => SpyLocation.IsOtherPlayerTroops(ActivePlayer) ? 2 : 0;
        public override List<Location> PreferrableLocationsToPlaceSpy =>
            Board.Locations.Where(l => l.IsOtherPlayerTroops(ActivePlayer)).ToList();
        public override int PlaceSpy => 1;
        public override bool IsSimpleAction => false;
    }

    internal class BlueWyrmlingReturnSpy : ReturnEnemySpyAction
    {
        public override int Mana => 3;
    }

    internal class BlueWyrmlingReturnTroop : StaticWarAction
    {
        public override int Mana => 3;
        public override int ReturnTroop => 1;
    }

    internal class DragonclawAction : StaticWarAction
    {
        public override int Swords => ActivePlayer.TrophyHall
            .Where(kv => kv.Key != ActivePlayer.Color)
            .Sum(kv => kv.Value) >= 5 ? 2 : 0;
        public override int Assasinate => 1;
    }

    internal class BlackWyrmlingAction : StaticWarAction
    {
        public override int Mana => 1;
        public override int AssasinateWhite => 1;
    }

    internal class WyrmspeakerAction : PromoteAction
    {
        public override int Mana => 1;
        public override int PromoteAnotherPlayedCardAtEndTurn => 1;
    }

    internal class CultFanaticAction : StaticGetManaSwordAction
    {
        public override int Mana => 2;
        public override int DevoureMarket => 1;
        public override bool IsSimpleAction => false;
    }

    internal class ClericOfLaogzedAction : PromoteAction
    {
        public override int MoveTroop => 1;
        public override int PromoteAnotherPlayedCardAtEndTurn => 1;
        public override bool IsSimpleAction => false;
    }

    internal class WhiteDragonAction : StaticWarAction
    {
        public override int Deploy => 3;
        public override int BonusVP =>
            (int)Math.Floor(Board.Locations.Where(l => l.IsControl(ActivePlayer.Color)).Count() / 2.0d);
    }
    internal class BlackDragonAction : StaticWarAction
    {
        public override int SupplantWhiteAnywhere => 1;
        public override int BonusVP =>
            (int)Math.Floor(ActivePlayer.TrophyHall[Color.WHITE] / 3.0d);
    }
    internal class BlueDragonAction : PromoteAction
    {
        public override int PromoteAnotherPlayedCardAtEndTurn => 2;
        public override int BonusVP =>
            (int)Math.Floor(ActivePlayer.InnerCircle.Count / 3.0d);
    }

    internal class GreenDragonPlaceSpyAction : PlaceSpyAction
    {
        public override int SupplantInSpyLocation => 1;
    }
    internal class GreenDragonReturnSpyAction : PromoteAction
    {
        public override int SupplantInSpyLocation => 1;
        public override int BonusVP => Board.Locations
            .Where(l => l.BonusMana > 0 && l.IsControl(ActivePlayer.Color))
            .Count();
    }

    internal class RathModarAction : PlaceSpyAction
    {
        public override int DrawCard => 2;
    }

    internal class RedDragonAction : ReturnEnemySpyAction
    {
        public override int Supplant => 1;
        public override int BonusVP => Board.Locations
            .Where(l => l.IsTotalControl(ActivePlayer.Color))
            .Count();
    }
    
}
