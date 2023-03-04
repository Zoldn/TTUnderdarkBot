using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using TUnderdark.TTSParser;
using UnderdarkAI.Utils;

namespace UnderdarkAI.AI.PlayableOptions
{
    internal class DrawCardsOption : PlayableOption
    {
        public int CardsToDraw { get; }
        public List<CardSpecificType> DrawnTypes { get; private set; }
        public DrawCardsOption(int cardCount, int outIteration) : base() 
        { 
            NextCardIteration = outIteration;
            CardsToDraw = cardCount;
            DrawnTypes = new(cardCount);
        }
        public override int MinVerbosity => 0;
        private void MoveCardsToHand(Turn turn, Player player, List<Card> takenCards)
        {
            foreach (var takenCard in takenCards)
            {
                player.Hand.Add(takenCard);
                turn.CardStates.Add(new TurnCardState(takenCard.SpecificType));
                player.Deck.Remove(takenCard);
                DrawnTypes.Add(takenCard.SpecificType);
            }
        }
        public override void ApplyOption(Board board, Turn turn)
        {
            int cardsToDrow = CardsToDraw;

            var player = board.Players[turn.Color];

            List<Card> takenCards;

            if (player.Deck.Count >= cardsToDrow)
            {
                takenCards = player.Deck
                    .Take(cardsToDrow)
                    .ToList();

                MoveCardsToHand(turn, player, takenCards);

                return;
            }

            cardsToDrow -= player.Deck.Count;

            takenCards = player.Deck;

            MoveCardsToHand(turn, player, takenCards);

            player.Deck.AddRange(player.Discard);
            player.Discard.Clear();

            player.Deck.Shuffle(turn.Random);

            takenCards = player.Deck
                .Take(cardsToDrow)
                .ToList();

            MoveCardsToHand(turn, player, takenCards);
        }

        public override string GetOptionText()
        {
            var drawnCardNames = string.Join(", ", DrawnTypes
                .Select(c => CardMapper.SpecificTypeCardMakers[c].Name));

            return $"\tDrawing cards: {drawnCardNames}";
        }
    }
}
