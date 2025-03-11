using System;
using System.Collections.Generic;

namespace Ordenacao.Services
{
    public class ShellSortService
    {
        public List<int> Sort(List<int> array)
        {
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

                    // Insere array[i] na sublista ordenada
                    while (j >= gap && array[j - gap] > temp)
                    {
                        array[j] = array[j - gap];
                        j -= gap;
                    }

                    array[j] = temp;
                }
            }

            return array;
        }
    }
}
