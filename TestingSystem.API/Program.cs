using TestingSystem.Core.Interfaces;
using TestingSystem.Infrastructure.Repositories;
using TestingSystem.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// CORS - разрешаем запросы с HTML страницы
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITestRepository, InMemoryTestRepository>();
builder.Services.AddScoped<ITestService, TestService>();
builder.Services.AddScoped<IAnswerValidator, AnswerValidator>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

var app = builder.Build();

app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

Console.WriteLine("=== Система тестирования знаний запущена ===");
Console.WriteLine("Swagger UI: http://localhost:5000/index.html");

app.Run();