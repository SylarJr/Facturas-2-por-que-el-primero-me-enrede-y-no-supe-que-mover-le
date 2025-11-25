using Facturas.Components;
using Facturas.Components.Data;
using Microsoft.Data.Sqlite;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<ServicioDeFacturas>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

String ruta = "mibase.db";

using var conexion = new SqliteConnection($"Data Source={ruta}");
conexion.Open();
var comando = conexion.CreateCommand();
comando.CommandText = @"
    CREATE TABLE IF NOT EXISTS facturas (
        identificador INTEGER PRIMARY KEY AUTOINCREMENT, 
        nombre TEXT, 
        articulos TEXT, 
        precio INTEGER, 
        total INTEGER, 
        fecha TEXT
    )";
comando.ExecuteNonQuery();



app.Run();
