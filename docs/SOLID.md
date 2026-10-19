# SOLID en Black Code — dónde está cada principio

> Cada dueño de módulo agrega las filas de su módulo en su PR. Esta tabla es lo que se muestra en la sustentación.

| Principio | Archivo | Qué demuestra | Autor |
|---|---|---|---|
| DIP | `BlackCode.Application/Abstractions/IAppDbContext.cs` + `BlackCode.Infrastructure/Persistence/AppDbContext.cs` | Application depende de una interfaz; solo Infrastructure sabe que la base es MySQL | Equipo |
| DIP | `BlackCode.Infrastructure/DependencyInjection.cs` + `BlackCode.Web/Program.cs` | Raíz de composición: el único lugar donde se conectan interfaces con implementaciones | Equipo |
| SRP | `BlackCode.Infrastructure/Security/AspNetContrasenaService.cs` | Hashear contraseñas es una responsabilidad separada del login | Equipo |
| SRP | `BlackCode.Infrastructure/Persistence/Configurations/*.cs` | Una clase de configuración por tabla | Equipo |
| OCP | `BlackCode.Domain/Pedidos/TransicionesPedido.cs` | Agregar un estado de pedido = agregar una entrada, sin tocar la lógica | Equipo |
| ISP | `BlackCode.Application/Abstractions/IUsuarioActual.cs` | Interfaz chica: solo lo que los servicios necesitan saber del usuario | Equipo |
| | | | |
