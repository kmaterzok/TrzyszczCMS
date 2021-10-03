using Core.Server.Services.Implementations.DbAccess;
using Core.Server.Services.Interfaces.DbAccess;
using DAL.Helpers;
using DAL.Migrations;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Reflection;
using TrzyszczCMS.Server.Data;
using TrzyszczCMS.Server.Handlers;

namespace TrzyszczCMS.Server
{
    public class Startup
    {
        #region Ctor & properties
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        #endregion

        #region --- Dependency Injection ---
        /// <summary>
        /// Register services for the following application.
        /// </summary>
        /// <param name="services">Service collection</param>
        private void RegisterServices(IServiceCollection services)
        {
            services.AddScoped(_ => (IAuthDatabaseService)new AuthDatabaseService(
                new PgsqlDatabaseStrategy(Configuration.GetConnectionString("AuthDbSqlConnection"))
            ));
        }
        #endregion

        /// <summary>
        /// Configure services for executing migrations.
        /// </summary>
        /// <param name="services">Service collection interface</param>
        private void ConfigureMigrations(IServiceCollection services)
        {
            // TODO: Move the following method it to another project, eg. DAL.
            services.AddLogging(c => c.AddFluentMigratorConsole())
                .AddFluentMigratorCore()
                .ConfigureRunner(c => c.AddPostgres()
                    .WithGlobalConnectionString(Configuration.GetConnectionString("ModifyDbSqlConnection"))
                    .ScanIn(Assembly.GetAssembly(typeof(MigrationManager))).For.Migrations()
                );
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            this.ConfigureMigrations(services);
            services.MigrateDatabase();

            var corsOrigins = Configuration.GetValue<string>("CorsOrigins")
                                           .Split(';');

            services.AddCors(options =>
                options.AddDefaultPolicy(builder =>
                    builder.WithOrigins(corsOrigins)
                           .AllowAnyMethod()
                           .AllowAnyHeader()));

            RegisterServices(services);

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddAuthentication(Constants.DEFAULT_AUTHENTICATION_SCHEME_NAME)
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(Constants.DEFAULT_AUTHENTICATION_SCHEME_NAME, null);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
