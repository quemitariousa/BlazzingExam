using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using BlazzingExam.Core.DTOs;

namespace BlazzingExam.WebApps.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            
            await builder.Build().RunAsync();
        }

        public static void AddServices(this WebAssemblyHostBuilder builder)
        {
            var version = 1;
            var baseUrl = $"{builder.HostEnvironment.BaseAddress}/Api/V{version}";

            builder.Services.AddHttpClient<ILoginViewModel, LoginViewModel>("BlazzingExamHttp",
                cl => cl.BaseAddress = new Uri($"{baseUrl}/Account"));
        }
    }
}
