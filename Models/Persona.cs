namespace MyControllerApi.Models;
public class Persona
{
    public int Id { get; set; }
    public int Dni {get; set;}
    public required string Nombre { get; set; }
    public required string Email {get; set;}
    public required string Password {get; set;}
}