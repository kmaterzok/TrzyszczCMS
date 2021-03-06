using Blazored.LocalStorage;
using Blazored.SessionStorage;
using TrzyszczCMS.Core.Application.Services.Implementation.Rest;
using TrzyszczCMS.Core.Application.Services.Interfaces;
using TrzyszczCMS.Core.Application.Services.Interfaces.Rest;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TrzyszczCMS.Client.Other;
using TrzyszczCMS.Client.Services.Implementation;
using TrzyszczCMS.Client.Services.Interfaces;
using TrzyszczCMS.Client.ViewModels.Administering;
using TrzyszczCMS.Client.ViewModels.SignIn;
using TrzyszczCMS.Client.ViewModels.PageContent;
using TrzyszczCMS.Client.ViewModels.Administering.Edit;
using TextCopy;
using TrzyszczCMS.Core.Shared.Helpers.Extensions;

namespace TrzyszczCMS.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            // --- Registration ---
            RegisterServices(builder.Services, builder.HostEnvironment.BaseAddress);
            builder.Services.AddUserPolicyAuthorisation();
            RegisterViewModels(builder.Services);
            // -------------------- 

            await builder.Build().RunAsync();
        }

        #region --- Dependency Injection ---
        /// <summary>
        /// Register services for the following application.
        /// </summary>
        /// <param name="services">Service collection</param>
        private static void RegisterServices(IServiceCollection services, string clientBaseAddress)
        {
            services.InjectClipboard();
            services.AddBlazoredLocalStorage();
            services.AddBlazoredSessionStorage();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(clientBaseAddress) });
            services.AddScoped<TokenHeaderHandler>();
            services.AddHttpClient(Core.Application.Models.Constants.HTTP_CLIENT_ANON_NAME, client => client.BaseAddress = new Uri(clientBaseAddress));
            services.AddHttpClient(Core.Application.Models.Constants.HTTP_CLIENT_AUTH_NAME, client => client.BaseAddress = new Uri(clientBaseAddress))
                .AddHttpMessageHandler<TokenHeaderHandler>();

            services.AddScoped<IDataDepository,      DataDepository>();
            services.AddScoped<IJSInteropService,    JSInteropService>();
            services.AddScoped<IAuthService,         AuthService>();
            services.AddScoped<IRestAuthService,     RestAuthService>();
            services.AddScoped<ILoadPageService,     LoadPageService>();
            services.AddScoped<IManagePageService,   ManagePageService>();
            services.AddScoped<IManageUserService,   ManageUserService>();
            services.AddScoped<IManageFileService,   ManageFileService>();
            services.AddScoped<IManageNavBarService, ManageNavBarService>();

            services.AddScoped<AuthenticationStateProvider, ApplicationAuthenticationStateProvider>();
        }
        /// <summary>
        /// Register ViewModels for the application.
        /// </summary>
        /// <param name="services">Service collection</param>
        private static void RegisterViewModels(IServiceCollection services)
        {
            services.AddTransient<SignInViewModel>();
            services.AddTransient<ManagePagesViewModel>();
            services.AddTransient<ManageUsersViewModel>();
            services.AddTransient<ManageFilesViewModel>();
            services.AddTransient<ModularPageViewModel>();
            services.AddTransient<PageEditorViewModel>();
            services.AddTransient<UserEditorViewModel>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<ChangeOwnPasswordViewModel>();
        }
        #endregion
    }
}
