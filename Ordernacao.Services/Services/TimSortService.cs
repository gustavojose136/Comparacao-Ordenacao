using System;
using System.Collections.Generic;

namespace Ordenacao.Services
{
    public class TimSortService
    {
        private const int RUN = 32; // Tamanho do RUN padrão no Tim Sort

        public List<int> Sort(List<int> array)
        {
            if (array == null || array.Count == 0)
                return new List<int>();

            int n = array.Count;

            // Aplicar Insertion Sort em subarrays de tamanho RUN
            for (int i = 0; i < n; i += RUN)
                InsertionSort(array, i, Math.Min(i + RUN - 1, n - 1));

            // Mesclar subarrays ordenados usando Merge Sort
            for (int size = RUN; size < n; size = 2 * size)
            {
                for (int left = 0; left < n; left += 2 * size)
                {
                    int mid = left + size - 1;
                    int right = Math.Min((left + 2 * size - 1), (n - 1));

                    if (mid < right)
                        Merge(array, left, mid, right);
                }
            }

            return array;
        }

        private void InsertionSort(List<int> array, int left, int right)
        {
            for (int i = left + 1; i <= right; i++)
            {
                int key = array[i];
                int j = i - 1;

                while (j >= left && array[j] > key)
                {
                    array[j + 1] = array[j];
                    j--;
                }

                array[j + 1] = key;
            }
        }

        private void Merge(List<int> array, int left, int mid, int right)
        {
            int len1 = mid - left + 1;
            int len2 = right - mid;

            var leftArr = new List<int>(len1);
            var rightArr = new List<int>(len2);

            for (int i = 0; i < len1; i++)
                leftArr.Add(array[left + i]);

            for (int i = 0; i < len2; i++)
                rightArr.Add(array[mid + 1 + i]);

            int i1 = 0, i2 = 0, k = left;

            while (i1 < len1 && i2 < len2)
            {
                if (leftArr[i1] <= rightArr[i2])
                    array[k++] = leftArr[i1++];
                else
                    array[k++] = rightArr[i2++];
            }

            while (i1 < len1)
                array[k++] = leftArr[i1++];

            while (i2 < len2)
                array[k++] = rightArr[i2++];
        }
    }
}
