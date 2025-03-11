using System;
using System.Collections.Generic;

namespace Ordenacao.Services
{
    public class SelectionSortService
    {
        public List<int> Sort(List<int> array)
        {
            if (array == null || array.Count == 0) return new List<int>();

            int n = array.Count;
            for (int i = 0; i < n - 1; i++)
            {
                int minIndex = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (array[j] < array[minIndex])
                        minIndex = j;
                }

                (array[i], array[minIndex]) = (array[minIndex], array[i]);
            }
            return array;
        }
    }
}
