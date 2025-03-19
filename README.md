# Relatório Técnico: Comparação de Algoritmos de Ordenação

## Equipe

- **Gustavo José Rosa**
- **Thomas Taiga Martinez**
- **Thiago Cirne**
- **Eduardo da Maia Haak**

## 1. Introdução
Este relatório apresenta os resultados de uma atividade prática realizada por uma equipe de até 4 alunos, com o objetivo de implementar, comparar e analisar a performance de diferentes algoritmos de ordenação. A atividade foi desenvolvida em C# e seguiu os requisitos especificados, incluindo a utilização do padrão de projeto Strategy, coleta de métricas com OpenTelemetry e visualização de logs com Jaeger. Além disso, foram gerados dados aleatórios para teste e analisados os resultados por meio de métricas e gráficos comparativos.
Mais específicamente, o processo de execução dos métodos de ordenação e contagem dos tempos de execução são realizados através de uma API que na ausência do Jaeger pode ser rodada e testada localmente e possui integração com Swagger.

## 2. Código-fonte Documentado e Organizado
O código-fonte foi estruturado de forma modular e organizada, seguindo boas práticas de desenvolvimento. A solução foi dividida em projetos e namespaces para facilitar a manutenção e a compreensão. A estrutura do projeto é a seguinte:

- **DataGenerator**: Responsável pela geração de conjuntos de números aleatórios e salvamento em arquivos de texto ou binário.
- **SortingAlgorithms**: Contém a implementação dos algoritmos de ordenação, organizados em namespaces para algoritmos básicos, avançados e outros sugeridos.
- **Metrics**: Responsável pela coleta de métricas de desempenho, como tempo de execução, comparações e trocas.
- **Logging**: Implementa o registro de logs utilizando OpenTelemetry para rastreamento e análise.
- **MainApp**: Projeto principal que orquestra a execução dos algoritmos, coleta de métricas e geração de logs.

### Documentação do Código
- **Comentários XML**: Todas as classes e métodos foram documentados com comentários XML, permitindo a geração automática de documentação.
- **Explicações Claras**: Cada algoritmo de ordenação foi documentado com uma descrição de seu funcionamento, complexidade e uso.
- **Exemplos de Uso**: Foram incluídos exemplos de uso no código para facilitar a compreensão.

## 3. Explicação do Uso do Padrão Strategy
O padrão Strategy foi utilizado para encapsular cada algoritmo de ordenação em uma classe separada, permitindo que eles sejam intercambiáveis dinamicamente. Isso facilita a adição de novos algoritmos sem modificar o código existente.

### Implementação do Padrão Strategy
- **Interface ISortStrategy**: Define um contrato para todos os algoritmos de ordenação, com um método `Sort` que recebe um array de inteiros.
- **Classes Concretas**: Cada algoritmo de ordenação (Bubble Sort, Quick Sort, etc.) implementa a interface `ISortStrategy`.
- **Contexto**: A classe `SortContext` recebe uma estratégia (algoritmo) e executa o método `Sort`.

### Código do Padrão Strategy
Abaixo está o trecho de código que implementa o padrão Strategy para a execução dos algoritmos de ordenação:

```csharp
[HttpGet("{algorithm}")]
public IActionResult GetSortResult(string algorithm, [FromQuery] string array = null, [FromQuery] int tamanho = 100000)
{
    try
    {
        var arrayInt = ObterArray(array, tamanho);

        // Start a custom span for the sorting operation
        using var span = _tracer.StartActiveSpan($"Sort-{algorithm}");
        span.SetAttribute("algorithm", algorithm); // Add algorithm name as an attribute
        span.SetAttribute("array_size", arrayInt.Count); // Add array size as an attribute

        // If the algorithm exists in the dictionary, execute it
        if (_sortServices.ContainsKey(algorithm.ToLower()))
        {
            var sortedArray = _sortServices[algorithm.ToLower()](arrayInt);
            span.SetAttribute("status", "success"); // Mark the span as successful
            return Ok(sortedArray);
        }
        else
        {
            span.SetAttribute("status", "error"); // Mark the span as failed
            span.SetStatus(Status.Error.WithDescription("Algorithm not found")); // Set error status
            return BadRequest(new { error = "Algoritmo de ordenação não encontrado" });
        }
    }
    catch (Exception ex)
    {
        // If an exception occurs, record it in the span
        using var span = _tracer.StartActiveSpan($"Sort-{algorithm}-Error");
        span.SetAttribute("algorithm", algorithm);
        
        span.SetAttribute("status", "error");
        span.RecordException(ex); // Record the exception details
        span.SetStatus(Status.Error.WithDescription(ex.Message));

        return BadRequest(new { error = "Erro ao processar a requisição", details = ex.Message });
    }
}
```

### Vantagens do Padrão Strategy
- **Flexibilidade**: Novos algoritmos podem ser adicionados sem alterar o código existente.
- **Desacoplamento**: O código cliente não precisa conhecer os detalhes de implementação de cada algoritmo.
- **Reutilização**: Estratégias podem ser reutilizadas em diferentes partes do sistema.

## 4. Descrição do Processo de Geração dos Dados
O processo de geração de dados foi implementado no projeto `DataGenerator`. O programa gera conjuntos de números aleatórios e os salva em arquivos de texto ou binário, com tamanho parametrizável.

### Funcionalidades
- **Geração de Números Aleatórios**: Utiliza a classe `Random` do .NET para gerar números inteiros.
- **Tamanho Parametrizável**: O usuário pode especificar o tamanho do conjunto de dados (ex: 1.000, 10.000, 100.000 números).

### Exemplo de Uso
O gerador de dados foi configurado para criar arquivos com diferentes tamanhos (1.000, 10.000 e 100.000 números), que foram utilizados como entrada para os algoritmos de ordenação.

## 5. Métricas e Gráficos Comparativos de Desempenho
Foram coletadas as seguintes métricas para cada algoritmo de ordenação:

- **Tempo de Execução**: Medido em segundos.
- **Número de Comparações**: Quantidade de comparações realizadas.
- **Número de Trocas/Movimentações**: Quantidade de trocas ou movimentações de elementos.

### Métricas Coletadas
As métricas foram coletadas para um conjunto de dados com 100.000 números. Abaixo está uma tabela resumida com os tempos de execução:

| Algoritmo        | Tamanho do Conjunto | Tempo (segundos) |
|------------------|---------------------|------------------|
| Selection Sort   | 100.000             | 0.30             |
| Insertion Sort   | 100.000             | 0.31             |
| Heap Sort        | 100.000             | 0.03             |
| Counting Sort    | 100.000             | 0.01             |
| Radix Sort       | 100.000             | 0.01             |
| Shell Sort       | 100.000             | 0.03             |
| Merge Sort       | 100.000             | 0.05             |
| Quick Sort       | 100.000             | 0.15             |

### Gráficos Comparativos
O gráfico abaixo mostra o tempo de execução de cada algoritmo para um conjunto de 100.000 números:

[graficocomp](https://github.com/user-attachments/assets/cf40bc80-3ff8-4cf4-8aa9-24f686d673ae)


#### Análise do Gráfico
- **Selection Sort** e **Insertion Sort** apresentaram os piores tempos de execução, próximos de 0,30 e 0,31 segundos, respectivamente, devido à sua complexidade \(O(n^2)\).
- **Counting Sort** e **Radix Sort** foram os mais rápidos, com tempos próximos de 0,01 segundos, beneficiando-se de sua complexidade linear \(O(n + k)\), ideal para números inteiros limitados.
- **Quick Sort** e **Merge Sort**, algoritmos baseados em "dividir para conquistar", apresentaram tempos melhores (0,15 e 0,05 segundos, respectivamente) em comparação com os algoritmos básicos, com complexidade \(O(n \log n)\).

## 6. Descrição da Ferramenta Utilizada para Logs e Análise dos Resultados
Foi utilizada a ferramenta **Jaeger** para rastreamento de execução com OpenTelemetry. O OpenTelemetry foi configurado para enviar logs contendo:

- Nome do algoritmo.
- Tamanho do conjunto de dados.
- Tempo de execução.
- Número de comparações e trocas.

### Configuração do OpenTelemetry
O OpenTelemetry foi configurado para enviar métricas e logs ao Jaeger, permitindo a visualização detalhada do desempenho dos algoritmos.

### Visualização no Jaeger
- **Tracing**: O Jaeger foi utilizado para rastrear a execução dos algoritmos, mostrando o tempo gasto em cada etapa.
- **Spans Personalizados**: Foram criados spans para registrar o início e o fim de cada operação de ordenação, incluindo atributos como o nome do algoritmo e o tamanho do conjunto de dados.

![image](https://github.com/user-attachments/assets/9bee8e17-8208-458e-bf39-ceb93f611f04)


## 7. Conclusão
### Desempenho dos Algoritmos
- **Algoritmos Básicos**: Selection Sort e Insertion Sort apresentaram desempenho inferior para grandes conjuntos de dados, com tempos de execução elevados devido à complexidade \(O(n^2)\).
- **Algoritmos Avançados**: Quick Sort e Merge Sort mostraram-se mais eficientes para grandes volumes de dados, com tempos de execução de 0,15 e 0,05 segundos, respectivamente, beneficiando-se da complexidade \(O(n \log n)\).
- **Outros Algoritmos**: Counting Sort e Radix Sort foram os mais rápidos, com tempos de 0,01 segundos, sendo ideais para conjuntos de inteiros limitados devido à sua complexidade linear \(O(n + k)\).

### Vale a Pena Usar Dividir e Conquistar?
Sim, algoritmos como Quick Sort e Merge Sort são ideais para grandes conjuntos de dados devido à sua eficiência e escalabilidade. No entanto, para conjuntos pequenos ou específicos (como inteiros limitados), algoritmos como Counting Sort ou Radix Sort podem ser mais adequados, pois apresentam complexidade linear e tempos de execução extremamente baixos.

### Considerações Finais
A atividade foi uma oportunidade valiosa para compreender as diferenças práticas entre os algoritmos de ordenação, bem como para aplicar conceitos de design de software (padrão Strategy) e monitoramento (OpenTelemetry com Jaeger). A análise detalhada das métricas e a visualização dos logs permitiram uma compreensão mais profunda do comportamento de cada algoritmo em diferentes cenários.
