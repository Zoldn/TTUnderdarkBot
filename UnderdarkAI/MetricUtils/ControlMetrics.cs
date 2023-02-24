using System;
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

        public static void GetVPForSiteControlMarkersInTheEnd(Board board, Turn turn)
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

                turn.VPs += location.BonusVP;
            }
        }
    }
}
