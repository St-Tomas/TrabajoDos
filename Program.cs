using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MyControllerApi.Data;
using MyControllerApi.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Configurar DbContext
string rutaBaseDeDatos = "miapp.db";
builder.Services.AddDbContext<BancoDbContext>(options =>
    options.UseSqlite($"Data Source={rutaBaseDeDatos}")
);

// Registrar servicios
builder.Services.AddScoped<IPersonaService, PersonaService>();

var app = builder.Build();

// Inicializar base de datos
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BancoDbContext>();
    dbContext.Database.EnsureCreated();
    Console.WriteLine("Conexión a la base de datos establecida");
    Console.WriteLine("Tablas creadas/verificadas");
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();