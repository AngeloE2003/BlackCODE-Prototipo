using BlackCode.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
namespace BlackCode.Application.Features.Cuenta;
public interface IProveedorAutenticacion
{
    string Codigo { get; }
    Task<UsuarioDto?> AutenticarAsync(string identificador, string secreto);
}
public sealed class ProveedorContrasena(IAppDbContext db, IContrasenaService claves, TimeProvider reloj)
    : IProveedorAutenticacion
{
    public string Codigo => "contrasena";
    public async Task<UsuarioDto?> AutenticarAsync(string identificador, string secreto)
    {
        var email = identificador.Trim().ToLowerInvariant();
        var usuario = await db.Usuarios.SingleOrDefaultAsync(u => u.Correo == email);
        var now = reloj.GetUtcNow().UtcDateTime;
        if (usuario is null || !usuario.Activo || usuario.BloqueadoHastaUtc > now) return null;
        var resultado = claves.Verificar(usuario.HashContrasena, secreto);
        if (resultado == ResultadoContrasena.Fallido)
        {
            usuario.IntentosFallidos++;
            if (usuario.IntentosFallidos >= 5)
            {
                usuario.BloqueadoHastaUtc = now.AddMinutes(15);
                usuario.IntentosFallidos = 0;
            }
            await db.SaveChangesAsync();
            return null;
        }
        usuario.IntentosFallidos = 0;
        usuario.BloqueadoHastaUtc = null;
        if (resultado == ResultadoContrasena.ExitosoRehashRequerido)
            usuario.HashContrasena = claves.Hashear(secreto);
        await db.SaveChangesAsync();
        return new(usuario.Id, usuario.NombreCompleto, usuario.Correo, usuario.Rol.ToString());
    }
}
