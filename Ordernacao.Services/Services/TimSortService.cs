using Ordernacao.Services.Services.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

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

            // Aplicar Insertion Sort a subarrays de tamanho RUN
            for (int i = 0; i < n; i += RUN)
                InsertionSort(array, i, Math.Min(i + RUN - 1, n - 1), ref comparisons, ref swaps);

            // Mesclagem progressiva das sublistas, usando paralelismo
            for (int size = RUN; size < n; size *= 2)
            {
                List<Task<Tuple<int, int>>> tasks = new List<Task<Tuple<int, int>>>();

                // Criar uma cópia da lista para evitar problemas de concorrência
                List<int> tempArray = new List<int>(array);

                for (int left = 0; left < n; left += 2 * size)
                {
                    int mid = left + size - 1;
                    int right = Math.Min((left + 2 * size - 1), (n - 1));

                    if (mid < right && mid >= 0 && right >= 0)
                    {
                        tasks.Add(Task.Run(() => Merge(tempArray, left, mid, right)));
                    }
                }

                // Esperar todas as tarefas de mesclagem terminarem
                Task.WhenAll(tasks).Wait();

                foreach (var task in tasks)
                {
                    comparisons += task.Result.Item1;
                    swaps += task.Result.Item2;
                }

                // Copiar os resultados mesclados de volta para o array principal
                array = new List<int>(tempArray);
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

        private Tuple<int, int> Merge(List<int> array, int left, int mid, int right)
        {
            int comparisons = 0, swaps = 0;

            // Garantir que os tamanhos dos arrays sejam válidos
            int len1 = Math.Max(0, mid - left + 1);
            int len2 = Math.Max(0, right - mid);

            if (len1 == 0 || len2 == 0) return Tuple.Create(comparisons, swaps);

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

            return Tuple.Create(comparisons, swaps);
        }
    }
}


// Parallelizing the Merge Process:
// The merging process is now parallelized using Task.Run. We create tasks for each merge operation and execute them 
// concurrently. After all tasks are created, we use Task.WhenAll to wait for them to finish before continuing.
// InsertionSort:
// InsertionSort is kept sequential as it is typically efficient for small subarrays (like those in TimSort) and 
// parallelizing it may not yield significant benefits for small-sized data.
// Thread Safety:
// The Merge method is still being executed with thread-safety in mind, as each merge task is independent of others.
// Task Management:
// The use of Task.WhenAll(tasks).Wait() ensures that all merge tasks complete before moving on to the next stage.