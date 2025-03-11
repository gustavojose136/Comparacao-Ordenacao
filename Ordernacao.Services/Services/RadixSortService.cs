using System;
using System.Collections.Generic;

namespace Ordenacao.Services
{
    public class RadixSortService
    {
        public List<int> Sort(List<int> array)
        {
            if (array == null || array.Count == 0)
                return array;

            // Encontra o valor máximo para determinar o número de dígitos
            int max = array[0];
            foreach (int num in array)
            {
                if (num > max)
                    max = num;
            }

            // Aplica o Counting Sort para cada dígito (começando da unidade)
            for (int exp = 1; max / exp > 0; exp *= 10)
            {
                CountSortByDigit(array, exp);
            }

            return array;
        }

        private void CountSortByDigit(List<int> array, int exp)
        {
            int n = array.Count;
            int[] output = new int[n];
            int[] count = new int[10];

            // Conta ocorrências para o dígito atual
            for (int i = 0; i < n; i++)
            {
                int digit = (array[i] / exp) % 10;
                count[digit]++;
            }

            // Calcula as posições reais
            for (int i = 1; i < 10; i++)
            {
                count[i] += count[i - 1];
            }

            // Constroi o array de saída de forma estável
            for (int i = n - 1; i >= 0; i--)
            {
                int digit = (array[i] / exp) % 10;
                output[count[digit] - 1] = array[i];
                count[digit]--;
            }

            // Copia o array ordenado para o original
            for (int i = 0; i < n; i++)
            {
                array[i] = output[i];
            }
        }
    }
}
