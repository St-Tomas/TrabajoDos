using MyControllerApi.Dtos;

namespace MyControllerApi.Services
{
    public interface ICuentaService
    {
        void CrearCuenta(CuentaCrearDto dto);
        List<CuentaMostrarDto> ObtenerTodas();
        CuentaMostrarDto? ObtenerPorId(int id);
        void ActualizarCuenta(int id, CuentaActualizarDto dto);
        void EliminarCuenta(int id);
    }
}