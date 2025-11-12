using Facturas.Components;
using Microsoft.Data.Sqlite;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

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
create table if not exists
facturas(identificador integer, nombre text, articulos text, precio interger, total interger, fecha date)
";
comando.ExecuteNonQuery();



app.Run();
