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
        private readonly BubbleSortService _bubbleSortService;
        private readonly SelectionSortService _selectionSortService;
        private readonly InsertionSortService _insertionSortService;
        private readonly MergeSortService _mergeSortService;
        private readonly QuickSortService _quickSortService;
        private readonly SortComparisonService _sortComparisonService;
        private readonly TimSortService _timSortService;    

        public OrdenacaoController(
            BubbleSortService bubbleSortService,
            SelectionSortService selectionSortService,
            InsertionSortService insertionSortService,
            MergeSortService mergeSortService,
            QuickSortService quickSortService,
            SortComparisonService sortComparisonService,
            TimSortService timSortService)
        {
            _bubbleSortService = bubbleSortService;
            _selectionSortService = selectionSortService;
            _insertionSortService = insertionSortService;
            _mergeSortService = mergeSortService;
            _quickSortService = quickSortService;
            _sortComparisonService = sortComparisonService;
            _timSortService = timSortService;
        }

        /// <summary>
        /// Retorna os tempos de execução de cada algoritmo de ordenação e o melhor deles.
        /// </summary>
        [HttpGet("comparativos")]
        public IActionResult GetComparativos([FromQuery] string array = null, [FromQuery] int tamanho = 10)
        {
            try
            {
                var arrayInt = ObterArray(array, tamanho);
                var comparativo = _sortComparisonService.CompareSorts(arrayInt);
                return Ok(comparativo);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Erro ao processar a requisição", details = ex.Message });
            }
        }

        /// <summary>
        /// Ordena um array utilizando o Bubble Sort.
        /// </summary>
        [HttpGet("bubble-sort")]
        public IActionResult GetBubbleSort([FromQuery] string array = null, [FromQuery] int tamanho = 10)
        {
            try
            {
                var arrayInt = ObterArray(array, tamanho);
                var sortedArray = _bubbleSortService.Sort(arrayInt);
                return Ok(sortedArray);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Erro ao processar a requisição", details = ex.Message });
            }
        }

        /// <summary>
        /// Ordena um array utilizando o Selection Sort.
        /// </summary>
        [HttpGet("selection-sort")]
        public IActionResult GetSelectionSort([FromQuery] string array = null, [FromQuery] int tamanho = 10)
        {
            try
            {
                var arrayInt = ObterArray(array, tamanho);
                var sortedArray = _selectionSortService.Sort(arrayInt);
                return Ok(sortedArray);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Erro ao processar a requisição", details = ex.Message });
            }
        }

        /// <summary>
        /// Ordena um array utilizando o Insertion Sort.
        /// </summary>
        [HttpGet("insertion-sort")]
        public IActionResult GetInsertionSort([FromQuery] string array = null, [FromQuery] int tamanho = 10)
        {
            try
            {
                var arrayInt = ObterArray(array, tamanho);
                var sortedArray = _insertionSortService.Sort(arrayInt);
                return Ok(sortedArray);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Erro ao processar a requisição", details = ex.Message });
            }
        }

        /// <summary>
        /// Ordena um array utilizando o Merge Sort.
        /// </summary>
        [HttpGet("merge-sort")]
        public IActionResult GetMergeSort([FromQuery] string array = null, [FromQuery] int tamanho = 10)
        {
            try
            {
                var arrayInt = ObterArray(array, tamanho);
                var sortedArray = _mergeSortService.Sort(arrayInt);
                return Ok(sortedArray);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Erro ao processar a requisição", details = ex.Message });
            }
        }

        /// <summary>
        /// Ordena um array utilizando o Quick Sort.
        /// </summary>
        [HttpGet("quick-sort")]
        public IActionResult GetQuickSort([FromQuery] string array = null, [FromQuery] int tamanho = 10)
        {
            try
            {
                var arrayInt = ObterArray(array, tamanho);
                var sortedArray = _quickSortService.Sort(arrayInt);
                return Ok(sortedArray);
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

        [HttpGet("tim-sort")]
        public IActionResult GetTimSort([FromQuery] string array = null, [FromQuery] int tamanho = 10)
        {
            try
            {
                var arrayInt = ObterArray(array, tamanho);
                var sortedArray = _timSortService.Sort(arrayInt);
                return Ok(sortedArray);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Erro ao processar a requisição", details = ex.Message });
            }
        }

    }
}
