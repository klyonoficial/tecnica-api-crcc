using Microsoft.EntityFrameworkCore;
using Serilog;
using Tecnica.Core.Infra;
using Tecnica.Core.Infra.Data;
using Tecnica.Domain.Daos;
using Tecnica.Infra;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
var connectionString =
    configuration[Constants.ConnectionStringPath]
        ?? throw new InvalidOperationException("Connection string"
        + "'DefaultConnection' not found.");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 0))));


DaoRegister.AddDaos(builder.Services);

Log.Logger = new LoggerConfiguration()
      .ReadFrom.Configuration(builder.Configuration)
      .Enrich.FromLogContext()
      //.Enrich.WithExceptionDetails()
      .Enrich.WithCorrelationId()
      .CreateLogger();
    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog(Log.Logger);
    builder.Host.UseSerilog(Log.Logger);


    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll",
                 builder =>
                 {
                     builder
                     .AllowAnyOrigin()
                     .AllowAnyMethod()
                     .AllowAnyHeader()
                     ;
                 });
    });


var app = builder.Build();
 app.MapControllerRoute(
     name: "default",
     pattern: "{controller=Home}/{action=Index}/{id?}");


Log.Information("Iniciando a execução das migrations...");
MigrationRunner.Run(configuration);
Log.Information("Migrations finalizadas.");

Log.Information("Inicializando cache de cidades...");
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await CidadeDao.InicializarCacheAsync(context);
    var quantidadeCidades = CidadeDao.ObterQuantidadeCidadesCache();
    Log.Information($"Cache de cidades inicializado com {quantidadeCidades} cidades.");
}
Log.Information("Cache de cidades inicializado.");


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tecnica API v1");
    c.RoutePrefix = "swagger";
});

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseCors("AllowAll");

Log.Information("Aplicação iniciada...");
app.Run();


