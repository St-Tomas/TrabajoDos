using Microsoft.EntityFrameworkCore;
using MyControllerApi.Data;
using MyControllerApi.Dtos;
using MyControllerApi.Models;

namespace MyControllerApi.Services;

public interface IPersonaService
{
    Task<IEnumerable<PersonaMostrarDTO>> ObtenerTodasAsync();
    Task<PersonaMostrarDTO?> ObtenerPorIdAsync(int id);
    Task<PersonaMostrarDTO> CrearPersonaAsync(PersonaCrearDTO dto);
    Task<bool> ActualizarPersonaAsync(int id, PersonaActualizarDTO dto);
    Task<bool> EliminarPersonaAsync(int id);
}

public class PersonaService : IPersonaService
{
    private readonly BancoDbContext _context;

    public PersonaService(BancoDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PersonaMostrarDTO>> ObtenerTodasAsync()
    {
        return await _context.Personas
            .Select(p => new PersonaMostrarDTO
            {
                Id = p.Id,
                Dni = p.Dni,
                Nombre = p.Nombre,
                Email = p.Email
            }).ToListAsync();
    }

    public async Task<PersonaMostrarDTO?> ObtenerPorIdAsync(int id)
    {
        var persona = await _context.Personas.FindAsync(id);
        if (persona == null) return null;

        return new PersonaMostrarDTO
        {
            Id = persona.Id,
            Dni = persona.Dni,
            Nombre = persona.Nombre,
            Email = persona.Email
        };
    }

    public async Task<PersonaMostrarDTO> CrearPersonaAsync(PersonaCrearDTO dto)
    {
        var nuevaPersona = new Persona
        {
            Dni = dto.Dni,
            Nombre = dto.Nombre,
            Email = dto.Email,
            Password = dto.Password
        };

        _context.Personas.Add(nuevaPersona);
        await _context.SaveChangesAsync();

        return new PersonaMostrarDTO
        {
            Id = nuevaPersona.Id,
            Dni = nuevaPersona.Dni,
            Nombre = nuevaPersona.Nombre,
            Email = nuevaPersona.Email
        };
    }

    public async Task<bool> ActualizarPersonaAsync(int id, PersonaActualizarDTO dto)
    {
        var persona = await _context.Personas.FindAsync(id);
        if (persona == null) return false;

        persona.Email = dto.Email;
        persona.Password = dto.Password;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EliminarPersonaAsync(int id)
    {
        var persona = await _context.Personas.FindAsync(id);
        if (persona == null) return false;

        _context.Personas.Remove(persona);
        await _context.SaveChangesAsync();
        return true;
    }
}