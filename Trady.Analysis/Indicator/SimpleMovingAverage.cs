﻿using System;
using System.Linq;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class SimpleMovingAverage : IndicatorBase
    {
        public SimpleMovingAverage(Equity equity, int periodCount) : base(equity, periodCount)
        {
        }

        public int PeriodCount => Parameters[0];

        protected override TickBase ComputeResultByIndex(int index)
        {
            decimal sma = index > 0 ? Equity.Skip(index - PeriodCount + 1).Take(PeriodCount).Average(c => c.Close) : 0;
            return new IndicatorResult(Equity[index].DateTime, sma);
        }

        public TimeSeries<IndicatorResult> Compute(DateTime? startTime = null, DateTime? endTime = null)
            => new TimeSeries<IndicatorResult>(Equity.Name, ComputeResults<IndicatorResult>(startTime, endTime), Equity.Period, Equity.MaxTickCount);

        public IndicatorResult ComputeByDateTime(DateTime dateTime)
            => ComputeResultByDateTime<IndicatorResult>(dateTime);

        public IndicatorResult ComputeByIndex(int index)
            => ComputeResultByIndex<IndicatorResult>(index);
    }
}
