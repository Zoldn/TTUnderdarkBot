using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace UnderdarkAI.AI.OptionGenerators
{
    internal class DeployBySwordOptionGenerator : OptionGenerator
    {
        public override SelectionState State => SelectionState.DEPLOY_BY_SWORD;

        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var ret = new List<PlayableOption>();

            if (turn.Swords < 1)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (board.Players[turn.Color].Troops == 0)
            {
                ret.Add(new DeployBySwordWithOutTroopsOption(isBaseAction: true) { Weight = 1.0d });
            }
            else
            {
                foreach (var (location, locationState) in turn.LocationStates)
                {
                    if (!locationState.HasPresence || location.FreeSpaces == 0)
                    {
                        continue;
                    }

                    ret.Add(new DeployBySwordOption(location.Id, isBaseAction: true) { Weight = 1.0d });
                }
            }

            return ret;
        }
    }

    internal class DeployBySwordOption : PlayableOption
    {
        public LocationId LocationId { get; }

        public override int MinVerbosity => 0;
        public bool IsCityTaken { get; private set; }
        public bool IsBaseAction { get; }
        public DeployBySwordOption(LocationId locationId, bool isBaseAction) : base()
        {
            LocationId = locationId;
            IsBaseAction = isBaseAction;
            NextState = SelectionState.CARD_OR_FREE_ACTION;
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

            turn.Swords--;
        }

        public override string GetOptionText()
        {
            string prefix = IsBaseAction ? "" : "\t";
            string suffix = IsBaseAction ? " by 1 sword" : "";
            string citySuffix = IsCityTaken ? ", gain 1 mana for control this site" : "";

            return $"{prefix}Deploying troop in {LocationId}{suffix}{citySuffix}";
        }
    }

    internal class DeployBySwordWithOutTroopsOption : PlayableOption
    {
        public bool IsBaseAction { get; }
        public DeployBySwordWithOutTroopsOption(bool isBaseAction = false) : base()
        {
            NextState = SelectionState.CARD_OR_FREE_ACTION;
            IsBaseAction = isBaseAction;
        }

        public override int MinVerbosity => 0;

        public override void ApplyOption(Board board, Turn turn)
        {
            board.Players[turn.Color].VPTokens++;

            turn.Swords--;
        }

        public override string GetOptionText()
        {
            string prefix = IsBaseAction ? "" : "\t";
            return $"{prefix}Try deploying troop but out of troops then gain 1 VP";
        }
    }
}
