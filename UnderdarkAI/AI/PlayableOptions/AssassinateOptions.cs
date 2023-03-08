using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace UnderdarkAI.AI.PlayableOptions
{
    internal static class AssassinateOptionHelper
    {
        internal static List<PlayableOption> Run(List<PlayableOption> options, Board board, Turn turn,
            int inIteration,
            int outIteration,
            bool isOnlyWhite = false,
            HashSet<LocationId>? specificLocation = null, 
            bool isLockingNextAssassination = false,
            bool isAnyWhere = false)
        {
            if (turn.State == SelectionState.SELECT_CARD_OPTION
                && turn.CardStateIteration == inIteration)
            {
                foreach (var (location, locationState) in turn.LocationStates)
                {
                    if (!locationState.HasPresence && !isAnyWhere)
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

                        options.Add(new AssassinateOption(location.Id, color, outIteration,
                            isLockingNextAssassination: isLockingNextAssassination));
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

    internal class AssassinateOption : PlayableOption
    {
        public LocationId LocationId { get; }
        public Color TargetColor { get; }
        public override int MinVerbosity => 0;
        public bool IsCityTaken { get; private set; }
        public bool IsBaseAction { get; }
        public bool IsLockingNextAssassinateLocation { get; private set; }
        public AssassinateOption(LocationId locationId, Color targetColor, int outIteration, bool isBaseAction = false, 
            bool isLockingNextAssassination = false) : base()
        {
            LocationId = locationId;
            TargetColor = targetColor;
            IsBaseAction = isBaseAction;
            IsCityTaken = false;
            IsLockingNextAssassinateLocation = isLockingNextAssassination;

            NextState = isBaseAction ? SelectionState.CARD_OR_FREE_ACTION : SelectionState.SELECT_CARD_OPTION;
            NextCardIteration = outIteration;
        }

        public override void ApplyOption(Board board, Turn turn)
        {
            if (IsBaseAction)
            {
                turn.Swords -= 3;
            }

            var location = board.LocationIds[LocationId];

            var prevControl = location.GetControlPlayer();

            location.Troops[TargetColor]--;

            board.Players[turn.Color].TrophyHall[TargetColor]++;

            var nowControl = location.GetControlPlayer();

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

            if (IsLockingNextAssassinateLocation)
            {
                turn.LockedAssasinationLocation = LocationId;
            }

            turn.LastKillColor = TargetColor;
        }

        public override string GetOptionText()
        {
            string price = IsBaseAction ? " by 3 swords" : "";
            string manaGet = IsCityTaken ? ", gain 1 mana for control this site" : "";
            string prefix = IsBaseAction ? "" : "\t";

            return $"{prefix}Assassinate {TargetColor} troop in {LocationId}{price}{manaGet}";
        }
    }
}
