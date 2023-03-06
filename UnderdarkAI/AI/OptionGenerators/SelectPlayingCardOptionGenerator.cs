using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators;
using UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators.Demons;
using UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators.Dragons;
using UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators.Drow;
using UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators.Elementals;
using UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators.Undead;
using UnderdarkAI.AI.PlayableOptions;

namespace UnderdarkAI.AI.OptionGenerators
{
    /// <summary>
    /// Выбирает опцию на карте
    /// </summary>
    internal class SelectPlayingCardOptionGenerator : OptionGenerator
    {
        public Dictionary<CardSpecificType, OptionGenerator> SpecificCardSelectors { get; private set; }
        public SelectPlayingCardOptionGenerator()
        {
            SpecificCardSelectors = new()
            {
                { CardSpecificType.NOBLE, new ResourceGainOptionSelector(mana: 1) },
                { CardSpecificType.SOLDIER, new ResourceGainOptionSelector(swords: 1) },
                { CardSpecificType.LOLTH, new ResourceGainOptionSelector(mana: 2) },
                { CardSpecificType.HOUSEGUARD, new ResourceGainOptionSelector(swords: 2) },
                #region Drow
                { CardSpecificType.ADVOCATE, new AdvocateOptionGenerator() },
                { CardSpecificType.DROW_NEGOTIATOR, new DrowNegotiatorOptionGenerator() },
                { CardSpecificType.CHOSEN_OF_LOLTH, new ChosenOfLolthOptionGenetator() },
                { CardSpecificType.COUNCIL_MEMBER, new CouncilMemberOptionGenetator() },
                { CardSpecificType.MATRON_MOTHER, new MatronMotherOptionGenetator() },
                { CardSpecificType.SPY_MASTER, new SpyMasterOptionGenetator() },
                { CardSpecificType.INFILTRATOR, new InfiltratorOptionGenetator() },
                { CardSpecificType.MASTER_OF_SORCERE, new MasterOfSorcereOptionGenetator() },
                { CardSpecificType.INFORMATION_BROCKER, new InformationBrockerOptionGenetator() },
                { CardSpecificType.SPELL_SPINNER, new SpellSpinnerOptionGenetator() },
                { CardSpecificType.ADVANCE_SCOUT, new AdvanceScoutOptionGenetator() },
                { CardSpecificType.MERCENARY_SQUAD, new MercenarySquadOptionGenetator() },
                { CardSpecificType.UNDERDARK_RANGER, new UnderdarkRangerOptionGenetator() },
                { CardSpecificType.MASTER_OF_MELEE_MAGTHERE, new MasterOfMeleeMagthereOptionGenerator() },
                { CardSpecificType.WEAPONMASTER, new WeaponMasterOptionGenerator() },
                { CardSpecificType.BLACKGUARD, new BlackguardOptionGenerator() },
                { CardSpecificType.BOUNTY_HUNTER, new BountyHunterOptionGenerator() },
                { CardSpecificType.DEATHBLADE, new DeathbladeOptionGenetator() },
                { CardSpecificType.DOPPELGANGER, new DoppelgangerOptionGenetator() },
                { CardSpecificType.INQUISITOR, new InquisitorOptionGenerator() },
                #endregion
                #region Dragons
                { CardSpecificType.BLUE_DRAGON, new BlueDragonOptionGenerator() },
                { CardSpecificType.BLUE_WYRMLING, new BlueWyrmlingOptionGenerator() },
                { CardSpecificType.CLERIC_OF_LAOGZED, new ClericOfLaogzedOptionGenerator() },
                { CardSpecificType.WYRMSPEAKER, new WyrmSpeakerOptionGenerator() },
                { CardSpecificType.CULT_FANATIC, new CultFanaticOptionGenerator() },
                { CardSpecificType.ENCHANTER_OF_THAY, new EnchanterOfThayOptionGenerator() },
                { CardSpecificType.WATCHER_OF_THAY, new WatcherOfThayOptionGenerator() },
                { CardSpecificType.GREEN_DRAGON, new GreenDragonOptionGenerator() },
                { CardSpecificType.GREEN_WYRMLING, new GreenWyrmlingOptionGenerator() },
                { CardSpecificType.RATH_MODAR, new RathModarOptionGenerator() },
                { CardSpecificType.BLACK_DRAGON, new BlackDragonOptionGenetator() },
                { CardSpecificType.BLACK_WYRMLING, new BlackWyrmlingOptionGenetator() },
                { CardSpecificType.KOBOLD, new KoboldOptionGenerator() },
                { CardSpecificType.WHITE_DRAGON, new WhiteDragonOptionGenetator() },
                { CardSpecificType.WHITE_WYRMLING, new WhiteWyrmlingOptionGenetator() },
                { CardSpecificType.DRAGON_CULTIST, new DragonCultistOptionGenerator() },
                { CardSpecificType.DRAGONCLAW, new DragonclawOptionGenetator() },
                { CardSpecificType.RED_DRAGON, new RedDragonOptionGenetator() },
                { CardSpecificType.RED_WYRMLING, new RedWyrmlingOptionGenerator() },
                { CardSpecificType.SEVERIN_SILRAJIN, new SeverinSilrajinOptionGenerator() },
                #endregion
                #region Undead
                { CardSpecificType.CULTIST_OF_MYRKUL, new CultistOfMyrkulOptionGenerator() },
                { CardSpecificType.HIGH_PRIEST_OF_MYRKUL, new HighPriestOfMyrkulOptionGenerator() },
                { CardSpecificType.NECROMANCER, new NecromancerOptionGenerator() },
                { CardSpecificType.VAMPIRE_SPAWN, new VampireSpawnOptionGenerator() },
                { CardSpecificType.VAMPIRE, new VampireOptionGenerator() },
                { CardSpecificType.BANSHEE, new BansheeOptionGenetator() },
                { CardSpecificType.CONJURER, new ConjurerOptionGenetator() },
                { CardSpecificType.GHOST, new GhostOptionGenetator() },
                { CardSpecificType.LICH, new LichOptionGenetator() },
                { CardSpecificType.WRAITH, new WraithOptionGenetator() },
                { CardSpecificType.MINOTUAR_SKELETON, new MinotaurSkeletonOptionGenetator() },
                { CardSpecificType.MUMMY_LORD, new MummyLordOptionGenetator() },
                { CardSpecificType.OGRE_ZOMBIE, new OgreZombieOptionGenerator() },
                { CardSpecificType.RAVENOUS_ZOMBIE, new RavenousZombieOptionGenetator() },
                { CardSpecificType.SKELETAL_HORDE, new SkeletalHordeOptionGenetator() },
                { CardSpecificType.CARRION_CRAWLER, new CarrionCrawlerOptionGenerator() },
                { CardSpecificType.DEATH_KNIGHT, new DeathKnightOptionGenetator() },
                { CardSpecificType.FLESH_GOLEM, new FleshGolemOptionGenetator() },
                { CardSpecificType.REVENANT, new RevenantOptionGenetator() },
                { CardSpecificType.WIGHT, new WightOptionGenetator() },
                #endregion
                #region Elementals
                { CardSpecificType.BLACK_EARTH_CULTIST, new BlackEarthCultistOptionGenerator() },
                { CardSpecificType.EARTH_ELEMENTAL_MYRMIDON, new EarthElementalMyrmidonOptionGenerator() },
                { CardSpecificType.EARTH_ELEMENTAL, new EarthElementalOptionGenerator() },
                { CardSpecificType.MARLOS_URNRAYLE, new MarlosUrnrayleOptionGenerator() },
                { CardSpecificType.OGREMOCH, new OgremochOptionGenerator() },
                { CardSpecificType.AERISI_KALINOTH, new AerisiKalinothOptionGenerator() },
                { CardSpecificType.AIR_ELEMENTAL_MYRMIDON, new AirElementalMyrmidonOptionGenerator() },
                { CardSpecificType.AIR_ELEMENTAL, new AirElementalOptionGenerator() },
                { CardSpecificType.HOWLING_HATRED_CULTIST, new HowlingHatredCultistOptionGenerator() },
                { CardSpecificType.YANCBIN, new YinCBinOptionGenerator() },
                { CardSpecificType.CRUSHING_WAVE_CULTIST, new CrushingWaveCultistOptionGenerator() },
                { CardSpecificType.GAR_SHATTERKEEL, new GarShatterkeelOptionGenetator() },
                { CardSpecificType.OLHYDRA, new OlhydraOptionGenetator() },
                { CardSpecificType.WATER_ELEMENTAL_MYRMIDON, new WaterElementalMyrmidonOptionGenerator() },
                { CardSpecificType.WATER_ELEMENTAL, new WaterElementalOptionGenerator() },
                { CardSpecificType.ETERNAL_FLAME_CULTIST, new EternalFlameCultistOptionGenerator() },
                { CardSpecificType.FIRE_ELEMENTAL_MYRMIDON, new FireElementalMyrmidonOptionGenerator() },
                { CardSpecificType.FIRE_ELEMENTAL, new FireElementalOptionGenerator() },
                { CardSpecificType.IMIX, new ImyxOptionGenerator() },
                { CardSpecificType.VANIFER, new VanyferOptionGenetator() },
                #endregion
                #region Demons
                { CardSpecificType.INSANE_OUTCAST, new InsaneOutcastOptionGenerator() },
                { CardSpecificType.HEZROU, new HezrouOptionGenetator() },
                { CardSpecificType.MYCONID_ADULT, new MyconidAdultOptionGenetator() },
                { CardSpecificType.MICONYD_SOVEREIGN, new MyconidSovereignOptionGenetator() },
                { CardSpecificType.NAFLESHNEE, new NaflshneeOptionGenetator() },
                { CardSpecificType.ZUGGTMOY, new ZuggtmoyOptionGenetator() },
                { CardSpecificType.GRAZZT, new GrazztOptionGenetator() },
                { CardSpecificType.JACKALWERE, new JackalWereOptionGenerator() },
                { CardSpecificType.NIGHT_HAG, new NightHagOptionGenetator() },
                { CardSpecificType.SUCCUBUS, new SuccubusOptionGenerator() },
                { CardSpecificType.VROCK, new VrockOptionGenerator() },
                #endregion
            };
        }
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            if (turn.ActiveCard is null)
            {
                throw new NullReferenceException();
            }

            if (SpecificCardSelectors.TryGetValue(turn.ActiveCard.Value, out var generator))
            {
                return generator.GeneratePlayableOptions(board, turn);
            }
            else
            {
                throw new NullReferenceException();
            }
        }
    }
}
