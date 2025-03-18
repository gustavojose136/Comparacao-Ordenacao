using Ordernacao.Services.Services.Interface;
using System;
using System.Collections.Generic;

namespace Ordenacao.Services
{
    public class SortingContext
    {
        private ISortStrategy _sortStrategy;

        public void SetSortStrategy(ISortStrategy sortStrategy)
        {
            _sortStrategy = sortStrategy;
        }

        public List<int> ExecuteSort(List<int> array)
        {
            if (_sortStrategy == null)
                throw new InvalidOperationException("Nenhuma estratégia de ordenação definida.");
            return _sortStrategy.Sort(array);
        }
    }
}
