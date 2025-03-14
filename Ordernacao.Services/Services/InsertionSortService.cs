﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ordenacao.Services
{
    public class InsertionSortService
    {
        public List<int> Sort(List<int> array)
        {
            var stopwatch = Stopwatch.StartNew();
            int comparisons = 0;
            int swaps = 0;

            if (array == null || array.Count == 0) return new List<int>();

            int n = array.Count;
            for (int i = 1; i < n; i++)
            {
                int key = array[i];
                int j = i - 1;

                while (j >= 0 && array[j] > key)
                {
                    array[j + 1] = array[j];
                    j--;
                    comparisons++;
                    swaps++;
                }
                array[j + 1] = key;
            }

            stopwatch.Stop();
            var elapsedTime = stopwatch.Elapsed;

            SortLogger.LogSortDetails(
                "InsertionSort",
                array.Count,
                (long)elapsedTime.TotalMilliseconds,
                comparisons,
                swaps
            );

            return array;
        }
    }
}
