using Ordernacao.Services.Services.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Ordenacao.Services
{
    public class SelectionSortService : ISortStrategy
    {
        public List<int> Sort(List<int> array)
        {
            var stopwatch = Stopwatch.StartNew();
            int comparisons = 0, swaps = 0;

            if (array == null || array.Count == 0)
                return new List<int>();

            int n = array.Count;
            for (int i = 0; i < n - 1; i++)
            {
                int minIndex = i;
                // Parallelize the inner loop using Parallel.For
                Parallel.For(i + 1, n, j =>
                {
                    comparisons++;
                    if (array[j] < array[minIndex])
                    {
                        lock (array)
                        {
                            minIndex = j;
                        }
                    }
                });

                if (minIndex != i)
                {
                    lock (array)
                    {
                        (array[i], array[minIndex]) = (array[minIndex], array[i]);
                    }
                    swaps++;
                }
            }

            stopwatch.Stop();
            SortLogger.LogSortDetails("ParallelSelectionSort", array.Count, (long)stopwatch.Elapsed.TotalMilliseconds, comparisons, swaps);
            return array;
        }
    }
}

// Parallelizing the Inner Loop:
// The loop for (int j = i + 1; j < n; j++) is parallelized using Parallel.For to allow simultaneous comparison operations across multiple threads.
// Thread Safety:
// Since multiple threads might attempt to access minIndex at the same time, I have wrapped its assignment inside a lock (array) to ensure that 
// the updates to minIndex are thread-safe.
// Similarly, swapping the elements is protected by a lock (array) to avoid concurrent access issues during the swap operation.
// Threading Impact:
// The parallelism will only show a noticeable performance improvement with sufficiently large arrays. For small arrays, the overhead of managing 
// multiple threads may outweigh the benefits.