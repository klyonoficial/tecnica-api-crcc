using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using Serilog;
using Microsoft.Extensions.Configuration;

namespace Tecnica.Core.Infra.Data
{
    public class MigrationRunner
    {
        public static void Run(IConfiguration configuration)
        {
            Log.Information("Migration.Run() iniciando");

            var serviceProvider = CreateServices(configuration);

            // Put the database update into a scope to ensure
            // that all resources will be disposed.
            using (var scope = serviceProvider.CreateScope())
            {
                UpdateDatabase(scope.ServiceProvider);
            }

            Log.Information("Migration.Run() terminou");
        }


        private static IServiceProvider CreateServices(IConfiguration configuration)
        {
            //return new ServiceCollection()
            //    // Add common FluentMigrator services
            //    .AddFluentMigratorCore()
            //    .ConfigureRunner(rb => rb

            //        .AddSqlServer2012()
            //        // Set the connection string
            //        .WithGlobalConnectionString(configuration[Constants.ConnectionStringPath])
            //        // Define the assembly containing the migrations
            //        //.ScanIn(typeof(MigrationRunner).Assembly).For.Migrations())
            //    // Enable logging to console in the FluentMigrator way
            //   // .AddLogging(lb => lb.AddFluentMigratorConsole())
            //    // Build the service provider
            //    .BuildServiceProvider(false);

            return new ServiceCollection()
            // Logging is the replacement for the old IAnnouncer
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            // Registration of all FluentMigrator-specific services
            .AddFluentMigratorCore()
            // Configure the runner
            .ConfigureRunner(
            builder => builder
                // Use MySQL
                .AddMySql8()
                 // The MySQL connection string
                 .WithGlobalConnectionString(configuration[Constants.ConnectionStringPath])
                // Specify the assembly with the migrations
                .WithMigrationsIn(typeof(MigrationRunner).Assembly))
                .BuildServiceProvider();
        }

        /// <summary>
        /// Update the database
        /// </summary>
        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            // Instantiate the runner
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            // Execute the migrations
             runner.MigrateUp();
        }


    }
}
