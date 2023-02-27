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
    /// Режим выбора опции на карте Choose one
    /// </summary>
    internal enum CardOption
    {
        NONE_OPTION,
        OPTION_A,
        OPTION_B,
    }
    /// <summary>
    /// Режим выбора конечного автомата
    /// </summary>
    internal enum SelectionState
    {
        /// <summary>
        /// Выбор - сыграть карту или свободное действие
        /// </summary>
        CARD_OR_FREE_ACTION,
        /// <summary>
        /// Выбор - какую карту сыграть
        /// </summary>
        SELECT_CARD,
        /// <summary>
        /// Выбор - сыграть свободное действие
        /// </summary>
        SELECT_BASE_ACTION,
        /// <summary>
        /// Выбор - какую опцию на карте сыграть
        /// </summary>
        SELECT_CARD_OPTION,
        /// <summary>
        /// Выбор - локации
        /// </summary>
        SELECT_LOCATION,
        /// <summary>
        /// Выбор - цвета
        /// </summary>
        SELECT_COLOR,
        /// <summary>
        /// Переключиться в режим конца хода
        /// </summary>
        SELECT_CARD_END_TURN,
        /// <summary>
        /// Остановка поиска, ход полностью завершен
        /// </summary>
        FINISH_SELECTION,
        BUY_CARD_BY_MANA,
        DEPLOY_BY_SWORD,
        ASSASSINATE_BY_SWORD,
        RETURN_ENEMY_SPY_BY_SWORD,
        /// <summary>
        /// Выбор эффекта карты в конце хода
        /// </summary>
        SELECT_END_TURN_CARD_OPTION,
    }

    internal class TurnCardState
    {
        public CardSpecificType SpecificType { get; }
        public CardState State { get; set; }
        /// <summary>
        /// Состояние конца хода (для опций "promote in the end of turn")
        /// </summary>
        public CardState EndTurnState { get; set; }
        public bool IsPromotedInTheEnd { get; set; }

        public TurnCardState(CardSpecificType specificType, CardState state = CardState.IN_HAND)
        {
            SpecificType = specificType;
            State = state;
            EndTurnState = CardState.DISCARDED;
            IsPromotedInTheEnd = false;
        }
        public TurnCardState Clone()
        {
            return new TurnCardState(SpecificType, State)
            {
                EndTurnState = EndTurnState,
                IsPromotedInTheEnd = IsPromotedInTheEnd,
            };
        }
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
        //public int VPs { get; set; }
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
        #region State of automata
        /// <summary>
        /// Текущее состояние выбора
        /// </summary>
        public SelectionState State { get; set; }
        /// <summary>
        /// Итерация выбора карты (для карт, вида "выберите 2/3 раза")
        /// </summary>
        public int CardStateIteration { get; set; }
        /// <summary>
        /// Опция на карте
        /// </summary>
        public CardOption CardOption { get; set; }
        #endregion
        public List<PlayableOption> EndTurnEffects { get; set; }
        public CardSpecificType? ActiveCard => CardStates
            .Where(s => s.State == CardState.NOW_PLAYING || s.EndTurnState == CardState.NOW_PLAYING)
            .Select(s => s.SpecificType)
            .SingleOrDefault();
        //public Dictionary<Card, CardState> CardStates { get; private set; }
        public bool IsBuyingEnabled { get; set; }
        public List<TurnCardState> CardStates { get; private set; }
        public Dictionary<Location, LocationState> LocationStates { get; private set; }
        public Turn(Color color)
        {
            Color = color;

            //SelectionSequence = new(50);
            EndTurnEffects = new(10);
            CardStates = new(5);

            IsBuyingEnabled = true;
            State = SelectionState.CARD_OR_FREE_ACTION;
            CardOption = CardOption.NONE_OPTION;
            CardStateIteration = 0;

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

        public Turn Clone(Board board)
        {
            var turn = new Turn(Color)
            {
                CardStates = CardStates
                    .Select(s => s.Clone()).ToList(),
                EndTurnEffects = EndTurnEffects.ToList(),
                FutureScore = FutureScore,
                LocationStates = LocationStates
                    .ToDictionary(
                        kv => board.LocationIds[kv.Key.Id], 
                        kv => kv.Value.Clone()
                        ),
                Mana = Mana,
                IsBuyingEnabled = IsBuyingEnabled,
                Swords = Swords,
                State = State,
                PresentScore = PresentScore,
                //VPs = VPs,
                Value = Value,
                CardStateIteration = CardStateIteration,
                CardOption = CardOption,
            };

            return turn;
        }
    }
}
