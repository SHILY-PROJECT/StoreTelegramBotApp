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
        services
            .AddDomain()
            .AddInfrastructureSql()
            .AddScoped<TelegramBotService>()
            .AddScoped<MenuCommand>();

        return services;
    }
}