using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using BlazzingExam.Core.DTOs;
using Microsoft.AspNetCore.Components.Authorization;
using BlazzingExam.Core.Security;

namespace BlazzingExam.WebApps.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            AddServices(builder);
            AddAuthorization(builder);

            await builder.Build().RunAsync();
        }

        private static void AddServices(WebAssemblyHostBuilder builder)
        {
            var version = 1;
            var baseUrl = $"{builder.HostEnvironment.BaseAddress}Api/V{version}";

            builder.Services.AddHttpClient<ILoginViewModel, LoginViewModel>("BlazzingExamHttp",
                cl => cl.BaseAddress = new Uri($"{baseUrl}/Account"));

            builder.Services.AddHttpClient<IRegisterViewModel, RegisterViewModel>("BlazzingExamHttp",
                cl => cl.BaseAddress = new Uri($"{baseUrl}/Account"));
        }

        private static void AddAuthorization(WebAssemblyHostBuilder builder)
        {
            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();

            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
        }
    }
}
