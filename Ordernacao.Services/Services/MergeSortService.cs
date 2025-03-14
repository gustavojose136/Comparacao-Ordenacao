using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ordenacao.Services
{
    public class MergeSortService
    {
        public List<int> Sort(List<int> array)
        {
            var stopwatch = Stopwatch.StartNew();
            int comparisons = 0;
            int swaps = 0;

            if (array == null || array.Count == 0) return new List<int>();
            var sortedArray = MergeSort(array, ref comparisons, ref swaps);

            stopwatch.Stop();
            var elapsedTime = stopwatch.Elapsed;

            SortLogger.LogSortDetails(
                "MergeSort",
                array.Count,
                (long)elapsedTime.TotalMilliseconds,
                comparisons,
                swaps
            );

            return sortedArray;
        }

        private List<int> MergeSort(List<int> array, ref int comparisons, ref int swaps)
        {
            if (array.Count <= 1)
                return array;

            int mid = array.Count / 2;
            var left = MergeSort(array.GetRange(0, mid), ref comparisons, ref swaps);
            var right = MergeSort(array.GetRange(mid, array.Count - mid), ref comparisons, ref swaps);

            return Merge(left, right, ref comparisons, ref swaps);
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
