using Microsoft.AspNetCore.Mvc;
using MyControllerApi.Dtos;
using MyControllerApi.Services;

namespace MyControllerApi.Controllers;

[ApiController]
[Route("api/[controller]")] // ruta api/persona
public class PersonaController : ControllerBase
{
    private readonly IPersonaService _personaService;

    public PersonaController(IPersonaService personaService)
    {
        _personaService = personaService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTodas()
    {
        var personas = await _personaService.ObtenerTodasAsync();
        return Ok(personas);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPorId(int id)
    {
        var persona = await _personaService.ObtenerPorIdAsync(id);
        if (persona == null) return NotFound("Persona no encontrada.");
        
        return Ok(persona);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] PersonaCrearDTO dto)
    {
        var nuevaPersona = await _personaService.CrearPersonaAsync(dto);
        
        // Devuelve un 201 y la ruta para consultar a la persona recién creada
        return CreatedAtAction(nameof(GetPorId), new { id = nuevaPersona.Id }, nuevaPersona);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] PersonaActualizarDTO dto)
    {
        var exito = await _personaService.ActualizarPersonaAsync(id, dto);
        if (!exito) return NotFound("Persona no encontrada.");
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var exito = await _personaService.EliminarPersonaAsync(id);
        if (!exito) return NotFound("Persona no encontrada.");
        
        return NoContent();
    }
}