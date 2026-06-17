namespace MyControllerApi.Dtos;
public class PersonaCrearDTO
{
    public int Dni { get; set; }
    public required string Nombre { get; set; }
    public required string Email {get; set;}
    public required string Password {get; set;}
}