using Ordernacao.Services.Services.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ordenacao.Services
{
    public class HeapSortService : ISortStrategy
    {
        public List<int> Sort(List<int> array)
        {
            var stopwatch = Stopwatch.StartNew();
            int comparisons = 0, swaps = 0;

            if (array == null || array.Count <= 1)
                return array;

            int n = array.Count;
            for (int i = n / 2 - 1; i >= 0; i--)
                Heapify(array, n, i, ref comparisons, ref swaps);

            for (int i = n - 1; i >= 0; i--)
            {
                (array[0], array[i]) = (array[i], array[0]);
                swaps++;
                Heapify(array, i, 0, ref comparisons, ref swaps);
            }

            stopwatch.Stop();
            SortLogger.LogSortDetails("HeapSort", array.Count, (long)stopwatch.Elapsed.TotalMilliseconds, comparisons, swaps);
            return array;
        }

        private void Heapify(List<int> array, int heapSize, int rootIndex, ref int comparisons, ref int swaps)
        {
            int largest = rootIndex;
            int left = 2 * rootIndex + 1;
            int right = 2 * rootIndex + 2;

            if (left < heapSize)
            {
                comparisons++;
                if (array[left] > array[largest])
                    largest = left;
            }

            if (right < heapSize)
            {
                comparisons++;
                if (array[right] > array[largest])
                    largest = right;
            }

            if (largest != rootIndex)
            {
                (array[rootIndex], array[largest]) = (array[largest], array[rootIndex]);
                swaps++;
                Heapify(array, heapSize, largest, ref comparisons, ref swaps);
            }
        }
    }
}
