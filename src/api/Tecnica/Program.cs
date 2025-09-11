using Serilog;
using Tecnica.Core.Infra.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();


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
ConfigurationManager configuration = builder.Configuration;

Log.Information("Iniciando a execução das migrations...");
MigrationRunner.Run(configuration);
Log.Information("Migrations finalizadas.");


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tecnica API v1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();


Log.Information("Aplicação iniciada...");
app.Run();


