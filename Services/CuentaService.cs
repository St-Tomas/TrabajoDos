using Microsoft.Data.Sqlite;
using MyControllerApi.Dtos;
using MyControllerApi.Models;

namespace MyControllerApi.Services
{
    public class CuentaService : ICuentaService
    {
        private readonly string _connectionString = "Data Source=miapp.db";
        
        public void CrearCuenta(CuentaCrearDto dto)
        {
            if (!Enum.IsDefined(typeof(TipoCuenta), dto.Tipo))
            {
                throw new Exception($"Error: El tipo de cuenta '{dto.Tipo}' no es válido. Solo se permite 0:CuentaCorriente o 1:CajaDeAhorro.");
            }
            using var conexion = new SqliteConnection(_connectionString);
            conexion.Open();

            // Esto es para ver si la persona existe antes de asociarle una cuenta
            var checkCmd = conexion.CreateCommand();
            checkCmd.CommandText = "SELECT COUNT(1) FROM Personas WHERE Id = @PersonaId";
            checkCmd.Parameters.AddWithValue("@PersonaId", dto.PersonaId);
            long existePersona = (long)checkCmd.ExecuteScalar()!;

            if (existePersona == 0)
            {
                throw new Exception("Error: La persona (dueño) especificada no existe.");
            }

            var insertCmd = conexion.CreateCommand();
            insertCmd.CommandText = @"
                INSERT INTO Cuentas (Tipo, Saldo, PersonaId) 
                VALUES (@Tipo, @Saldo, @PersonaId)";
            
            insertCmd.Parameters.AddWithValue("@Tipo", dto.Tipo.ToString());
            insertCmd.Parameters.AddWithValue("@Saldo", dto.Saldo);
            insertCmd.Parameters.AddWithValue("@PersonaId", dto.PersonaId);

            insertCmd.ExecuteNonQuery();
            Console.WriteLine(_connectionString);
        }
        public List<CuentaMostrarDto> ObtenerTodas()
        {
            var lista = new List<CuentaMostrarDto>();

            using var conexion = new SqliteConnection(_connectionString);
            conexion.Open();

            var cmd = conexion.CreateCommand();
            cmd.CommandText = "SELECT Id, Tipo, Saldo, PersonaId FROM Cuentas";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Enum.TryParse(reader.GetString(1), out TipoCuenta tipoEnum);

                lista.Add(new CuentaMostrarDto
                {
                    Id = reader.GetInt32(0),
                    Tipo = tipoEnum,
                    Saldo = reader.GetDecimal(2),
                    PersonaId = reader.GetInt32(3)
                });
            }

            return lista;
        }
        public CuentaMostrarDto? ObtenerPorId(int id)
        {
            using var conexion = new SqliteConnection(_connectionString);
            conexion.Open();

            var cmd = conexion.CreateCommand();
            cmd.CommandText = "SELECT Id, Tipo, Saldo, PersonaId FROM Cuentas WHERE Id = @Id";
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                Enum.TryParse(reader.GetString(1), out TipoCuenta tipoEnum);
                return new CuentaMostrarDto
                {
                    Id = reader.GetInt32(0),
                    Tipo = tipoEnum,
                    Saldo = reader.GetDecimal(2),
                    PersonaId = reader.GetInt32(3)
                };
            }

            return null;
        }

        // Nada mas podemos actualizarle el saldo, no nombres, id o tipos de cuenta
        public void ActualizarCuenta(int id, CuentaActualizarDto dto)
        {
            using var conexion = new SqliteConnection(_connectionString);
            conexion.Open();
        
            var cmd = conexion.CreateCommand();
            cmd.CommandText = @"
                UPDATE Cuentas 
                SET Saldo = @Saldo 
                WHERE Id = @Id";
        
            cmd.Parameters.AddWithValue("@Saldo", dto.Saldo);
            cmd.Parameters.AddWithValue("@Id", id);
        
            cmd.ExecuteNonQuery();
        }
        public void EliminarCuenta(int id)
        {
            using var conexion = new SqliteConnection(_connectionString);
            conexion.Open();

            var cmd = conexion.CreateCommand();
            cmd.CommandText = "DELETE FROM Cuentas WHERE Id = @Id";
            cmd.Parameters.AddWithValue("@Id", id);

            cmd.ExecuteNonQuery();
        }
    }
}