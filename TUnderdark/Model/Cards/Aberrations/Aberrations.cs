using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUnderdark.Model.Cards.Aberrations
{
    /*
    internal class Gauth : Card
    {
        public Gauth()
        {

        }
        public override int ManaCost => 3;
        public override int VP => 2;
        public override int PromoteVP => 3;
        public override CardType CardType => CardType.MALICE;
        public override string Name => "Gauth";
        public override string ShortName => "GTH";
        public override Race Race => Race.ABERRATION;
    }

    internal class BrainwashedSlave : Card
    {
        public BrainwashedSlave()
        {

        }
        public override int ManaCost => 4;
        public override int VP => 2;
        public override int PromoteVP => 3;
        public override CardType CardType => CardType.GUILE;
        public override string Name => "Brainwashed Slave";
        public override string ShortName => "BRS";
        public override Race Race => Race.ABERRATION;
    }
    internal class Spectator : Card
    {
        public Spectator()
        {

        }
        public override int ManaCost => 4;
        public override int VP => 2;
        public override int PromoteVP => 3;
        public override CardType CardType => CardType.MALICE;
        public override string Name => "Spectator";
        public override string ShortName => "SPC";
        public override Race Race => Race.ABERRATION;
    }
    internal class Grimlock : Card
    {
        public Grimlock()
        {

        }
        public override int ManaCost => 1;
        public override int VP => 0;
        public override int PromoteVP => 1;
        public override CardType CardType => CardType.CONQUEST;
        public override string Name => "Grimlock";
        public override string ShortName => "GRM";
        public override Race Race => Race.GRIMLOCK;
    }
    internal class MindWitness : Card
    {
        public MindWitness()
        {

        }
        public override int ManaCost => 3;
        public override int VP => 1;
        public override int PromoteVP => 3;
        public override CardType CardType => CardType.MALICE;
        public override string Name => "Mind Witness";
        public override string ShortName => "MWT";
        public override Race Race => Race.ABERRATION;
    }
    internal class CraniumRats : Card
    {
        public CraniumRats()
        {

        }
        public override int ManaCost => 2;
        public override int VP => 1;
        public override int PromoteVP => 3;
        public override CardType CardType => CardType.CONQUEST;
        public override string Name => "Cranium Rats";
        public override string ShortName => "RAT";
        public override Race Race => Race.BEAST;
    }
    internal class IntellectDevourer : Card
    {
        public IntellectDevourer()
        {

        }
        public override int ManaCost => 4;
        public override int VP => 1;
        public override int PromoteVP => 3;
        public override CardType CardType => CardType.AMBITION;
        public override string Name => "Intellect Devourer";
        public override string ShortName => "INT";
        public override Race Race => Race.ABERRATION;
    }
    internal class Nothic : Card
    {
        public Nothic()
        {

        }
        public override int ManaCost => 3;
        public override int VP => 1;
        public override int PromoteVP => 3;
        public override CardType CardType => CardType.GUILE;
        public override string Name => "Nothic";
        public override string ShortName => "NTC";
        public override Race Race => Race.ABERRATION;
    }
    internal class Cloaker : Card
    {
        public Cloaker()
        {

        }
        public override int ManaCost => 2;
        public override int VP => 1;
        public override int PromoteVP => 3;
        public override CardType CardType => CardType.GUILE;
        public override string Name => "Cloaker";
        public override string ShortName => "CLK";
        public override Race Race => Race.ABERRATION;
    }
    internal class Ambassador : Card
    {
        public Ambassador()
        {

        }
        public override int ManaCost => 3;
        public override int VP => 1;
        public override int PromoteVP => 3;
        public override CardType CardType => CardType.AMBITION;
        public override string Name => "Ambassador";
        public override string ShortName => "AMB";
        public override Race Race => Race.ILLITHID;
    }
    internal class Chuul : Card
    {
        public Chuul()
        {

        }
        public override int ManaCost => 3;
        public override int VP => 1;
        public override int PromoteVP => 4;
        public override CardType CardType => CardType.GUILE;
        public override string Name => "Chuul";
        public override string ShortName => "CHU";
        public override Race Race => Race.ABERRATION;
    }

    internal class Quaggoth : Card
    {
        public Quaggoth()
        {

        }
        public override int ManaCost => 5;
        public override int VP => 2;
        public override int PromoteVP => 4;
        public override CardType CardType => CardType.CONQUEST;
        public override string Name => "Quaggoth";
        public override string ShortName => "QUA";
        public override Race Race => Race.QUAGGOTH;
    }
    internal class UmberHulk : Card
    {
        public UmberHulk()
        {

        }
        public override int ManaCost => 4;
        public override int VP => 2;
        public override int PromoteVP => 4;
        public override CardType CardType => CardType.CONQUEST;
        public override string Name => "Umber Hulk";
        public override string ShortName => "HLK";
        public override Race Race => Race.MONSTROSITY;
    }
    internal class Puppeteer : Card
    {
        public Puppeteer()
        {

        }
        public override int ManaCost => 5;
        public override int VP => 2;
        public override int PromoteVP => 6;
        public override CardType CardType => CardType.AMBITION;
        public override string Name => "Puppeteer";
        public override string ShortName => "PPP";
        public override Race Race => Race.ILLITHID;
    }
    internal class Aboleth : Card
    {
        public Aboleth()
        {

        }
        public override int ManaCost => 7;
        public override int VP => 4;
        public override int PromoteVP => 7;
        public override CardType CardType => CardType.GUILE;
        public override string Name => "Aboleth";
        public override string ShortName => "ABO";
        public override Race Race => Race.ABERRATION;
    }
    internal class DeathTyrant : Card
    {
        public DeathTyrant()
        {

        }
        public override int ManaCost => 7;
        public override int VP => 3;
        public override int PromoteVP => 6;
        public override CardType CardType => CardType.MALICE;
        public override string Name => "Death Tyrant";
        public override string ShortName => "TYR";
        public override Race Race => Race.ABERRATION;
    }
    internal class Neogi : Card
    {
        public Neogi()
        {

        }
        public override int ManaCost => 7;
        public override int VP => 4;
        public override int PromoteVP => 8;
        public override CardType CardType => CardType.CONQUEST;
        public override string Name => "Neogi";
        public override string ShortName => "NEO";
        public override Race Race => Race.ABERRATION;
    }
    internal class Ulitharid : Card
    {
        public Ulitharid()
        {

        }
        public override int ManaCost => 6;
        public override int VP => 3;
        public override int PromoteVP => 6;
        public override CardType CardType => CardType.AMBITION;
        public override string Name => "Ulitharid";
        public override string ShortName => "ULI";
        public override Race Race => Race.ILLITHID;
    }
    internal class Beholder : Card
    {
        public Beholder()
        {

        }
        public override int ManaCost => 5;
        public override int VP => 3;
        public override int PromoteVP => 6;
        public override CardType CardType => CardType.MALICE;
        public override string Name => "Beholder";
        public override string ShortName => "BEH";
        public override Race Race => Race.ABERRATION;
    }
    internal class ElderBrain : Card
    {
        public ElderBrain()
        {

        }
        public override int ManaCost => 7;
        public override int VP => 4;
        public override int PromoteVP => 9;
        public override CardType CardType => CardType.AMBITION;
        public override string Name => "ElderBrain";
        public override string ShortName => "ELD";
        public override Race Race => Race.ABERRATION;
    }
    */
}
