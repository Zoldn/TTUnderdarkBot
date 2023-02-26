using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnderdarkAI.Utils;

namespace UnderdarkAI.AI
{
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
        public double AverageScore { get; }
        public double StandardDeviationScore { get; }
        public MonteCarloSelectionStatus Status { get; set; }
        public double DistanceToBest { get; set; }
        public MonteCarloSelectionInfo(PlayableOption option, IEnumerable<double> scores)
        {
            PlayableOption = option;
            Scores = scores.ToList();

            Status = MonteCarloSelectionStatus.NOT_ANALYSED;

            AverageScore = Scores.Average();

            if (Scores.Count <= 1)
            {
                StandardDeviationScore = double.PositiveInfinity;
            }
            else
            {
                StandardDeviationScore = Math.Sqrt(
                    Scores.Sum(v => Math.Pow(v - AverageScore, 2)) 
                    / Scores.Count / (Scores.Count - 1)
                    );
            }

            DistanceToBest = 0.0d;
        }
    }

    internal class MonteCarloSelectionInfoAnalyzer
    {
        public static double TOLERANCE = 0.1d;
        public List<MonteCarloSelectionInfo> SelectionInfos { get; }
        public MonteCarloSelectionInfoAnalyzer(List<MonteCarloSelectionInfo> selectionInfos)
        {
            SelectionInfos = selectionInfos;
        }
        public MonteCarloSelectionInfo SelectBest(Random random)
        {
            var maxAverage = SelectionInfos.Max(i => i.AverageScore);

            //var goodOptions = SelectionInfos
            //    .Where(i => i.AverageScore >= maxAverage - TOLERANCE)
            //    .ToList();

            MonteCarloSelectionInfo bestOption = SelectionInfos
                .First(o => o.AverageScore == maxAverage);

            //if (goodOptions.Count == 1)
            //{
            //    bestOption = goodOptions[0];
            //}
            //else
            //{
            //    bestOption = RandomSelector.SelectRandom(goodOptions, random);
            //}

            //double distanceToOthers = double.PositiveInfinity;

            foreach (var info in SelectionInfos)
            {
                if (info == bestOption)
                {
                    continue;
                }

                if (bestOption.StandardDeviationScore + info.StandardDeviationScore == 0.0d)
                {
                    if (Math.Abs(bestOption.AverageScore - info.AverageScore) < 1e-4)
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
                    //distanceToOthers = Math.Min(distanceToOthers, 0);
                }
                else
                {
                    info.DistanceToBest = (bestOption.AverageScore - info.AverageScore) /
                        (bestOption.StandardDeviationScore + info.StandardDeviationScore);
                    //distanceToOthers = Math.Min(distanceToOthers, (bestOption.AverageScore - info.AverageScore) /
                    //    (bestOption.StandardDeviationScore + info.StandardDeviationScore));
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
                    bestOption.Status = MonteCarloSelectionStatus.BEST;
                }
                else if (distance > 2.0d)
                {
                    bestOption.Status = MonteCarloSelectionStatus.GREAT;
                }
                else if (distance > 1.0d)
                {
                    bestOption.Status = MonteCarloSelectionStatus.GOOD;
                }
                else if (distance > -TOLERANCE)
                {
                    bestOption.Status = MonteCarloSelectionStatus.NORMAL;
                }
            }
            else
            {
                bestOption = RandomSelector.SelectRandom(goodOptions, random);
                bestOption.Status = MonteCarloSelectionStatus.NORMAL;
            }

            //var distanceToOthers = SelectionInfos
            //    .Where(e => e != bestOption)
            //    .Min(e => (bestOption.AverageScore - e.AverageScore) /
            //        (bestOption.StandardDeviationScore + e.StandardDeviationScore));

            //if (distanceToOthers > 3.0d)
            //{
            //    bestOption.Status = MonteCarloSelectionStatus.BEST;
            //}
            //else if (distanceToOthers > 2.0d)
            //{
            //    bestOption.Status = MonteCarloSelectionStatus.GREAT;
            //}
            //else if (distanceToOthers > 1.0d)
            //{
            //    bestOption.Status = MonteCarloSelectionStatus.GOOD;
            //}
            //else if (distanceToOthers > -TOLERANCE)
            //{
            //    bestOption.Status = MonteCarloSelectionStatus.NORMAL;
            //}
            //else
            //{
            //    bestOption.Status = MonteCarloSelectionStatus.BAD;
            //}

            //if (bestOption.Status == MonteCarloSelectionStatus.BAD)
            //{
            //    int e = 1;
            //}

            return bestOption;
        }
    }
}
