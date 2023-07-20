using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnderdarkAI.Utils;

namespace TUnderdark.Model
{
    public class MapState : IEquatable<MapState>
    {
        public List<Location> Locations { get; init; }
        public Dictionary<LocationId, Location> LocationIds { get; set; }
        public MapState()
        {
            Locations = new List<Location>();
            LocationIds = Locations.ToDictionary(l => l.Id);
        }

        public bool Equals(MapState other)
        {
            if (other is null)
            {
                return false;
            }

            foreach (var location in Locations)
            {
                if (!location.Equals(other.LocationIds[location.Id]))
                {
                    return false;
                }
            }

            return true;
        }

        internal MapState Clone()
        {
            var copy = new MapState()
            {
                Locations = Locations.Select(l => l.Clone()).ToList(),
            };

            copy.LocationIds = copy.Locations
                .ToDictionary(l => l.Id);

            return copy;
        }

        public override int GetHashCode()
        {
            return HashcodeHelper.OfEach(Locations);
        }
    }
}
