﻿using Trady.Core;
using static Trady.Analysis.Indicator.ExponentialMovingAverageOscillator;

namespace Trady.Analysis.Indicator
{
    public partial class ExponentialMovingAverageOscillator : IndicatorBase<IndicatorResult>
    {
        private ExponentialMovingAverage _emaIndicator1, _emaIndicator2;

        public ExponentialMovingAverageOscillator(Equity equity, int periodCount1, int periodCount2)
            : base(equity, periodCount1, periodCount2)
        {
            _emaIndicator1 = new ExponentialMovingAverage(equity, periodCount1);
            _emaIndicator2 = new ExponentialMovingAverage(equity, periodCount2);
        }

        public int PeriodCount1 => Parameters[0];

        public int PeriodCount2 => Parameters[1];

        protected override IndicatorResult ComputeByIndexImpl(int index)
        {
            var osc = _emaIndicator1.ComputeByIndex(index).Ema - _emaIndicator2.ComputeByIndex(index).Ema;
            return new IndicatorResult(Equity[index].DateTime, osc);
        }
    }
}