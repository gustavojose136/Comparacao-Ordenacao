using System;
using System.Collections.Generic;

namespace Ordenacao.Services
{
    public class CountingSortService
    {
        public List<int> Sort(List<int> array)
        {
            if (array == null || array.Count == 0)
                return array;

            // Determina o valor mínimo e máximo para definir o range
            int min = array[0];
            int max = array[0];
            foreach (int num in array)
            {
                if (num < min)
                    min = num;
                if (num > max)
                    max = num;
            }

            int range = max - min + 1;
            int[] count = new int[range];
            int[] output = new int[array.Count];

            // Conta as ocorrências de cada valor
            foreach (int num in array)
            {
                count[num - min]++;
            }

            // Calcula as posições finais
            for (int i = 1; i < count.Length; i++)
            {
                count[i] += count[i - 1];
            }

            // Constrói o array de saída (estável)
            for (int i = array.Count - 1; i >= 0; i--)
            {
                output[count[array[i] - min] - 1] = array[i];
                count[array[i] - min]--;
            }

            // Copia o array ordenado de volta para o original
            for (int i = 0; i < array.Count; i++)
            {
                array[i] = output[i];
            }

            return array;
        }
    }
}
