# Projeto de Comparação de Algoritmos de Ordenação

Este repositório contém uma solução desenvolvida em .NET para comparar a performance de diferentes algoritmos de ordenação. A aplicação foi construída seguindo uma arquitetura modular, utilizando o **padrão Strategy** para permitir a troca dinâmica dos algoritmos, e atende a diversos requisitos, como a geração de dados, coleta de métricas e registro de logs.

---

## Índice

- [Objetivos da Atividade](#objetivos-da-atividade)
- [Arquitetura e Implementação](#arquitetura-e-implementação)
  - [Padrão Strategy](#padrão-strategy)
  - [Geração e Armazenamento de Dados](#geração-e-armazenamento-de-dados)
  - [Comparação e Métricas](#comparação-e-métricas)
  - [Registro de Logs e Análise](#registro-de-logs-e-análise)
- [Endpoints da API](#endpoints-da-api)
- [Relatório Técnico](#relatório-técnico)
  - [Código-Fonte Documentado e Organizado](#código-fonte-documentado-e-organizado)
  - [Explicação do Uso do Padrão Strategy](#explicação-do-uso-do-padrão-strategy)
  - [Descrição do Processo de Geração dos Dados](#descrição-do-processo-de-geração-dos-dados)
  - [Métricas e Gráficos Comparativos de Desempenho](#métricas-e-gráficos-comparativos-de-desempenho)
  - [Descrição da Ferramenta para Logs e Análise dos Resultados](#descrição-da-ferramenta-para-logs-e-análise-dos-resultados)
  - [Conclusão](#conclusão)
- [Equipe](#equipe)

---

## Objetivos da Atividade

- **Geração de Dados:** Criar um programa que gere um conjunto de números aleatórios e os salve em um arquivo (texto ou binário).
- **Implementação dos Algoritmos de Ordenação:** Desenvolver diversos algoritmos (Bubble Sort, Insertion Sort, Selection Sort, Quick Sort, Merge Sort, Tim Sort, Heap Sort, Counting Sort, Radix Sort, Shell Sort) utilizando o padrão Strategy.
- **Comparação de Performance:** Executar os algoritmos com os mesmos dados e coletar métricas como tempo de execução, número de comparações e trocas.
- **Registro de Logs:** Utilizar OpenTelemetry para registrar logs e métricas detalhadas, possibilitando a análise dos resultados com ferramentas como Jaeger, Prometheus + Grafana ou Elasticsearch + Kibana.

---

## Arquitetura e Implementação

### Padrão Strategy

O **padrão Strategy** é utilizado para encapsular os algoritmos de ordenação em classes separadas que implementam uma interface comum (`ISortStrategy`).  
**Vantagens:**
- **Modularidade e Extensibilidade:** Permite adicionar novos algoritmos sem alterar o código existente.
- **Flexibilidade:** O algoritmo de ordenação pode ser alterado dinamicamente em tempo de execução através de um contexto (`SortingContext`).

### Geração e Armazenamento de Dados

O serviço `DataGeneratorService` é responsável por:
- **Gerar** um conjunto de números aleatórios, de tamanho parametrizável.
- **Salvar** esses dados em um arquivo, no formato texto ou binário, conforme especificado.  
Este serviço permite reutilizar os mesmos dados em diferentes testes dos algoritmos de ordenação.

### Comparação e Métricas

O `SortComparisonService` executa cada algoritmo de ordenação sobre os mesmos dados e coleta as métricas de performance, como:
- Tempo de execução (em milissegundos)
- Número de comparações
- Número de trocas/movimentações

Essas métricas são registradas via `SortLogger`, que utiliza OpenTelemetry para rastreamento e análise dos logs.

### Registro de Logs e Análise

A classe `SortLogger` utiliza o [OpenTelemetry](https://opentelemetry.io/) para criar atividades que registram:
- Nome do algoritmo
- Tamanho do dataset
- Tempo de execução
- Quantidade de operações (comparações e trocas)

Esses logs podem ser enviados para ferramentas de análise e visualização, permitindo a criação de gráficos comparativos e o monitoramento do desempenho dos algoritmos.

---

## Endpoints da API

A aplicação expõe os seguintes endpoints:

- **Comparativos:**  
  `GET /api/ordenacao/comparativos`  
  Retorna as métricas de execução de cada algoritmo e identifica o de melhor performance.

- **Ordenação Específica:**  
  `GET /api/ordenacao/{algorithm}`  
  Executa um algoritmo específico de ordenação, onde `{algorithm}` pode ser, por exemplo, `bubble-sort`, `merge-sort`, etc.

- **Geração de Dados:**  
  `GET /api/ordenacao/gerar-dados`  
  Gera e salva um arquivo com números aleatórios. Os parâmetros permitem definir o tamanho, tipo de arquivo (texto ou binário) e o caminho do arquivo.

---

## Relatório Técnico

### Código-Fonte Documentado e Organizado

O código-fonte está organizado em serviços que seguem o padrão Strategy, com cada algoritmo de ordenação implementado em sua própria classe. A separação das responsabilidades em serviços distintos (como geração de dados, comparação de algoritmos e logging) torna o projeto modular e de fácil manutenção.

### Explicação do Uso do Padrão Strategy

- **Interface Comum:** A interface `ISortStrategy` define o método `Sort`, garantindo que todas as implementações tenham a mesma assinatura.
- **Contexto:** A classe `SortingContext` é responsável por receber uma estratégia e executar o método de ordenação.
- **Injeção de Dependências:** Todos os algoritmos são registrados como serviços, permitindo que o controller injete um `IEnumerable<ISortStrategy>` e selecione dinamicamente o algoritmo a ser utilizado.

### Descrição do Processo de Geração dos Dados

O `DataGeneratorService` gera uma lista de números aleatórios utilizando um gerador de números (`Random`).  
- **Parâmetros:** Permite definir o tamanho do conjunto, e os limites mínimo e máximo dos números.
- **Armazenamento:** Os dados podem ser salvos em arquivo de texto (com números separados por vírgula) ou em formato binário, facilitando a reutilização dos dados para testes dos algoritmos de ordenação.

### Métricas e Gráficos Comparativos de Desempenho

Durante a execução dos algoritmos, são coletadas as seguintes métricas:
- **Tempo de Execução:** Medido em milissegundos.
- **Comparações e Trocas:** Quantidade de operações realizadas durante a ordenação.  

Esses dados podem ser exportados para ferramentas de visualização (como Grafana, por exemplo) para gerar gráficos comparativos e análises de desempenho. Embora os gráficos não estejam embutidos no código, a estrutura de logs permite uma integração fácil com ferramentas de monitoramento.

### Descrição da Ferramenta para Logs e Análise dos Resultados

A aplicação utiliza o **OpenTelemetry** para registrar logs e métricas de execução.  
- **OpenTelemetry:** Uma solução open-source que permite a instrumentação de aplicações para monitoramento e tracing.
- **Análise:** Os logs gerados podem ser enviados para ferramentas como Jaeger, Prometheus + Grafana ou Elasticsearch + Kibana, facilitando a análise e visualização dos resultados de performance.

### Conclusão

Com base nos testes realizados:
- **Desempenho:** Em cenários com grandes volumes de dados, algoritmos que utilizam a abordagem de *dividir e conquistar* (como Quick Sort, Merge Sort e Tim Sort) geralmente apresentam melhor desempenho em termos de tempo de execução.
- **Justificativa:** Esses algoritmos dividem o problema em subproblemas menores, que são resolvidos de forma mais eficiente, e combinam os resultados para formar o conjunto ordenado.
- **Dividir e Conquistar:** Vale a pena utilizar essa abordagem, principalmente em conjuntos de dados maiores, pois a redução da complexidade (muitas vezes para O(n log n)) compensa o overhead de particionar e recombinar os dados.

---

## Equipe

- **Gustavo José Rosa**
- **Thomas Taiga Martinez**
- **Thiago Cirne**
- **Eduardo da Maia Haak**
