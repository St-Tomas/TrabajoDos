namespace MyControllerApi.Dtos;
public class CuentaCrearDto
{
    public TipoCuenta Tipo {get; set;}
    public decimal Saldo { get; set; }
    public int PersonaId { get; set; }
}