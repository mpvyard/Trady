﻿using System;
using Trady.Analysis.Indicator.Helper;
using Trady.Core;
using static Trady.Analysis.Indicator.ExponentialMovingAverage;

namespace Trady.Analysis.Indicator
{
    public partial class ExponentialMovingAverage : IndicatorBase<IndicatorResult>
    {
        private GenericExponentialMovingAverage _ema;

        public ExponentialMovingAverage(Equity equity, int periodCount) : base(equity, periodCount)
        {
            _ema = new GenericExponentialMovingAverage(
                equity,
                0,
                i => Equity[i].Close, 
                i => Equity[i].Close, 
                periodCount);
        }

        public int PeriodCount => Parameters[0];

        public override IndicatorResult ComputeByIndex(int index)
            => new IndicatorResult(Equity[index].DateTime, _ema.ComputeByIndex(index).Ema);
    }
}
