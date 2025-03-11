using System;
using System.Collections.Generic;

namespace Ordenacao.Services
{
    public class QuickSortService
    {
        public List<int> Sort(List<int> array)
        {
            if (array == null || array.Count == 0) return new List<int>();
            QuickSort(array, 0, array.Count - 1);
            return array;
        }

        private void QuickSort(List<int> array, int low, int high)
        {
            if (low < high)
            {
                int pi = Partition(array, low, high);
                QuickSort(array, low, pi - 1);
                QuickSort(array, pi + 1, high);
            }
        }

        private int Partition(List<int> array, int low, int high)
        {
            int pivot = array[high];
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                if (array[j] < pivot)
                {
                    i++;
                    (array[i], array[j]) = (array[j], array[i]);
                }
            }
            (array[i + 1], array[high]) = (array[high], array[i + 1]);
            return i + 1;
        }
    }
}
