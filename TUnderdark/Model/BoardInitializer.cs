using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUnderdark.Model
{
    internal enum LocationId
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
        Yathchol2ChadNasad,
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
    }

    internal static class BoardInitializer
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
            }
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Gauntlgrym2Jhachalkhyn, "Gauntlgrym2Jhachalkhyn")
                );

            board.Locations.Add(new Location(LocationId.Jhachalkhyn)
            {
                BonusMana = 0,
                ControlVPs = 4,
                Name = "Jhachalkhyn",
                Size = 4,
                IsStart = false,
            }
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Jhachalkhyn2Buiyrandyn, "Jhachalkhyn2Buiyrandyn")
                );

            board.Locations.Add(new Location(LocationId.Buiyrandyn)
            {
                BonusMana = 0,
                ControlVPs = 3,
                Name = "Buiyrandyn",
                Size = 3,
                IsStart = true,
            }
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Buiyrandyn2StoneShaft, "Buiyrandyn2StoneShaft")
                );

            board.Locations.Add(new Location(LocationId.StoneShaft)
            {
                BonusMana = 0,
                ControlVPs = 4,
                Name = "StoneShaft",
                Size = 2,
                IsStart = false,
            }
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.StoneShaft2ChChitl, "StoneShaft2ChChitl")
                );

            board.Locations.Add(new Location(LocationId.ChChitl)
            {
                BonusMana = 1,
                BonusVP = 1,
                ControlVPs = 2,
                Name = "ChChitl",
                Size = 3,
                IsStart = false,
            }
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.ChChitl2Kanaglym, "ChChitl2Kanaglym")
                );

            board.Locations.Add(new Location(LocationId.Kanaglym)
            {
                BonusMana = 0,
                ControlVPs = 3,
                Name = "Kanaglym",
                Size = 3,
                IsStart = false,
            }
               );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Kanaglym2Skullport, "Kanaglym2Skullport")
                );

            board.Locations.Add(new Location(LocationId.Skullport)
            {
                BonusMana = 0,
                ControlVPs = 4,
                Name = "Skullport",
                Size = 5,
                IsStart = true,
            }
               );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Skullport2StoneShaft, "Skullport2StoneShaft")
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Skullport2Labyrinth, "Skullport2Labyrinth")
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Labyrinth2Skullport, "Labyrinth2Skullport")
                );

            board.Locations.Add(new Location(LocationId.Labyrinth)
            {
                BonusMana = 0,
                ControlVPs = 3,
                Name = "Labyrinth",
                Size = 3,
                IsStart = false,
            }
               );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Labyrinth2Buiyrandyn, "Labyrinth2Buiyrandyn")
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Labyrinth2Araumycos, "Labyrinth2Araumycos")
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Araumycos2Labyrinth, "Araumycos2Labyrinth")
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Labyrinth2Gracklstugh, "Labyrinth2Gracklstugh")
                );

            board.Locations.Add(new Location(LocationId.Gracklstugh)
            {
                BonusMana = 0,
                ControlVPs = 3,
                Name = "Gracklstugh",
                Size = 4,
                IsStart = false,
            }
               );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Gracklstugh2Jhachalkhyn, "Gracklstugh2Jhachalkhyn")
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Gracklstugh2MantolDerith, "Gracklstugh2MantolDerith")
                );

            board.Locations.Add(new Location(LocationId.MantolDerith)
            {
                BonusMana = 0,
                ControlVPs = 4,
                Name = "MantolDerith",
                Size = 5,
                IsStart = true,
            }
               );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.MantolDerith2Wormwrithings, "MantolDerith2Wormwrithings")
                );

            board.Locations.Add(new Location(LocationId.Wormwrithings)
            {
                BonusMana = 0,
                ControlVPs = 3,
                Name = "Wormwrithings",
                Size = 3,
                IsStart = false,
            }
              );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.MantolDerith2Blingdenstone, "MantolDerith2Blingdenstone")
                );

            board.Locations.Add(new Location(LocationId.Blingdenstone)
            {
                BonusMana = 0,
                ControlVPs = 4,
                Name = "Blingdenstone",
                Size = 2,
                IsStart = false,
            }
              );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.MantolDerith2Blingdenfire, "MantolDerith2Blingdenfire")
                );

            board.Locations.Add(new Location(LocationId.Blingdenfire)
            {
                BonusMana = 0,
                ControlVPs = 2,
                Name = "Blingdenfire",
                Size = 2,
                IsStart = false,
            }
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Blingdenfire2Menzoberranzan, "Blingdenfire2Menzoberranzan")
                );

            board.Locations.Add(new Location(LocationId.Menzoberranzan)
            {
                BonusMana = 1,
                BonusVP = 2,
                ControlVPs = 5,
                Name = "Menzoberranzan",
                Size = 6,
                IsStart = false,
            }
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Menzoberranzan2Bridge, "Menzoberranzan2Bridge")
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Bridge2Menzoberranzan, "Bridge2Menzoberranzan")
                );

            board.Locations.Add(new Location(LocationId.Bridge)
            {
                BonusMana = 0,
                ControlVPs = 1,
                Name = "Bridge",
                Size = 1,
                IsStart = false,
            }
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Bridge2Gracklstugh, "Bridge2Gracklstugh")
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Gracklstugh2Bridge, "Gracklstugh2Bridge")
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Bridge2Araumycos, "Bridge2Araumycos")
                );

            board.Locations.Add(new Location(LocationId.Araumycos)
            {
                BonusMana = 1,
                BonusVP = 3,
                ControlVPs = 3,
                Name = "Araumycos",
                Size = 4,
                IsStart = false,
            }
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Araumycos2Eryndlyn, "Araumycos2Eryndlyn")
                );

            board.Locations.Add(new Location(LocationId.Eryndlyn)
            {
                BonusMana = 0,
                ControlVPs = 3,
                Name = "Eryndlyn",
                Size = 3,
                IsStart = true,
            }
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Eryndlyn2Kanaglym, "Eryndlyn2Kanaglym")
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Kanaglym2Tsenviilyq, "Kanaglym2Tsenviilyq")
                );

            board.Locations.Add(new Location(LocationId.Tsenviilyq)
            {
                BonusMana = 1,
                BonusVP = 1,
                ControlVPs = 4,
                Name = "Tsenviilyq",
                Size = 3,
                IsStart = false,
            }
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Tsenviilyq2Llacerellyn, "Tsenviilyq2Llacerellyn")
                );

            board.Locations.Add(new Location(LocationId.Llacerellyn)
            {
                BonusMana = 0,
                ControlVPs = 2,
                Name = "Llacerellyn",
                Size = 2,
                IsStart = false,
            }
                 );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Llacerellyn2Eryndlyn, "Llacerellyn2Eryndlyn")
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Llacerellyn2ChedNasad, "Llacerellyn2ChedNasad")
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.ChedNasad2Llacerellyn, "ChedNasad2Llacerellyn")
                );

            board.Locations.Add(new Location(LocationId.ChedNasad)
            {
                BonusMana = 0,
                ControlVPs = 3,
                Name = "ChedNasad",
                Size = 4,
                IsStart = true,
            }
                 );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.ChedNasad2Araumycos, "ChedNasad2Araumycos")
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.ChedNasad2Bridge, "ChedNasad2Bridge")
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.ChedNasad2Legion, "ChedNasad2Legion")
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Legion2ChedNasad, "Legion2ChedNasad")
                );

            board.Locations.Add(new Location(LocationId.Legion)
            {
                BonusMana = 0,
                ControlVPs = 3,
                Name = "Legion",
                Size = 2,
                IsStart = false,
            }
                 );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Legion2Everfire, "Legion2Everfire")
                );

            board.Locations.Add(new Location(LocationId.Everfire)
            {
                BonusMana = 0,
                ControlVPs = 3,
                Name = "Everfire",
                Size = 3,
                IsStart = false,
            }
                 );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Everfire2Menzoberranzan, "Everfire2Menzoberranzan")
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Menzoberranzan2Everfire, "Menzoberranzan2Everfire")
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Everfire2Bridge, "Everfire2Bridge")
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Everfire2Chaulssin, "Everfire2Chaulssin")
                );

            board.Locations.Add(new Location(LocationId.Chaulssin)
            {
                BonusMana = 0,
                ControlVPs = 4,
                Name = "Chaulssin",
                Size = 5,
                IsStart = true,
            }
                 );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Chaulssin2Phaerlin, "Chaulssin2Phaerlin")
                );

            board.Locations.Add(new Location(LocationId.Phaerlin)
            {
                BonusMana = 1,
                BonusVP = 1,
                ControlVPs = 2,
                Name = "Phaerlin",
                Size = 3,
                IsStart = false,
            }
                 );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Phaerlin2Yathchol, "Phaerlin2Yathchol")
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.OldYathchol2Phaerlin, "OldYathchol2Phaerlin")
                );

            board.Locations.Add(new Location(LocationId.Yathchol)
            {
                BonusMana = 0,
                ControlVPs = 4,
                Name = "Yathchol",
                Size = 2,
                IsStart = false,
            }
                 );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Yathchol2Legion, "Yathchol2Legion")
                );

            board.Locations.Add(
               Location.MakeTunnel(LocationId.Yathchol2ChadNasad, "Yathchol2ChadNasad")
               );

            board.Locations.Add(
               Location.MakeTunnel(LocationId.Yathchol2OldYathchol, "Yathchol2OldYathchol")
               );

            board.Locations.Add(new Location(LocationId.OldYathchol)
            {
                BonusMana = 0,
                ControlVPs = 3,
                Name = "OldYathchol",
                Size = 3,
                IsStart = false,
            }
                 );

            board.Locations.Add(
               Location.MakeTunnel(LocationId.OldYathchol2Ruins, "OldYathchol2Ruins")
               );

            board.Locations.Add(new Location(LocationId.Ruins)
            {
                BonusMana = 0,
                ControlVPs = 5,
                Name = "Ruins",
                Size = 6,
                IsStart = false,
            }
                 );

            board.Locations.Add(
               Location.MakeTunnel(LocationId.Ruins2SSZuraassnee, "Ruins2SSZuraassnee")
               );

            board.Locations.Add(new Location(LocationId.SSZuraassnee)
            {
                BonusMana = 1,
                BonusVP = 1,
                ControlVPs = 2,
                Name = "SSZuraassnee",
                Size = 3,
                IsStart = false,
            }
                 );

            board.Locations.Add(
              Location.MakeTunnel(LocationId.SSZuraassnee2Llacerellyn, "SSZuraassnee2Llacerellyn")
              );

            board.Locations.Add(
              Location.MakeTunnel(LocationId.Llacerellyn2SSZuraassnee, "Llacerellyn2SSZuraassnee")
              );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Wormwrithings2Gauntlgrym, "Wormwrithings2Gauntlgrym")
                );

            board.Locations.Add(
                Location.MakeTunnel(LocationId.Ruins2ChedNasad, "Ruins2ChedNasad")
                );
        }
        public static void Initialize(Board board)
        {
            CreateLocations(board);

            TestBoardInitialization.CheckBoardCreation(board);
        }
    }
}
