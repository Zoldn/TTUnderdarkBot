using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.OptionGenerators;
using UnderdarkAI.Utils;

namespace UnderdarkAI.AI.PlayableOptions
{
    internal static class MoveTroopHelper
    {
        public static List<PlayableOption> Run(List<PlayableOption> options, Board board, Turn turn,
                int inIteration,
                int targetIteration,
                int outIteration
            )
        {
            if (turn.State == SelectionState.SELECT_CARD_OPTION
                && turn.CardStateIteration == inIteration)
            {
                OptionUtils.GetMoveFromOptions(options, board, turn,
                    targetIteration,
                    outIteration);
            }

            if (turn.State == SelectionState.SELECT_CARD_OPTION
                && turn.CardStateIteration == targetIteration)
            {
                OptionUtils.GetMoveToOptions(options, board, turn, outIteration);
            }

            return options;
        }
    }

    internal class MoveFromOption : PlayableOption
    {
        public LocationId LocationId { get; }
        public Color Color { get; }
        public MoveFromOption(LocationId locationId, Color color) : base()
        {
            LocationId = locationId;
            Color = color;
        }
        public override int MinVerbosity => 10;

        public override void ApplyOption(Board board, Turn turn)
        {
            turn.LocationMoveFrom = LocationId;
            turn.ColorMove = Color;
        }

        public override string GetOptionText()
        {
            return $"\tMoving {Color} troop from {LocationId}";
        }
    }

    internal class MoveToOption : PlayableOption
    {
        public LocationId LocationFrom { get; private set; }
        public Color Color { get; private set; }
        public LocationId LocationTo { get; }
        public bool IsFromCityTaken { get; private set; }
        public bool IsToCityLost { get; private set; }
        public MoveToOption(LocationId locationId) : base()
        {
            LocationTo = locationId;
            IsFromCityTaken = false;
            IsToCityLost = false;
        }
        public override int MinVerbosity => 0;

        public override void ApplyOption(Board board, Turn turn)
        {
            if (!turn.LocationMoveFrom.HasValue || !turn.ColorMove.HasValue)
            {
                throw new NullReferenceException();
            }

            LocationFrom = turn.LocationMoveFrom.Value;
            Color = turn.ColorMove.Value;

            var locationFrom = board.LocationIds[LocationFrom];
            var locationTo = board.LocationIds[LocationTo];

            var prevFromControl = locationFrom.GetControlPlayer();
            var prevToControl = locationTo.GetControlPlayer();

            locationFrom.Troops[Color]--;
            locationTo.Troops[Color]++;

            var nowFromControl = locationFrom.GetControlPlayer();
            var nowToControl = locationTo.GetControlPlayer();

            ///Проверяем, захватили ли этим деплоем город
            ///Если да, то добавляем 1 ману
            if (locationFrom.BonusMana > 0 && prevFromControl != turn.Color && nowFromControl == turn.Color)
            {
                turn.Mana += 1;
                IsFromCityTaken = true;
            }
            else
            {
                IsFromCityTaken = false;
            }

            ///Проверяем, захватили ли этим деплоем город
            ///Если да, то добавляем 1 ману
            if (locationTo.BonusMana > 0 && prevToControl == turn.Color && nowToControl != turn.Color)
            {
                turn.Mana -= 1;
                IsToCityLost = true;
            }
            else
            {
                IsToCityLost = false;
            }

            turn.LocationMoveFrom = null;
            turn.ColorMove = null;
        }

        public override string GetOptionText()
        {
            string suffix1 = IsFromCityTaken ? $", gain 1 mana for control {LocationFrom} site" : "";
            string suffix2 = IsToCityLost ? $", loss 1 mana for control {LocationTo} site" : "";

            return $"\tMoving {Color} troop from {LocationFrom} to {LocationTo}{suffix1}{suffix2}";
        }
    }
}
