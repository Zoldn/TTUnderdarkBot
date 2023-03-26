using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TUnderdark.Utils;
using UnderdarkAI.Utils;

namespace UnderdarkAI.AI
{
    public enum AggregateMode
    {
        AVERAGE = 0,
        MAX,
    }
    internal enum MonteCarloSelectionStatus
    {
        NOT_ANALYSED = 0,
        BAD,
        NORMAL,
        GOOD,
        GREAT,
        BEST,
        ONLY_OPTION,
    }
    internal class MonteCarloSelectionInfo
    {
        public PlayableOption PlayableOption { get; }
        public IReadOnlyList<double> Scores { get; }
        public AggregateMode AggregateMode { get; }

        /// <summary>
        /// Агрегированный score варианта
        /// </summary>
        public double FinalScore { get; }
        public double StandardDeviationScore { get; }
        //public MonteCarloSelectionStatus Status { get; set; }
        public double DistanceToBest { get; set; }
        public MonteCarloSelectionInfo(PlayableOption option, IEnumerable<double> scores,
            AggregateMode aggregateMode)
        {
            PlayableOption = option;
            Scores = scores.ToList();
            AggregateMode = aggregateMode;

            option.MonteCarloStatus = MonteCarloSelectionStatus.NOT_ANALYSED;

            FinalScore = AggregateMode switch
            {
                AggregateMode.MAX => Scores.Max(),
                AggregateMode.AVERAGE => Scores.Average(),
                _ => throw new ArgumentOutOfRangeException(),
            };

            if (Scores.Count <= 1)
            {
                StandardDeviationScore = double.PositiveInfinity;
            }
            else
            {
                StandardDeviationScore = Math.Sqrt(
                    Scores.Sum(v => Math.Pow(v - FinalScore, 2)) 
                    / Scores.Count / (Scores.Count - 1)
                    );
            }

            DistanceToBest = 0.0d;
        }
    }

    public enum SelectBestMode
    {
        /// <summary>
        /// Выбирается рандомный вариант из стандартного отклонения от лучшего
        /// </summary>
        RETURN_ANY_IN_STD = 0,
        /// <summary>
        /// Выбираются только лучшие
        /// </summary>
        RETURN_ONLY_BEST,
    }

    internal class MonteCarloSelectionInfoAnalyzer
    {
        public static double TOLERANCE = 0.1d;
        public List<MonteCarloSelectionInfo> SelectionInfos { get; }
        public SelectBestMode SelectionMode { get; }
        public MonteCarloSelectionInfoAnalyzer(List<MonteCarloSelectionInfo> selectionInfos, 
            SelectBestMode selectionMode)
        {
            SelectionInfos = selectionInfos;
            SelectionMode = selectionMode;
        }
        public MonteCarloSelectionInfo SelectBest(Random random)
        {
            var selectedOption = SelectionMode switch
            {
                SelectBestMode.RETURN_ANY_IN_STD => SelectInSTD(random),
                SelectBestMode.RETURN_ONLY_BEST => SelectOnlyInBest(random),
                _ => throw new ArgumentOutOfRangeException(),
            };

            return selectedOption;
        }


        private MonteCarloSelectionInfo SelectOnlyInBest(Random random)
        {
            var bestScore = SelectionInfos.Max(i => i.FinalScore);

            var bestOptions = SelectionInfos
                .Where(i => i.FinalScore >= bestScore - 1e-6)
                .ToList();

            if (bestOptions.Count == 1)
            {
                return bestOptions.Single();
            }

            var bestOption = RandomSelector.SelectRandom(bestOptions, random);
            bestOption.PlayableOption.MonteCarloStatus = MonteCarloSelectionStatus.NORMAL;

            return bestOption;
        }

        private MonteCarloSelectionInfo SelectInSTD(Random random)
        {
            var bestOption = SelectionInfos.ArgMax(i => i.FinalScore);

            foreach (var info in SelectionInfos)
            {
                if (info == bestOption)
                {
                    continue;
                }

                if (bestOption.StandardDeviationScore + info.StandardDeviationScore == 0.0d)
                {
                    if (Math.Abs(bestOption.FinalScore - info.FinalScore) < 1e-4)
                    {
                        info.DistanceToBest = 0.0d;
                    }
                    else
                    {
                        info.DistanceToBest = double.PositiveInfinity;
                    }
                    //distanceToOthers = Math.Min(distanceToOthers, double.PositiveInfinity);
                }
                else if (double.IsPositiveInfinity(bestOption.StandardDeviationScore + info.StandardDeviationScore))
                {
                    info.DistanceToBest = 0.0d;
                }
                else
                {
                    info.DistanceToBest = (bestOption.FinalScore - info.FinalScore) /
                        (bestOption.StandardDeviationScore + info.StandardDeviationScore);
                }
            }

            var goodOptions = SelectionInfos
                .Where(s => Math.Abs(s.DistanceToBest) < TOLERANCE)
                .ToList();

            if (goodOptions.Count == 1)
            {
                var distance = SelectionInfos
                    .Where(s => s != bestOption)
                    .Min(s => Math.Abs(s.DistanceToBest));

                if (distance > 3.0d)
                {
                    bestOption.PlayableOption.MonteCarloStatus = MonteCarloSelectionStatus.BEST;
                }
                else if (distance > 2.0d)
                {
                    bestOption.PlayableOption.MonteCarloStatus = MonteCarloSelectionStatus.GREAT;
                }
                else if (distance > 1.0d)
                {
                    bestOption.PlayableOption.MonteCarloStatus = MonteCarloSelectionStatus.GOOD;
                }
                else if (distance > -TOLERANCE)
                {
                    bestOption.PlayableOption.MonteCarloStatus = MonteCarloSelectionStatus.NORMAL;
                }
            }
            else
            {
                bestOption = RandomSelector.SelectRandom(goodOptions, random);
                bestOption.PlayableOption.MonteCarloStatus = MonteCarloSelectionStatus.NORMAL;
            }

            return bestOption;
        }
    }
}
