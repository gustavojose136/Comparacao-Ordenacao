using Ordernacao.Services.Services.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ordenacao.Services
{
    public class BubbleSortService : ISortStrategy
    {
        public List<int> Sort(List<int> array)
        {
            var stopwatch = Stopwatch.StartNew();
            int comparisons = 0, swaps = 0;
            int n = array.Count;

            for (int i = 0; i < n - 1; i++)
            {
                bool swapped = false;
                for (int j = 0; j < n - i - 1; j++)
                {
                    comparisons++;
                    if (array[j] > array[j + 1])
                    {
                        (array[j], array[j + 1]) = (array[j + 1], array[j]);
                        swaps++;
                        swapped = true;
                    }
                }
                if (!swapped)
                    break;
            }

            stopwatch.Stop();
            SortLogger.LogSortDetails("BubbleSort", array.Count, (long)stopwatch.Elapsed.TotalMilliseconds, comparisons, swaps);
            return array;
        }
    }
}
