namespace MyControllerApi.Models;
public class Persona
{
    public int Id { get; set; }
    public int Dni {get; set;}
    public required string Nombre { get; set; } //Estos requiered te quitan los warning y supuestamente pide que SI O SI le pongas valor al inicializar, estan tmb en los DTO
    public required string Email {get; set;}
    public required string Password {get; set;}
}