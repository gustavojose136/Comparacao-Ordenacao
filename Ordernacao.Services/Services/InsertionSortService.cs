using System;
using System.Collections.Generic;

namespace Ordenacao.Services
{
    public class InsertionSortService
    {
        public List<int> Sort(List<int> array)
        {
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
                }
                array[j + 1] = key;
            }
            return array;
        }
    }
}
