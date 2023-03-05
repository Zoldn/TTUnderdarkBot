using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.OptionGenerators;
using UnderdarkAI.AI.PlayableOptions;

namespace UnderdarkAI.AI.WeightGenerators
{
    internal interface IWeightGenerator
    {
        public void FillPromoteOptions<T>(Board board, Turn turn, List<T> options)
            where T : IPromoteCardOption;
        public void FillDeployOptions(Board board, Turn turn, List<DeployOption> options);
        public void FillReturnEnemySpyOptions(Board board, Turn turn, List<ReturnEnemySpyOption> options);
        public void FillPlaceSpyOptions(Board board, Turn turn, List<PlaceSpyOption> options);
        public void FillBuyOptions(Board board, Turn turn, List<BuyingOption> boptions);
    }
}
