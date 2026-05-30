using AELA.API.Data;
using AELA.API.Middleware;
using AELA.API.Models;
using AELA.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Controllers + Swagger
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Banco de dados Oracle (mesma string do Banco Exemplo, com seu RM/senha)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

// Injeção de dependência: liga a INTERFACE à IMPLEMENTAÇÃO.
// Trocar o algoritmo de score depois é só mudar esta linha.
builder.Services.AddScoped<IReadinessCalculator, ReadinessCalculatorService>();

// Registra o tratador global de exceções
builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(); // Ativa o middleware de exceções
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();