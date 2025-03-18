using Ordernacao.Services.Services.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ordenacao.Services
{
    public class TimSortService : ISortStrategy
    {
        private const int RUN = 32;

        public List<int> Sort(List<int> array)
        {
            var stopwatch = Stopwatch.StartNew();
            int comparisons = 0, swaps = 0;

            if (array == null || array.Count == 0)
                return new List<int>();

            int n = array.Count;

            // Aplica Insertion Sort para subarrays de tamanho RUN
            for (int i = 0; i < n; i += RUN)
                InsertionSort(array, i, Math.Min(i + RUN - 1, n - 1), ref comparisons, ref swaps);

            // Mescla os subarrays ordenados progressivamente
            for (int size = RUN; size < n; size *= 2)
            {
                for (int left = 0; left < n; left += 2 * size)
                {
                    int mid = left + size - 1;
                    int right = Math.Min((left + 2 * size - 1), (n - 1));
                    if (mid < right)
                        Merge(array, left, mid, right, ref comparisons, ref swaps);
                }
            }

            stopwatch.Stop();
            SortLogger.LogSortDetails("TimSort", array.Count, (long)stopwatch.Elapsed.TotalMilliseconds, comparisons, swaps);
            return array;
        }

        private void InsertionSort(List<int> array, int left, int right, ref int comparisons, ref int swaps)
        {
            for (int i = left + 1; i <= right; i++)
            {
                int key = array[i];
                int j = i - 1;
                while (j >= left && array[j] > key)
                {
                    array[j + 1] = array[j];
                    j--;
                    comparisons++;
                    swaps++;
                }
                array[j + 1] = key;
            }
        }

        private void Merge(List<int> array, int left, int mid, int right, ref int comparisons, ref int swaps)
        {
            int len1 = mid - left + 1;
            int len2 = right - mid;

            List<int> leftArr = new List<int>(len1);
            List<int> rightArr = new List<int>(len2);

            for (int i = 0; i < len1; i++)
                leftArr.Add(array[left + i]);

            for (int i = 0; i < len2; i++)
                rightArr.Add(array[mid + 1 + i]);

            int i1 = 0, i2 = 0, k = left;

            while (i1 < len1 && i2 < len2)
            {
                comparisons++;
                if (leftArr[i1] <= rightArr[i2])
                    array[k++] = leftArr[i1++];
                else
                    array[k++] = rightArr[i2++];
            }

            while (i1 < len1)
            {
                array[k++] = leftArr[i1++];
                swaps++;
            }

            while (i2 < len2)
            {
                array[k++] = rightArr[i2++];
                swaps++;
            }
        }
    }
}
