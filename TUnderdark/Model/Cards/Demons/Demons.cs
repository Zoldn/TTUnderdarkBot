using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUnderdark.Model.Cards.Demons
{
    /*
    internal class Ghoul : Card
    {
        public Ghoul()
        {

        }
        public override int ManaCost => 4;
        public override int VP => 2;
        public override int PromoteVP => 4;
        public override CardType CardType => CardType.MALICE;
        public override string Name => "Ghoul";
        public override string ShortName => "GHL";
        public override Race Race => Race.UNDEAD;
    }

    internal class Jackalwere : Card
    {
        public Jackalwere()
        {

        }
        public override int ManaCost => 4;
        public override int VP => 2;
        public override int PromoteVP => 4;
        public override CardType CardType => CardType.GUILE;
        public override string Name => "Jackalwere";
        public override string ShortName => "JWH";
        public override Race Race => Race.SHAPECHANGER;
    }

    internal class Derro : Card
    {
        public Derro()
        {

        }
        public override int ManaCost => 3;
        public override int VP => 1;
        public override int PromoteVP => 4;
        public override CardType CardType => CardType.CONQUEST;
        public override string Name => "Derro";
        public override string ShortName => "DRR";
        public override Race Race => Race.DERRO;
    }

    internal class Marilith : Card
    {
        public Marilith()
        {

        }
        public override int ManaCost => 6;
        public override int VP => 2;
        public override int PromoteVP => 5;
        public override CardType CardType => CardType.MALICE;
        public override string Name => "Marilith";
        public override string ShortName => "MRL";
        public override Race Race => Race.FIEND;
    }

    internal class Nafleshnee : Card
    {
        public Nafleshnee()
        {

        }
        public override int ManaCost => 5;
        public override int VP => 2;
        public override int PromoteVP => 5;
        public override CardType CardType => CardType.AMBITION;
        public override string Name => "Nafleshnee";
        public override string ShortName => "NFL";
        public override Race Race => Race.FIEND;
    }

    internal class MiconydSovereign : Card
    {
        public MiconydSovereign()
        {

        }
        public override int ManaCost => 4;
        public override int VP => 2;
        public override int PromoteVP => 5;
        public override CardType CardType => CardType.AMBITION;
        public override string Name => "Miconyd Sovereign";
        public override string ShortName => "MSV";
        public override Race Race => Race.MICONYD;
    }

    internal class Ettin : Card
    {
        public Ettin()
        {

        }
        public override int ManaCost => 4;
        public override int VP => 1;
        public override int PromoteVP => 2;
        public override CardType CardType => CardType.CONQUEST;
        public override string Name => "Ettin";
        public override string ShortName => "ETT";
        public override Race Race => Race.GIANT;
    }

    internal class Succubus : Card
    {
        public Succubus()
        {

        }
        public override int ManaCost => 5;
        public override int VP => 2;
        public override int PromoteVP => 5;
        public override CardType CardType => CardType.GUILE;
        public override string Name => "Succubus";
        public override string ShortName => "SUC";
        public override Race Race => Race.FIEND;
    }

    internal class Hezrou : Card
    {
        public Hezrou()
        {

        }
        public override int ManaCost => 3;
        public override int VP => 1;
        public override int PromoteVP => 3;
        public override CardType CardType => CardType.AMBITION;
        public override string Name => "Hezrou";
        public override string ShortName => "HEZ";
        public override Race Race => Race.FIEND;
    }

    internal class MindFlayer : Card
    {
        public MindFlayer()
        {

        }
        public override int ManaCost => 3;
        public override int VP => 1;
        public override int PromoteVP => 3;
        public override CardType CardType => CardType.MALICE;
        public override string Name => "Mind Flayer";
        public override string ShortName => "MFL";
        public override Race Race => Race.ILLITHID;
    }

    internal class GibberingMouther : Card
    {
        public GibberingMouther()
        {

        }
        public override int ManaCost => 2;
        public override int VP => 1;
        public override int PromoteVP => 3;
        public override CardType CardType => CardType.CONQUEST;
        public override string Name => "Gibbering Mouther";
        public override string ShortName => "GMT";
        public override Race Race => Race.ABERRATION;
    }

    internal class MyconidAdult : Card
    {
        public MyconidAdult()
        {

        }
        public override int ManaCost => 3;
        public override int VP => 1;
        public override int PromoteVP => 3;
        public override CardType CardType => CardType.AMBITION;
        public override string Name => "Myconid Adult";
        public override string ShortName => "MAD";
        public override Race Race => Race.MICONYD;
    }

    internal class NightHag : Card
    {
        public NightHag()
        {

        }
        public override int ManaCost => 3;
        public override int VP => 1;
        public override int PromoteVP => 3;
        public override CardType CardType => CardType.GUILE;
        public override string Name => "Night Hag";
        public override string ShortName => "NHG";
        public override Race Race => Race.FIEND;
    }

    internal class Orcus : Card
    {
        public Orcus()
        {

        }
        public override int ManaCost => 8;
        public override int VP => 5;
        public override int PromoteVP => 10;
        public override CardType CardType => CardType.MALICE;
        public override string Name => "Orcus";
        public override string ShortName => "ORC";
        public override Race Race => Race.FIEND;
    }
    internal class Zuggtmoy : Card
    {
        public Zuggtmoy()
        {

        }
        public override int ManaCost => 6;
        public override int VP => 3;
        public override int PromoteVP => 6;
        public override CardType CardType => CardType.AMBITION;
        public override string Name => "Zuggtmoy";
        public override string ShortName => "ZUG";
        public override Race Race => Race.FIEND;
    }

    internal class Balor : Card
    {
        public Balor()
        {

        }
        public override int ManaCost => 6;
        public override int VP => 3;
        public override int PromoteVP => 6;
        public override CardType CardType => CardType.CONQUEST;
        public override string Name => "Balor";
        public override string ShortName => "BLR";
        public override Race Race => Race.FIEND;
    }

    internal class Vrock : Card
    {
        public Vrock()
        {

        }
        public override int ManaCost => 5;
        public override int VP => 2;
        public override int PromoteVP => 4;
        public override CardType CardType => CardType.GUILE;
        public override string Name => "Vrock";
        public override string ShortName => "VRC";
        public override Race Race => Race.FIEND;
    }

    internal class Grazzt : Card
    {
        public Grazzt()
        {

        }
        public override int ManaCost => 6;
        public override int VP => 2;
        public override int PromoteVP => 5;
        public override CardType CardType => CardType.GUILE;
        public override string Name => "Graz'zt";
        public override string ShortName => "GRZ";
        public override Race Race => Race.FIEND;
    }
    internal class Glabrezu : Card
    {
        public Glabrezu()
        {

        }
        public override int ManaCost => 5;
        public override int VP => 2;
        public override int PromoteVP => 4;
        public override CardType CardType => CardType.MALICE;
        public override string Name => "Glabrezu";
        public override string ShortName => "GLB";
        public override Race Race => Race.FIEND;
    }

    internal class Demongorgon : Card
    {
        public Demongorgon()
        {

        }
        public override int ManaCost => 8;
        public override int VP => 5;
        public override int PromoteVP => 10;
        public override CardType CardType => CardType.CONQUEST;
        public override string Name => "Demongorgon";
        public override string ShortName => "DGR";
        public override Race Race => Race.FIEND;
    }

    internal class InsaneOutcast : Card
    {
        public InsaneOutcast()
        {

        }
        public override int ManaCost => 0;
        public override bool IsPurchasable => false;
        public override int VP => -1;
        public override int PromoteVP => 0;
        public override CardType CardType => CardType.INSANE;
        public override string Name => "Insane Outcast";
        public override string ShortName => "INS";
        public override Race Race => Race.DROW;
    }

    */
}
