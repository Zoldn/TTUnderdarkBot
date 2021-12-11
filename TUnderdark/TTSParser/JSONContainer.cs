using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUnderdark.TTSParser
{
    internal class Transform 
    {
        public double posX { get; set; }
        public double posY { get; set; }
        public double posZ { get; set; }
    }

    internal class TTSObjectState
    {
        public string GUID { get; set; }
        public Transform Transform { get; set; }
        public List<TTSObjectState> ContainedObjects { get; set; }
        public string Name { get; set; }
        public List<int> DeckIDs { get; set; }
        public int CardId { get; set; }
        public string Nickname { get; set; }
        public string LuaScript { get; set; }
        public TTSObjectState()
        {
            ContainedObjects = new();
        }

        public override string ToString()
        {
            return $"{GUID}, {Nickname}, {CardId}";
        }

        public bool IsPositionIn(double x1, double x2, double z1, double z2)
        {
            return x1 <= Transform.posX && Transform.posX <= x2
                && z1 <= Transform.posZ && Transform.posZ <= z2;
        }
    }

    internal class JSONContainer
    {
        public string GameMode { get; set; }
        public List<TTSObjectState> ObjectStates { get; set; }
        public JSONContainer() 
        {
            ObjectStates = new();
        }
    }
}
