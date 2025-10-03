using Microsoft.EntityFrameworkCore;
using Serilog;
using Tecnica.Core.Infra;
using Tecnica.Core.Infra.Data;
using Tecnica.Domain.Daos;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Conexão com banco
var connectionString =
    configuration[Constants.ConnectionStringPath]
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

DaoRegister.AddDaos(builder.Services);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    //.Enrich.WithExceptionDetails()
    .Enrich.WithCorrelationId()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(Log.Logger);
builder.Host.UseSerilog(Log.Logger);

// CORS - permite qualquer origem (útil para front Docker ou localhost)
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:8080") // endereço do seu front Vue
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});


// Porta dinâmica (Heroku ou Docker)
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(int.Parse(port));
});

var app = builder.Build();

// CORS precisa ser chamado antes do UseRouting e endpoints
app.UseCors("AllowAll");

// Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    // Acessível via http://localhost:5000/swagger
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tecnica API v1");
    c.RoutePrefix = "swagger";
});

// HTTPS redirection (opcional, no Docker ou Heroku pode ignorar)
app.UseHttpsRedirection();

app.MapControllers();

Log.Information("Aplicação iniciada na porta {Port}", port);

// Executa migrations
Log.Information("Iniciando a execução das migrations...");
MigrationRunner.Run(configuration);
Log.Information("Migrations finalizadas.");

app.Run();
