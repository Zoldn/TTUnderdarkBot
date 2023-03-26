using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TUnderdark.Model
{

    public class Player
    {
        public Color Color { get; set; }
        public List<Card> Deck { get; set; }
        public List<Card> Hand { get; set; }
        public List<Card> Discard { get; set; }
        public List<Card> InnerCircle { get; set; }
        public Dictionary<Color, int> TrophyHall { get; set; }
        public int VPTokens { get; set; }
        public int Spies { get; set; }
        public int Troops { get; set; }
        public int TrophyHallVP => VPTokens
            + TrophyHall.Sum(kv => kv.Value);
        public int DeckVP => Deck.Sum(c => c.VP)
            + Hand.Sum(c => c.VP)
            + Discard.Sum(c => c.VP);
        public int PromoteVP => InnerCircle.Sum(c => c.PromoteVP);
        public int VP => DeckVP + PromoteVP + TrophyHallVP;

        public int CurrentDeckSize => Deck.Count + Hand.Count + Discard.Count;
        public bool IsFirstPlayer { get; set; }
        public bool IsReturnableSpies => Spies < 5;

        public string Name { get; set; }
        public string SteamId { get; set; }
        /// <summary>
        /// Является ли игрок человеком или ботом
        /// </summary>
        public bool IsHuman { get; set; }

        public Player(Color color)
        {
            Color = color;
            Deck = new List<Card>();
            Hand = new List<Card>();
            Discard = new List<Card>();
            InnerCircle = new List<Card>();

            TrophyHall = Enum.GetValues(typeof(Color))
                .Cast<Color>()
                .ToDictionary(c => c, c => 0);

            VPTokens = 0;
            Spies = 5;
            Troops = 40;

            IsFirstPlayer = false;

            Name = color.ToString();
            SteamId = string.Empty;
        }

        public Player Clone()
        {
            var player = new Player(Color)
            {
                Deck = Deck.Select(c => c.Clone()).ToList(),
                Hand = Hand.Select(c => c.Clone()).ToList(),
                Discard = Discard.Select(c => c.Clone()).ToList(),
                InnerCircle = InnerCircle.Select(c => c.Clone()).ToList(),
                TrophyHall = TrophyHall.ToDictionary(kv => kv.Key, kv => kv.Value),
                VPTokens = VPTokens,
                Spies = Spies,
                Troops = Troops,
                Name = Name,
                SteamId = SteamId,
                IsFirstPlayer = IsFirstPlayer,
                IsHuman = IsHuman,
            };

            return player;
        }
    }
}
