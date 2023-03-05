using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.OptionGenerators;

namespace UnderdarkAI.AI.PlayableOptions
{
    internal static class TakeTroopFromEnemyTrophyAndDeploy 
    {
        public static List<PlayableOption> Run(List<PlayableOption> options, Board board, Turn turn,
            int inIteration,
            int outIteration,
            int exitIteration,
            HashSet<Color>? filterPlayerColors = null,
            bool isOnlyWhite = false,
            bool isAnywhere = false)
        {
            if (turn.State == SelectionState.SELECT_CARD_OPTION
                && turn.CardStateIteration == inIteration)
            {
                if (!OptionUtils.IsDeployAvailable(board, turn, isAnywhere: isAnywhere))
                {
                    options.Add(new DoNothingOption(exitIteration));
                }

                foreach (var (playerColor, player) in board.Players)
                {
                    if (playerColor == turn.Color)
                    {
                        continue;
                    }

                    if (filterPlayerColors != null && !filterPlayerColors.Contains(playerColor))
                    {
                        continue;
                    }

                    foreach (var (troopColor, count) in player.TrophyHall)
                    {
                        if (count == 0)
                        {
                            continue;
                        }

                        if (isOnlyWhite && troopColor != Color.WHITE)
                        {
                            continue;
                        }

                        options.Add(new TakeTroopFromEnemyTrophyOption(
                            playerColor, troopColor, outIteration)
                        {
                            Weight = 1.0d + (troopColor == turn.Color ? 10.0d : 0.0d) +
                                (troopColor == Color.WHITE ? 4.0d : 0.0d)
                        });
                    }
                }

                if (options.Count == 0)
                {
                    options.Add(new DoNothingOption(exitIteration));
                }
            }

            return options;
        }
    }

    internal class TakeTroopFromEnemyTrophyOption : PlayableOption
    {
        public Color TargetPlayerColor { get; }
        public Color TargetTroopColor { get; }
        public TakeTroopFromEnemyTrophyOption(Color targetPlayerColor, Color targetTroopColor,
            int outIteration) : base()
        {
            TargetPlayerColor = targetPlayerColor;
            TargetTroopColor = targetTroopColor;
            NextCardIteration = outIteration;
        }

        public override int MinVerbosity => 0;

        public override void ApplyOption(Board board, Turn turn)
        {
            board.Players[TargetPlayerColor].TrophyHall[TargetTroopColor]--;

            turn.TakeFromTroopsColor = TargetTroopColor;
        }

        public override string GetOptionText()
        {
            return $"\tTaking {TargetTroopColor} troops from {TargetPlayerColor} player";
        }
    }
}
