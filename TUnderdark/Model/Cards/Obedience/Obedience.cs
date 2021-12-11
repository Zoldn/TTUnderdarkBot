using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model.CardActions;

namespace TUnderdark.Model.Cards.Obedience
{
    internal class Noble : Card
    {
        public Noble() 
        {
            Actions.Add(new StaticGetManaSwordAction(1, 0));
        }
        public override int ManaCost => 0;
        public override bool IsPurchasable => false;
        public override int VP => 0;
        public override int PromoteVP => 1;
        public override CardType CardType => CardType.OBEDIENCE;
        public override string Name => "Noble";
        public override string ShortName => "NBL";
        public override Race Race => Race.DROW;
    }

    internal class Soldier : Card
    {
        public Soldier()
        {
            Actions.Add(new StaticGetManaSwordAction(0, 1));
        }
        public override int ManaCost => 0;
        public override bool IsPurchasable => false;
        public override int VP => 0;
        public override int PromoteVP => 1;
        public override CardType CardType => CardType.OBEDIENCE;
        public override string Name => "Soldier";
        public override string ShortName => "SLD";
        public override Race Race => Race.DROW;
    }

    internal class PriestessOfLolth : Card
    {
        public PriestessOfLolth()
        {
            Actions.Add(new StaticGetManaSwordAction(2, 0));
        }
        public override int ManaCost => 2;
        public override int VP => 1;
        public override int PromoteVP => 2;
        public override CardType CardType => CardType.OBEDIENCE;
        public override string Name => "Priestess of Lolth";
        public override string ShortName => "LOL";
        public override Race Race => Race.DROW;
    }

    internal class Houseguard : Card
    {
        public Houseguard()
        {
            Actions.Add(new StaticGetManaSwordAction(0, 2));
        }
        public override int ManaCost => 3;
        public override int VP => 1;
        public override int PromoteVP => 3;
        public override CardType CardType => CardType.OBEDIENCE;
        public override string Name => "Houseguard";
        public override string ShortName => "HGD";
        public override Race Race => Race.DROW;
    }
}
