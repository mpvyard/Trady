﻿using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Strategy
{
    internal static class AnalyzableLocator
    {
        private static IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        private static MemoryCacheEntryOptions _policy = new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(1)
        };

        public static TAnalyzable GetOrCreateAnalyzable<TAnalyzable>(this IList<Candle> candles, params int[] parameters)
            where TAnalyzable: IAnalyzable
        {
            string key = $"{candles.GetHashCode()}#{typeof(TAnalyzable).Name}#{string.Join("|", parameters)}";
            if (!_cache.TryGetValue(key, out TAnalyzable output))
            {
                var paramsList = new List<object>();
                paramsList.Add(candles);
                paramsList.AddRange(parameters.Select(p => (object)p));

                // Get the default constructor for instantiation
                var ctor = typeof(TAnalyzable).GetConstructors(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                    .Where(c => c.GetParameters().Any() && typeof(IList<Candle>).Equals(c.GetParameters().First().ParameterType))
                    .FirstOrDefault();

                if (ctor == null)
                    throw new TargetInvocationException("Can't find default constructor for instantiation, please make sure that the analyzable has a constructor with IList<Candle> as the first parameter",
                        new ArgumentNullException(nameof(ctor)));

                output = _cache.Set(key, (TAnalyzable)ctor.Invoke(paramsList.ToArray()), _policy);
            }
            return output;
        }
    }
}