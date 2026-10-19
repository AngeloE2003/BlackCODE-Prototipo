using BlackCode.Application.Abstractions;
using BlackCode.Domain.Common;
using BlackCode.Domain.Usuarios;
using Microsoft.EntityFrameworkCore;
namespace BlackCode.Application.Features.Cuenta;
public record UsuarioDto(int Id, string Nombre, string Correo, string Rol);
public interface ICuentaService
{
    Task<UsuarioDto> RegistrarAsync(string nombre, string correo, string contrasena);
    Task<UsuarioDto?> AutenticarAsync(string correo, string contrasena, string proveedor = "contrasena");
}
public sealed class CuentaService(IAppDbContext db, IContrasenaService claves, TimeProvider reloj,
    IRegistroValidator validacion, IEnumerable<IProveedorAutenticacion> proveedores) : ICuentaService
{
    public async Task<UsuarioDto> RegistrarAsync(string nombre, string correo, string contrasena)
    {
        correo = correo.Trim().ToLowerInvariant();
        validacion.Validar(nombre, correo, contrasena);
        if (await db.Usuarios.AnyAsync(u => u.Correo == correo))
            throw new DomainException("Ya existe una cuenta con ese correo.");
        var now = reloj.GetUtcNow().UtcDateTime;
        var usuario = new Usuario
        {
            NombreCompleto = nombre.Trim(), Correo = correo, Rol = RolUsuario.Cliente,
            HashContrasena = claves.Hashear(contrasena), CreadoUtc = now, TerminosAceptadosUtc = now
        };
        db.Usuarios.Add(usuario);
        try { await db.SaveChangesAsync(); }
        catch (DbUpdateException) { throw new DomainException("No se pudo registrar el correo. Puede que ya exista una cuenta."); }
        return new(usuario.Id, usuario.NombreCompleto, usuario.Correo, usuario.Rol.ToString());
    }
    public Task<UsuarioDto?> AutenticarAsync(string correo, string contrasena, string proveedor = "contrasena")
    {
        var estrategia = proveedores.SingleOrDefault(p => p.Codigo == proveedor)
            ?? throw new DomainException("La forma de acceso seleccionada no está disponible.");
        return estrategia.AutenticarAsync(correo, contrasena);
    }
}
