using Ordernacao.Services.Services.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ordenacao.Services
{
    public class CountingSortService : ISortStrategy
    {
        public List<int> Sort(List<int> array)
        {
            var stopwatch = Stopwatch.StartNew();
            int comparisons = 0;

            if (array == null || array.Count == 0)
                return array;

            int min = array[0], max = array[0];
            foreach (int num in array)
            {
                comparisons++;
                if (num < min)
                    min = num;
                if (num > max)
                    max = num;
            }

            int range = max - min + 1;
            int[] count = new int[range];
            int[] output = new int[array.Count];

            foreach (int num in array)
            {
                comparisons++;
                count[num - min]++;
            }

            for (int i = 1; i < count.Length; i++)
                count[i] += count[i - 1];

            for (int i = array.Count - 1; i >= 0; i--)
            {
                output[count[array[i] - min] - 1] = array[i];
                count[array[i] - min]--;
            }

            for (int i = 0; i < array.Count; i++)
                array[i] = output[i];

            stopwatch.Stop();
            SortLogger.LogSortDetails("CountingSort", array.Count, (long)stopwatch.Elapsed.TotalMilliseconds, comparisons, 0);
            return array;
        }
    }
}
