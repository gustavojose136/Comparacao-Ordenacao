using Ordernacao.Services.Services.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Ordenacao.Services
{
    public class SortComparisonService
    {
        private readonly List<ISortStrategy> _sortStrategies;

        public SortComparisonService(IEnumerable<ISortStrategy> sortStrategies)
        {
            _sortStrategies = sortStrategies.ToList();
        }

        public object CompareSorts(List<int> array)
        {
            var results = new Dictionary<string, double>();

            foreach (var strategy in _sortStrategies)
            {
                var sortingContext = new SortingContext();
                sortingContext.SetSortStrategy(strategy);
                var tempArray = new List<int>(array);
                var stopwatch = Stopwatch.StartNew();

                sortingContext.ExecuteSort(tempArray);

                stopwatch.Stop();
                results[strategy.GetType().Name] = stopwatch.ElapsedMilliseconds / 1000.0;
            }

            var bestAlgorithm = results.OrderBy(x => x.Value).First();

            return new
            {
                tempos = results,
                melhor = new { Algoritmo = bestAlgorithm.Key, Tempo = bestAlgorithm.Value }
            };
        }
    }
}
