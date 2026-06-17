using Microsoft.Data.Sqlite;
using MyControllerApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options => 
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddOpenApi();

// Inyección de tu servicio de cuentas
builder.Services.AddScoped<ICuentaService, CuentaService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// ========================================================
// 📦 INICIALIZACIÓN DE LA BASE DE DATOS (¡ANTES DEL RUN!)
// ========================================================
// Forzamos una ruta absoluta en el directorio de ejecución para evitar fallos de carpetas
string rutaBaseDeDatos = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "miapp.db");

using (var conexion = new SqliteConnection($"Data Source={rutaBaseDeDatos}"))
{
    conexion.Open();
    Console.WriteLine("-> La conexión a SQLite funciona perfecto.");

    // 1. Crear tabla Personas
    using var crearTablaPersonas = conexion.CreateCommand();
    crearTablaPersonas.CommandText = @"
    CREATE TABLE IF NOT EXISTS Personas (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        Dni INTEGER NOT NULL,
        Nombre TEXT NOT NULL,
        Email TEXT NOT NULL,
        Contraseña TEXT NOT NULL
    );";
    crearTablaPersonas.ExecuteNonQuery();
    Console.WriteLine("-> Tabla Personas verificada/creada.");

    // 2. Crear tabla Cuentas (Corregida la coma del final y agregada la Foreign Key para el TP)
    using var crearTablaCuentas = conexion.CreateCommand();
    crearTablaCuentas.CommandText = @"
    CREATE TABLE IF NOT EXISTS Cuentas (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        TipoCuenta TEXT NOT NULL,
        Saldo NUMERIC NOT NULL,
        PersonaId INTEGER NOT NULL,
        FOREIGN KEY (PersonaId) REFERENCES Personas(Id) ON DELETE CASCADE
    );";
    crearTablaCuentas.ExecuteNonQuery();
    Console.WriteLine("-> Tabla Cuentas verificada/creada.");

// ========================================================

// ¡El Run va AL FINAL de todo!

// INSERT TEMPORAL PARA PRUEBAS
using var insertarPersonaPrueba = conexion.CreateCommand();
insertarPersonaPrueba.CommandText = @"
INSERT INTO Personas (Id, Dni, Nombre, Email, Contraseña) 
VALUES (1, 12345678, 'Mista Test', 'mista@test.com', '1234')
ON CONFLICT(Id) DO NOTHING;"; // El ON CONFLICT evita que rompa si volvés a ejecutar
insertarPersonaPrueba.ExecuteNonQuery();
Console.WriteLine("-> Persona de prueba ID: 1 insertada.");
}


app.Run();