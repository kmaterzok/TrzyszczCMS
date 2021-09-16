using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace DAL.Migrations
{
    /// <summary>
    /// Migrations executor
    /// </summary>
    public static class MigrationManager
    {
        /// <summary>
        /// Migrate a database to the newest version of its structure.
        /// </summary>
        /// <param name="host">Service collection interface</param>
        public static void MigrateDatabase(this IServiceCollection services)
        {
            var migrationService = services.BuildServiceProvider()
                                           .GetRequiredService<IMigrationRunner>();

            migrationService.ListMigrations();
            migrationService.MigrateUp();
        }
    }
}
