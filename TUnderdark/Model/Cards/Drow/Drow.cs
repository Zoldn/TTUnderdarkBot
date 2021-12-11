using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model.CardActions;

namespace TUnderdark.Model.Cards.Drow
{
    internal class BountyHunter : Card
    {
        public BountyHunter()
        {
            Actions.Add(new StaticGetManaSwordAction(0, 3));
        }
        public override int ManaCost => 4;
        public override int VP => 2;
        public override int PromoteVP => 4;
        public override CardType CardType => CardType.MALICE;
        public override string Name => "Bounty Hunter";
        public override string ShortName => "BHT";
        public override Race Race => Race.DROW;
    }

    internal class DrowNegotiator : Card
    {
        public DrowNegotiator()
        {
            Actions.Add(new DrowNegotiatorAction());
        }
        public override int ManaCost => 3;
        public override int VP => 1;
        public override int PromoteVP => 2;
        public override CardType CardType => CardType.AMBITION;
        public override string Name => "Drow Negotiator";
        public override string ShortName => "DNG";
        public override Race Race => Race.DROW;
    }

    internal class Infiltrator : Card
    {
        public Infiltrator()
        {
            Actions.Add(new InfiltratorAction());
        }
        public override int ManaCost => 2;
        public override int VP => 1;
        public override int PromoteVP => 2;
        public override CardType CardType => CardType.GUILE;
        public override string Name => "Infiltrator";
        public override string ShortName => "INF";
        public override Race Race => Race.DROW;
    }

    internal class Doppelganger : Card
    {
        public Doppelganger()
        {
            Actions.Add(new StaticWarAction(supplant: 1));
        }
        public override int ManaCost => 5;
        public override int VP => 3;
        public override int PromoteVP => 5;
        public override CardType CardType => CardType.MALICE;
        public override string Name => "Doppelganger";
        public override string ShortName => "DOP";
        public override Race Race => Race.DOPPELGANGER;
    }

    internal class Advocate : Card
    {
        public Advocate()
        {
            Actions.Add(new PromoteAction(otherPlayedCard: 1));
            Actions.Add(new StaticGetManaSwordAction(mana: 2));
        }
        public override int ManaCost => 2;
        public override int VP => 1;
        public override int PromoteVP => 2;
        public override CardType CardType => CardType.AMBITION;
        public override string Name => "Advocate";
        public override string ShortName => "ADV";
        public override Race Race => Race.DROW;
    }

    internal class SpyMaster : Card
    {
        public SpyMaster()
        {
            Actions.Add(new PlaceSpyAction());
        }
        public override int ManaCost => 2;
        public override int VP => 1;
        public override int PromoteVP => 2;
        public override CardType CardType => CardType.GUILE;
        public override string Name => "Spy Master";
        public override string ShortName => "SMS";
        public override Race Race => Race.DROW;
    }

    internal class ChosenOfLolth : Card
    {
        public ChosenOfLolth()
        {
            Actions.Add(new ChosenOfLolthReturnSpy());
            Actions.Add(new ChosenOfLolthReturnTroop());
        }
        public override int ManaCost => 4;
        public override int VP => 2;
        public override int PromoteVP => 4;
        public override CardType CardType => CardType.AMBITION;
        public override string Name => "Chosen of Lolth";
        public override string ShortName => "CHL";
        public override Race Race => Race.DROW;
    }

    internal class UnderdarkRanger : Card
    {
        public UnderdarkRanger()
        {
            Actions.Add(new StaticWarAction(assasinateWhite: 2));
        }
        public override int ManaCost => 4;
        public override int VP => 2;
        public override int PromoteVP => 4;
        public override CardType CardType => CardType.CONQUEST;
        public override string Name => "Underdark Ranger";
        public override string ShortName => "URG";
        public override Race Race => Race.DROW;
    }

    internal class AdvancedScout : Card
    {
        public AdvancedScout()
        {
            Actions.Add(new StaticWarAction(supplantWhite: 1));
        }
        public override int ManaCost => 3;
        public override int VP => 1;
        public override int PromoteVP => 3;
        public override CardType CardType => CardType.CONQUEST;
        public override string Name => "Advanced Scout";
        public override string ShortName => "ASC";
        public override Race Race => Race.DROW;
    }

    internal class MasterOfMeleeMagthere : Card
    {
        public MasterOfMeleeMagthere()
        {
            Actions.Add(new StaticWarAction(supplantWhiteAnywhere: 1));
            Actions.Add(new StaticWarAction(deploy: 4));
        }
        public override int ManaCost => 5;
        public override int VP => 2;
        public override int PromoteVP => 5;
        public override CardType CardType => CardType.CONQUEST;
        public override string Name => "Master of Melee-Magthere";
        public override string ShortName => "MMM";
        public override Race Race => Race.DROW;
    }

    internal class InformationBrocker : Card
    {
        public InformationBrocker()
        {
            Actions.Add(new PlaceSpyAction());
            Actions.Add(new DrawCardAction(drawCards: 3));
        }
        public override int ManaCost => 5;
        public override int VP => 2;
        public override int PromoteVP => 5;
        public override CardType CardType => CardType.GUILE;
        public override string Name => "Information Brocker";
        public override string ShortName => "INF";
        public override Race Race => Race.DROW;
    }

    internal class MercenarySquad : Card
    {
        public MercenarySquad()
        {
            Actions.Add(new StaticWarAction(deploy: 3));
        }
        public override int ManaCost => 3;
        public override int VP => 1;
        public override int PromoteVP => 4;
        public override CardType CardType => CardType.CONQUEST;
        public override string Name => "Mercenary Squad";
        public override string ShortName => "MSQ";
        public override Race Race => Race.DROW;
    }

    internal class SpellSpinner : Card
    {
        public SpellSpinner()
        {
            Actions.Add(new PlaceSpyAction());
            Actions.Add(new SpellSpinnerReturnSpy());
        }
        public override int ManaCost => 3;
        public override int VP => 1;
        public override int PromoteVP => 3;
        public override CardType CardType => CardType.GUILE;
        public override string Name => "Spell Spinner";
        public override string ShortName => "SSP";
        public override Race Race => Race.DROW;
    }

    internal class Blackguard : Card
    {
        public Blackguard()
        {
            Actions.Add(new StaticWarAction(assasinate: 1));
            Actions.Add(new StaticGetManaSwordAction(swords: 2));
        }
        public override int ManaCost => 3;
        public override int VP => 1;
        public override int PromoteVP => 3;
        public override CardType CardType => CardType.MALICE;
        public override string Name => "Blackguard";
        public override string ShortName => "BKG";
        public override Race Race => Race.DROW;
    }

    internal class WeaponMaster : Card
    {
        public WeaponMaster()
        {
            Actions.Add(new StaticWarAction(assasinateWhite: 3));
            Actions.Add(new StaticWarAction(deploy: 3));
            Actions.Add(new StaticWarAction(supplantWhite: 1, deploy: 1));
            Actions.Add(new StaticWarAction(supplantWhite: 1, assasinateWhite: 1));
            Actions.Add(new StaticWarAction(deploy: 2, assasinateWhite: 1));
        }
        public override int ManaCost => 6;
        public override int VP => 3;
        public override int PromoteVP => 6;
        public override CardType CardType => CardType.CONQUEST;
        public override string Name => "Weapon Master";
        public override string ShortName => "WMS";
        public override Race Race => Race.DROW;
    }

    internal class Deathblade : Card
    {
        public Deathblade()
        {
            Actions.Add(new StaticWarAction(assasinate: 2));
        }
        public override int ManaCost => 6;
        public override int VP => 3;
        public override int PromoteVP => 6;
        public override CardType CardType => CardType.MALICE;
        public override string Name => "Deathblade";
        public override string ShortName => "DBL";
        public override Race Race => Race.DROW;
    }

    internal class CouncilMember : Card
    {
        public CouncilMember()
        {
            Actions.Add(new CouncilMemberAction());
        }
        public override int ManaCost => 6;
        public override int VP => 3;
        public override int PromoteVP => 6;
        public override CardType CardType => CardType.AMBITION;
        public override string Name => "Council Member";
        public override string ShortName => "CMM";
        public override Race Race => Race.DROW;
    }

    internal class Inquisitor : Card
    {
        public Inquisitor()
        {
            Actions.Add(new StaticWarAction(assasinate: 1));
            Actions.Add(new StaticGetManaSwordAction(mana: 2));
        }
        public override int ManaCost => 3;
        public override int VP => 2;
        public override int PromoteVP => 4;
        public override CardType CardType => CardType.MALICE;
        public override string Name => "Inquisitor";
        public override string ShortName => "INQ";
        public override Race Race => Race.DROW;
    }

    internal class MasterOfSorcere : Card
    {
        public MasterOfSorcere()
        {
            Actions.Add(new PlaceSpyAction(spies: 2));
            Actions.Add(new ReturnOwnSpyAction(swords: 4));
        }
        public override int ManaCost => 5;
        public override int VP => 2;
        public override int PromoteVP => 5;
        public override CardType CardType => CardType.GUILE;
        public override string Name => "Masters of Sorcere";
        public override string ShortName => "MOS";
        public override Race Race => Race.DROW;
    }

    internal class MatronMother : Card
    {
        public MatronMother()
        {
            Actions.Add(new MatronMotherAction());
        }
        public override int ManaCost => 6;
        public override int VP => 3;
        public override int PromoteVP => 6;
        public override CardType CardType => CardType.AMBITION;
        public override string Name => "Matron Mother";
        public override string ShortName => "MAM";
        public override Race Race => Race.DROW;
    }
}
