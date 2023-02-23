using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUnderdark.Model.Cards.Elementals
{
    /*
    internal class AirElemental : Card
    {
        public AirElemental()
        {

        }
        public override int ManaCost => 3;
        public override int VP => 1;
        public override int PromoteVP => 2;
        public override CardType CardType => CardType.GUILE;
        public override string Name => "Air Elemental";
        public override string ShortName => "AEL";
        public override Race Race => Race.ELEMENTAL;
    }

    internal class CrushingWaveCultist : Card
    {
        public CrushingWaveCultist()
        {

        }
        public override int ManaCost => 3;
        public override int VP => 1;
        public override int PromoteVP => 4;
        public override CardType CardType => CardType.CONQUEST;
        public override string Name => "Crushing Wave Cultist";
        public override string ShortName => "CWC";
        public override Race Race => Race.HUMAN;
    }

    internal class FireElemental : Card
    {
        public FireElemental()
        {

        }
        public override int ManaCost => 3;
        public override int VP => 1;
        public override int PromoteVP => 3;
        public override CardType CardType => CardType.MALICE;
        public override string Name => "Fire Elemental";
        public override string ShortName => "FEL";
        public override Race Race => Race.ELEMENTAL;
    }
    internal class EternalFlameCultist : Card
    {
        public EternalFlameCultist()
        {

        }
        public override int ManaCost => 4;
        public override int VP => 2;
        public override int PromoteVP => 4;
        public override CardType CardType => CardType.MALICE;
        public override string Name => "Eternal Flame Cultist";
        public override string ShortName => "EFC";
        public override Race Race => Race.HUMAN;
    }
    internal class WaterElemental : Card
    {
        public WaterElemental()
        {

        }
        public override int ManaCost => 2;
        public override int VP => 1;
        public override int PromoteVP => 2;
        public override CardType CardType => CardType.CONQUEST;
        public override string Name => "Water Elemental";
        public override string ShortName => "WEL";
        public override Race Race => Race.ELEMENTAL;
    }
    internal class BlackEarthCultist : Card
    {
        public BlackEarthCultist()
        {

        }
        public override int ManaCost => 2;
        public override int VP => 1;
        public override int PromoteVP => 2;
        public override CardType CardType => CardType.AMBITION;
        public override string Name => "Black Earth Cultist";
        public override string ShortName => "BEC";
        public override Race Race => Race.HUMAN;
    }
    internal class HowlingHatredCultist : Card
    {
        public HowlingHatredCultist()
        {

        }
        public override int ManaCost => 3;
        public override int VP => 1;
        public override int PromoteVP => 3;
        public override CardType CardType => CardType.GUILE;
        public override string Name => "Howling Hatred Cultist";
        public override string ShortName => "HHC";
        public override Race Race => Race.HUMAN;
    }
    internal class WaterElementalMyrmidon : Card
    {
        public WaterElementalMyrmidon()
        {

        }
        public override int ManaCost => 4;
        public override int VP => 2;
        public override int PromoteVP => 4;
        public override CardType CardType => CardType.CONQUEST;
        public override string Name => "Water Elemental Myrmidon";
        public override string ShortName => "WEM";
        public override Race Race => Race.ELEMENTAL;
    }
    internal class GarShatterkeel : Card
    {
        public GarShatterkeel()
        {

        }
        public override int ManaCost => 5;
        public override int VP => 2;
        public override int PromoteVP => 5;
        public override CardType CardType => CardType.CONQUEST;
        public override string Name => "Gar Shatterkeel";
        public override string ShortName => "GSH";
        public override Race Race => Race.HUMAN;
    }
    internal class AerisiKalinoth : Card
    {
        public AerisiKalinoth()
        {

        }
        public override int ManaCost => 5;
        public override int VP => 2;
        public override int PromoteVP => 5;
        public override CardType CardType => CardType.GUILE;
        public override string Name => "Aerisi Kalinoth";
        public override string ShortName => "AEK";
        public override Race Race => Race.ELF;
    }
    internal class MarlosUrnrayle : Card
    {
        public MarlosUrnrayle()
        {

        }
        public override int ManaCost => 5;
        public override int VP => 2;
        public override int PromoteVP => 5;
        public override CardType CardType => CardType.AMBITION;
        public override string Name => "Marlos Urnrayle";
        public override string ShortName => "WUR";
        public override Race Race => Race.MEDUSA;
    }
    internal class Vanifer : Card
    {
        public Vanifer()
        {

        }
        public override int ManaCost => 5;
        public override int VP => 2;
        public override int PromoteVP => 5;
        public override CardType CardType => CardType.MALICE;
        public override string Name => "Vanifer";
        public override string ShortName => "VAN";
        public override Race Race => Race.TIEFLING;
    }
    internal class FireElementalMyrmidon : Card
    {
        public FireElementalMyrmidon()
        {

        }
        public override int ManaCost => 4;
        public override int VP => 1;
        public override int PromoteVP => 3;
        public override CardType CardType => CardType.MALICE;
        public override string Name => "Fire Elemental Myrmidon";
        public override string ShortName => "FEM";
        public override Race Race => Race.ELEMENTAL;
    }
    internal class EarthElemental : Card
    {
        public EarthElemental()
        {

        }
        public override int ManaCost => 3;
        public override int VP => 1;
        public override int PromoteVP => 3;
        public override CardType CardType => CardType.AMBITION;
        public override string Name => "Earth Elemental";
        public override string ShortName => "EEL";
        public override Race Race => Race.ELEMENTAL;
    }
    internal class AirElementalMyrmidon : Card
    {
        public AirElementalMyrmidon()
        {

        }
        public override int ManaCost => 4;
        public override int VP => 1;
        public override int PromoteVP => 3;
        public override CardType CardType => CardType.GUILE;
        public override string Name => "Air Elemental Myrmidon";
        public override string ShortName => "AEM";
        public override Race Race => Race.ELEMENTAL;
    }
    internal class EarthElementalMyrmidon : Card
    {
        public EarthElementalMyrmidon()
        {

        }
        public override int ManaCost => 4;
        public override int VP => 2;
        public override int PromoteVP => 4;
        public override CardType CardType => CardType.AMBITION;
        public override string Name => "Earth Elemental Myrmidon";
        public override string ShortName => "EEM";
        public override Race Race => Race.ELEMENTAL;
    }
    internal class Ogremoch : Card
    {
        public Ogremoch()
        {

        }
        public override int ManaCost => 6;
        public override int VP => 3;
        public override int PromoteVP => 7;
        public override CardType CardType => CardType.AMBITION;
        public override string Name => "Ogremoch";
        public override string ShortName => "OGE";
        public override Race Race => Race.ELEMENTALPRINCE;
    }
    internal class Imix : Card
    {
        public Imix()
        {

        }
        public override int ManaCost => 6;
        public override int VP => 3;
        public override int PromoteVP => 6;
        public override CardType CardType => CardType.MALICE;
        public override string Name => "Imix";
        public override string ShortName => "IMX";
        public override Race Race => Race.ELEMENTALPRINCE;
    }
    internal class YanCBin : Card
    {
        public YanCBin()
        {

        }
        public override int ManaCost => 6;
        public override int VP => 3;
        public override int PromoteVP => 6;
        public override CardType CardType => CardType.GUILE;
        public override string Name => "Yan-C-Bin";
        public override string ShortName => "YAN";
        public override Race Race => Race.ELEMENTALPRINCE;
    }
    internal class Olhydra : Card
    {
        public Olhydra()
        {

        }
        public override int ManaCost => 6;
        public override int VP => 3;
        public override int PromoteVP => 6;
        public override CardType CardType => CardType.CONQUEST;
        public override string Name => "Olhydra";
        public override string ShortName => "HYD";
        public override Race Race => Race.ELEMENTALPRINCE;
    }
    */
}
