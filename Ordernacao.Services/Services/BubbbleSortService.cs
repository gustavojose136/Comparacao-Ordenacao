using Ordernacao.Services.Services.Interface;
using System;
using System.Collections.Generic;

namespace Ordenacao.Services
{
    public class BubbleSortService : IBubbleSortService
    {
        public List<int> Sort(List<int> array)
        {
            if (array == null || array.Count == 0) return new List<int>();

            int n = array.Count;
            bool swapped;

            for (int i = 0; i < n - 1; i++)
            {
                swapped = false;
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (array[j] > array[j + 1])
                    {
                        (array[j], array[j + 1]) = (array[j + 1], array[j]);
                        swapped = true;
                    }
                }
                if (!swapped) break;
            }
            return array;
        }
    }
}
