using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using TUnderdark.TTSParser;
using UnderdarkAI.AI.PlayableOptions;
using UnderdarkAI.AI.WeightGenerators;

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
        PROMOTED,
        HOLD,
    }
    internal enum CardLocation
    {
        IN_HAND = 0,
        DISCARD,
        DEVOURED,
        MARKET,
        INNER_CIRCLE,
        INSANE_OUTCASTS,
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
        ON_DISCARD_CARD_SELECTION,
        ON_DISCARD_CARD,
        END_TURN_ON_DISCARD_CARD_SELECTION,
        END_TURN_ON_DISCARD_CARD,
    }

    internal class TurnCardState
    {
        public CardSpecificType SpecificType { get; }
        public CardState State { get; set; }
        /// <summary>
        /// Состояние конца хода (для опций "promote in the end of turn")
        /// </summary>
        public CardState EndTurnState { get; set; }
        /// <summary>
        /// Где физически сейчас находится сыгранная карта
        /// </summary>
        public CardLocation CardLocation { get; set; }
        public bool IsPromotedInTheEnd { get; set; }
        public bool IsUlitharidTarget { get; init; }
        public bool IsElderBrainTarget { get; init; }
        public CardLocation? PrevLocation { get; init; }

        public TurnCardState(CardSpecificType specificType, CardState state = CardState.IN_HAND)
        {
            SpecificType = specificType;
            State = state;
            EndTurnState = CardState.DISCARDED;
            CardLocation = CardLocation.IN_HAND;
            IsPromotedInTheEnd = false;
            IsUlitharidTarget = false;
            IsElderBrainTarget = false;
            PrevLocation = null;
        }
        public TurnCardState Clone()
        {
            return new TurnCardState(SpecificType, State)
            {
                EndTurnState = EndTurnState,
                IsPromotedInTheEnd = IsPromotedInTheEnd,
                CardLocation = CardLocation,
                IsUlitharidTarget = IsUlitharidTarget,
                IsElderBrainTarget = IsElderBrainTarget,
                PrevLocation = PrevLocation,
            };
        }
        public override string ToString()
        {
            return $"{CardMapper.SpecificTypeCardMakers[SpecificType].Name} - State: {State}; EndState: {EndTurnState};" +
                $" Location {CardLocation}";
        }
    }

    /// <summary>
    /// Текущий ход
    /// </summary>
    internal class Turn
    {
        /// <summary>
        /// Генератор весов для опций выбора
        /// </summary>
        public IWeightGenerator WeightGenerator { get; }

        public Random Random { get; }

        /// <summary>
        /// Цвет текущего игрока
        /// </summary>
        public Color Color { get; private set; }
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
        //public CardOption CardOption { get; set; }
        #endregion
        public List<PlayableOption> EndTurnEffects { get; set; }
        public CardSpecificType? ActiveCard => CardStates
            .Where(s => s.State == CardState.NOW_PLAYING || s.EndTurnState == CardState.NOW_PLAYING)
            .Select(s => s.SpecificType)
            .SingleOrDefault();
        public bool IsBuyingEnabled { get; set; }
        public List<TurnCardState> CardStates { get; private set; }
        public Dictionary<Location, LocationState> LocationStates { get; private set; }
        public bool IsOriginal { get; private set; }
        #region Moving state
        public LocationId? LocationMoveFrom { get; internal set; }
        public Color? ColorMove { get; internal set; }
        /// <summary>
        /// Только что поставленные шпионы
        /// </summary>
        public List<LocationId> PlacedSpies { get; internal set; }
        /// <summary>
        /// Возвращенные шпионы
        /// </summary>
        public List<LocationId> ReturnedSpies { get; internal set; }
        /// <summary>
        /// Можно ли покупать карту с devoured
        /// </summary>
        public bool IsBuyTopDevouredEnabled { get; internal set; }
        /// <summary>
        /// Забранный юнит из trophy hall-а
        /// </summary>
        public Color? TakeFromTroopsColor { get; internal set; }
        /// <summary>
        /// Фиксация следующего убийства в этой локации
        /// </summary>
        public LocationId? LockedAssasinationLocation { get; internal set; }
        #endregion
        #region Colors for insane outcasts
        public HashSet<Color> EnemyPlayers { get; internal set; }
        public HashSet<Color> AllPlayers { get; internal set; }
        public HashSet<Color> ThisPlayer { get; internal set; }
        /// <summary>
        /// Рядом с какими игроками выставлены трупсы
        /// </summary>
        public HashSet<Color> AdjacentPlayersToDeploy { get; internal set; }
        public Stack<HoldCardStackElement> HoldedCardStack { get; internal set; }
        public Queue<DiscardInfo> DiscardCardQueue { get; internal set; }
        public int MaxQuaggothKills { get; internal set; }
        public int QuaggothKills { get; internal set; }
        public Color? LastKillColor { get; internal set; }
        public CardSpecificType? CurrentDiscardingCard { get; internal set; }

        #endregion
        public Turn(Color color, IWeightGenerator weightGenerator, Random random, bool isOriginal = true)
        {
            Random = random;
            WeightGenerator = weightGenerator;
            Color = color;

            AllPlayers = new HashSet<Color>() { Color.RED, Color.YELLOW, Color.GREEN, Color.BLUE };
            ThisPlayer = new HashSet<Color>() { color };
            EnemyPlayers = AllPlayers.Where(c => c != color).ToHashSet();

            EndTurnEffects = new(10);
            CardStates = new(5);

            IsBuyingEnabled = true;
            IsBuyTopDevouredEnabled = false;
            State = SelectionState.CARD_OR_FREE_ACTION;
            CardStateIteration = 0;

            LocationStates = new();
            IsOriginal = isOriginal;

            PlacedSpies = new();
            ReturnedSpies = new();
            AdjacentPlayersToDeploy = new();

            TakeFromTroopsColor = null; 
            LockedAssasinationLocation = null;

            HoldedCardStack = new Stack<HoldCardStackElement>();
            DiscardCardQueue = new Queue<DiscardInfo>();

            MaxQuaggothKills = 0;
            QuaggothKills = 0;

            LastKillColor = null;
            CurrentDiscardingCard = null;
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
            var turn = new Turn(Color, WeightGenerator, Random, isOriginal: false)
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
                Value = Value,
                CardStateIteration = CardStateIteration,
                ColorMove = ColorMove,
                LocationMoveFrom = LocationMoveFrom,
                PlacedSpies = PlacedSpies.Select(s => s).ToList(),
                ReturnedSpies = ReturnedSpies.Select(s => s).ToList(),
                IsBuyTopDevouredEnabled = IsBuyTopDevouredEnabled,
                TakeFromTroopsColor = TakeFromTroopsColor,
                LockedAssasinationLocation = LockedAssasinationLocation,
                AdjacentPlayersToDeploy = AdjacentPlayersToDeploy.ToHashSet(),
                HoldedCardStack = new Stack<HoldCardStackElement>(HoldedCardStack.Select(e => e.Clone())),
                DiscardCardQueue = new Queue<DiscardInfo>(DiscardCardQueue.Select(e => e.Clone())),
                MaxQuaggothKills = MaxQuaggothKills,
                QuaggothKills = QuaggothKills,
                LastKillColor = LastKillColor,
                CurrentDiscardingCard = CurrentDiscardingCard,
            };

            return turn;
        }

        internal void MakeCurrentCardPlayed(EndCardAction endCardAction)
        {
            var cardState = CardStates.Single(s => s.State == CardState.NOW_PLAYING);

            if (HoldedCardStack.Any())
            {
                var prevCard = HoldedCardStack.Pop();

                var prevCardState = CardStates.First(s => s.State == CardState.HOLD
                    && s.CardLocation == prevCard.CardLocation
                    && prevCard.CardSpecificType == s.SpecificType
                );

                prevCardState.State = CardState.NOW_PLAYING;
                endCardAction.NextState = SelectionState.SELECT_CARD_OPTION;
                endCardAction.NextCardIteration = prevCard.CardStateIteration;
            }

            if (cardState.CardLocation == CardLocation.DEVOURED)
            {
                cardState.State = CardState.DEVOURED;
            }
            else if (cardState.CardLocation == CardLocation.INNER_CIRCLE)
            {
                cardState.State = CardState.PROMOTED;
            }
            else if (cardState.CardLocation == CardLocation.INSANE_OUTCASTS)
            {
                cardState.State = CardState.DEVOURED;
            }
            else
            {
                cardState.State = CardState.PLAYED;
            }

            PlacedSpies.Clear();
            ReturnedSpies.Clear();

            LockedAssasinationLocation = null;
            AdjacentPlayersToDeploy.Clear();

            QuaggothKills = 0;
            MaxQuaggothKills = 0;

            LastKillColor = null;
        }

        internal void MakeCurrentCardPlayedEndTurn()
        {
            CardStates.Single(s => s.EndTurnState == CardState.NOW_PLAYING).EndTurnState = CardState.PLAYED;
        }

        internal bool IsFocus(CardType cardType)
        {
            var count = CardStates
                .Count(s => CardMapper.SpecificTypeCardMakers[s.SpecificType].CardType == cardType
                    && (s.State == CardState.IN_HAND 
                        || s.State == CardState.NOW_PLAYING
                        || s.State == CardState.PLAYED)
                    && (s.CardLocation == CardLocation.IN_HAND)
                );

            return count > 1;
        }

        internal Card GetCard(Board board, CardSpecificType target, CardLocation targetLocation)
        {
            switch (targetLocation)
            {
                case CardLocation.IN_HAND:
                    return board.Players[Color].Hand.First(s => s.SpecificType == target);
                case CardLocation.DISCARD:
                    return board.Players[Color].Discard.First(s => s.SpecificType == target);
                case CardLocation.DEVOURED:
                    return board.Devoured.First(s => s.SpecificType == target);
                case CardLocation.MARKET:
                    if (target == CardSpecificType.LOLTH)
                    {
                        return CardMapper.SpecificTypeCardMakers[CardSpecificType.LOLTH].Clone();
                    }
                    else if (target == CardSpecificType.HOUSEGUARD)
                    {
                        return CardMapper.SpecificTypeCardMakers[CardSpecificType.HOUSEGUARD].Clone();
                    }
                    else
                    {
                        return board.Market.First(s => s.SpecificType == target);
                    }
                case CardLocation.INNER_CIRCLE:
                    return board.Players[Color].InnerCircle.First(s => s.SpecificType == target);
                case CardLocation.INSANE_OUTCASTS:
                    return CardMapper.SpecificTypeCardMakers[CardSpecificType.INSANE_OUTCAST].Clone();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal void RemoveCardFromLocation(Board board, Card targetCard, CardLocation targetLocation)
        {
            switch (targetLocation)
            {
                case CardLocation.IN_HAND:
                    board.Players[Color].Hand.Remove(targetCard);
                    break;
                case CardLocation.DISCARD:
                    board.Players[Color].Discard.Remove(targetCard);
                    break;
                case CardLocation.DEVOURED:
                    board.Devoured.Remove(targetCard);
                    break;
                case CardLocation.MARKET:
                    if (targetCard.SpecificType == CardSpecificType.LOLTH)
                    {
                        board.Lolths--;
                    }
                    else if (targetCard.SpecificType == CardSpecificType.HOUSEGUARD)
                    {
                        board.HouseGuards--;
                    }
                    else
                    {
                        board.Market.Remove(targetCard);
                    }
                    break;
                case CardLocation.INNER_CIRCLE:
                    board.Players[Color].InnerCircle.Remove(targetCard);
                    break;
                case CardLocation.INSANE_OUTCASTS:
                    board.InsaneOutcasts--;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal void MoveCardToLocation(Board board, Card targetCard, CardLocation targetLocation)
        {
            switch (targetLocation)
            {
                case CardLocation.IN_HAND:
                    board.Players[Color].Hand.Add(targetCard);
                    break;
                case CardLocation.DISCARD:
                    board.Players[Color].Discard.Add(targetCard);
                    break;
                case CardLocation.DEVOURED:
                    board.Devoured.Add(targetCard);
                    break;
                case CardLocation.MARKET:
                    if (targetCard.SpecificType == CardSpecificType.LOLTH)
                    {
                        board.Lolths++;
                    }
                    else if (targetCard.SpecificType == CardSpecificType.HOUSEGUARD)
                    {
                        board.HouseGuards++;
                    }
                    else
                    {
                        board.Market.Add(targetCard);
                    }
                    break;
                case CardLocation.INNER_CIRCLE:
                    board.Players[Color].InnerCircle.Add(targetCard);
                    break;
                case CardLocation.INSANE_OUTCASTS:
                    board.InsaneOutcasts++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal CardSpecificType? DrawNewCardOnMarket(Board board)
        {
            if (board.Deck.Count  == 0)
            {
                return null;
            }

            var card = board.Deck[0];

            board.Deck.Remove(card);

            board.Market.Add(card);

            return card.SpecificType;
        }

        internal void MoveCard(Board board, Card targetCard, CardLocation from, CardLocation to)
        {
            RemoveCardFromLocation(board, targetCard, from);
            MoveCardToLocation(board, targetCard, to);
        }
    }
}
