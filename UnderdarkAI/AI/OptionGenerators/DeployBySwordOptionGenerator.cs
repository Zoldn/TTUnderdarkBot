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
                ret.Add(new DeployBySwordWithOutTroopsOption() { Weight = 1.0d });
            }
            else
            {
                foreach (var (location, locationState) in turn.LocationStates)
                {
                    if (!locationState.HasPresence || location.FreeSpaces == 0)
                    {
                        continue;
                    }

                    ret.Add(new DeployBySwordOption(location.Id) { Weight = 1.0d });
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

        public DeployBySwordOption(LocationId locationId)
        {
            LocationId = locationId;
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

        public override void UpdateTurnState(Turn turn)
        {
            turn.State = SelectionState.CARD_OR_FREE_ACTION;
        }

        public override string GetOptionText()
        {
            if (IsCityTaken)
            {
                return $"Deploying troop in {LocationId} by 1 sword, gain 1 mana for control this site";
            }

            return $"Deploying troop in {LocationId} by 1 sword";
        }
    }

    internal class DeployBySwordWithOutTroopsOption : PlayableOption
    {
        public DeployBySwordWithOutTroopsOption() { }

        public override int MinVerbosity => 0;

        public override void ApplyOption(Board board, Turn turn)
        {
            board.Players[turn.Color].VPTokens++;

            turn.Swords--;
        }

        public override void UpdateTurnState(Turn turn)
        {
            turn.State = SelectionState.CARD_OR_FREE_ACTION;
        }

        public override string GetOptionText()
        {
            return $"Try deploying troop by 1 sword but out of troops then gain 1 VP";
        }
    }
}
