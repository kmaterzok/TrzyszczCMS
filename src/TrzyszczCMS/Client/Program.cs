using Blazored.LocalStorage;
using Core.Application.Services.Implementation.Rest;
using Core.Application.Services.Interfaces;
using Core.Application.Services.Interfaces.Rest;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TrzyszczCMS.Client.Other;
using TrzyszczCMS.Client.Services.Implementations;
using TrzyszczCMS.Client.Services.Interfaces;
using TrzyszczCMS.Client.ViewModels.SignIn;

namespace TrzyszczCMS.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            RegisterServices(builder.Services, builder.HostEnvironment.BaseAddress);
            RegisterViewModels(builder.Services);
            await builder.Build().RunAsync();
        }

        #region --- Dependency Injection ---
        /// <summary>
        /// Register services for the following application.
        /// </summary>
        /// <param name="services">Service collection</param>
        private static void RegisterServices(IServiceCollection services, string clientBaseAddress)
        {
            services.AddBlazoredLocalStorage();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(clientBaseAddress) });
            services.AddHttpClient(Core.Application.Models.Constants.HTTP_CLIENT_ANON_NAME, client => client.BaseAddress = new Uri(clientBaseAddress));
            services.AddHttpClient(Core.Application.Models.Constants.HTTP_CLIENT_AUTH_NAME, client => client.BaseAddress = new Uri(clientBaseAddress))
                .AddHttpMessageHandler<TokenHeaderHandler>();
            services.AddAuthorizationCore();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRestAuthService, RestAuthService>();


            services.AddScoped<AuthenticationStateProvider, ApplicationAuthenticationStateProvider>();
        }
        /// <summary>
        /// Register ViewModels for the application.
        /// </summary>
        /// <param name="services">Service collection</param>
        private static void RegisterViewModels(IServiceCollection services)
        {
            services.AddTransient<SignInViewModel>();
        }
        #endregion
    }
}