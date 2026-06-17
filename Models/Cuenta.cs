namespace MyControllerApi.Models;
public class Cuenta
{
    public int Id { get; set; }
    public TipoCuenta Tipo {get; set;}
    public decimal Saldo { get; set; }
    public int PersonaId { get; set; }
    
    // Navegación a Persona
    public Persona? Persona { get; set; }
}