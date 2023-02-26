using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUnderdark.Model
{
    public enum LocationId
    {
        Gauntlgrym,
        Gauntlgrym2Jhachalkhyn,
        Jhachalkhyn,
        Jhachalkhyn2Buiyrandyn,
        Buiyrandyn,
        Buiyrandyn2StoneShaft,
        StoneShaft,
        StoneShaft2ChChitl,
        ChChitl,
        ChChitl2Kanaglym,
        Kanaglym,
        Kanaglym2Skullport,
        Skullport,
        Skullport2StoneShaft,
        Skullport2Labyrinth,
        Labyrinth2Skullport,
        Labyrinth,
        Labyrinth2Buiyrandyn,
        Labyrinth2Gracklstugh,
        Gracklstugh,
        Gracklstugh2Jhachalkhyn,
        Gracklstugh2MantolDerith,
        MantolDerith,
        MantolDerith2Wormwrithings,
        Wormwrithings,
        Wormwrithings2Gauntlgrym,
        MantolDerith2Blingdenstone,
        Blingdenstone,
        MantolDerith2Blingdenfire,
        Blingdenfire,
        Blingdenfire2Menzoberranzan,
        Menzoberranzan,
        Menzoberranzan2Bridge,
        Bridge2Menzoberranzan,
        Bridge,
        Bridge2Gracklstugh,
        Gracklstugh2Bridge,
        Bridge2Araumycos,
        Araumycos,
        Araumycos2Labyrinth,
        Labyrinth2Araumycos,
        Araumycos2Eryndlyn,
        Eryndlyn,
        Eryndlyn2Kanaglym,
        Kanaglym2Tsenviilyq,
        Tsenviilyq,
        Tsenviilyq2Llacerellyn,
        Llacerellyn,
        Llacerellyn2Eryndlyn,
        Llacerellyn2ChedNasad,
        ChedNasad2Llacerellyn,
        ChedNasad,
        ChedNasad2Araumycos,
        ChedNasad2Bridge,
        ChedNasad2Legion,
        Legion2ChedNasad,
        Legion,
        Legion2Everfire,
        Everfire,
        Everfire2Bridge,
        Everfire2Menzoberranzan,
        Menzoberranzan2Everfire,
        Everfire2Chaulssin,
        Chaulssin,
        Chaulssin2Phaerlin,
        Phaerlin,
        Phaerlin2Yathchol,
        Yathchol,
        Yathchol2Legion,
        Yathchol2ChedNasad,
        Yathchol2OldYathchol,
        OldYathchol,
        OldYathchol2Phaerlin,
        OldYathchol2Ruins,
        Ruins,
        Ruins2ChedNasad,
        Ruins2SSZuraassnee,
        SSZuraassnee,
        SSZuraassnee2Llacerellyn,
        Llacerellyn2SSZuraassnee,
        Phaerlin2Everfire,
    }

    public static class BoardInitializer
    {
        private static void CreateLocations(Board board)
        {
            board.Locations.Add(new Location(LocationId.Gauntlgrym)
            {
                BonusMana = 1,
                BonusVP = 1,
                ControlVPs = 2,
                Name = "Gauntlgrym",
                Size = 3,
                IsStart = false,
                NeighboorIds = new List<LocationId>() { 
                    LocationId.Gauntlgrym2Jhachalkhyn, 
                    LocationId.Wormwrithings2Gauntlgrym
                }
            }
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Gauntlgrym2Jhachalkhyn, "Gauntlgrym2Jhachalkhyn",
                LocationId.Gauntlgrym, LocationId.Jhachalkhyn)
                );

            board.Locations.Add(new Location(LocationId.Jhachalkhyn)
            {
                BonusMana = 0,
                ControlVPs = 4,
                Name = "Jhachalkhyn",
                Size = 4,
                IsStart = false,
                NeighboorIds = new List<LocationId>() {
                    LocationId.Gauntlgrym2Jhachalkhyn,
                    LocationId.Jhachalkhyn2Buiyrandyn,
                    LocationId.Gracklstugh2Jhachalkhyn,
                }
            }
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Jhachalkhyn2Buiyrandyn, "Jhachalkhyn2Buiyrandyn",
                LocationId.Jhachalkhyn, LocationId.Buiyrandyn)
                );

            board.Locations.Add(new Location(LocationId.Buiyrandyn)
            {
                BonusMana = 0,
                ControlVPs = 3,
                Name = "Buiyrandyn",
                Size = 3,
                IsStart = true,
                NeighboorIds = new List<LocationId>() {
                    LocationId.Buiyrandyn2StoneShaft,
                    LocationId.Jhachalkhyn2Buiyrandyn,
                    LocationId.Labyrinth2Buiyrandyn,
                }
            }
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Buiyrandyn2StoneShaft, "Buiyrandyn2StoneShaft",
                LocationId.Buiyrandyn, LocationId.StoneShaft)
                );

            board.Locations.Add(new Location(LocationId.StoneShaft)
            {
                BonusMana = 0,
                ControlVPs = 4,
                Name = "StoneShaft",
                Size = 2,
                IsStart = false,
                NeighboorIds = new List<LocationId>() {
                    LocationId.Buiyrandyn2StoneShaft,
                    LocationId.StoneShaft2ChChitl,
                    LocationId.Skullport2StoneShaft,
                }
            }
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.StoneShaft2ChChitl, "StoneShaft2ChChitl",
                LocationId.StoneShaft, LocationId.ChChitl)
                );

            board.Locations.Add(new Location(LocationId.ChChitl)
            {
                BonusMana = 1,
                BonusVP = 1,
                ControlVPs = 2,
                Name = "ChChitl",
                Size = 3,
                IsStart = false,
                NeighboorIds = new List<LocationId>() {
                    LocationId.StoneShaft2ChChitl,
                    LocationId.ChChitl2Kanaglym,
                }
            }
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.ChChitl2Kanaglym, "ChChitl2Kanaglym",
                LocationId.ChChitl, LocationId.Kanaglym)
                );

            board.Locations.Add(new Location(LocationId.Kanaglym)
            {
                BonusMana = 0,
                ControlVPs = 3,
                Name = "Kanaglym",
                Size = 3,
                IsStart = false,
                NeighboorIds = new List<LocationId>() {
                    LocationId.Kanaglym2Skullport,
                    LocationId.Kanaglym2Tsenviilyq,
                    LocationId.Eryndlyn2Kanaglym,
                    LocationId.ChChitl2Kanaglym,
                }
            }
               );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Kanaglym2Skullport, "Kanaglym2Skullport",
                LocationId.Kanaglym, LocationId.Skullport
                )
                );

            board.Locations.Add(new Location(LocationId.Skullport)
            {
                BonusMana = 0,
                ControlVPs = 4,
                Name = "Skullport",
                Size = 5,
                IsStart = true,
                NeighboorIds = new List<LocationId>() {
                    LocationId.Skullport2Labyrinth,
                    LocationId.Skullport2StoneShaft,
                    LocationId.Kanaglym2Skullport,
                }
            }
               );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Skullport2StoneShaft, "Skullport2StoneShaft",
                LocationId.Skullport, LocationId.StoneShaft)
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Skullport2Labyrinth, "Skullport2Labyrinth",
                LocationId.Skullport, LocationId.Labyrinth2Skullport)
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Labyrinth2Skullport, "Labyrinth2Skullport",
                LocationId.Labyrinth, LocationId.Skullport2Labyrinth)
                );

            board.Locations.Add(new Location(LocationId.Labyrinth)
            {
                BonusMana = 0,
                ControlVPs = 3,
                Name = "Labyrinth",
                Size = 3,
                IsStart = false,
                NeighboorIds = new List<LocationId>() {
                    LocationId.Labyrinth2Buiyrandyn,
                    LocationId.Labyrinth2Gracklstugh,
                    LocationId.Labyrinth2Skullport,
                    LocationId.Labyrinth2Araumycos,
                }
            }
               );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Labyrinth2Buiyrandyn, "Labyrinth2Buiyrandyn",
                LocationId.Labyrinth, LocationId.Buiyrandyn)
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Labyrinth2Araumycos, "Labyrinth2Araumycos",
                LocationId.Labyrinth, LocationId.Araumycos2Labyrinth)
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Araumycos2Labyrinth, "Araumycos2Labyrinth",
                LocationId.Araumycos, LocationId.Labyrinth2Araumycos)
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Labyrinth2Gracklstugh, "Labyrinth2Gracklstugh",
                LocationId.Labyrinth, LocationId.Gracklstugh)
                );

            board.Locations.Add(new Location(LocationId.Gracklstugh)
            {
                BonusMana = 0,
                ControlVPs = 3,
                Name = "Gracklstugh",
                Size = 4,
                IsStart = false,
                NeighboorIds = new List<LocationId>() {
                    LocationId.Gracklstugh2Bridge,
                    LocationId.Gracklstugh2Jhachalkhyn,
                    LocationId.Gracklstugh2MantolDerith,
                    LocationId.Labyrinth2Gracklstugh,
                }
            }
               );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Gracklstugh2Jhachalkhyn, "Gracklstugh2Jhachalkhyn",
                LocationId.Gracklstugh, LocationId.Jhachalkhyn)
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Gracklstugh2MantolDerith, "Gracklstugh2MantolDerith",
                LocationId.Gracklstugh, LocationId.MantolDerith)
                );

            board.Locations.Add(new Location(LocationId.MantolDerith)
            {
                BonusMana = 0,
                ControlVPs = 4,
                Name = "MantolDerith",
                Size = 5,
                IsStart = true,
                NeighboorIds = new List<LocationId>() {
                    LocationId.MantolDerith2Blingdenfire,
                    LocationId.MantolDerith2Blingdenstone,
                    LocationId.MantolDerith2Wormwrithings,
                    LocationId.Gracklstugh2MantolDerith,
                }
            }
               );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.MantolDerith2Wormwrithings, "MantolDerith2Wormwrithings",
                LocationId.MantolDerith, LocationId.Wormwrithings)
                );

            board.Locations.Add(new Location(LocationId.Wormwrithings)
            {
                BonusMana = 0,
                ControlVPs = 3,
                Name = "Wormwrithings",
                Size = 3,
                IsStart = false,
                NeighboorIds = new List<LocationId>() {
                    LocationId.Wormwrithings2Gauntlgrym,
                    LocationId.MantolDerith2Wormwrithings,
                }
            }
              );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.MantolDerith2Blingdenstone, "MantolDerith2Blingdenstone",
                LocationId.MantolDerith, LocationId.Blingdenstone)
                );

            board.Locations.Add(new Location(LocationId.Blingdenstone)
            {
                BonusMana = 0,
                ControlVPs = 4,
                Name = "Blingdenstone",
                Size = 2,
                IsStart = false,
                NeighboorIds = new List<LocationId>() {
                    LocationId.MantolDerith2Blingdenstone,
                }
            }
              );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.MantolDerith2Blingdenfire, "MantolDerith2Blingdenfire",
                LocationId.MantolDerith, LocationId.Blingdenfire)
                );

            board.Locations.Add(new Location(LocationId.Blingdenfire)
            {
                BonusMana = 0,
                ControlVPs = 2,
                Name = "Blingdenfire",
                Size = 2,
                IsStart = false,
                NeighboorIds = new List<LocationId>() {
                    LocationId.MantolDerith2Blingdenfire,
                    LocationId.Blingdenfire2Menzoberranzan,
                }
            }
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Blingdenfire2Menzoberranzan, "Blingdenfire2Menzoberranzan",
                LocationId.Blingdenfire, LocationId.Menzoberranzan)
                );

            board.Locations.Add(new Location(LocationId.Menzoberranzan)
            {
                BonusMana = 1,
                BonusVP = 2,
                ControlVPs = 5,
                Name = "Menzoberranzan",
                Size = 6,
                IsStart = false,
                NeighboorIds = new List<LocationId>() {
                    LocationId.Menzoberranzan2Bridge,
                    LocationId.Menzoberranzan2Everfire,
                    LocationId.Blingdenfire2Menzoberranzan,
                }
            }
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Menzoberranzan2Bridge, "Menzoberranzan2Bridge",
                LocationId.Menzoberranzan, LocationId.Bridge2Menzoberranzan)
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Bridge2Menzoberranzan, "Bridge2Menzoberranzan",
                LocationId.Bridge, LocationId.Menzoberranzan2Bridge)
                );

            board.Locations.Add(new Location(LocationId.Bridge)
            {
                BonusMana = 0,
                ControlVPs = 1,
                Name = "Bridge",
                Size = 1,
                IsStart = false,
                NeighboorIds = new List<LocationId>() {
                    LocationId.Bridge2Menzoberranzan,
                    LocationId.Bridge2Araumycos,
                    LocationId.Bridge2Gracklstugh,
                    LocationId.Everfire2Bridge,
                    LocationId.ChedNasad2Bridge,
                }
            }
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Bridge2Gracklstugh, "Bridge2Gracklstugh",
                LocationId.Bridge, LocationId.Gracklstugh2Bridge)
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Gracklstugh2Bridge, "Gracklstugh2Bridge",
                LocationId.Gracklstugh, LocationId.Bridge2Gracklstugh)
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Bridge2Araumycos, "Bridge2Araumycos",
                LocationId.Bridge, LocationId.Araumycos)
                );

            board.Locations.Add(new Location(LocationId.Araumycos)
            {
                BonusMana = 1,
                BonusVP = 3,
                ControlVPs = 3,
                Name = "Araumycos",
                Size = 4,
                IsStart = false,
                NeighboorIds = new List<LocationId>() {
                    LocationId.Araumycos2Eryndlyn,
                    LocationId.Araumycos2Labyrinth,
                    LocationId.Bridge2Araumycos,
                    LocationId.ChedNasad2Araumycos,
                }
            }
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Araumycos2Eryndlyn, "Araumycos2Eryndlyn",
                LocationId.Araumycos, LocationId.Eryndlyn)
                );

            board.Locations.Add(new Location(LocationId.Eryndlyn)
            {
                BonusMana = 0,
                ControlVPs = 3,
                Name = "Eryndlyn",
                Size = 3,
                IsStart = true,
                NeighboorIds = new List<LocationId>() {
                    LocationId.Eryndlyn2Kanaglym,
                    LocationId.Araumycos2Eryndlyn,
                    LocationId.Llacerellyn2Eryndlyn,
                }
            }
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Eryndlyn2Kanaglym, "Eryndlyn2Kanaglym",
                LocationId.Eryndlyn, LocationId.Kanaglym)
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Kanaglym2Tsenviilyq, "Kanaglym2Tsenviilyq",
                LocationId.Kanaglym, LocationId.Tsenviilyq)
                );

            board.Locations.Add(new Location(LocationId.Tsenviilyq)
            {
                BonusMana = 1,
                BonusVP = 1,
                ControlVPs = 4,
                Name = "Tsenviilyq",
                Size = 3,
                IsStart = false,
                NeighboorIds = new List<LocationId>() {
                    LocationId.Tsenviilyq2Llacerellyn,
                    LocationId.Kanaglym2Tsenviilyq,
                }
            }
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Tsenviilyq2Llacerellyn, "Tsenviilyq2Llacerellyn",
                LocationId.Tsenviilyq, LocationId.Llacerellyn)
                );

            board.Locations.Add(new Location(LocationId.Llacerellyn)
            {
                BonusMana = 0,
                ControlVPs = 2,
                Name = "Llacerellyn",
                Size = 2,
                IsStart = false,
                NeighboorIds = new List<LocationId>() {
                    LocationId.Llacerellyn2ChedNasad,
                    LocationId.Llacerellyn2Eryndlyn,
                    LocationId.Llacerellyn2SSZuraassnee,
                    LocationId.Tsenviilyq2Llacerellyn,
                }
            }
                 );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Llacerellyn2Eryndlyn, "Llacerellyn2Eryndlyn",
                LocationId.Llacerellyn, LocationId.Eryndlyn)
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Llacerellyn2ChedNasad, "Llacerellyn2ChedNasad",
                LocationId.Llacerellyn, LocationId.ChedNasad2Llacerellyn)
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.ChedNasad2Llacerellyn, "ChedNasad2Llacerellyn",
                LocationId.ChedNasad, LocationId.Llacerellyn2ChedNasad)
                );

            board.Locations.Add(new Location(LocationId.ChedNasad)
            {
                BonusMana = 0,
                ControlVPs = 3,
                Name = "ChedNasad",
                Size = 4,
                IsStart = true,
                NeighboorIds = new List<LocationId>() {
                    LocationId.ChedNasad2Araumycos,
                    LocationId.ChedNasad2Bridge,
                    LocationId.ChedNasad2Legion,
                    LocationId.ChedNasad2Llacerellyn,
                    LocationId.Ruins2ChedNasad,
                    LocationId.Yathchol2ChedNasad,
                }
            }
                 );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.ChedNasad2Araumycos, "ChedNasad2Araumycos",
                LocationId.ChedNasad, LocationId.Araumycos)
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.ChedNasad2Bridge, "ChedNasad2Bridge",
                LocationId.ChedNasad, LocationId.Bridge)
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.ChedNasad2Legion, "ChedNasad2Legion",
                LocationId.ChedNasad, LocationId.Legion2ChedNasad)
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Legion2ChedNasad, "Legion2ChedNasad",
                LocationId.Legion, LocationId.ChedNasad2Legion)
                );

            board.Locations.Add(new Location(LocationId.Legion)
            {
                BonusMana = 0,
                ControlVPs = 3,
                Name = "Legion",
                Size = 2,
                IsStart = false,
                NeighboorIds = new List<LocationId>() {
                    LocationId.Legion2ChedNasad,
                    LocationId.Legion2Everfire,
                    LocationId.Yathchol2Legion,
                }
            }
                 );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Legion2Everfire, "Legion2Everfire",
                LocationId.Legion, LocationId.Everfire)
                );

            board.Locations.Add(new Location(LocationId.Everfire)
            {
                BonusMana = 0,
                ControlVPs = 3,
                Name = "Everfire",
                Size = 3,
                IsStart = false,
                NeighboorIds = new List<LocationId>() {
                    LocationId.Everfire2Bridge,
                    LocationId.Everfire2Chaulssin,
                    LocationId.Everfire2Menzoberranzan,
                    LocationId.Phaerlin2Everfire,
                    LocationId.Legion2Everfire,
                }
            }
                 );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Everfire2Menzoberranzan, "Everfire2Menzoberranzan",
                LocationId.Everfire, LocationId.Menzoberranzan2Everfire)
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Menzoberranzan2Everfire, "Menzoberranzan2Everfire",
                LocationId.Menzoberranzan, LocationId.Everfire2Menzoberranzan)
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Everfire2Bridge, "Everfire2Bridge",
                LocationId.Everfire, LocationId.Bridge)
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Everfire2Chaulssin, "Everfire2Chaulssin",
                LocationId.Everfire, LocationId.Chaulssin)
                );

            board.Locations.Add(new Location(LocationId.Chaulssin)
            {
                BonusMana = 0,
                ControlVPs = 4,
                Name = "Chaulssin",
                Size = 5,
                IsStart = true,
                NeighboorIds = new List<LocationId>() {
                    LocationId.Chaulssin2Phaerlin,
                    LocationId.Everfire2Chaulssin,
                }
            }
                 );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Chaulssin2Phaerlin, "Chaulssin2Phaerlin",
                LocationId.Chaulssin, LocationId.Phaerlin)
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Phaerlin2Everfire, "Phaerlin2Everfire",
                LocationId.Phaerlin, LocationId.Everfire)
                );

            board.Locations.Add(new Location(LocationId.Phaerlin)
            {
                BonusMana = 1,
                BonusVP = 1,
                ControlVPs = 2,
                Name = "Phaerlin",
                Size = 3,
                IsStart = false,
                NeighboorIds = new List<LocationId>() {
                    LocationId.Phaerlin2Yathchol,
                    LocationId.Chaulssin2Phaerlin,
                    LocationId.OldYathchol2Phaerlin,
                    LocationId.Phaerlin2Everfire,
                }
            }
                 );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Phaerlin2Yathchol, "Phaerlin2Yathchol",
                LocationId.Phaerlin, LocationId.Yathchol)
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.OldYathchol2Phaerlin, "OldYathchol2Phaerlin",
                LocationId.OldYathchol, LocationId.Phaerlin)
                );

            board.Locations.Add(new Location(LocationId.Yathchol)
            {
                BonusMana = 0,
                ControlVPs = 4,
                Name = "Yathchol",
                Size = 2,
                IsStart = false,
                NeighboorIds = new List<LocationId>() {
                    LocationId.Phaerlin2Yathchol,
                    LocationId.Yathchol2ChedNasad,
                    LocationId.Yathchol2Legion,
                    LocationId.Yathchol2OldYathchol,
                }
            }
                 );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Yathchol2Legion, "Yathchol2Legion",
                LocationId.Yathchol, LocationId.Legion)
                );

            board.Locations.Add(
               Location.MakeTunnel(LocationId.Yathchol2ChedNasad, "Yathchol2ChedNasad",
               LocationId.Yathchol, LocationId.ChedNasad)
               );

            board.Locations.Add(
               Location.MakeTunnel(LocationId.Yathchol2OldYathchol, "Yathchol2OldYathchol",
               LocationId.Yathchol, LocationId.OldYathchol)
               );

            board.Locations.Add(new Location(LocationId.OldYathchol)
            {
                BonusMana = 0,
                ControlVPs = 3,
                Name = "OldYathchol",
                Size = 3,
                IsStart = false,
                NeighboorIds = new List<LocationId>() {
                    LocationId.OldYathchol2Phaerlin,
                    LocationId.OldYathchol2Ruins,
                    LocationId.Yathchol2OldYathchol,
                }
            }
                 );

            board.Locations.Add(
               Location.MakeTunnel(LocationId.OldYathchol2Ruins, "OldYathchol2Ruins",
               LocationId.OldYathchol, LocationId.Ruins)
               );

            board.Locations.Add(new Location(LocationId.Ruins)
            {
                BonusMana = 0,
                ControlVPs = 5,
                Name = "Ruins",
                Size = 6,
                IsStart = false,
                NeighboorIds = new List<LocationId>() {
                    LocationId.Ruins2ChedNasad,
                    LocationId.Ruins2SSZuraassnee,
                    LocationId.OldYathchol2Ruins,
                }
            }
                 );

            board.Locations.Add(
               Location.MakeTunnel(LocationId.Ruins2SSZuraassnee, "Ruins2SSZuraassnee",
               LocationId.Ruins, LocationId.SSZuraassnee)
               );

            board.Locations.Add(new Location(LocationId.SSZuraassnee)
            {
                BonusMana = 1,
                BonusVP = 1,
                ControlVPs = 2,
                Name = "SSZuraassnee",
                Size = 3,
                IsStart = false,
                NeighboorIds = new List<LocationId>() {
                    LocationId.SSZuraassnee2Llacerellyn,
                    LocationId.Ruins2SSZuraassnee,
                }
            }
                 );

            board.Locations.Add(
              Location.MakeTunnel(LocationId.SSZuraassnee2Llacerellyn, "SSZuraassnee2Llacerellyn",
              LocationId.SSZuraassnee, LocationId.Llacerellyn2SSZuraassnee)
              );

            board.Locations.Add(
              Location.MakeTunnel(LocationId.Llacerellyn2SSZuraassnee, "Llacerellyn2SSZuraassnee",
              LocationId.Llacerellyn, LocationId.SSZuraassnee2Llacerellyn)
              );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Wormwrithings2Gauntlgrym, "Wormwrithings2Gauntlgrym",
                LocationId.Wormwrithings, LocationId.Gauntlgrym)
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Ruins2ChedNasad, "Ruins2ChedNasad",
                LocationId.Ruins, LocationId.ChedNasad)
                );

            board.LocationIds = board
                .Locations
                .ToDictionary(l => l.Id);
        }

        private static void LinkLocations(Board board)
        {
            var dict = board.Locations
                .ToDictionary(l => l.Id);

            foreach (var location in board.Locations)
            {
                location.Neighboors = location.NeighboorIds
                    .Select(id => dict[id])
                    .ToHashSet();
            }
        }

        public static Board Initialize(bool isWithChecks = false)
        {
            Console.WriteLine("Creating virtual board...");

            var board = new Board();

            CreateLocations(board);

            LinkLocations(board);

            if (isWithChecks)
            {
                TestBoardInitialization.CheckBoardCreation(board);
            }

            Console.WriteLine("Board has created");

            return board;
        }
    }
}
