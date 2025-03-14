using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ordenacao.Services
{
    public class HeapSortService
    {
        public List<int> Sort(List<int> array)
        {
            var stopwatch = Stopwatch.StartNew();
            int comparisons = 0;

            if (array == null || array.Count <= 1)
                return array;

            int n = array.Count;

            for (int i = n / 2 - 1; i >= 0; i--)
                Heapify(array, n, i);

            for (int i = n - 1; i >= 0; i--)
            {
                int temp = array[0];
                array[0] = array[i];
                array[i] = temp;

                Heapify(array, i, 0);
            }

            stopwatch.Stop();
            var elapsedTime = stopwatch.Elapsed;

            SortLogger.LogSortDetails(
                "HeapSort",
                array.Count,
                (long)elapsedTime.TotalMilliseconds,
                comparisons,
                0
            );

            return array;
        }

        private void Heapify(List<int> array, int heapSize, int rootIndex)
        {
            int largest = rootIndex;
            int leftChild = 2 * rootIndex + 1;
            int rightChild = 2 * rootIndex + 2;

            if (leftChild < heapSize && array[leftChild] > array[largest])
                largest = leftChild;

            if (rightChild < heapSize && array[rightChild] > array[largest])
                largest = rightChild;

            if (largest != rootIndex)
            {
                int swap = array[rootIndex];
                array[rootIndex] = array[largest];
                array[largest] = swap;

                Heapify(array, heapSize, largest);
            }
        }
    }
}
