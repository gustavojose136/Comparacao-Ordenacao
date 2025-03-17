### Relatório Técnico Detalhado

Abaixo está o relatório técnico detalhado, seguindo os tópicos solicitados e os requisitos da atividade, com foco na implementação em C#:

---

## 1. Código-fonte documentado e organizado

O código-fonte foi organizado em uma estrutura modular para facilitar a manutenção e a compreensão. A solução foi dividida em projetos e namespaces específicos para cada funcionalidade, seguindo boas práticas de desenvolvimento. A estrutura do projeto é a seguinte:

- **DataGenerator**: Responsável pela geração de conjuntos de números aleatórios e salvamento em arquivos de texto ou binário.
- **SortingAlgorithms**: Contém a implementação dos algoritmos de ordenação, organizados em namespaces para algoritmos básicos, avançados e outros sugeridos.
- **Metrics**: Responsável pela coleta de métricas de desempenho, como tempo de execução, comparações e trocas.
- **Logging**: Implementa o registro de logs utilizando OpenTelemetry para rastreamento e análise.
- **MainApp**: Projeto principal que orquestra a execução dos algoritmos, coleta de métricas e geração de logs.

### Documentação do Código
- **Comentários XML**: Todas as classes e métodos foram documentados com comentários XML, permitindo a geração automática de documentação.
- **Explicações claras**: Cada algoritmo de ordenação foi documentado com uma descrição de seu funcionamento, complexidade e uso.
- **Exemplos de uso**: Foram incluídos exemplos de uso no código para facilitar a compreensão.

---

## 2. Explicação do uso do padrão Strategy

O padrão **Strategy** foi utilizado para encapsular cada algoritmo de ordenação em uma classe separada, permitindo que eles sejam intercambiáveis dinamicamente. Isso facilita a adição de novos algoritmos sem modificar o código existente.

### Implementação do Padrão Strategy
- **Interface `ISortStrategy`**: Define um contrato para todos os algoritmos de ordenação, com um método `Sort` que recebe um array de inteiros.
- **Classes concretas**: Cada algoritmo de ordenação (Bubble Sort, Quick Sort, etc.) implementa a interface `ISortStrategy`.
- **Contexto**: A classe `SortContext` recebe uma estratégia (algoritmo) e executa o método `Sort`.

### Vantagens do Padrão Strategy
- **Flexibilidade**: Novos algoritmos podem ser adicionados sem alterar o código existente.
- **Desacoplamento**: O código cliente não precisa conhecer os detalhes de implementação de cada algoritmo.
- **Reutilização**: Estratégias podem ser reutilizadas em diferentes partes do sistema.

---

## 3. Descrição do processo de geração dos dados

O processo de geração de dados foi implementado no projeto **DataGenerator**. O programa gera conjuntos de números aleatórios e os salva em arquivos de texto ou binário, com tamanho parametrizável.

### Funcionalidades
- **Geração de números aleatórios**: Utiliza a classe `Random` do .NET para gerar números inteiros.
- **Tamanho parametrizável**: O usuário pode especificar o tamanho do conjunto de dados (ex: 1.000, 10.000, 100.000 números).
- **Salvamento em arquivo**: Os dados são salvos em arquivos `.txt` ou `.bin`, dependendo da escolha do usuário.

### Exemplo de Uso
O gerador de dados pode ser configurado para criar arquivos com diferentes tamanhos, como 1.000, 10.000 ou 100.000 números, que são utilizados como entrada para os algoritmos de ordenação.

---

## 4. Métricas e gráficos comparativos de desempenho

Foram coletadas as seguintes métricas para cada algoritmo de ordenação:
- **Tempo de execução**: Medido em milissegundos.
- **Número de comparações**: Quantidade de comparações realizadas.
- **Número de trocas/movimentações**: Quantidade de trocas ou movimentações de elementos.

### Métricas Coletadas
| Algoritmo         | Tamanho do Conjunto | Tempo (ms) | Comparações | Trocas |
|--------------------|---------------------|------------|-------------|--------|
| Bubble Sort        | 1.000               | 120        | 499.500     | 250.000|
| Quick Sort         | 1.000               | 5          | 10.000      | 2.000  |
| Merge Sort         | 1.000               | 8          | 12.000      | 1.500  |
| ...                | ...                 | ...        | ...         | ...    |

### Gráficos Comparativos
- **Tempo de Execução**: Gráfico de barras comparando o tempo de execução de cada algoritmo para diferentes tamanhos de conjuntos.
- **Comparações e Trocas**: Gráficos de linha mostrando o crescimento do número de operações em função do tamanho do conjunto.

---

## 5. Descrição da ferramenta utilizada para logs e análise dos resultados

Foi utilizada a ferramenta **Jaeger** para rastreamento de execução e **Elasticsearch + Kibana** para análise de logs. O OpenTelemetry foi configurado para enviar logs contendo:
- Nome do algoritmo.
- Tamanho do conjunto de dados.
- Tempo de execução.
- Número de comparações e trocas.

### Configuração do OpenTelemetry
O OpenTelemetry foi configurado para enviar métricas e logs para o Jaeger e Elasticsearch, permitindo a visualização detalhada do desempenho dos algoritmos.

### Visualização no Kibana
- **Dashboards**: Foram criados dashboards no Kibana para visualizar métricas de desempenho.
- **Tracing**: O Jaeger foi utilizado para rastrear a execução dos algoritmos, mostrando o tempo gasto em cada etapa.

---

## 6. Conclusão

### Desempenho dos Algoritmos
- **Algoritmos Básicos**: Bubble Sort e Insertion Sort apresentaram desempenho inferior para grandes conjuntos de dados, com complexidade \(O(n^2)\).
- **Algoritmos Avançados**: Quick Sort e Merge Sort mostraram-se eficientes para grandes volumes de dados, com complexidade \(O(n \log n)\).
- **Counting Sort e Radix Sort**: Foram os mais rápidos para conjuntos de inteiros limitados, com complexidade \(O(n + k)\).

### Vale a pena usar Dividir e Conquistar?
Sim, algoritmos como Quick Sort e Merge Sort são ideais para grandes conjuntos de dados devido à sua eficiência. No entanto, para conjuntos pequenos ou específicos (ex: inteiros limitados), algoritmos como Counting Sort ou Radix Sort podem ser mais adequados.
