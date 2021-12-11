using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model.CardActions;

namespace TUnderdark.Model.Cards.Dragons
{
    internal class WhiteWyrmling : Card
    {
        public WhiteWyrmling()
        {
            Actions.Add(new WhiteWyrmlingAction());
            Actions.Add(new StaticWarAction(deploy: 2));
        }
        public override int ManaCost => 2;
        public override int VP => 1;
        public override int PromoteVP => 3;
        public override CardType CardType => CardType.CONQUEST;
        public override string Name => "White Wyrmling";
        public override string ShortName => "WWY";
        public override Race Race => Race.DRAGON;
    }

    internal class Kobold : Card
    {
        public Kobold()
        {
            Actions.Add(new StaticWarAction(assasinate: 1));
            Actions.Add(new StaticWarAction(deploy: 1));
        }
        public override int ManaCost => 1;
        public override int VP => 1;
        public override int PromoteVP => 2;
        public override CardType CardType => CardType.CONQUEST;
        public override string Name => "Kobold";
        public override string ShortName => "KBL";
        public override Race Race => Race.KOBOLD;
    }

    internal class GreenWyrmling : Card
    {
        public GreenWyrmling()
        {
            Actions.Add(new GreenWyrmlingAction());
        }
        public override int ManaCost => 4;
        public override int VP => 2;
        public override int PromoteVP => 4;
        public override CardType CardType => CardType.GUILE;
        public override string Name => "Green Wyrmling";
        public override string ShortName => "GWY";
        public override Race Race => Race.DRAGON;
    }

    internal class DragonCultist : Card
    {
        public DragonCultist()
        {
            Actions.Add(new StaticGetManaSwordAction(mana: 2));
            Actions.Add(new StaticGetManaSwordAction(swords: 2));
        }
        public override int ManaCost => 3;
        public override int VP => 1;
        public override int PromoteVP => 4;
        public override CardType CardType => CardType.MALICE;
        public override string Name => "Dragon Cultist";
        public override string ShortName => "DCU";
        public override Race Race => Race.HUMAN;
    }

    internal class EnchanterOfThay : Card
    {
        public EnchanterOfThay()
        {
            Actions.Add(new PlaceSpyAction());
            Actions.Add(new ReturnOwnSpyAction(swords: 4));
        }
        public override int ManaCost => 4;
        public override int VP => 1;
        public override int PromoteVP => 3;
        public override CardType CardType => CardType.GUILE;
        public override string Name => "Enchanter of Thay";
        public override string ShortName => "ETH";
        public override Race Race => Race.HUMAN;
    }

    internal class RedWyrmling : Card
    {
        public RedWyrmling()
        {
            Actions.Add(new StaticGetManaSwordAction(mana: 2, swords: 2));
        }
        public override int ManaCost => 5;
        public override int VP => 3;
        public override int PromoteVP => 5;
        public override CardType CardType => CardType.MALICE;
        public override string Name => "Red Wyrmling";
        public override string ShortName => "RWY";
        public override Race Race => Race.DRAGON;
    }

    internal class WatcherOfThay : Card
    {
        public WatcherOfThay()
        {
            Actions.Add(new PlaceSpyAction());
            Actions.Add(new ReturnOwnSpyAction(mana: 3));
        }
        public override int ManaCost => 3;
        public override int VP => 2;
        public override int PromoteVP => 3;
        public override CardType CardType => CardType.GUILE;
        public override string Name => "Watcher of Thay";
        public override string ShortName => "WTH";
        public override Race Race => Race.HUMAN;
    }

    internal class BlueWyrmling : Card
    {
        public BlueWyrmling()
        {
            Actions.Add(new BlueWyrmlingReturnSpy());
            Actions.Add(new BlueWyrmlingReturnTroop());
        }
        public override int ManaCost => 5;
        public override int VP => 2;
        public override int PromoteVP => 4;
        public override CardType CardType => CardType.AMBITION;
        public override string Name => "Blue Wyrmling";
        public override string ShortName => "BWY";
        public override Race Race => Race.DRAGON;
    }

    internal class Dragonclaw : Card
    {
        public Dragonclaw()
        {
            Actions.Add(new DragonclawAction());
        }
        public override int ManaCost => 4;
        public override int VP => 1;
        public override int PromoteVP => 3;
        public override CardType CardType => CardType.MALICE;
        public override string Name => "Dragonclaw";
        public override string ShortName => "DCL";
        public override Race Race => Race.HUMAN;
    }

    internal class BlackWyrmling : Card
    {
        public BlackWyrmling()
        {
            Actions.Add(new BlackWyrmlingAction());
        }
        public override int ManaCost => 3;
        public override int VP => 1;
        public override int PromoteVP => 4;
        public override CardType CardType => CardType.CONQUEST;
        public override string Name => "Black Wyrmling";
        public override string ShortName => "KWY";
        public override Race Race => Race.DRAGON;
    }

    internal class Wyrmspeaker : Card
    {
        public Wyrmspeaker()
        {
            Actions.Add(new WyrmspeakerAction());
        }
        public override int ManaCost => 3;
        public override int VP => 1;
        public override int PromoteVP => 3;
        public override CardType CardType => CardType.AMBITION;
        public override string Name => "Wyrmspeaker";
        public override string ShortName => "WSP";
        public override Race Race => Race.DWARF;
    }

    internal class CultFanatic : Card
    {
        public CultFanatic()
        {
            Actions.Add(new CultFanaticAction());
        }
        public override int ManaCost => 3;
        public override int VP => 1;
        public override int PromoteVP => 4;
        public override CardType CardType => CardType.AMBITION;
        public override string Name => "Cult Fanatic";
        public override string ShortName => "CFA";
        public override Race Race => Race.HALFDRAGON;
    }

    internal class ClericOfLaogzed : Card
    {
        public ClericOfLaogzed()
        {
            Actions.Add(new ClericOfLaogzedAction());
        }
        public override int ManaCost => 4;
        public override int VP => 2;
        public override int PromoteVP => 4;
        public override CardType CardType => CardType.AMBITION;
        public override string Name => "Cleric of Laogzed";
        public override string ShortName => "COL";
        public override Race Race => Race.TROGLODYTE;
    }

    internal class WhiteDragon : Card
    {
        public WhiteDragon()
        {
            Actions.Add(new WhiteDragonAction());
        }
        public override int ManaCost => 6;
        public override int VP => 2;
        public override int PromoteVP => 5;
        public override CardType CardType => CardType.CONQUEST;
        public override string Name => "White Dragon";
        public override string ShortName => "WDR";
        public override Race Race => Race.DRAGON;
    }

    internal class BlackDragon : Card
    {
        public BlackDragon()
        {
            Actions.Add(new BlackDragonAction());
        }
        public override int ManaCost => 7;
        public override int VP => 3;
        public override int PromoteVP => 7;
        public override CardType CardType => CardType.CONQUEST;
        public override string Name => "Black Dragon";
        public override string ShortName => "KDR";
        public override Race Race => Race.DRAGON;
    }

    internal class BlueDragon : Card
    {
        public BlueDragon()
        {
            Actions.Add(new BlueDragonAction());
        }
        public override int ManaCost => 8;
        public override int VP => 4;
        public override int PromoteVP => 8;
        public override CardType CardType => CardType.AMBITION;
        public override string Name => "Blue Dragon";
        public override string ShortName => "BDR";
        public override Race Race => Race.DRAGON;
    }

    internal class GreenDragon : Card
    {
        public GreenDragon()
        {
            Actions.Add(new GreenDragonPlaceSpyAction());
            Actions.Add(new GreenDragonReturnSpyAction());
        }
        public override int ManaCost => 7;
        public override int VP => 3;
        public override int PromoteVP => 7;
        public override CardType CardType => CardType.GUILE;
        public override string Name => "Green Dragon";
        public override string ShortName => "GDR";
        public override Race Race => Race.DRAGON;
    }

    internal class RathModar : Card
    {
        public RathModar()
        {
            Actions.Add(new RathModarAction());
        }
        public override int ManaCost => 6;
        public override int VP => 2;
        public override int PromoteVP => 5;
        public override CardType CardType => CardType.GUILE;
        public override string Name => "Rath Modar";
        public override string ShortName => "RMD";
        public override Race Race => Race.HUMAN;
    }

    internal class SeverinSilrajin : Card
    {
        public SeverinSilrajin()
        {
            Actions.Add(new StaticGetManaSwordAction(swords: 5));
        }
        public override int ManaCost => 7;
        public override int VP => 4;
        public override int PromoteVP => 8;
        public override CardType CardType => CardType.MALICE;
        public override string Name => "Severin Silrajin";
        public override string ShortName => "SSJ";
        public override Race Race => Race.HUMAN;
    }

    internal class RedDragon : Card
    {
        public RedDragon()
        {
            Actions.Add(new RedDragonAction());
        }
        public override int ManaCost => 8;
        public override int VP => 4;
        public override int PromoteVP => 8;
        public override CardType CardType => CardType.MALICE;
        public override string Name => "Red Dragon";
        public override string ShortName => "RDR";
        public override Race Race => Race.DRAGON;
    }
}
