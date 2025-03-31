using Ordernacao.Services.Services.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Ordenacao.Services
{
    public class MergeSortService : ISortStrategy
    {
        public List<int> Sort(List<int> array)
        {
            var stopwatch = Stopwatch.StartNew();
            int comparisons = 0, swaps = 0;

            if (array == null || array.Count == 0)
                return new List<int>();

            var sortedArray = MergeSort(array, ref comparisons, ref swaps);

            stopwatch.Stop();
            SortLogger.LogSortDetails("ParallelMergeSort", array.Count, (long)stopwatch.Elapsed.TotalMilliseconds, comparisons, swaps);
            return sortedArray;
        }

        private List<int> MergeSort(List<int> array, ref int comparisons, ref int swaps)
        {
            if (array.Count <= 1)
                return array;

            int mid = array.Count / 2;

            // Parallelize the sorting of left and right subarrays
            var leftTask = Task.Run(() => MergeSortAndCount(array.GetRange(0, mid)));
            var rightTask = Task.Run(() => MergeSortAndCount(array.GetRange(mid, array.Count - mid)));

            Task.WhenAll(leftTask, rightTask).Wait(); // Ensure both tasks complete

            var left = leftTask.Result.Item1;
            var right = rightTask.Result.Item1;
            comparisons += leftTask.Result.Item2;
            swaps += leftTask.Result.Item3;

            return Merge(left, right, ref comparisons, ref swaps);
        }

        private Tuple<List<int>, int, int> MergeSortAndCount(List<int> array)
        {
            int comparisons = 0, swaps = 0;

            if (array.Count <= 1)
                return Tuple.Create(array, comparisons, swaps);

            int mid = array.Count / 2;
            var left = MergeSortAndCount(array.GetRange(0, mid));
            var right = MergeSortAndCount(array.GetRange(mid, array.Count - mid));

            comparisons += left.Item2 + right.Item2;
            swaps += left.Item3 + right.Item3;

            var merged = Merge(left.Item1, right.Item1, ref comparisons, ref swaps);
            return Tuple.Create(merged, comparisons, swaps);
        }

        private List<int> Merge(List<int> left, List<int> right, ref int comparisons, ref int swaps)
        {
            List<int> result = new List<int>();
            int i = 0, j = 0;

            while (i < left.Count && j < right.Count)
            {
                comparisons++;
                if (left[i] < right[j])
                    result.Add(left[i++]);
                else
                    result.Add(right[j++]);
            }

            swaps += left.Count - i + right.Count - j;
            result.AddRange(left.GetRange(i, left.Count - i));
            result.AddRange(right.GetRange(j, right.Count - j));

            return result;
        }
    }
}

// Parallelized Recursion:
// The MergeSort method has been modified to spawn two parallel tasks for the left and right subarrays. 
// We use Task.Run to execute the sorting of each subarray concurrently.
// The Task.WhenAll(leftTask, rightTask).Wait() ensures that both tasks complete before proceeding with the merge step.

// Merge Operation:
// The merge itself remains sequential, as it’s difficult to parallelize the merge step in a simple and efficient way.