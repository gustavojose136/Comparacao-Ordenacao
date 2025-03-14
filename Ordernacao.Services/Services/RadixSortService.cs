using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ordenacao.Services
{
    public class RadixSortService
    {
        public List<int> Sort(List<int> array)
        {
            var stopwatch = Stopwatch.StartNew();
            int comparisons = 0;
            int swaps = 0;

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
                CountSortByDigit(array, exp, ref comparisons, ref swaps);
            }

            stopwatch.Stop();
            var elapsedTime = stopwatch.Elapsed;

            SortLogger.LogSortDetails(
                "RadixSort",
                array.Count,
                (long)elapsedTime.TotalMilliseconds,
                comparisons,
                swaps
            );

            return array;
        }

        private void CountSortByDigit(List<int> array, int exp, ref int comparisons, ref int swaps)
        {
            int n = array.Count;
            int[] output = new int[n];
            int[] count = new int[10];

            for (int i = 0; i < n; i++)
            {
                int digit = (array[i] / exp) % 10;
                count[digit]++;
                comparisons++;
            }

            for (int i = 1; i < 10; i++)
            {
                count[i] += count[i - 1];
            }

            for (int i = n - 1; i >= 0; i--)
            {
                int digit = (array[i] / exp) % 10;
                output[count[digit] - 1] = array[i];
                count[digit]--;
                swaps++;
            }

            for (int i = 0; i < n; i++)
            {
                array[i] = output[i];
            }
        }
    }
}
