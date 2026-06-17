namespace MyControllerApi.Dtos;
using Models;
public class CuentaMostrarDto
{
    public int Id { get; set; }
    public TipoCuenta Tipo {get; set;}
    public decimal Saldo { get; set; }
    public int PersonaId { get; set; }
}