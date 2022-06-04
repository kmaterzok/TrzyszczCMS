using TrzyszczCMS.Core.Server.Models.Settings;
using TrzyszczCMS.Core.Server.Services.Implementation;
using TrzyszczCMS.Core.Server.Services.Implementation.DbAccess;
using TrzyszczCMS.Core.Server.Services.Implementation.DbAccess.Modify;
using TrzyszczCMS.Core.Server.Services.Implementation.DbAccess.Read;
using TrzyszczCMS.Core.Server.Services.Implementations.DbAccess;
using TrzyszczCMS.Core.Server.Services.Interfaces;
using TrzyszczCMS.Core.Server.Services.Interfaces.DbAccess;
using TrzyszczCMS.Core.Server.Services.Interfaces.DbAccess.Modify;
using TrzyszczCMS.Core.Server.Services.Interfaces.DbAccess.Read;
using TrzyszczCMS.Core.Shared.Exceptions;
using TrzyszczCMS.Core.Shared.Helpers.Extensions;
using TrzyszczCMS.Core.Infrastructure.Helpers;
using TrzyszczCMS.Core.Infrastructure.Helpers.Interfaces;
using TrzyszczCMS.Core.Infrastructure.Migrations;
using TrzyszczCMS.Core.Infrastructure.Migrations.Extensions;
using TrzyszczCMS.Core.Infrastructure.Models.Config;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using TrzyszczCMS.Server.Data;
using TrzyszczCMS.Server.Handlers;
using TrzyszczCMS.TrzyszczCMS.Core.Server.Services.Interfaces;
using TrzyszczCMS.TrzyszczCMS.Core.Server.Services.Implementation;

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
            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));
            services.Configure<CryptoSettings>   (Configuration.GetSection("CryptoSettings"));
            services.Configure<StorageSettings>  (Configuration.GetSection("StorageSettings"));

            var storagePath = Configuration.GetValue<string>("StorageSettings:Path");
            if (!Directory.Exists(storagePath))
            {
                throw new InvalidMemberException($"Path {storagePath} does not belong to an existing directory.");
            }

            services.AddSingleton<IFileFacade,              FileHandler>();
            services.AddSingleton<IDatabaseStrategyFactory, DatabaseStrategyFactory>();
            services.AddSingleton<IRepetitiveTaskService,   RepetitiveTaskService>();

            services.AddScoped<ICryptoService,         CryptoService>();
            services.AddScoped<IStorageService,        StorageService>();
            services.AddScoped<ILoadFileDbService,     LoadFileDbService>();
            services.AddScoped<IAuthDatabaseService,   AuthDatabaseService>();
            services.AddScoped<ILoadPageDbService,     LoadPageDbService>();
            services.AddScoped<IManagePageDbService,   ManagePageDbService>();
            services.AddScoped<IManageUserDbService,   ManageUserDbService>();
            services.AddScoped<IManageFileDbService,   ManageFileDbService>();
            services.AddScoped<IManageNavBarDbService, ManageNavBarDbService>();
        }
        #endregion

        #region Configuring
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

            services.Configure<FormOptions>(options =>
                options.MultipartBodyLengthLimit = int.MaxValue);

            RegisterServices(services);

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddAuthentication(Constants.DEFAULT_AUTHENTICATION_SCHEME_NAME)
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(Constants.DEFAULT_AUTHENTICATION_SCHEME_NAME, null);

            services.AddUserPolicyAuthorisation();
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
        #endregion
    }
}
