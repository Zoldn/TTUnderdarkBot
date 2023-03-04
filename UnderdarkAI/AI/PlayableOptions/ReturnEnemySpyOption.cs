using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace UnderdarkAI.AI.PlayableOptions
{
    internal class ReturnEnemySpyOption : PlayableOption
    {
        public LocationId LocationId { get; }
        public Color TargetColor { get; }
        public override int MinVerbosity => 0;
        public bool IsBaseAction { get; }
        public ReturnEnemySpyOption(LocationId locationId, Color targetColor, int outIteration, bool isBaseAction = false) : base()
        {
            LocationId = locationId;
            TargetColor = targetColor;
            if (isBaseAction)
            {
                NextState = SelectionState.CARD_OR_FREE_ACTION;
            }
            else
            {
                NextState = SelectionState.SELECT_CARD_OPTION;
            }
            NextCardIteration = outIteration;
            IsBaseAction = isBaseAction;
        }

        public override void ApplyOption(Board board, Turn turn)
        {
            if (IsBaseAction)
            {
                turn.Swords -= 3;
            }

            board.LocationIds[LocationId].Spies[TargetColor] = false;

            board.Players[TargetColor].Spies++;
        }

        public override string GetOptionText()
        {
            if (IsBaseAction)
            {
                return $"Return {TargetColor} spy from {LocationId} by 3 swords";
            }

            return $"\tReturn {TargetColor} spy from {LocationId}";
        }
    }
}
