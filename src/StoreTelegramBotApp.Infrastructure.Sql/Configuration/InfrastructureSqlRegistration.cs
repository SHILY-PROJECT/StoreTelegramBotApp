using Microsoft.Extensions.DependencyInjection;

namespace StoreTelegramBotApp.Infrastructure.Sql.Configuration;

public static class InfrastructureSqlRegistration
{
    public static IServiceCollection AddInfrastructureSql(this IServiceCollection services)
    {
        return services;
    }
}