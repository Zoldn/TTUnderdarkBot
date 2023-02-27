using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace UnderdarkAI.AI.OptionGenerators
{
    internal class ReturnSpyBySwordOptionGenerator : OptionGenerator
    {
        public override SelectionState State => SelectionState.RETURN_ENEMY_SPY_BY_SWORD;

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

                foreach (var (color, isSpy) in location.Spies)
                {
                    if (color == turn.Color || !isSpy)
                    {
                        continue;
                    }

                    ret.Add(new ReturnSpyBySwordOption(location.Id, color) { Weight = 1.0d });
                }
            }

            return ret;
        }
    }

    internal class ReturnSpyBySwordOption : PlayableOption
    {
        public LocationId LocationId { get; }
        public Color TargetColor { get; }
        public override int MinVerbosity => 0;
        public ReturnSpyBySwordOption(LocationId locationId, Color targetColor)
        {
            LocationId = locationId;
            TargetColor = targetColor;
        }

        public override void ApplyOption(Board board, Turn turn)
        {
            turn.Swords -= 3;

            board.LocationIds[LocationId].Spies[TargetColor] = false;

            board.Players[TargetColor].Spies++;
        }

        public override void UpdateTurnState(Turn turn)
        {
            turn.State = SelectionState.CARD_OR_FREE_ACTION;
        }

        public override string GetOptionText()
        {
            return $"Return {TargetColor} spy from {LocationId} by 3 swords";
        }
    }
}
