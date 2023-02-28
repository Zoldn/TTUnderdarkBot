using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace UnderdarkAI.AI.OptionGenerators
{
    internal class AssasinateBySwordOptionGenerator : OptionGenerator
    {
        public override SelectionState State => SelectionState.ASSASSINATE_BY_SWORD;

        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            if (turn.Swords < 3)
            {
                throw new ArgumentOutOfRangeException();
            }

            var ret = new List<PlayableOption>();

            foreach (var (location, locationState) in turn.LocationStates)
            {
                if (!locationState.HasPresence)
                {
                    continue;
                }

                foreach (var (color, troop) in location.Troops)
                {
                    if (color == turn.Color || troop == 0)
                    {
                        continue;
                    }

                    ret.Add(new AssassinateBySwordOption(location.Id, color) { Weight = 1.0d });
                }
            }

            return ret;
        }
    }

    internal class AssassinateBySwordOption : PlayableOption
    {
        public LocationId LocationId { get; }
        public Color TargetColor { get; }
        public override int MinVerbosity => 0;
        public bool IsCityTaken { get; private set; }
        public AssassinateBySwordOption(LocationId locationId, Color targetColor)
        {
            LocationId = locationId;
            TargetColor = targetColor;
            IsCityTaken = false;
        }

        public override void ApplyOption(Board board, Turn turn)
        {
            turn.Swords -= 3;

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
        }

        public override void UpdateTurnState(Turn turn)
        {
            turn.State = SelectionState.CARD_OR_FREE_ACTION;
        }

        public override string GetOptionText()
        {
            if (IsCityTaken)
            {
                return $"Assassinate {TargetColor} troop in {LocationId} by 3 swords, gain 1 mana for control this site";
            }
            return $"Assassinate {TargetColor} troop in {LocationId} by 3 swords";
        }
    }
}
