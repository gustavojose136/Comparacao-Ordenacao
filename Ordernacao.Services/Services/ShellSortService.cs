using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ordenacao.Services
{
    public class ShellSortService
    {
        public List<int> Sort(List<int> array)
        {
            var stopwatch = Stopwatch.StartNew();
            int comparisons = 0;
            int swaps = 0;

            if (array == null || array.Count == 0)
                return array;

            int n = array.Count;

            // Inicializa o gap e o reduz progressivamente até 1
            for (int gap = n / 2; gap > 0; gap /= 2)
            {
                for (int i = gap; i < n; i++)
                {
                    int temp = array[i];
                    int j = i;

                    comparisons++; // Count comparison for each while check

                    // Insere array[i] na sublista ordenada
                    while (j >= gap && array[j - gap] > temp)
                    {
                        array[j] = array[j - gap];
                        j -= gap;
                        swaps++; // Count each swap
                    }

                    array[j] = temp;
                }
            }

            stopwatch.Stop();
            var elapsedTime = stopwatch.Elapsed;

            // Log the execution details
            SortLogger.LogSortDetails(
                "ShellSort",
                array.Count,
                (long)elapsedTime.TotalMilliseconds,
                comparisons,
                swaps
            );

            return array;
        }
    }
}
