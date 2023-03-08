using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using TUnderdark.TTSParser;

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

            return $"\tForce {players} player(s) to discard a card from hand";
        }
    }
}
