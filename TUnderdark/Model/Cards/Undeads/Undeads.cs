using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUnderdark.Model.Cards.Undeads
{
    /*
    internal class OgreZombie : Card
    {
        public OgreZombie()
        {

        }
        public override int ManaCost => 4;
        public override int VP => 2;
        public override int PromoteVP => 4;
        public override CardType CardType => CardType.CONQUEST;
        public override string Name => "Ogre Zombie";
        public override string ShortName => "OGR";
        public override Race Race => Race.UNDEAD;
    }

    internal class Conjurer : Card
    {
        public Conjurer()
        {

        }
        public override int ManaCost => 5;
        public override int VP => 2;
        public override int PromoteVP => 3;
        public override CardType CardType => CardType.GUILE;
        public override string Name => "Conjurer";
        public override string ShortName => "CNJ";
        public override Race Race => Race.HUMAN;
    }

    internal class Wight : Card
    {
        public Wight()
        {

        }
        public override int ManaCost => 3;
        public override int VP => 1;
        public override int PromoteVP => 3;
        public override CardType CardType => CardType.MALICE;
        public override string Name => "Wight";
        public override string ShortName => "WGH";
        public override Race Race => Race.UNDEAD;
    }

    internal class Banshee : Card
    {
        public Banshee()
        {

        }
        public override int ManaCost => 4;
        public override int VP => 2;
        public override int PromoteVP => 4;
        public override CardType CardType => CardType.GUILE;
        public override string Name => "Banshee";
        public override string ShortName => "BSH";
        public override Race Race => Race.UNDEAD;
    }

    internal class CultistOfMyrkul : Card
    {
        public CultistOfMyrkul()
        {

        }
        public override int ManaCost => 2;
        public override int VP => 1;
        public override int PromoteVP => 2;
        public override CardType CardType => CardType.AMBITION;
        public override string Name => "Cultis of Myrkul";
        public override string ShortName => "CMY";
        public override Race Race => Race.HUMAN;
    }

    internal class MinotuarSkeleton : Card
    {
        public MinotuarSkeleton()
        {

        }
        public override int ManaCost => 3;
        public override int VP => 1;
        public override int PromoteVP => 3;
        public override CardType CardType => CardType.CONQUEST;
        public override string Name => "Minotuar Skeleton";
        public override string ShortName => "MSK";
        public override Race Race => Race.UNDEAD;
    }

    internal class Wraith : Card
    {
        public Wraith()
        {

        }
        public override int ManaCost => 2;
        public override int VP => 0;
        public override int PromoteVP => 1;
        public override CardType CardType => CardType.GUILE;
        public override string Name => "Wraith";
        public override string ShortName => "WRT";
        public override Race Race => Race.UNDEAD;
    }

    internal class CarrionCrawler : Card
    {
        public CarrionCrawler()
        {

        }
        public override int ManaCost => 2;
        public override int VP => 0;
        public override int PromoteVP => 2;
        public override CardType CardType => CardType.MALICE;
        public override string Name => "Carrion Crawler";
        public override string ShortName => "CCR";
        public override Race Race => Race.MONSTROSITY;
    }

    internal class SkeletalHorde : Card
    {
        public SkeletalHorde()
        {

        }
        public override int ManaCost => 2;
        public override int VP => 1;
        public override int PromoteVP => 2;
        public override CardType CardType => CardType.CONQUEST;
        public override string Name => "Skeletal Horde";
        public override string ShortName => "SKH";
        public override Race Race => Race.UNDEAD;
    }

    internal class RavenousZombie : Card
    {
        public RavenousZombie()
        {

        }
        public override int ManaCost => 3;
        public override int VP => 1;
        public override int PromoteVP => 2;
        public override CardType CardType => CardType.CONQUEST;
        public override string Name => "Ravenous Zombie";
        public override string ShortName => "RZB";
        public override Race Race => Race.UNDEAD;
    }

    internal class VampireSpawn : Card
    {
        public VampireSpawn()
        {

        }
        public override int ManaCost => 2;
        public override int VP => 1;
        public override int PromoteVP => 3;
        public override CardType CardType => CardType.AMBITION;
        public override string Name => "Vampire Spawn";
        public override string ShortName => "VSP";
        public override Race Race => Race.UNDEAD;
    }

    internal class FleshGolem : Card
    {
        public FleshGolem()
        {

        }
        public override int ManaCost => 3;
        public override int VP => 1;
        public override int PromoteVP => 3;
        public override CardType CardType => CardType.MALICE;
        public override string Name => "Flesh Golem";
        public override string ShortName => "FGL";
        public override Race Race => Race.CONSTRUCT;
    }

    internal class Ghost : Card
    {
        public Ghost()
        {

        }
        public override int ManaCost => 3;
        public override int VP => 1;
        public override int PromoteVP => 3;
        public override CardType CardType => CardType.GUILE;
        public override string Name => "Ghost";
        public override string ShortName => "GHS";
        public override Race Race => Race.UNDEAD;
    }

    internal class Necromancer : Card
    {
        public Necromancer()
        {

        }
        public override int ManaCost => 5;
        public override int VP => 1;
        public override int PromoteVP => 4;
        public override CardType CardType => CardType.AMBITION;
        public override string Name => "Necromancer";
        public override string ShortName => "NCR";
        public override Race Race => Race.HUMAN;
    }

    internal class Vampire : Card
    {
        public Vampire()
        {

        }
        public override int ManaCost => 7;
        public override int VP => 4;
        public override int PromoteVP => 7;
        public override CardType CardType => CardType.AMBITION;
        public override string Name => "Vampire";
        public override string ShortName => "VMP";
        public override Race Race => Race.UNDEAD;
    }

    internal class MummyLord : Card
    {
        public MummyLord()
        {

        }
        public override int ManaCost => 6;
        public override int VP => 3;
        public override int PromoteVP => 6;
        public override CardType CardType => CardType.CONQUEST;
        public override string Name => "Mummy Lord";
        public override string ShortName => "MML";
        public override Race Race => Race.UNDEAD;
    }

    internal class DeathKnight : Card
    {
        public DeathKnight()
        {

        }
        public override int ManaCost => 6;
        public override int VP => 3;
        public override int PromoteVP => 7;
        public override CardType CardType => CardType.MALICE;
        public override string Name => "DeathKnight";
        public override string ShortName => "DKN";
        public override Race Race => Race.UNDEAD;
    }

    internal class Revenant : Card
    {
        public Revenant()
        {

        }
        public override int ManaCost => 4;
        public override int VP => 1;
        public override int PromoteVP => 4;
        public override CardType CardType => CardType.MALICE;
        public override string Name => "Revenant";
        public override string ShortName => "REV";
        public override Race Race => Race.UNDEAD;
    }

    internal class HighPriestOfMyrkul : Card
    {
        public HighPriestOfMyrkul()
        {

        }
        public override int ManaCost => 6;
        public override int VP => 3;
        public override int PromoteVP => 6;
        public override CardType CardType => CardType.AMBITION;
        public override string Name => "High Priest of Myrkul";
        public override string ShortName => "HPM";
        public override Race Race => Race.HUMAN;
    }

    internal class Lich : Card
    {
        public Lich()
        {

        }
        public override int ManaCost => 7;
        public override int VP => 4;
        public override int PromoteVP => 7;
        public override CardType CardType => CardType.GUILE;
        public override string Name => "Lich";
        public override string ShortName => "LCH";
        public override Race Race => Race.UNDEAD;
    }
    */
}
