using Ordernacao.Services.Services.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

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

            var result = QuickSort(new List<int>(array), 0, array.Count - 1);
            comparisons = result.Item2;
            swaps = result.Item3;

            stopwatch.Stop();
            SortLogger.LogSortDetails("QuickSort", array.Count, (long)stopwatch.Elapsed.TotalMilliseconds, comparisons, swaps);
            return result.Item1;
        }

        private Tuple<List<int>, int, int> QuickSort(List<int> array, int low, int high)
        {
            int comparisons = 0, swaps = 0;

            if (low < high)
            {
                int pi = Partition(array, low, high, ref comparisons, ref swaps);

                // Create subarrays instead of modifying the main list
                List<int> leftSubArray = new List<int>(array.GetRange(low, pi - low));
                List<int> rightSubArray = new List<int>(array.GetRange(pi + 1, high - pi));

                // Perform parallel sorting on left and right partitions
                var leftTask = Task.Run(() => QuickSort(leftSubArray, 0, leftSubArray.Count - 1));
                var rightTask = Task.Run(() => QuickSort(rightSubArray, 0, rightSubArray.Count - 1));

                Task.WhenAll(leftTask, rightTask).Wait();

                var leftResult = leftTask.Result;
                var rightResult = rightTask.Result;

                comparisons += leftResult.Item2 + rightResult.Item2;
                swaps += leftResult.Item3 + rightResult.Item3;

                // Combine the results correctly
                array = leftResult.Item1.Concat(new List<int> { array[pi] }).Concat(rightResult.Item1).ToList();
            }

            return Tuple.Create(array, comparisons, swaps);
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


// Parallel Sorting:
// The QuickSort method now uses Task.Run to perform parallel quicksort on the left and right partitions of the array.
// These tasks are awaited using Task.WhenAll(leftTask, rightTask).Wait() to ensure both partitions are processed before combining the results.
// Avoiding ref:
// Instead of passing comparisons and swaps as ref parameters, these are tracked locally in each method and returned as part of a Tuple<List<int>, int, int>.
// The results for comparisons and swaps from each partition are accumulated in the QuickSort method.
// Partition Logic:
// The partitioning logic remains the same as in your base code, but it now works with the parallelized sorting tasks.