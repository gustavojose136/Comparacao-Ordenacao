using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ordenacao.Services
{
    public class SortComparisonService
    {
        private readonly BubbleSortService _bubbleSortService;
        private readonly SelectionSortService _selectionSortService;
        private readonly InsertionSortService _insertionSortService;
        private readonly MergeSortService _mergeSortService;
        private readonly QuickSortService _quickSortService;
        private readonly TimSortService _timSortService;
        private readonly HeapSortService _heapSortService;
        private readonly CountingSortService _countingSortService;
        private readonly RadixSortService _radixSortService;
        private readonly ShellSortService _shellSortService;

        public SortComparisonService(
            BubbleSortService bubbleSortService,
            SelectionSortService selectionSortService,
            InsertionSortService insertionSortService,
            MergeSortService mergeSortService,
            QuickSortService quickSortService,
            TimSortService timSortService,
            HeapSortService heapSortService,
            CountingSortService countingSortService,
            RadixSortService radixSortService,
            ShellSortService shellSortService)
        {
            _bubbleSortService = bubbleSortService;
            _selectionSortService = selectionSortService;
            _insertionSortService = insertionSortService;
            _mergeSortService = mergeSortService;
            _quickSortService = quickSortService;
            _timSortService = timSortService;
            _heapSortService = heapSortService;
            _countingSortService = countingSortService;
            _radixSortService = radixSortService;
            _shellSortService = shellSortService;
        }

        public object CompareSorts(List<int> array)
        {
            var algorithms = new Dictionary<string, Func<List<int>, List<int>>>
            {
                { "Bubble Sort", _bubbleSortService.Sort },
                { "Selection Sort", _selectionSortService.Sort },
                { "Insertion Sort", _insertionSortService.Sort },
                { "Heap Sort", _heapSortService.Sort },
                { "Counting Sort", _countingSortService.Sort },
                { "Radix Sort", _radixSortService.Sort },
                { "Shell Sort", _shellSortService.Sort },
                { "Merge Sort", _mergeSortService.Sort },
                { "Quick Sort", _quickSortService.Sort },
                { "Tim Sort", _timSortService.Sort }
            };
                
            var results = new Dictionary<string, double>();

            foreach (var algorithm in algorithms)
            {
                var tempArray = new List<int>(array);
                var stopwatch = Stopwatch.StartNew();

                algorithm.Value(tempArray);

                stopwatch.Stop();
                results[algorithm.Key] = stopwatch.ElapsedMilliseconds / 1000.0;
            }

            var bestAlgorithm = "";
            double bestTime = double.MaxValue;

            foreach (var result in results)
            {
                if (result.Value < bestTime)
                {
                    bestTime = result.Value;
                    bestAlgorithm = result.Key;
                }
            }

            return new
            {
                tempos = results,
                melhor = new { Algoritmo = bestAlgorithm, Tempo = bestTime }
            };
        }
    }
}
