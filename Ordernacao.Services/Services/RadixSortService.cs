using Ordernacao.Services.Services.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Ordenacao.Services
{
    public class RadixSortService : ISortStrategy
    {
        public List<int> Sort(List<int> array)
        {
            var stopwatch = Stopwatch.StartNew();
            int comparisons = 0, swaps = 0;

            if (array == null || array.Count == 0)
                return array;

            int max = array[0];
            foreach (int num in array)
            {
                comparisons++;
                if (num > max)
                    max = num;
            }

            for (int exp = 1; max / exp > 0; exp *= 10)
            {
                var result = CountSortByDigit(array, exp);
                comparisons += result.Item2;
                swaps += result.Item3;
            }

            stopwatch.Stop();
            SortLogger.LogSortDetails("RadixSort", array.Count, (long)stopwatch.Elapsed.TotalMilliseconds, comparisons, swaps);
            return array;
        }

        private Tuple<List<int>, int, int> CountSortByDigit(List<int> array, int exp)
        {
            int comparisons = 0, swaps = 0;
            int n = array.Count;
            int[] output = new int[n];
            int[] count = new int[10];

            // Parallelizing the first pass for counting digits
            Parallel.For(0, n, i =>
            {
                int digit = (array[i] / exp) % 10;
                lock (count)
                {
                    count[digit]++;
                }
                comparisons++;
            });

            // Parallelizing the accumulation of counts
            Parallel.For(1, 10, i =>
            {
                count[i] += count[i - 1];
            });

            // Parallelizing the assignment of sorted elements to the output array
            Parallel.For(n - 1, -1, i =>
            {
                int digit = (array[i] / exp) % 10;
                lock (output)
                {
                    output[count[digit] - 1] = array[i];
                }
                count[digit]--;
                swaps++;
            });

            // Copying the output array to the original array
            for (int i = 0; i < n; i++)
            {
                array[i] = output[i];
            }

            return Tuple.Create(array, comparisons, swaps);
        }
    }
}

// Parallelism in CountSortByDigit:
// Counting Digits: Parallelized the loop that counts the occurrences of each digit by using Parallel.For.
// Accumulating Counts: Parallelized the accumulation of counts for each digit.
// Assigning to Output Array: Parallelized the loop where the sorted elements are assigned to the output array.