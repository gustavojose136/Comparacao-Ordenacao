using Ordernacao.Services.Services.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Ordenacao.Services
{
    public class InsertionSortService : ISortStrategy
    {
        public List<int> Sort(List<int> array)
        {
            var stopwatch = Stopwatch.StartNew();
            int comparisons = 0, swaps = 0;

            if (array == null || array.Count == 0)
                return new List<int>();

            int n = array.Count;

            // Use Parallel.For for the outer loop
            Parallel.For(1, n, i =>
            {
                int key = array[i];
                int j = i - 1;

                while (j >= 0 && array[j] > key)
                {
                    array[j + 1] = array[j];
                    j--;
                    comparisons++;
                    swaps++;
                }
                array[j + 1] = key;
            });

            stopwatch.Stop();
            SortLogger.LogSortDetails("ParallelInsertionSort", array.Count, (long)stopwatch.Elapsed.TotalMilliseconds, comparisons, swaps);
            return array;
        }
    }
}

// Used Parallel.For to parallelize the outer loop (which iterates through the elements of the array).
// he inner while loop remains sequential because each comparison and swap in that loop depends on the result of 
// the previous comparison (i.e., it’s not independent of the other comparisons).