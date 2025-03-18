using Microsoft.AspNetCore.Mvc;
using Ordenacao.Services;
using Ordernacao.Services.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ordenacao.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdenacaoController : ControllerBase
    {
        private readonly SortingContext _sortingContext;
        private readonly IDictionary<string, ISortStrategy> _sortStrategies;
        private readonly SortComparisonService _sortComparisonService;
        private readonly DataGeneratorService _dataGeneratorService;

        public OrdenacaoController(IEnumerable<ISortStrategy> sortStrategies,
                                   SortComparisonService sortComparisonService,
                                   DataGeneratorService dataGeneratorService)
        {
            _sortingContext = new SortingContext();
            // Usa o nome da classe (removendo "Service") em lowercase para chave
            _sortStrategies = sortStrategies.ToDictionary(s => s.GetType().Name.Replace("Service", "").ToLower());
            _sortComparisonService = sortComparisonService;
            _dataGeneratorService = dataGeneratorService;
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
                var comparativo = _sortComparisonService.CompareSorts(arrayInt);
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

                if (_sortStrategies.TryGetValue(algorithm.ToLower(), out var strategy))
                {
                    _sortingContext.SetSortStrategy(strategy);
                    var sortedArray = _sortingContext.ExecuteSort(arrayInt);
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
        /// Endpoint para gerar um arquivo com números aleatórios.
        /// </summary>
        [HttpGet("gerar-dados")]
        public IActionResult GerarDados([FromQuery] int tamanho = 100000,
                                        [FromQuery] string fileType = "text",
                                        [FromQuery] string filePath = "dados.txt")
        {
            try
            {
                var randomNumbers = _dataGeneratorService.GenerateRandomNumbers(tamanho);
                // Define o tipo de arquivo: "binary" para binário, qualquer outro para texto
                FileType tipo = fileType.ToLower() == "binary" ? FileType.Binary : FileType.Text;
                _dataGeneratorService.SaveNumbersToFile(randomNumbers, filePath, tipo);

                return Ok(new { message = $"Arquivo gerado com sucesso em {filePath}" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Erro ao gerar o arquivo", details = ex.Message });
            }
        }

        /// <summary>
        /// Converte a string recebida ou gera um array aleatório.
        /// </summary>
        private List<int> ObterArray(string array, int tamanho)
        {
            if (!string.IsNullOrWhiteSpace(array))
            {
                return new List<int>(Array.ConvertAll(array.Split(','), int.Parse));
            }

            var random = new Random();
            return Enumerable.Range(0, tamanho).Select(_ => random.Next(0, 100)).ToList();
        }
    }
}
