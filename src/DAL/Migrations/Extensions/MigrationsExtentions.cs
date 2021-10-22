using DAL.Models.Config;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DAL.Migrations.Extensions
{
    /// <summary>
    /// Extension methods for simpler use of migrations.
    /// </summary>
    public static class MigrationsExtentions
    {
        /// <summary>
        /// Register the service for database migrations.
        /// </summary>
        /// <param name="services">Service collection that will get migrations</param>
        public static void AddMigrations(this IServiceCollection services)
        {
            services.AddLogging(c => c.AddFluentMigratorConsole())
                .AddFluentMigratorCore()
                .ConfigureRunner(c => c.AddPostgres()
                    .WithGlobalConnectionString(services.BuildServiceProvider()
                                                        .GetRequiredService<IConfiguration>()
                                                        .GetConnectionString(nameof(ConnectionStrings.ModifyDbSqlConnection)))
                    .ScanIn(Assembly.GetAssembly(typeof(MigrationManager))).For.Migrations()
                );
        }
    }
}
