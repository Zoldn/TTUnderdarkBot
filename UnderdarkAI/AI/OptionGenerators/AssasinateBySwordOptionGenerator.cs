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

                    ret.Add(new AssassinateBySwordOption(location.Id, color, isBaseAction: true) { Weight = 1.0d });
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
        public bool IsBaseAction { get; }
        public AssassinateBySwordOption(LocationId locationId, Color targetColor, bool isBaseAction = false)
        {
            LocationId = locationId;
            TargetColor = targetColor;
            IsCityTaken = false;

            NextState = SelectionState.CARD_OR_FREE_ACTION;
            IsBaseAction = isBaseAction;
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
