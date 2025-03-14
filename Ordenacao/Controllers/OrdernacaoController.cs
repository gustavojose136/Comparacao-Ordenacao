using Microsoft.AspNetCore.Mvc;
using Ordenacao.Services;
using System;
using System.Collections.Generic;

namespace Ordenacao.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdenacaoController : ControllerBase
    {
        private readonly IDictionary<string, Func<List<int>, List<int>>> _sortServices;
        private readonly SortComparisonService _sortComparisonService; // Add this field

        public OrdenacaoController(
            BubbleSortService bubbleSortService,
            SelectionSortService selectionSortService,
            InsertionSortService insertionSortService,
            MergeSortService mergeSortService,
            QuickSortService quickSortService,
            SortComparisonService sortComparisonService, // Inject SortComparisonService
            TimSortService timSortService)
        {
            // Set up the dictionary for sorting algorithms
            _sortServices = new Dictionary<string, Func<List<int>, List<int>>>
            {
                { "bubble-sort", bubbleSortService.Sort },
                { "selection-sort", selectionSortService.Sort },
                { "insertion-sort", insertionSortService.Sort },
                { "merge-sort", mergeSortService.Sort },
                { "quick-sort", quickSortService.Sort },
                { "tim-sort", timSortService.Sort }
            };

            // Assign the injected SortComparisonService
            _sortComparisonService = sortComparisonService;
        }

        /// <summary>
        /// Retorna os tempos de execução de cada algoritmo de ordenação e o melhor deles.
        /// </summary>
        [HttpGet("comparativos")]
        public IActionResult GetComparativos([FromQuery] string array = null, [FromQuery] int tamanho = 100000)
        {
            try
            {
                var arrayInt = ObterArray(array, tamanho);
                var comparativo = _sortComparisonService.CompareSorts(arrayInt); // Use SortComparisonService here
                return Ok(comparativo);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Erro ao processar a requisição", details = ex.Message });
            }
        }

        /// <summary>
        /// Ordena um array utilizando o algoritmo especificado.
        /// </summary>
        [HttpGet("{algorithm}")]
        public IActionResult GetSortResult(string algorithm, [FromQuery] string array = null, [FromQuery] int tamanho = 100000)
        {
            try
            {
                var arrayInt = ObterArray(array, tamanho);

                // If the algorithm exists in the dictionary, execute it
                if (_sortServices.ContainsKey(algorithm.ToLower()))
                {
                    var sortedArray = _sortServices[algorithm.ToLower()](arrayInt);
                    return Ok(sortedArray);
                }
                else
                {
                    return BadRequest(new { error = "Algoritmo de ordenação não encontrado" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Erro ao processar a requisição", details = ex.Message });
            }
        }

        /// <summary>
        /// Gera um array aleatório caso o usuário não forneça um.
        /// </summary>
        private List<int> ObterArray(string array, int tamanho)
        {
            if (!string.IsNullOrWhiteSpace(array))
            {
                return new List<int>(Array.ConvertAll(array.Split(','), int.Parse));
            }

            var random = new Random();
            var randomArray = new List<int>();
            for (int i = 0; i < tamanho; i++)
            {
                randomArray.Add(random.Next(0, 100)); // Números aleatórios entre 0 e 99
            }
            return randomArray;
        }
    }
}
