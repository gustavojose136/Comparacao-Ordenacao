using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordernacao.Services.Services.Interface
{
    public interface ISortStrategy
    {
        List<int> Sort(List<int> array);
    }

}
