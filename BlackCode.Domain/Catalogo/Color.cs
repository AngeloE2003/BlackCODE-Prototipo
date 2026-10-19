namespace BlackCode.Domain.Catalogo;

public class Color
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;

    // Para pintar el puntito de color del filtro, ej. "#C0C0C0"
    public string CodigoHex { get; set; } = "#000000";
}
