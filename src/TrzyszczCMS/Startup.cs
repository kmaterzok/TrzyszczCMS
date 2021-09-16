using TrzyszczCMS.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FluentMigrator.Runner;
using DAL.Migrations;
using System.Reflection;

namespace TrzyszczCMS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        #region Dependency Injection
        /// <summary>
        /// Register services for the following application.
        /// </summary>
        /// <param name="services">Service collection</param>
        private static void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<WeatherForecastService>();
        }
        /// <summary>
        /// Register ViewModels for the application.
        /// </summary>
        /// <param name="services">Service collection</param>
        private static void RegisterViewModels(IServiceCollection services)
        {
            // This function has done nothing thus far.
        }
        #endregion  

        #region Configuring methods
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddMvc().WithRazorPagesRoot("/Views");

            //services.AddLogging(loggingBuilder =>
            //{
            //    // configure Logging with NLog
            //    //loggingBuilder.ClearProviders();
            //    loggingBuilder.SetMinimumLevel(LogLevel.Information);
            //    loggingBuilder.AddNLog("NLog.config");
            //});
            ////var appSettingsSection = Configuration.GetSection("AppSettings");
            ////services.Configure<AppSettings>(appSettingsSection);
            
            this.ConfigureMigrations(services);
            services.MigrateDatabase();

            RegisterServices(services);
            RegisterViewModels(services);
        }

        /// <summary>
        /// Configure services for executing migrations.
        /// </summary>
        /// <param name="services">Service collection interface</param>
        private void ConfigureMigrations(IServiceCollection services)
        {
            services.AddLogging(c => c.AddFluentMigratorConsole())
                .AddFluentMigratorCore()
                .ConfigureRunner(c => c.AddPostgres()
                    .WithGlobalConnectionString(Configuration.GetConnectionString("ModifyDbSqlConnection"))
                    .ScanIn(Assembly.GetAssembly(typeof(MigrationManager))).For.Migrations()
                );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
        #endregion
    }
}
