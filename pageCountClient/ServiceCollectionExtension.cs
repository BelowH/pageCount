using Microsoft.Extensions.DependencyInjection;
using pageCountClient.Domain;
using pageCountClient.Repository;
using pageCountClient.Services;

namespace pageCountClient;

public static class ServiceCollectionExtension
{

    public static IServiceCollection AddPageCount(this IServiceCollection services, PageCountOptions options)
    {
        services.AddSingleton(options);
        services.AddScoped<IPageCountRepository, PageCountRepository>();
        services.AddScoped<IPageCountService, PageCountService>();
        return services;
    }  
    
    
}