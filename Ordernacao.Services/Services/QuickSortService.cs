using Ordernacao.Services.Services.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ordenacao.Services
{
    public class QuickSortService : ISortStrategy
    {
        public List<int> Sort(List<int> array)
        {
            var stopwatch = Stopwatch.StartNew();
            int comparisons = 0, swaps = 0;

            if (array == null || array.Count == 0)
                return new List<int>();

            QuickSort(array, 0, array.Count - 1, ref comparisons, ref swaps);

            stopwatch.Stop();
            SortLogger.LogSortDetails("QuickSort", array.Count, (long)stopwatch.Elapsed.TotalMilliseconds, comparisons, swaps);
            return array;
        }

        private void QuickSort(List<int> array, int low, int high, ref int comparisons, ref int swaps)
        {
            if (low < high)
            {
                int pi = Partition(array, low, high, ref comparisons, ref swaps);
                QuickSort(array, low, pi - 1, ref comparisons, ref swaps);
                QuickSort(array, pi + 1, high, ref comparisons, ref swaps);
            }
        }

        private int Partition(List<int> array, int low, int high, ref int comparisons, ref int swaps)
        {
            int pivot = array[high];
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                comparisons++;
                if (array[j] < pivot)
                {
                    i++;
                    (array[i], array[j]) = (array[j], array[i]);
                    swaps++;
                }
            }
            (array[i + 1], array[high]) = (array[high], array[i + 1]);
            swaps++;
            return i + 1;
        }
    }
}
