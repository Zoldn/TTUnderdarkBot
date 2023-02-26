using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using TUnderdark.TTSParser;

namespace UnderdarkAI.AI.CardEffects
{
    internal class DiscardCurrentEffect : IAtomicEffect
    {
        public bool IsNewInfoRecieved => false;

        public Card? Card { get; set; }

        public double Value { get; set; }

        public void ApplyEffect(Board board, Turn turn)
        {
            var sCard = turn.ActiveCard;

            if (sCard is null)
            {
                throw new NullReferenceException();
            }

            var cardState = turn.CardStates.FirstOrDefault(s => s.SpecificType == sCard);

            if (cardState is null)
            {
                throw new NullReferenceException();
            }

            cardState.State = CardState.DISCARDED;

            turn.State = SelectionState.CARD_OR_FREE_ACTION;
        }

        public void PrintEffect()
        {
            Console.WriteLine($"\tDiscarded {Card}");
        }
    }
    internal class ResourceGainEffect : IAtomicEffect
    {
        public int Mana { get; set; }
        public int Swords { get; set; }
        public ResourceGainEffect(CardSpecificType card, int mana = 0, int swords = 0)
        {
            Card = CardMapper.SpecificTypeCardMakers[card].Clone();
            Mana = mana;
            Swords = swords;
        }

        public bool IsNewInfoRecieved => false;

        public Card? Card { get; set; }

        public double Value { get; set; }

        public void ApplyEffect(Board board, Turn turn)
        {
            turn.Mana += Mana;
            turn.Swords += Swords;

            turn.State = SelectionState.CARD_OR_FREE_ACTION;

            if (turn.ActiveCard is null)
            {
                throw new NullReferenceException();
            }

            var card = turn.CardStates.FirstOrDefault(s => s.SpecificType == turn.ActiveCard);

            if (card is null)
            {
                throw new NullReferenceException();
            }

            card.State = CardState.PLAYED;
        }

        public void PrintEffect()
        {
            Console.WriteLine($"\tPlaying card {Card.Name} to gain {Mana} mana and {Swords} swords");
        }
    }

    internal class BuyCardEffect : IAtomicEffect
    {
        public BuyCardEffect(Card card)
        {
            Card = card;
        }

        public bool IsNewInfoRecieved { get; set; }

        public Card? Card { get; set; }
        public Card? NewCardOnMarket { get; set; }

        public double Value { get; set; }

        public void ApplyEffect(Board board, Turn turn)
        {
            if (Card is null)
            {
                throw new NullReferenceException();
            }

            turn.Mana -= Card.ManaCost;
            board.Market.Remove(Card);
            board.Players[turn.Color].Discard.Add(Card);

            IsNewInfoRecieved = false;

            if (Card.CardType != CardType.OBEDIENCE)
            {
                if (board.Deck.Count > 0)
                {
                    var newCard = board.Deck.First();
                    board.Market.Add(newCard);
                    board.Deck.Remove(newCard);

                    NewCardOnMarket = newCard;

                    IsNewInfoRecieved = true;
                }
            }
            else
            {
                if (Card.SpecificType == CardSpecificType.LOLTH)
                {
                    board.Lolths--;
                }
                if (Card.SpecificType == CardSpecificType.HOUSEGUARD)
                {
                    board.HouseGuards--;
                }
            }

            turn.State = SelectionState.CARD_OR_FREE_ACTION;
        }

        public void PrintEffect()
        {
            Console.WriteLine(TextInfo);
        }

        public override string ToString()
        {
            return TextInfo;
        }

        public string TextInfo
        {
            get
            {
                if (NewCardOnMarket != null)
                {
                    return $"Buying {Card}, new card on market is {NewCardOnMarket}";
                }
                else
                {
                    return $"Buying {Card}";
                }
            }
        }
    }

    internal class DeployUnitBySwordEffect : IAtomicEffect
    {
        public bool IsNewInfoRecieved => false;

        public Card? Card => null;
        public Location Location { get; }

        public double Value { get; set ; }
        public DeployUnitBySwordEffect(Location location)
        {
            Location = location;
        }

        public void ApplyEffect(Board board, Turn turn)
        {
            if (board.Players[turn.Color].Troops > 0)
            {
                board.Players[turn.Color].Troops--;

                Location.Troops[turn.Color] += 1;

                if (Location.Troops[turn.Color] == 1)
                {
                    foreach (var neighboor in Location.Neighboors)
                    {
                        turn.LocationStates[neighboor].HasPresence = true;
                    }
                }
            }
            else
            {
                board.Players[turn.Color].VPTokens++;
                //turn.VPs++;
            }
            
            turn.Swords--;

            turn.State = SelectionState.CARD_OR_FREE_ACTION;
        }

        public string TextInfo => $"Placing troop at {Location} by 1 sword";

        public void PrintEffect()
        {
            Console.WriteLine(TextInfo);
        }

        public override string ToString()
        {
            return TextInfo;
        }
    }

    internal class AssassinateBySwords : IAtomicEffect
    {
        public bool IsNewInfoRecieved => false;

        public Card? Card => null;

        public double Value { get; set; }
        public Color Color { get; }
        public Location Location { get; }
        public AssassinateBySwords(Location location, Color color)
        {
            Location = location;
            Color = color;
        }

        public void ApplyEffect(Board board, Turn turn)
        {
            turn.Swords -= 3;

            Location.Troops[Color]--;

            board.Players[turn.Color].TrophyHall[Color]++;

            turn.State = SelectionState.CARD_OR_FREE_ACTION;
        }

        public string TextInfo => $"Assasinate {Color} troop in {Location} by 3 swords";

        public void PrintEffect()
        {
            Console.WriteLine();
        }

        public override string ToString()
        {
            return TextInfo;
        }
    }

    internal class ReturnSpyBySwords : IAtomicEffect
    {
        public bool IsNewInfoRecieved => false;

        public Card? Card => null;

        public double Value { get; set; }
        public Color Color { get; }
        public Location Location { get; }
        public ReturnSpyBySwords(Location location, Color color)
        {
            Location = location;
            Color = color;
        }

        public void ApplyEffect(Board board, Turn turn)
        {
            turn.Swords -= 3;

            Location.Spies[Color] = false;
            board.Players[Color].Spies++;

            turn.State = SelectionState.CARD_OR_FREE_ACTION;
        }

        public string TextInfo => $"Return {Color} spy in {Location} by 3 swords";

        public void PrintEffect()
        {
            Console.WriteLine(TextInfo);
        }
        public override string ToString()
        {
            return TextInfo;
        }
    }
}
