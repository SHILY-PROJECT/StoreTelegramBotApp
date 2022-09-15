using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StoreTelegramBot.CommandCenter.Configuration;
using StoreTelegramBot.CommandCenter.TelegramBot;

namespace StoreTelegramBot.CommandCenter;

public class Startup
{
    public static async Task Main()
    {
        var cts = new CancellationTokenSource();
        await CreateBuilder().Build().Services.GetRequiredService<TelegramBotService>().Start(cts.Token);
        Console.ReadKey();
    }
    
    private static IHostBuilder CreateBuilder() => Host
        .CreateDefaultBuilder()
        .ConfigureServices(s => s.AddCommandCenter());
}