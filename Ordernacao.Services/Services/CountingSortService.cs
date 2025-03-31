using Ordernacao.Services.Services.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

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
            object lockObj = new();

            Parallel.ForEach(array, num =>
            {
                lock (lockObj)
                {
                    if (num < min) min = num;
                    if (num > max) max = num;
                }
                comparisons++;
            });

            int range = max - min + 1;
            int[] count = new int[range];
            int[] output = new int[array.Count];

            Parallel.ForEach(array, num =>
            {
                lock (lockObj)
                {
                    count[num - min]++;
                }
                comparisons++;
            });

            for (int i = 1; i < count.Length; i++)
                count[i] += count[i - 1];

            for (int i = array.Count - 1; i >= 0; i--)
            {
                output[count[array[i] - min] - 1] = array[i];
                count[array[i] - min]--;
            }

            Parallel.For(0, array.Count, i =>
            {
                array[i] = output[i];
            });

            stopwatch.Stop();
            SortLogger.LogSortDetails("ParallelCountingSort", array.Count, (long)stopwatch.Elapsed.TotalMilliseconds, comparisons, 0);
            return array;
        }
    }
}

// A versão paralelizada do CountingSortService agora usa Parallel.ForEach para encontrar os valores mínimo e máximo e preencher a 
// contagem de forma concorrente. A cópia final do array também foi paralelizada para melhor desempenho. 