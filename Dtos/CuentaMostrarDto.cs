namespace MyControllerApi.Dtos;
public class CuentaMostrarDto
{
    public TipoCuenta Tipo {get; set;}
    public decimal Saldo { get; set; }
    public int PersonaId { get; set; }
}