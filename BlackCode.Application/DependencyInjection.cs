using Microsoft.Extensions.DependencyInjection;
using BlackCode.Application.Features.Catalogo;
using BlackCode.Application.Features.Cuenta;
using BlackCode.Application.Features.Carrito;
using BlackCode.Application.Features.Pedidos;
using BlackCode.Application.Features.Administracion;

namespace BlackCode.Application;

public static class DependencyInjection
{
    /// <summary>
    /// Aquí se registran los servicios de cada feature (una línea por servicio).
    /// Ejemplo: services.AddScoped&lt;ICatalogQueryService, CatalogQueryService&gt;();
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICatalogoService, CatalogoService>();
        services.AddSingleton<IFiltroProducto, FiltroTexto>();
        services.AddSingleton<IFiltroProducto, FiltroCategoria>();
        services.AddSingleton<IFiltroProducto, FiltroColor>();
        services.AddSingleton<IFiltroProducto, FiltroPrecio>();
        services.AddSingleton<IFiltroProducto, FiltroTalla>();
        services.AddScoped<ICuentaService, CuentaService>();
        services.AddSingleton<IRegistroValidator, RegistroValidator>();
        services.AddScoped<IProveedorAutenticacion, ProveedorContrasena>();
        services.AddScoped<ICarritoService, CarritoService>();
        services.AddScoped<ICalculoCarritoService, CalculoCarritoService>();
        services.AddSingleton<IReglaDescuento, SinDescuento>();
        services.AddSingleton<IMetodoPago, PagoTarjetaSimulado>();
        services.AddSingleton<IMetodoPago, PagoTransferenciaSimulado>();
        services.AddScoped<IPagoService, PagoService>();
        services.AddSingleton<INotificacionPedidoService, NotificacionPedidoService>();
        services.AddScoped<IPedidoService, PedidoService>();
        services.AddScoped<IAdministracionService, AdministracionService>();
        services.AddScoped<IInventarioService, InventarioService>();
        services.AddScoped<IReporteService, ReporteService>();
        services.AddSingleton<IExportadorReporte, ExportadorCsv>();
        services.AddSingleton<IExportadorReporte, ExportadorJson>();
        return services;
    }
}
