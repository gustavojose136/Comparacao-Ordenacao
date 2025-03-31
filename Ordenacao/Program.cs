using OpenTelemetry;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics; // Adicionado para métricas
using OpenTelemetry.Exporter;
using Ordenacao.Services;
using Ordernacao.Services.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

// Configure OpenTelemetry Tracing & Metrics
builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
    {
        tracerProviderBuilder
            .AddAspNetCoreInstrumentation() // Instrumentação de requisições HTTP (tracing)
            .AddHttpClientInstrumentation() // Instrumentação de chamadas HTTP do cliente (tracing)
            .AddSource("Ordenacao") // Nome do tracer
            .AddJaegerExporter(options =>
            {
                options.AgentHost = "localhost"; // Host do Jaeger
                options.AgentPort = 5775; // Porta do Jaeger
            });
    })
    .WithMetrics(metricsProviderBuilder =>
    {
        metricsProviderBuilder
            .AddAspNetCoreInstrumentation(); // Instrumentação de métricas para ASP.NET Core
    });

// Add logging with OpenTelemetry
builder.Logging.AddOpenTelemetry(options =>
{
    options.IncludeScopes = true;
    options.IncludeFormattedMessage = true;
    options.ParseStateValues = true;
});

// Add services & controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ISortStrategy, BubbleSortService>();
builder.Services.AddScoped<ISortStrategy, CountingSortService>();
builder.Services.AddScoped<ISortStrategy, HeapSortService>();
builder.Services.AddScoped<ISortStrategy, InsertionSortService>();
builder.Services.AddScoped<ISortStrategy, MergeSortService>();
builder.Services.AddScoped<ISortStrategy, QuickSortService>();
builder.Services.AddScoped<ISortStrategy, RadixSortService>();
builder.Services.AddScoped<ISortStrategy, SelectionSortService>();
builder.Services.AddScoped<ISortStrategy, ShellSortService>();
builder.Services.AddScoped<ISortStrategy, TimSortService>();

builder.Services.AddScoped<SortComparisonService>();
builder.Services.AddScoped<DataGeneratorService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();