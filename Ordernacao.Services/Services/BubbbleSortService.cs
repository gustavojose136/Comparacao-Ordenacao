using Ordernacao.Services.Services.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Ordenacao.Services
{
    public class BubbleSortService : ISortStrategy
    {
        public List<int> Sort(List<int> array)
        {
            var stopwatch = Stopwatch.StartNew();
            int comparisons = 0, swaps = 0;
            int n = array.Count;
            bool swapped;

            for (int i = 0; i < n - 1; i++)
            {
                swapped = false;

                Parallel.For(0, n - i - 1, j =>
                {
                    if (array[j] > array[j + 1])
                    {
                        lock (array)
                        {
                            (array[j], array[j + 1]) = (array[j + 1], array[j]);
                            swaps++;
                        }
                        swapped = true;
                    }
                    comparisons++;
                });

                if (!swapped)
                    break;
            }

            stopwatch.Stop();
            SortLogger.LogSortDetails("ParallelBubbleSort", array.Count, (long)stopwatch.Elapsed.TotalMilliseconds, comparisons, swaps);
            return array;
        }
    }
}

// O BubbleSortService agora usa Parallel.For para distribuir as comparações, com um bloqueio (lock) para evitar condições de corrida ao 
// realizar trocas. Esse método pode reduzir ligeiramente o tempo de execução, mas Bubble Sort não se beneficia muito da paralelização. 