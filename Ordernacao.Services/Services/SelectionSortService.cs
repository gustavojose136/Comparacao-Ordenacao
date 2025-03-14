using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ordenacao.Services
{
    public class SelectionSortService
    {
        public List<int> Sort(List<int> array)
        {
            var stopwatch = Stopwatch.StartNew();
            int comparisons = 0;
            int swaps = 0;

            if (array == null || array.Count == 0) return new List<int>();

            int n = array.Count;
            for (int i = 0; i < n - 1; i++)
            {
                int minIndex = i;
                for (int j = i + 1; j < n; j++)
                {
                    comparisons++;
                    if (array[j] < array[minIndex])
                        minIndex = j;
                }

                if (minIndex != i)
                {
                    (array[i], array[minIndex]) = (array[minIndex], array[i]);
                    swaps++;
                }
            }

            stopwatch.Stop();
            var elapsedTime = stopwatch.Elapsed;

            SortLogger.LogSortDetails(
                "SelectionSort",
                array.Count,
                (long)elapsedTime.TotalMilliseconds,
                comparisons,
                swaps
            );

            return array;
        }
    }
}
