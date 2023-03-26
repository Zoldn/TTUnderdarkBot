using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using TUnderdark.TTSParser;
using UnderdarkAI.AI.OptionGenerators;
using UnderdarkAI.AI.PlayableOptions;
using UnderdarkAI.Context;

namespace UnderdarkAI.AI.WeightGenerators
{
    internal class StaticWeightGenerator : IWeightGenerator
    {
        private readonly ModelContext context;
        public StaticWeightGenerator(ModelContext context)
        {
            this.context = context;
        }

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

        public void FillDeployOptions(Board board, Turn turn, List<DeployOption> options)
        {
            foreach (var option in options)
            {
                option.Weight = 1.0d;
            }
        }

        public void FillPlaceSpyOptions(Board board, Turn turn, List<PlaceSpyOption> options)
        {
            if (!turn.ActiveCard.HasValue)
            {
                throw new ArgumentOutOfRangeException("No active card for placing spy");
            }

            var cardStat = context.CardsStatsDict[turn.ActiveCard.Value];

            foreach (var option in options)
            {
                option.Weight = 1.0d;

                var location = board.LocationIds[option.LocationId];

                var fullController = location.GetFullControl();

                if (location.BonusVP > 0 && fullController != turn.Color && fullController != null)
                {
                    option.Weight += location.BonusVP;
                }

                if (location.IsSpyPlacable && fullController != turn.Color && fullController != null)
                {
                    option.Weight += 1.0d;
                }

                if (cardStat.IsPlaceSpyForEnemyTroops
                    && location.Troops.Any(kv => kv.Key != turn.Color && kv.Key != Color.WHITE && kv.Value > 0))
                {
                    option.Weight += 1.0d;
                }

                if (cardStat.IsPlaceSpyForEnemySpy
                    && location.Spies.Any(kv => kv.Key != turn.Color && kv.Value))
                {
                    option.Weight += 2.0d;
                }
            }
        }

        public void FillBuyOptions(Board board, Turn turn, List<BuyingOption> options)
        {
            foreach (var option in options)
            {
                var card = CardMapper.SpecificTypeCardMakers[option.SpecificType];

                option.Weight = 1.0d + (double)card.VP / card.ManaCost;
            }
        }
    }
}
