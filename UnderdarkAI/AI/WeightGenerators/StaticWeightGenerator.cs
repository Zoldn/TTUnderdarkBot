using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using TUnderdark.TTSParser;
using UnderdarkAI.AI.OptionGenerators;
using UnderdarkAI.AI.PlayableOptions;

namespace UnderdarkAI.AI.WeightGenerators
{
    internal class StaticWeightGenerator : IWeightGenerator
    {
        public void FillPromoteOptions<T>(Board board, Turn turn, List<T> options)
            where T : IPromoteCardOption
        {
            foreach (var option in options)
            {
                var card = CardMapper.SpecificTypeCardMakers[option.PromoteTarget];

                option.Weight = 1.0d + (card.PromoteVP - card.VP);
            }
        }

        public void FillReturnEnemySpyOptions(Board board, Turn turn, List<ReturnEnemySpyOption> options)
        {
            foreach (var option in options)
            {
                var location = board.LocationIds[option.LocationId];

                option.Weight = 1.0d;

                if (location.BonusVP > 0 && location.GetControlPlayer() == turn.Color)
                {
                    option.Weight += 1.0d;
                }

                if (location.BonusVP > 0 && location.Troops[turn.Color] == location.Size)
                {
                    option.Weight += 1.0d;
                }
            }
        }

        public void FillDeployOptions(Board board, Turn turn, List<DeployBySwordOption> options)
        {
            foreach (var option in options)
            {
                option.Weight = 1.0d;
            }
        }
    }
}
