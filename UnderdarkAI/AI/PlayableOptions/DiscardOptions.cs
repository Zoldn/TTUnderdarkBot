using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using TUnderdark.TTSParser;
using UnderdarkAI.Utils;

namespace UnderdarkAI.AI.PlayableOptions
{
    internal static class OpponentDiscardHelper
    {
        public static void Run(List<PlayableOption> options, Board board, Turn turn,
            CardSpecificType initiator,
            HashSet<Color> specificTargets,
            int inIteration,
            int outIteration,
            bool isToAll = false,
            int cardLimit = 3
            )
        {
            if (turn.State == SelectionState.SELECT_CARD_OPTION
                && turn.CardStateIteration == inIteration)
            {
                var legalTargets = specificTargets
                    .Where(c => board.Players[c].Hand.Count > cardLimit)
                    .ToList();

                if (!isToAll)
                {
                    foreach (var target in legalTargets)
                    {
                        options.Add(new OpponentDiscardOption(new List<Color>(1) { target }, initiator, outIteration));
                    }
                }
                else
                {
                    options.Add(new OpponentDiscardOption(legalTargets, initiator, outIteration));
                }
                
                if (options.Count == 0)
                {
                    options.Add(new DoNothingOption(outIteration));
                }
            }
        }

        internal static void RunEndTurn(List<PlayableOption> options, Board board, 
            Turn turn, CardSpecificType initiator, 
            HashSet<Color> specificTargets, 
            int inIteration, int outIteration, 
            bool isToAll = false, 
            int cardLimit = 3)
        {
            if (turn.State == SelectionState.SELECT_END_TURN_CARD_OPTION
                && turn.CardStateIteration == inIteration)
            {
                var legalTargets = specificTargets
                    .Where(c => board.Players[c].Hand.Count > cardLimit)
                    .ToList();

                if (!isToAll)
                {
                    foreach (var target in legalTargets)
                    {
                        options.Add(new OpponentDiscardOption(new List<Color>(1) { target }, initiator, outIteration) 
                        {
                            NextState = SelectionState.SELECT_END_TURN_CARD_OPTION
                        });
                    }
                }
                else
                {
                    options.Add(new OpponentDiscardOption(legalTargets, initiator, outIteration) 
                    {
                        NextState = SelectionState.SELECT_END_TURN_CARD_OPTION
                    });
                }

                if (options.Count == 0)
                {
                    options.Add(new DoNothingEndTurnOption(outIteration));
                }
            }
        }
    }

    internal class DiscardInfo
    {
        public Color TargetPlayerColor { get; }
        public Color SourcePlayerColor { get; }
        public CardSpecificType CardSpecificType { get; }
        public DiscardInfo(Color sourcePlayerColor, Color targetPlayerColor, CardSpecificType cardSpecificType)
        {
            TargetPlayerColor = targetPlayerColor;
            SourcePlayerColor = sourcePlayerColor;
            CardSpecificType = cardSpecificType;
        }
        public DiscardInfo Clone()
        {
            return new DiscardInfo(SourcePlayerColor, TargetPlayerColor, CardSpecificType);
        }
        public override string ToString()
        {
            return $"Player {TargetPlayerColor} discarding card due to player's {SourcePlayerColor} " +
                $"{CardMapper.SpecificTypeCardMakers[CardSpecificType].Name}";
        }
    }

    internal class OpponentDiscardOption : PlayableOption
    {
        public override int MinVerbosity => 0;
        public List<Color> TargetPlayersColor { get; }
        public CardSpecificType CardSpecificType { get; }
        public OpponentDiscardOption(List<Color> targetPlayers, CardSpecificType initiator, int outIteration) : base()
        {
            TargetPlayersColor = targetPlayers;
            CardSpecificType = initiator;
            NextCardIteration = outIteration;
        }
        public override void ApplyOption(Board board, Turn turn)
        {
            foreach (var color in TargetPlayersColor)
            {
                turn.DiscardCardQueue.Enqueue(new DiscardInfo(turn.Color, color, CardSpecificType));
            }
        }

        public override string GetOptionText()
        {
            string players = TargetPlayersColor.Count > 0 ? string.Join(", ", TargetPlayersColor) : "none";

            return $"\tForce {players} player(s) to discard a card from hand by " +
                $"{CardMapper.SpecificTypeCardMakers[CardSpecificType].Name}";
        }
    }

    internal class NeogiActivation : PlayableOption
    {
        public override int MinVerbosity => 10;
        public NeogiActivation(int outIteration)
        {
            NextCardIteration = outIteration;
        }
        public override void ApplyOption(Board board, Turn turn)
        {
            var st = turn.CardStates.Single(e => e.State == CardState.NOW_PLAYING);

            st.EndTurnState = CardState.IN_HAND;
        }

        public override string GetOptionText()
        {
            return $"\tNeogi end turn activation";
        }
    }

    internal static class ChooseDiscardHelper
    {
        public static void Run(List<PlayableOption> options, Board board,
            Turn turn, CardSpecificType initiator, Color targetPlayer, bool isEndTurn)
        {
            if (turn.State == SelectionState.ON_DISCARD_CARD_SELECTION
                || turn.State == SelectionState.END_TURN_ON_DISCARD_CARD_SELECTION)
            {
                List<CardSpecificType> cardsInHand;

                if (targetPlayer == turn.Color)
                {
                    cardsInHand = turn.CardStates
                        .Where(s => s.State == CardState.IN_HAND)
                        .Select(s => s.SpecificType)
                        .Distinct()
                        .ToList();
                }
                else
                {
                    cardsInHand = board.Players[targetPlayer]
                        .Hand
                        .Select(s => s.SpecificType)
                        .Distinct()
                        .ToList();
                }

                foreach (var card in cardsInHand)
                {
                    options.Add(new ChooseCardToDiscard(targetPlayer, card) 
                    {
                        NextState = isEndTurn ? SelectionState.END_TURN_ON_DISCARD_CARD :
                            SelectionState.ON_DISCARD_CARD
                    });
                }

                if (options.Count == 0)
                {
                    options.Add(new NoCardToDiscardOption(targetPlayer)
                    {
                        NextState = isEndTurn ? SelectionState.SELECT_CARD_END_TURN :
                            SelectionState.CARD_OR_FREE_ACTION
                    });
                }
            }
        }
    }

    internal class ChooseCardToDiscard : PlayableOption
    {
        public Color PlayerColor { get; }
        public CardSpecificType DiscardedCard { get; }
        public ChooseCardToDiscard(Color color, CardSpecificType discardedCard) : base()
        {
            PlayerColor = color;
            DiscardedCard = discardedCard;
            NextCardIteration = 0;
            NextState = SelectionState.ON_DISCARD_CARD;
        }

        public override int MinVerbosity => 0;

        public override void ApplyOption(Board board, Turn turn)
        {
            turn.CurrentDiscardingCard = DiscardedCard;
        }

        public override string GetOptionText()
        {
            return $"\tDiscarding " +
                $"{CardMapper.SpecificTypeCardMakers[DiscardedCard].Name} from hand by {PlayerColor}";
        }
    }

    internal class NoCardToDiscardOption : PlayableOption
    {
        public Color TargetPlayer { get; }
        public NoCardToDiscardOption(Color targetPlayer) : base()
        {
            NextCardIteration = 0;
            TargetPlayer = targetPlayer;
            NextState = SelectionState.CARD_OR_FREE_ACTION;
        }

        public override int MinVerbosity => 0;

        public override void ApplyOption(Board board, Turn turn)
        {
            turn.DiscardCardQueue.Dequeue();
        }

        public override string GetOptionText()
        {
            return $"\tNothing to discard from hand";
        }
    }

    internal class PromoteSelfOnDiscardOption : PlayableOption
    {
        public Color TargetPlayer { get; }
        public CardSpecificType SpecificType { get; }
        public PromoteSelfOnDiscardOption(Color targetPlayer, CardSpecificType specificType) : base()
        {
            TargetPlayer = targetPlayer;
            NextState = SelectionState.ON_DISCARD_CARD;
            SpecificType = specificType;
        }

        public override int MinVerbosity => 0;

        public override void ApplyOption(Board board, Turn turn)
        {
            if (turn.Color == TargetPlayer)
            {
                var state = turn.CardStates
                    .First(s => s.SpecificType == SpecificType && s.State == CardState.IN_HAND);

                state.State = CardState.DISCARDED;
            }

            var player = board.Players[TargetPlayer];

            var card = player.Hand.First(c => c.SpecificType == SpecificType);

            player.Hand.Remove(card);
            player.InnerCircle.Add(card);

            turn.CurrentDiscardingCard = null;
            turn.DiscardCardQueue.Dequeue();
        }

        public override string GetOptionText()
        {
            return $"\t{CardMapper.SpecificTypeCardMakers[SpecificType].Name} " +
                $"promote self on discard";
        }
    }

    internal class DrawCardOnDiscardOption : PlayableOption
    {
        public Color TargetPlayer { get; }
        public CardSpecificType SpecificType { get; }
        public List<CardSpecificType> DrawnCards { get; private set; }
        public int DrawCards { get; }
        public DrawCardOnDiscardOption(Color targetPlayer, CardSpecificType specificType, int cards) : base()
        {
            TargetPlayer = targetPlayer;
            NextState = SelectionState.ON_DISCARD_CARD;
            SpecificType = specificType;
            DrawCards = cards;
            DrawnCards = new(cards);
        }

        public override int MinVerbosity => 0;

        public override void ApplyOption(Board board, Turn turn)
        {
            if (turn.Color == TargetPlayer)
            {
                var state = turn.CardStates
                    .First(s => s.SpecificType == SpecificType && s.State == CardState.IN_HAND);

                state.State = CardState.DISCARDED;
            }

            var player = board.Players[TargetPlayer];

            var card = player.Hand.First(c => c.SpecificType == SpecificType);

            player.Hand.Remove(card);
            player.Discard.Add(card);

            turn.CurrentDiscardingCard = null;
            turn.DiscardCardQueue.Dequeue();

            int cardToDrawLeft = DrawCards;

            if (cardToDrawLeft > player.Deck.Count)
            {
                cardToDrawLeft -= player.Deck.Count;
            }
            else
            {
                cardToDrawLeft = 0;
            }

            var drawnCards = player.Deck.Take(DrawCards).ToList();

            foreach (var drawnCard in drawnCards)
            {
                player.Hand.Add(drawnCard);
                player.Deck.Remove(drawnCard);
                DrawnCards.Add(drawnCard.SpecificType);

                if (turn.Color == TargetPlayer)
                {
                    var state = new TurnCardState(drawnCard.SpecificType, CardState.IN_HAND);

                    turn.CardStates.Add(state);
                }
            }

            if (cardToDrawLeft > 0)
            {
                player.Deck.AddRange(player.Discard);
                player.Deck.Shuffle(turn.Random);
                player.Discard.Clear();

                drawnCards = player.Deck.Take(cardToDrawLeft).ToList();

                foreach (var drawnCard in drawnCards)
                {
                    player.Hand.Add(drawnCard);
                    player.Deck.Remove(drawnCard);
                    DrawnCards.Add(drawnCard.SpecificType);

                    if (turn.Color == TargetPlayer)
                    {
                        var state = new TurnCardState(drawnCard.SpecificType, CardState.IN_HAND);

                        turn.CardStates.Add(state);
                    }
                }
            }
        }

        public override string GetOptionText()
        {
            string cards = DrawnCards.Count > 0 ? ": " + string.Join(", ", DrawnCards
                .Select(c => CardMapper.SpecificTypeCardMakers[c].Name)) : "";

            return $"\t{CardMapper.SpecificTypeCardMakers[SpecificType].Name} " +
                $"draw {DrawCards} card(s){cards}";
        }
    }

    internal class NoEffectDiscardOption : PlayableOption
    {
        public Color TargetPlayer { get; }
        public CardSpecificType SpecificType { get; }
        public NoEffectDiscardOption(Color targetPlayer, CardSpecificType specificType) : base()
        {
            TargetPlayer = targetPlayer;
            NextState = SelectionState.ON_DISCARD_CARD;
            SpecificType = specificType;
        }

        public override int MinVerbosity => 10;

        public override void ApplyOption(Board board, Turn turn)
        {
            if (turn.Color == TargetPlayer)
            {
                var state = turn.CardStates
                    .First(s => s.SpecificType == SpecificType && s.State == CardState.IN_HAND);

                state.State = CardState.DISCARDED;
            }

            var player = board.Players[TargetPlayer];

            var card = player.Hand.First(c => c.SpecificType == SpecificType);

            player.Hand.Remove(card);
            player.Discard.Add(card);

            turn.CurrentDiscardingCard = null;
            turn.DiscardCardQueue.Dequeue();
        }

        public override string GetOptionText()
        {
            return $"\t{CardMapper.SpecificTypeCardMakers[SpecificType].Name} " +
                $" has discarded by {TargetPlayer}";
        }
    }

    internal class ReverseDiscardOption : PlayableOption
    {
        public Color TargetPlayer { get; }
        public Color SourcePlayer { get; }
        public CardSpecificType SpecificType { get; }
        public ReverseDiscardOption(Color targetPlayer, Color sourcePlayer, CardSpecificType specificType) : base()
        {
            //NextCardIteration = outIteration;
            TargetPlayer = targetPlayer;
            NextState = SelectionState.ON_DISCARD_CARD;
            SpecificType = specificType;
            SourcePlayer = sourcePlayer;
        }

        public override int MinVerbosity => 0;

        public override void ApplyOption(Board board, Turn turn)
        {
            if (turn.Color == TargetPlayer)
            {
                var state = turn.CardStates
                    .First(s => s.SpecificType == SpecificType && s.State == CardState.IN_HAND);

                state.State = CardState.DISCARDED;
            }

            var player = board.Players[TargetPlayer];

            var card = player.Hand.First(c => c.SpecificType == SpecificType);

            player.Hand.Remove(card);
            player.Discard.Add(card);

            turn.CurrentDiscardingCard = null;
            turn.DiscardCardQueue.Dequeue();

            turn.DiscardCardQueue.Enqueue(new DiscardInfo(TargetPlayer, SourcePlayer, SpecificType));
        }

        public override string GetOptionText()
        {
            return $"\t{CardMapper.SpecificTypeCardMakers[SpecificType].Name} " +
                $" has discarded by {TargetPlayer} and force {SourcePlayer} to discard a card";
        }
    }
}
