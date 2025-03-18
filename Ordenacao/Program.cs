using OpenTelemetry;
using OpenTelemetry.Trace;
using OpenTelemetry.Exporter;
using Ordenacao.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure OpenTelemetry Tracing & Logging
builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
    {
        tracerProviderBuilder
            .AddAspNetCoreInstrumentation() // Para instrumentar automaticamente requisições HTTP
            .AddHttpClientInstrumentation() // Para instrumentar automaticamente requisições HTTP feitas pelo client
            .AddSource("Ordenacao") // Nome do tracer
            .AddJaegerExporter(options =>
            {
                options.AgentHost = "localhost"; // Defina o host do agente Jaeger (pode ser alterado conforme seu ambiente)
                options.AgentPort = 5775; // Porta do agente Jaeger
            });
    })
    .WithMetrics(metricsProviderBuilder =>
    {
        metricsProviderBuilder.AddAspNetCoreInstrumentation();
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

builder.Services.AddScoped<BubbleSortService>();
builder.Services.AddScoped<InsertionSortService>();
builder.Services.AddScoped<MergeSortService>();
builder.Services.AddScoped<QuickSortService>();
builder.Services.AddScoped<SelectionSortService>();
builder.Services.AddScoped<TimSortService>();
builder.Services.AddScoped<HeapSortService>();
builder.Services.AddScoped<ShellSortService>();
builder.Services.AddScoped<CountingSortService>();
builder.Services.AddScoped<RadixSortService>();
builder.Services.AddScoped<SortComparisonService>();

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
