using Ordenacao.Services;

var builder = WebApplication.CreateBuilder(args);

// Adiciona servi�os ao container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registra os servi�os de ordena��o
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

// Servi�o que compara os algoritmos
builder.Services.AddScoped<SortComparisonService>();

var app = builder.Build();

// Configura o pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
