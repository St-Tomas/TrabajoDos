using Microsoft.Data.Sqlite;
using MyControllerApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options => 
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddOpenApi();

builder.Services.AddScoped<ICuentaService, CuentaService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

string rutaBaseDeDatos = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "miapp.db");

using (var conexion = new SqliteConnection($"Data Source={rutaBaseDeDatos}"))
{
    conexion.Open();
    Console.WriteLine("-> La conexión a SQLite funciona perfecto.");

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


// Esta cuenta es para ir testeando si me deja crear las cuentas, sin personas no me deja xd
using var insertarPersonaPrueba = conexion.CreateCommand();
insertarPersonaPrueba.CommandText = @"
INSERT INTO Personas (Id, Dni, Nombre, Email, Contraseña) 
VALUES (1, 12345678, 'Mista Test', 'mista@test.com', '1234')
ON CONFLICT(Id) DO NOTHING;";
insertarPersonaPrueba.ExecuteNonQuery();
Console.WriteLine("-> Persona de prueba ID: 1 insertada.");
}

app.Run();