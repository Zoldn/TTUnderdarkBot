using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace UnderdarkAI.AI
{
    internal class LocationState
    {
        public static int UNREACHABLE = int.MaxValue;
        public static int NOT_ANALYSED = int.MinValue;
        public Location Location { get; }
        /// <summary>
        /// Есть ли присутствие в локации
        /// </summary>
        public bool HasPresence { get; set; }
        /// <summary>
        /// Сколько надо поставить пехотинцев, чтобы получить присутствие в локации,
        /// 0 - присутствие уже есть, 
        /// UNREACHABLE_DISTANCE - невозможно дойти
        /// NOT_ANALYSED - еще не рассчитано
        /// </summary>
        public int Distance { get; set; }
        /// <summary>
        /// Можно ли из этой локации распространяться дальше
        /// </summary>
        public bool IsPropagatable { get; set; }
        public Color PlayerColor { get; }
        public LocationState(Location location, Color playerColor)
        {
            Location = location;
            PlayerColor = playerColor;

            Distance = NOT_ANALYSED;
            HasPresence = false;
            IsPropagatable = false;
        }
    }
}
