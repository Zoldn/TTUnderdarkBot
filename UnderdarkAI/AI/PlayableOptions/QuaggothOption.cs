using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace UnderdarkAI.AI.PlayableOptions
{
    internal class QuaggothSetupOption : PlayableOption
    {
        public override int MinVerbosity => 0;
        public int Kills { get; private set; }
        public QuaggothSetupOption(int outIteration)
        {
            NextCardIteration = outIteration;
        }

        public override void ApplyOption(Board board, Turn turn)
        {
            int controlLocations = 0;

            foreach (var location in board.Locations)
            {
                if (location.GetControlPlayer() == turn.Color)
                {
                    controlLocations++;
                }
            }

            Kills = controlLocations;
            turn.MaxQuaggothKills = controlLocations;
            turn.QuaggothKills = 0;
        }

        public override string GetOptionText()
        {
            return $"\tQuaggoth can kill up to {Kills} of white troop(s)";
        }
    }

    internal class KillCounterOption : PlayableOption
    {
        public override int MinVerbosity => 10;
        public KillCounterOption(int outIteration)
        {
            NextCardIteration = outIteration;
        }

        public override void ApplyOption(Board board, Turn turn)
        {
            turn.QuaggothKills++;
        }

        public override string GetOptionText()
        {
            return $"\tQuaggoth counting kill of white troop(s)";
        }
    }
}
