using Microsoft.Extensions.DependencyInjection;

namespace BlackCode.Application;

public static class DependencyInjection
{
    /// <summary>
    /// Aquí se registran los servicios de cada feature (una línea por servicio).
    /// Ejemplo: services.AddScoped&lt;ICatalogQueryService, CatalogQueryService&gt;();
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services;
    }
}
