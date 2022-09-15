using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StoreTelegramBot.CommandCenter.TelegramBot;
using StoreTelegramBot.CommandCenter.TelegramBot.Commands;
using StoreTelegramBotApp.Domain.Configuration;
using StoreTelegramBotApp.Infrastructure.Sql.Configuration;

namespace StoreTelegramBot.CommandCenter.Configuration;

public static class CommandCenterRegistration
{
    public static IServiceCollection AddCommandCenter(this IServiceCollection services)
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .AddUserSecrets<Startup>()
            .Build();
        
        var tbc = config.GetSection("TelegramBotConfiguration").Get<TelegramBotConfiguration>();

        services
            .AddSingleton(config)
            .AddSingleton(tbc)
            .AddDomain()
            .AddInfrastructureSql()
            .AddScoped<TelegramBotService>()
            .AddScoped<MenuCommand>();

        return services;
    }
}