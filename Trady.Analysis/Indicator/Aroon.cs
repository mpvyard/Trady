﻿using System.Linq;
using Trady.Analysis.Indicator.Helper;
using Trady.Core;
using static Trady.Analysis.Indicator.Aroon;

namespace Trady.Analysis.Indicator
{
    public partial class Aroon : IndicatorBase<IndicatorResult>
    {
        private HighestHigh _highestHigh;
        private LowestLow _lowestLow;

        public Aroon(Equity equity, int periodCount) : base(equity, periodCount)
        {
            _highestHigh = new HighestHigh(equity, periodCount);
            _lowestLow = new LowestLow(equity, periodCount);
        }

        public int PeriodCount => Parameters[0];

        protected override IndicatorResult ComputeByIndexImpl(int index)
        {
            if (index < PeriodCount - 1)
                return new IndicatorResult(Equity[index].DateTime, null, null);

            var highestCloseIndex = index - PeriodCount + 1 + Equity
                .Skip(index - PeriodCount + 1)
                .Take(PeriodCount)
                .ToList()
                .FindLastIndexOrDefault(c => c.High == _highestHigh.ComputeByIndex(index).HighestHigh).Value;

            var lowestCloseIndex = index - PeriodCount + 1 + Equity
                .Skip(index - PeriodCount + 1)
                .Take(PeriodCount)
                .ToList()
                .FindLastIndexOrDefault(c => c.Low == _lowestLow.ComputeByIndex(index).LowestLow).Value;

            var up = 100.0m * (PeriodCount - (index - highestCloseIndex)) / PeriodCount;
            var down = 100.0m * (PeriodCount - (index - lowestCloseIndex)) / PeriodCount;

            return new IndicatorResult(Equity[index].DateTime, up, down);
        }
    }
}