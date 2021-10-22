using Core.Server.Models.Settings;
using Core.Server.Services.Implementation;
using Core.Server.Services.Implementation.DbAccess.Read;
using Core.Server.Services.Implementations.DbAccess;
using Core.Server.Services.Interfaces;
using Core.Server.Services.Interfaces.DbAccess;
using Core.Server.Services.Interfaces.DbAccess.Read;
using DAL.Helpers;
using DAL.Helpers.Interfaces;
using DAL.Migrations;
using DAL.Migrations.Extensions;
using DAL.Models.Config;
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
            services.Configure<CryptoSettings>(Configuration.GetSection("CryptoSettings"));
            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));

            services.AddScoped<ICryptoService, CryptoService>();
            services.AddScoped<IDatabaseStrategyFactory, DatabaseStrategyFactory>();
            services.AddScoped<IAuthDatabaseService, AuthDatabaseService>();
            services.AddScoped<ILoadPageDbService, LoadPageDbService>();
        }
        #endregion


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMigrations();
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
