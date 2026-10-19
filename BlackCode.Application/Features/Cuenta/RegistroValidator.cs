using System.Net.Mail;
using BlackCode.Domain.Common;
namespace BlackCode.Application.Features.Cuenta;
public interface IRegistroValidator
{
    void Validar(string nombre, string correo, string contrasena);
}
public sealed class RegistroValidator : IRegistroValidator
{
    public void Validar(string nombre, string correo, string contrasena)
    {
        if (nombre.Trim().Length < 2 || nombre.Length > 120 || !MailAddress.TryCreate(correo, out _)
            || correo.Length > 254 || contrasena.Length < 8 || contrasena.Length > 128)
            throw new DomainException("Revisa tus datos. La contraseña debe tener entre 8 y 128 caracteres.");
    }
}
