﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TUnderdark.Model
{
    internal class Location
    {
        public static Location MakeTunnel(LocationId id, string name, LocationId from, LocationId to)
        {
            return new Location(id)
            {
                Size = 1,
                Name = name,
                IsSite = false,
                IsStart = false,
                BonusMana = 0,
                ControlVPs = 0,
                Id = id,
                NeighboorIds = new List<LocationId>() { from, to },
            };
        }

        public LocationId Id { get; set; }
        public int Size { get; set; }
        public string Name { get; set; }
        public Dictionary<Color, int> Troops { get; set; }
        public Dictionary<Color, bool> Spies { get; set; }
        public int ControlVPs { get; set; }
        public int TotalControlVPs => ControlVPs + (IsSite ? 2 : 0);
        public bool IsSite { get; set; }
        public bool IsStart { get; set; }
        public bool IsTunnel => !IsSite;
        public bool IsSpyPlacable => !IsTunnel;
        public bool HasControlMarker => BonusMana > 0;
        public int BonusMana { get; set; }
        public HashSet<Location> Neighboors { get; set; }
        public List<LocationId> NeighboorIds { get; set; }
        public int BonusVP { get; set; }

        public Location(LocationId id)
        {
            Id = id;
            Size = 1;
            Name = "";

            Troops = Enum.GetValues(typeof(Color))
                .Cast<Color>()
                .ToDictionary(c => c, c => 0);

            Spies = Enum.GetValues(typeof(Color))
                .Cast<Color>()
                .ToDictionary(c => c, c => false);

            ControlVPs = 0;
            IsSite = true;
            BonusMana = 0;
            BonusVP = 0;
            Neighboors = new();
            IsStart = false;

            NeighboorIds = new List<LocationId>();
        }
        public bool HasTroops(Color color) => Troops[color] > 0;

        public bool IsPresence(Color color) => Spies[color] 
            || HasTroops(color)
            || Neighboors.Any(n => n.HasTroops(color));

        public bool IsControl(Color color)
        {
            int maxPresence = Troops.Max(kv => kv.Value);

            return Troops[color] == maxPresence 
                && Troops.Where(kv => kv.Value == maxPresence).Count() == 1;
        }

        public bool IsFullControl(Color color)
        {
            return Spies.All(kv => kv.Key == color || !kv.Value) 
                && Troops[color] == Size;
        }

        public Color? GetControlPlayer()
        {
            int maxPresence = Troops.Max(kv => kv.Value);

            var candidates = Troops.Where(kv => kv.Value == maxPresence).ToList();

            if (candidates.Count > 1)
            {
                return null;
            }

            var controller = candidates.Select(kv => kv.Key).Single();

            if (controller == Color.WHITE)
            {
                return null;
            }

            return controller;
        }

        public Color? GetFullControl()
        {
            var candidates = Troops
                .Where(kv => kv.Value == Size)
                .Select(kv => kv.Key)
                .ToList();

            if (!candidates.Any())
            {
                return null;
            }

            var candidate = candidates.Single();

            if (candidate == Color.WHITE)
            {
                return null;
            }

            if (Spies.Any(kv => kv.Value && kv.Key != candidate))
            {
                return null;
            }

            return candidate;
        }
    }
}
