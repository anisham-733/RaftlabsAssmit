using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RaftLabs.APIClient.Interfaces;
using RaftLabs.APIClient.Services;
class Program
{
    static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder()
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false);
    })
    .ConfigureServices((context, services) =>
    {
        var baseUrl = context.Configuration["ApiSettings:BaseUrl"];
        var apiKey = context.Configuration["ApiSettings:ApiKey"];
        // Add this to verify config values are loaded
        Console.WriteLine("Base URL: " + baseUrl);
        Console.WriteLine("API KEY: " + apiKey);

        // Register IHttpClientFactory and inject HttpClient
        services.AddHttpClient();

        // Register memory cache
        services.AddMemoryCache();

        // Register the service interface and implementation
        services.AddTransient<IExternalUserService, ExternalUserService>();

        // Register ReqResApiClient using HttpClient and config
        services.AddTransient<IReqResApiClient>(sp =>
        {
            var client = sp.GetRequiredService<IHttpClientFactory>().CreateClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Add("x-api-key", apiKey);
            // ✅ Confirm the header is applied
            foreach (var h in client.DefaultRequestHeaders)
                Console.WriteLine($"Header: {h.Key} = {string.Join(", ", h.Value)}");

            return new ReqResApiClient(client, baseUrl);
        });


    })
    .Build();



        var userService = host.Services.GetRequiredService<IExternalUserService>();
        var user = await userService.GetUserByIdAsync(2);
        Console.WriteLine($"{user.First_Name} {user.Last_Name} - {user.Email}");

        var users = await userService.GetAllUsersAsync();
        foreach (var u in users)
        {
            Console.WriteLine($"{u.Id}: {u.First_Name} {u.Last_Name}");
        }
    }
}
