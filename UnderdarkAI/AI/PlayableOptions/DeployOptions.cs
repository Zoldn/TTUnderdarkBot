using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace UnderdarkAI.AI.PlayableOptions
{
    internal static class DeployOptionHelper
    {
        public static List<PlayableOption> Run(List<PlayableOption> options, Board board, Turn turn,
            int inIteration, 
            int outIteration, 
            bool isFromTrophy = false, bool isAnywhere = false)
        {
            if (turn.State == SelectionState.SELECT_CARD_OPTION
                && turn.CardStateIteration == inIteration)
            {
                if (board.Players[turn.Color].Troops == 0 && !isFromTrophy)
                {

                    options.Add(new DeployWithOutTroopsOption(outIteration) { Weight = 1.0d });
                }
                else
                {
                    Color deployableColor = isFromTrophy ? turn.TakeFromTroopsColor.Value : turn.Color;

                    foreach (var (location, locationState) in turn.LocationStates)
                    {
                        if (!isAnywhere && !locationState.HasPresence)
                        {
                            continue;
                        }

                        if (location.FreeSpaces == 0)
                        {
                            continue;
                        }

                        options.Add(new DeployOption(location.Id, outIteration, 
                            color: deployableColor));
                    }

                    if (options.Count == 0)
                    {
                        options.Add(new DoNothingOption(outIteration));
                    }
                }
            }

            return options;
        }
    }

    internal class DeployOption : PlayableOption
    {
        public Color DeployColor { get; }
        public LocationId LocationId { get; }
        public override int MinVerbosity => 0;
        public bool IsCityTaken { get; private set; }
        public bool IsBaseAction { get; }
        public DeployOption(LocationId locationId, int outIteration, Color color, bool isBaseAction = false) : base()
        {
            NextState = isBaseAction ? SelectionState.CARD_OR_FREE_ACTION : SelectionState.SELECT_CARD_OPTION;
            NextCardIteration = outIteration;
            LocationId = locationId;
            IsBaseAction = isBaseAction;

            DeployColor = color; 
        }

        public override void ApplyOption(Board board, Turn turn)
        {
            board.Players[turn.Color].Troops--;

            var location = board.LocationIds[LocationId];

            var prevControl = location.GetControlPlayer();

            location.Troops[turn.Color] += 1;

            var nowControl = location.GetControlPlayer();

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

            if (IsBaseAction)
            {
                turn.Swords--;
            }

            turn.TakeFromTroopsColor = null;

            foreach (var (color, count) in location.Troops)
            {
                if (color == Color.WHITE || color == turn.Color || count == 0)
                {
                    continue;
                }

                turn.AdjacentPlayersToDeploy.Add(color);
            }

            foreach (var neighboor in location.Neighboors)
            {
                foreach (var (color, count) in neighboor.Troops)
                {
                    if (color == Color.WHITE || color == turn.Color || count == 0)
                    {
                        continue;
                    }

                    turn.AdjacentPlayersToDeploy.Add(color);
                }
            }
        }

        public override string GetOptionText()
        {
            string prefix = IsBaseAction ? "" : "\t";
            string suffix = IsBaseAction ? " by 1 sword" : "";
            string citySuffix = IsCityTaken ? ", gain 1 mana for control this site" : "";

            return $"{prefix}Deploying {DeployColor} troop in {LocationId}{suffix}{citySuffix}";
        }
    }

    internal class DeployWithOutTroopsOption : PlayableOption
    {
        public bool IsBaseAction { get; }
        public DeployWithOutTroopsOption(int outIteration, bool isBaseAction = false) : base()
        {
            NextState = isBaseAction ? SelectionState.CARD_OR_FREE_ACTION : SelectionState.SELECT_CARD_OPTION;
            NextCardIteration = outIteration;
            IsBaseAction = isBaseAction;
        }

        public override int MinVerbosity => 0;

        public override void ApplyOption(Board board, Turn turn)
        {
            board.Players[turn.Color].VPTokens++;

            if (IsBaseAction)
            {
                turn.Swords--;
            }
        }

        public override string GetOptionText()
        {
            string prefix = IsBaseAction ? "" : "\t";
            return $"{prefix}Try deploying troop but out of troops then gain 1 VP";
        }
    }
}
