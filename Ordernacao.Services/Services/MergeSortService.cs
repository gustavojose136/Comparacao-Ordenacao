using System;
using System.Collections.Generic;

namespace Ordenacao.Services
{
    public class MergeSortService
    {
        public List<int> Sort(List<int> array)
        {
            if (array == null || array.Count == 0) return new List<int>();
            return MergeSort(array);
        }

        private List<int> MergeSort(List<int> array)
        {
            if (array.Count <= 1)
                return array;

            int mid = array.Count / 2;
            var left = MergeSort(array.GetRange(0, mid));
            var right = MergeSort(array.GetRange(mid, array.Count - mid));

            return Merge(left, right);
        }

        private List<int> Merge(List<int> left, List<int> right)
        {
            List<int> result = new List<int>();
            int i = 0, j = 0;

            while (i < left.Count && j < right.Count)
            {
                if (left[i] < right[j])
                    result.Add(left[i++]);
                else
                    result.Add(right[j++]);
            }

            result.AddRange(left.GetRange(i, left.Count - i));
            result.AddRange(right.GetRange(j, right.Count - j));

            return result;
        }
    }
}
