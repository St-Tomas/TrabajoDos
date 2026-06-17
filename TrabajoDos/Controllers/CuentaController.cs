using Microsoft.AspNetCore.Mvc;
using MyControllerApi.Dtos;
using MyControllerApi.Services;

namespace MyControllerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CuentaController : ControllerBase
    {
        private readonly ICuentaService _cuentaService;

        public CuentaController(ICuentaService cuentaService)
        {
            _cuentaService = cuentaService;
        }

        [HttpPost]
        public IActionResult Crear([FromBody] CuentaCrearDto dto)
        {
            try
            {
                _cuentaService.CrearCuenta(dto);
                return StatusCode(201, "Cuenta creada con éxito.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult ObtenerTodas()
        {
            var cuentas = _cuentaService.ObtenerTodas();
            return Ok(cuentas);
        }

        [HttpGet("{id}")]
        public IActionResult ObtenerPorId(int id)
        {
            var cuenta = _cuentaService.ObtenerPorId(id);
            if (cuenta == null) return NotFound($"No se encontró la cuenta con ID {id}");
            return Ok(cuenta);
        }

        [HttpPut("{id}")]
        public IActionResult Actualizar(int id, [FromBody] CuentaActualizarDto dto)
        {
            Console.WriteLine($"-> Intentando actualizar la cuenta ID: {id}");
            var existente = _cuentaService.ObtenerPorId(id);
            if (existente == null) 
            {
                Console.WriteLine($"-> Error: La cuenta {id} no existe en la BD.");
                return NotFound($"No se encontró la cuenta con ID {id}");
            }

            _cuentaService.ActualizarCuenta(id, dto);
            return Ok("Cuenta actualizada con éxito.");
        }

        [HttpDelete("{id}")]
        public IActionResult Eliminar(int id)
        {
            Console.WriteLine($"-> Intentando eliminar la cuenta ID: {id}");
            var existente = _cuentaService.ObtenerPorId(id);
            if (existente == null) 
            {
                Console.WriteLine($"-> Error: La cuenta {id} no existe en la BD.");
                return NotFound($"No se encontró la cuenta con ID {id}");
            }

            _cuentaService.EliminarCuenta(id);
            return Ok("Cuenta eliminada con éxito.");
        }
    }
}