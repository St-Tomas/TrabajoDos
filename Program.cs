using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MyControllerApi.Data;
using MyControllerApi.Services;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<BancoDbContext>(options =>options.UseSqlite("Data Source=miapp.db"));
builder.Services.AddScoped<IPersonaService, PersonaService>();
builder.Services.AddScoped<ICuentaService, CuentaService>();
var app = builder.Build();

// base de datos 
string rutaBaseDeDatos = "miapp.db";
Console.WriteLine(Path.GetFullPath(rutaBaseDeDatos));
using var conexion = new SqliteConnection($"Data Source={rutaBaseDeDatos}");
conexion.Open();
Console.WriteLine("la conexion anda");
var crearTabla = conexion.CreateCommand();
crearTabla.CommandText = @"
CREATE TABLE IF NOT EXISTS Personas (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
Dni INTEGER NOT NULL,
Nombre TEXT NOT NULL,
Email TEXT NOT NULL,
Password TEXT NOT NULL
)";
crearTabla.ExecuteNonQuery();
Console.WriteLine("Creamos la tabla Personas");
crearTabla = conexion.CreateCommand();
crearTabla.CommandText = @"
CREATE TABLE IF NOT EXISTS Cuentas (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
Tipo INTEGER NOT NULL,
Saldo NUMERIC NOT NULL,
PersonaId INTEGER NOT NULL
)";
crearTabla.ExecuteNonQuery();
Console.WriteLine("Creamos la tabla Cuentas");
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection(); da errores por convertir de http a https
app.UseAuthorization();
app.MapControllers();
app.Run();