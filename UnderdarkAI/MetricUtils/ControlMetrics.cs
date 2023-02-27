﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI;

namespace UnderdarkAI.MetricUtils
{
    internal static class ControlMetrics
    {
        public static void GetStartManaFromSites(Board board, Player player, Turn turn)
        {
            foreach (var location in board.Locations)
            {
                if (!location.HasControlMarker)
                {
                    continue;
                }

                if (location.GetControlPlayer() != player.Color)
                {
                    continue;
                }

                turn.Mana += location.BonusMana;
            }
        }

        public static void GetVPForSiteControlMarkersInTheEnd(Board board, Turn turn, int verbosity = 0)
        {
            foreach (var location in board.Locations)
            {
                if (location.BonusVP == 0)
                {
                    continue;
                }

                if (location.GetFullControl() != turn.Color)
                {
                    continue;
                }

                //turn.VPs += location.BonusVP;
                board.Players[turn.Color].VPTokens += location.BonusVP;

                if (verbosity == 0)
                {
                    Console.WriteLine($"Gain {location.BonusVP} VP for {location.Name}");
                }
            }
        }

        internal static void PromoteCardsInTheEnd(Board board, Turn turn)
        {
            foreach (var cardState in turn.CardStates)
            {
                if (!cardState.IsPromotedInTheEnd)
                {
                    continue;
                }

                var target = board.Players[turn.Color].Hand
                    .First(c => c.SpecificType == cardState.SpecificType);

                board.Players[turn.Color].Hand.Remove(target);

                board.Players[turn.Color].InnerCircle.Add(target);
            }
        }
    }
}
