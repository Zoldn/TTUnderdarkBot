using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace UnderdarkAI.AI.PlayableOptions
{
    internal class ReturnTroopOption : PlayableOption
    {
        public override int MinVerbosity => 0;
        public LocationId LocationId { get; }
        public Color Color { get; }
        public bool IsCityTaken { get; private set; }
        public ReturnTroopOption(LocationId locationId, Color color)
        {
            LocationId = locationId;
            Color = color;
            IsCityTaken = false;
        }

        public override void ApplyOption(Board board, Turn turn)
        {
            var location = board.LocationIds[LocationId];

            var prevControl = location.GetControlPlayer();

            location.Troops[Color]--;
            board.Players[Color].Troops++;

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
        }

        public override string GetOptionText()
        {
            if (IsCityTaken)
            {
                return $"\tReturn {Color} troop from {LocationId}, gain 1 mana for control this site";
            }

            return $"\tReturn {Color} troop from {LocationId}";
        }
    }
}
