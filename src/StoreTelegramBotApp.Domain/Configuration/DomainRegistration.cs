using Microsoft.Extensions.DependencyInjection;
using StoreTelegramBotApp.Domain.Products;

namespace StoreTelegramBotApp.Domain.Configuration;

public static class DomainRegistration
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();

        return services;
    }
}