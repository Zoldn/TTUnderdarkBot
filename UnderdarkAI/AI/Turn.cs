using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace UnderdarkAI.AI
{
    /// <summary>
    /// Состояние карт для розыгрыша из руки
    /// </summary>
    internal enum CardState
    {
        IN_HAND = 0,
        NOW_PLAYING,
        PLAYED,
        DEVOURED,
        DISCARDED,
    }
    /// <summary>
    /// Режим выбора конечного автомата
    /// </summary>
    internal enum SelectionState
    {
        CARD_OR_FREE_ACTION,
        SELECT_CARD,
        SELECT_FREE_ACTION,
        SELECT_CARD_OPTION,
        SELECT_LOCATION,
        SELECT_COLOR,
    }

    /// <summary>
    /// Текущий ход
    /// </summary>
    internal class Turn
    {
        /// <summary>
        /// Цвет текущего игрока
        /// </summary>
        public Color Color { get; }
        /// <summary>
        /// Current Mana Points
        /// </summary>
        public int Mana { get; set; }
        /// <summary>
        /// Current Sword Points
        /// </summary>
        public int Swords { get; set; }
        /// <summary>
        /// Additional earned VPs from cards
        /// </summary>
        public int VPs { get; set; }
        /// <summary>
        /// Value of this turn as total
        /// </summary>
        public double Value { get; set; }
        /// <summary>
        /// Score of current positions
        /// </summary>
        public double PresentScore { get; set; }
        /// <summary>
        /// Estimate score of future
        /// </summary>
        public double FutureScore { get; set; }
        /// <summary>
        /// Текущее состояние выбора
        /// </summary>
        public SelectionState State { get; set; }
        public List<IAtomicEffect> SelectionSequence { get; set; }
        public List<IAtomicEffect> EndTurnEffects { get; set; }
        public Card? ActiveCard => CardStates
            .Where(kv => kv.Value == CardState.NOW_PLAYING)
            .Select(kv => kv.Key)
            .SingleOrDefault();
        public Dictionary<Card, CardState> CardStates { get; private set; }
        public Dictionary<Location, LocationState> LocationStates { get; private set; }
        public Turn(Color color)
        {
            Color = color;

            SelectionSequence = new(50);
            EndTurnEffects = new(10);
            CardStates = new Dictionary<Card, CardState>();

            State = SelectionState.CARD_OR_FREE_ACTION;

            LocationStates = new();
        }

        public void DebugPrintDistances()
        {
            var locationStates = LocationStates
                .Select(kv => kv.Value)
                .OrderBy(kv => kv.Distance)
                .ToList();

            foreach (var locationState in locationStates)
            {
                Console.WriteLine($"Location {locationState.Location} distance is {locationState.Distance}");
            }
        }
    }
}
