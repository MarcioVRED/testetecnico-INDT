using Insurance.Application;
using Insurance.Application.Contracts;
using Insurance.Application.Messaging;
using Insurance.ContratacaoService.Api.Middleware;
using Insurance.ContratacaoService.Infra.Persistence;
using Insurance.Domain.Repositories.Contracts;
using Insurance.Infra.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (!string.IsNullOrEmpty(dbHost) && !string.IsNullOrEmpty(dbName) && !string.IsNullOrEmpty(dbPassword))
{
    connectionString = $"Server={dbHost};Database={dbName};User Id=sa;Password={dbPassword};TrustServerCertificate=True";
}

builder.Services.AddDbContext<ContratacaoDbContext>(options =>
    options.UseSqlServer(
        connectionString,
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null
        )
    )
);
builder.Services.AddScoped<IContratacaoRepository, ContratacaoRepository>();

builder.Services.AddScoped<IContratacaoApplicationService, ContratacaoApplicationService>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<PropostaAprovadaConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.ReceiveEndpoint("proposta-aprovada-queue", e =>
        {
            e.ConfigureConsumer<PropostaAprovadaConsumer>(context);
        });
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Insurance - Contratacao API",
        Version = "v1",
        Description = "API de Contratacao de Seguros"
    });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<ContratacaoDbContext>();
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocorreu um erro durante a migração do banco de dados.");
    }
}

app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Insurance API V1"));
}

app.UseHttpsRedirection();
app.MapControllers();
app.MapGet("/health", () => Results.Ok("Healthy"));

app.Run();