using Ordernacao.Services.Services.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Ordenacao.Services
{
    public class ShellSortService : ISortStrategy
    {
        public List<int> Sort(List<int> array)
        {
            var stopwatch = Stopwatch.StartNew();
            int comparisons = 0, swaps = 0;

            if (array == null || array.Count == 0)
                return array;

            int n = array.Count;
            // Parallelizing the outer loop using Parallel.For
            for (int gap = n / 2; gap > 0; gap /= 2)
            {
                // Parallelize the inner loop
                Parallel.For(gap, n, i =>
                {
                    int temp = array[i];
                    int j = i;
                    comparisons++; // first comparison in the while loop

                    while (j >= gap && array[j - gap] > temp)
                    {
                        array[j] = array[j - gap];
                        j -= gap;
                        swaps++;
                        comparisons++; // each additional iteration
                    }
                    array[j] = temp;
                });
            }

            stopwatch.Stop();
            SortLogger.LogSortDetails("ParallelShellSort", array.Count, (long)stopwatch.Elapsed.TotalMilliseconds, comparisons, swaps);
            return array;
        }
    }
}

// Parallelizing the Outer Loop:
// The outer loop for (int gap = n / 2; gap > 0; gap /= 2) controls the gap size and is iterated over several phases. We parallelize the 
// inner for loop that processes each element of the array based on the current gap using Parallel.For.
// Thread Safety:
// Although we parallelized the inner loop, we must still be cautious about operations on shared data, such as comparisons and swaps. In 
// this case, since the array[j] and array[j - gap] elements could be updated by different threads concurrently, the code should handle 
// those operations safely.
// Performance Consideration:
// Similar to other algorithms, the performance gain from parallelism depends on the size of the input array. For smaller arrays, the 
// overhead of managing threads might be greater than the benefit of parallelizing the operations.