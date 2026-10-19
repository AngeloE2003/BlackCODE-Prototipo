using BlackCode.Application.Features.Cuenta;
using BlackCode.Application.Features.Carrito;
using BlackCode.Domain.Common;
using BlackCode.Web.Models;
using BlackCode.Web.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace BlackCode.Web.Controllers;
public sealed class CuentaController(ICuentaService cuentas,ICarritoService carrito,CarritoIdentidad identidad) : Controller {
    [HttpGet] public IActionResult Login(string? returnUrl=null) => View(new LoginViewModel { ReturnUrl=returnUrl });
    [HttpPost] public async Task<IActionResult> Login(LoginViewModel model) {
        if (ModelState.IsValid) {
            var u = await cuentas.AutenticarAsync(model.Correo,model.Contrasena);
            if (u is not null) { await Sesion(u); return Destino(model.ReturnUrl,u.Rol); }
            ModelState.AddModelError("","Correo o contraseña incorrectos, o cuenta temporalmente bloqueada.");
        }
        model.Contrasena=""; return View(model);
    }
    [HttpGet] public IActionResult Registro(string? returnUrl=null) => View(new RegistroViewModel { ReturnUrl=returnUrl });
    [HttpPost] public async Task<IActionResult> Registro(RegistroViewModel model) {
        if (ModelState.IsValid) {
            try { var u=await cuentas.RegistrarAsync(model.Nombre,model.Correo,model.Contrasena); await Sesion(u); return Destino(model.ReturnUrl,u.Rol); }
            catch (DomainException ex) { ModelState.AddModelError("",ex.Message); }
        }
        model.Contrasena=""; model.Confirmacion=""; return View(model);
    }
    private async Task Sesion(UsuarioDto u) {
        if (identidad.Token is string token) await carrito.FusionarAsync(token,u.Id);
        var principal=new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier,u.Id.ToString()),new Claim(ClaimTypes.Name,u.Nombre),new Claim(ClaimTypes.Role,u.Rol)],CookieAuthenticationDefaults.AuthenticationScheme));
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,principal);
        Response.Cookies.Delete(CarritoIdentidad.Cookie);
    }
    private IActionResult Destino(string? returnUrl,string rol) => !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl) ? LocalRedirect(returnUrl) : RedirectToAction("Index",rol=="Admin" ? "Admin" : "Catalogo");
    [Authorize,HttpPost] public async Task<IActionResult> Logout() { await HttpContext.SignOutAsync(); return RedirectToAction("Index","Home"); }
    public IActionResult AccesoDenegado() { Response.StatusCode=403; return View(); }
}
