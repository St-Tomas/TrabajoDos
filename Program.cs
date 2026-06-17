using Microsoft.Data.Sqlite;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
// base de datos 
string rutaBaseDeDatos = "miapp.db";

using var conexion = new SqliteConnection($"Data Source={rutaBaseDeDatos}");
conexion.Open();
Console.WriteLine("la conexion anda");
// 4. Creamos una tabla llamada "Personas"
// Si ya existe, no hace nada (gracias al IF NOT EXISTS)
var crearTabla = conexion.CreateCommand();
crearTabla.CommandText = @"
CREATE TABLE IF NOT EXISTS Personas (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
Dni INTEGER NOT NULL,
Nombre TEXT NOT NULL,
Email TEXT NOT NULL,
Contraseña TEXT NOT NULL
)";
crearTabla.ExecuteNonQuery();
Console.WriteLine("Creamos la tabla Personas");
crearTabla = conexion.CreateCommand();
crearTabla.CommandText = @"
CREATE TABLE IF NOT EXISTS Cuentas (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
TipoCuenta TEXT NOT NULL,
Saldo NUMERIC NOT NULL,
PersonaId INTEGER NOT NULL,
)";
crearTabla.ExecuteNonQuery();
Console.WriteLine("Creamos la tabla Cuentas");
