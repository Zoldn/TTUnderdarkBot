using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace UnderdarkAI.AI.PlayableOptions
{
    internal static class SupplantOptionHelper
    {
        public static List<PlayableOption> Run(List<PlayableOption> options, Board board, Turn turn,
            int inIteration,
            int outIteration,
            bool isAnywhere = false,
            bool isOnlyWhite = false,
            HashSet<LocationId>? specificLocation = null
            )
        {
            if (turn.State == SelectionState.SELECT_CARD_OPTION
                && turn.CardStateIteration == inIteration)
            {
                //Debug.Assert(!isAnywhere || specificLocation == null);

                foreach (var (location, locationState) in turn.LocationStates)
                {
                    if (!locationState.HasPresence && !isAnywhere)
                    {
                        continue;
                    }

                    if (specificLocation != null && !specificLocation.Contains(location.Id))
                    {
                        continue;
                    }

                    foreach (var (color, count) in location.Troops)
                    {
                        if (count == 0 || color == turn.Color)
                        {
                            continue;
                        }

                        if (color != Color.WHITE && isOnlyWhite)
                        {
                            continue;
                        }

                        options.Add(new SupplantOption(location.Id, color, outIteration));
                    }
                }

                if (options.Count == 0)
                {
                    options.Add(new DoNothingOption(outIteration));
                }
            }

            return options;
        }
    }

    internal class SupplantOption : PlayableOption
    {
        public LocationId LocationId { get; }
        public Color Color { get; }
        public bool IsCityTaken { get; private set; }
        public bool IsDeployed { get; private set; }
        public SupplantOption(LocationId locationId, Color color, int outIteration)
        {
            LocationId = locationId;
            Color = color;
            NextCardIteration = outIteration;
            IsCityTaken = false;
            IsDeployed = false;
        }

        public override int MinVerbosity => 0;

        public override void ApplyOption(Board board, Turn turn)
        {
            /// Assassinate
            board.Players[turn.Color].TrophyHall[Color]++;

            var location = board.LocationIds[LocationId];

            var prevControl = location.GetControlPlayer();

            location.Troops[Color]--;
            /// Deploy

            if (board.Players[turn.Color].Troops > 0)
            {
                board.Players[turn.Color].Troops--;
                location.Troops[turn.Color] += 1;
                IsDeployed = true;
            }
            else
            {
                board.Players[turn.Color].VPTokens++;
                IsDeployed = false;
            }
            
            var nowControl = location.GetControlPlayer();

            /// Обновляем контроль при необходимости
            if (location.Troops[turn.Color] == 1)
            {
                foreach (var neighboor in location.Neighboors)
                {
                    turn.LocationStates[neighboor].HasPresence = true;
                }
            }

            ///Проверяем, захватили ли этим деплоем город
            ///Если да, то добавляем 1 ману
            if (location.BonusMana > 0 && prevControl != turn.Color && nowControl == turn.Color)
            {
                turn.Mana += 1;
                IsCityTaken = true;
            }
            else
            {
                IsCityTaken = false;
            }
        }

        public override string GetOptionText()
        {
            string suffix1 = IsCityTaken ? ", gain 1 mana for control this site" : "";
            string suffix2 = IsDeployed ? "" : ", out of troops, gained 1 VP instead";

            return $"\tSupplant {Color} troop in {LocationId}{suffix1}{suffix2}";
        }
    }
}
